
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
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
        parameters ??= new Dictionary<string, object?>();
        var functions = new FunctionsX(parameters);

        var script = BuildCsScript(expression, parameters);

        try
        {
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
            throw new FxException($"Compilation error: {ex.Message}");
        }
        catch (Exception ex)
        {
            throw new FxException($"Evaluation error: {ex.Message}");
        }
    }

    /// <summary>
    /// 실행가능한 C# 스크립트를 생성합니다.
    /// </summary>
    private static string BuildCsScript(string expression, IDictionary<string, object?> parameters)
    {
        var script = BuildCsExpression(expression, parameters);
        // IFERROR 구문을 찾아서, try-catch로 변환합니다.
        return script;
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
        // 문자열 리터럴 패턴: "..." 또는 '...'를 찾습니다.
        var stringLiteralPattern = @"(""[^""]*""|'[^']*')";

        // 문자열 리터럴을 임시 플레이스홀더로 대체하고, 원본 문자열 리터럴을 저장합니다.
        var stringLiterals = new List<string>();
        var modifiedExpression = Regex.Replace(expression, stringLiteralPattern, match =>
        {
            stringLiterals.Add(match.Value);
            return $"$stringLiteral${stringLiterals.Count - 1}$"; // 문자열 리터럴 플레이스홀더
        });

        // 연산자와 함께 사용되는 변수 참조 변환
        modifiedExpression = TransformVariableReferences(modifiedExpression, parameters);

        // 임시로 대체했던 문자열 리터럴을 원본으로 복원합니다.
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