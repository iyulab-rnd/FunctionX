
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp.RuntimeBinder;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
namespace FunctionX;

/// <summary>
/// Excel의 Formulas 기능을 모방한 구현입니다.
/// Fx는 parameters에 의한 변수의 매핑을 지원합니다.
/// </summary>
public static partial class Fx
{
    public static async Task<object?> EvaluateAsync(string expression, IDictionary<string, object?>? parameters = null)
    {
        try
        {
            CheckSafeExpression(expression);

            parameters ??= new Dictionary<string, object?>();
            var functions = new FxFunctions(parameters);

            var script = BuildCsScript(expression, parameters);
#if DEBUG
            Debug.WriteLine($"Expression: {script}");
#endif
            var options = ScriptOptions.Default
                    .WithImports("System", "System.Linq")
                    .AddReferences(Assembly.Load("System.Core"));

            var result = await CSharpScript.EvaluateAsync(script, options, globals: functions);
            return result;
        }
        catch (CompilationErrorException ex)
        {
            throw new FxCompilationErrorException(ex.Message);
        }
        catch (ArgumentException ex)
        {
            throw new FxValueException(ex.Message);
        }
        catch (SecurityException ex)
        {
            throw new FxUnsafeExpressionException(ex.Message); // 보안 관련 예외
        }
        catch (InvalidOperationException ex)
        {
            throw new FxExpressionException("Invalid operation: " + ex.Message); // 잘못된 연산 예외
        }
        catch (RuntimeBinderException ex)
        {
            throw new FxExpressionException("Runtime binding error: " + ex.Message); // 바인딩 오류
        }
        catch (FxException)
        {
            throw; // FxException 유형의 예외는 재발생
        }
        catch (Exception ex)
        {
            throw new FxExpressionException("Unexpected error: " + ex.Message); // 기타 예외
        }
    }

    private static bool CheckSafeExpression(string expression)
    {
        // 금지된 키워드 목록
        var forbiddenKeywords = new string[] 
        {
            "import",
            "System.IO", "Process", "Assembly", "File", "Directory", "Thread", "Task", "Environment",
            "Reflection", "DllImport", "Console", "Window", "Registry"
        };

        // 금지된 키워드가 포함되어 있는지 확인
        foreach (var keyword in forbiddenKeywords)
        {
            if (expression.Contains(keyword))
            {
                throw new FxUnsafeExpressionException(keyword);
            }
        }

        // 추가적인 보안 위험을 차단하기 위한 정규 표현식 검사
        var dynamicInvokePattern = new Regex(@"\bGetType\b|\bGetMethod\b|\bGetProperty\b|\bInvokeMember\b");
        if (dynamicInvokePattern.IsMatch(expression))
        {
            throw new FxUnsafeExpressionException("Dynamic invoke");
        }

        return true; // 위의 모든 검사를 통과하면 true 반환
    }

    /// <summary>
    /// 실행가능한 C# 스크립트를 생성합니다.
    /// </summary>
    private static string BuildCsScript(string expression, IDictionary<string, object?> parameters)
    {
        var script = TransformToTryCatchBlocks(expression);
#if DEBUG
        Debug.WriteLine($"[Logs]");
        Debug.WriteLine($"input: {expression}");
        Debug.WriteLine($"output: {script}");
#endif
        script = BuildCsExpression(script, parameters);
        return script;
    }

    // IFERROR 함수를 try-catch 블록으로 변환합니다.
    // 재귀적으로 모든 중첩된 IFERROR 함수도 실행가능한 try-catch 함수로 변환합니다.
    // 실행가능한 C# 스크립트를 생성하여 반환합니다.
    // 예)
    // input: "IFERROR(10 / 5, "ERROR")"
    // output: "try { return 10 / 5; } catch { return "ERROR"; }"
    // input2: "IFERROR(INT(@a) / INT(@b), "ERROR")
    // ouput2: "try { return INT(@a) / INT(@b); } catch { return "ERROR"; }"
    // input3: "IFERROR(IFERROR(INT(@a) / INT(@b), "ERROR"), "ON ERROR")"
    // ouput3: "try { return (Func<object>)(() => { try { return INT(@a) / INT(@b); } catch { return "ERROR"; } })(); } catch { return "ON ERROR"; }"
    public static string TransformToTryCatchBlocks(string input)
    {
        return TransformIFERROR(input, false);
    }

    private static string TransformIFERROR(string input, bool isNested)
    {
        var regex = new Regex(@"IFERROR\(((?:[^()]|(?<Open>\()|(?<-Open>\)))+)(?(Open)(?!)),\s*""(?<error>[^""]*)""\)");
        if (!regex.IsMatch(input))
        {
            return input;
        }

        return regex.Replace(input, match =>
        {
            var innerExpression = match.Groups[1].Value;
            var errorText = match.Groups["error"].Value;
            var transformedInnerExpression = TransformIFERROR(innerExpression, true);

            if (isNested)
            {
                return $"((Func<object>)(() => {{ try {{ return {transformedInnerExpression}; }} catch {{ return \"{errorText}\"; }} }}))()";
            }
            else
            {
                return $"try {{ return {transformedInnerExpression}; }} catch {{ return \"{errorText}\"; }}";
            }
        });
    }


    /// <summary>
    /// @변수를 적절한 Get함수로 치환합니다.
    /// GetItems()는 object[]를 반환합니다.
    /// GetItem()은 object?를 반환합니다.
    /// GetValues()는 double[]의 값을 반환합니다.
    /// GetValue()는 double의 값을 반환합니다.
    /// </summary>
    private static string BuildCsExpression(string expression, IDictionary<string, object?> parameters)
    {
        var stringLiterals = new List<string>();
        var modifiedExpression = Regex.Replace(expression, @"(""([^""\\]|\\.)*""|'([^'\\]|\\.)*')", match =>
        {
            var literal = match.Value;
            // 홑따옴표로 둘러싸인 문자열을 쌍따옴표로 변환합니다.
            if (literal.StartsWith("'") && literal.EndsWith("'"))
            {
                // 홑따옴표 내부의 홑따옴표 이스케이프 처리 (예: 'It\'s fine')
                var replaced = literal[1..^1].Replace("\\'", "'");
                // C# 스타일의 이스케이프로 변환합니다.
                replaced = replaced.Replace("'", "\\'");
                // 쌍따옴표로 감싸줍니다.
                literal = $"\"{replaced}\"";
            }
            stringLiterals.Add(literal);
            return $"$stringLiteral${stringLiterals.Count - 1}$";
        });

        modifiedExpression = TransformVariableReferences(modifiedExpression, parameters);

        modifiedExpression = Regex.Replace(modifiedExpression, @"\$stringLiteral\$(\d+)\$", match =>
        {
            int index = int.Parse(match.Groups[1].Value);
            return stringLiterals[index];
        });

        return modifiedExpression;
    }


    private static string TransformVariableReferences(string expression, IDictionary<string, object?> parameters)
    {
        var operationPattern = @"(@\w+)(\s*(==|!=|>|<|>=|<=|\+|\-|\*|\/|%|\^|&&|\|\||<<|>>|!)\s*@?\w+)+";

        expression = Regex.Replace(expression, operationPattern, match =>
        {
            return Regex.Replace(match.Value, @"\@\w+", m => ReplaceVariableWithGetValueOrValuesCall(m, parameters));
        });

        expression = Regex.Replace(expression, @"\@\w+", m => ReplaceVariableWithGetItemOrItemsCall(m, parameters));

        return expression;
    }

    private static string ReplaceVariableWithGetValueOrValuesCall(Match match, IDictionary<string, object?> parameters)
    {
        var variableName = match.Value[1..]; // '@' 제거
        if (parameters.TryGetValue(variableName, out object? value))
        {
            if (value == null)
            {
                return "null";
            }
            else if (value.GetType().IsArray)
            {
                return $"GetValues(\"{variableName}\")";
            }
            else
            {
                return $"GetValue(\"{variableName}\")";
            }
        }
        return match.Value;
    }

    private static string ReplaceVariableWithGetItemOrItemsCall(Match match, IDictionary<string, object?> parameters)
    {
        var variableName = match.Value[1..]; // '@' 제거
        if (parameters.TryGetValue(variableName, out object? value))
        {
            if (value == null)
            {
                return "null";
            }
            else if (value is IEnumerable && value is not string)
            {
                return $"GetItems(\"{variableName}\")";
            }
            else
            {
                return $"GetItem(\"{variableName}\")";
            }
        }
        return match.Value;
    }

}