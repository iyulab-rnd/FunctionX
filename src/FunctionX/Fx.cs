using NCalc;
using System.Text;

namespace FunctionX;

/// <summary>
/// Excel의 Formulas 기능을 모방한 구현입니다.
/// Fx는 parameters에 의한 변수의 매핑을 지원합니다.
/// Expression의 평가 및 계산은 NCalc 라이브러리를 사용합니다.
/// </summary>
public static partial class Fx
{
    private static object[] ListDictionaryToObjectArray(List<Dictionary<string, object?>> list)
    {
        if (list == null || list.Count == 0)
        {
            return []; // 비어있는 리스트의 경우, 비어있는 객체 배열을 반환
        }

        // 첫 번째 딕셔너리의 키들(열 이름)을 추출합니다.
        var columnNames = list[0].Keys.ToArray();
        var numRows = list.Count;
        var numColumns = columnNames.Length;

        // 반환할 객체 배열을 생성합니다. 첫 행에는 열 이름이 들어갑니다.
        object[] result = new object[numRows + 1]; // 데이터 행 수 + 열 이름 행
        result[0] = columnNames; // 첫 번째 행에 열 이름 저장

        // 리스트의 각 딕셔너리(각 행에 해당)를 순회하며 값을 배열에 저장합니다.
        for (int i = 0; i < numRows; i++)
        {
            object?[] rowValues = new object[numColumns];
            for (int j = 0; j < numColumns; j++)
            {
                // 현재 행의 현재 열에 해당하는 값(혹은 null)을 저장
                list[i].TryGetValue(columnNames[j], out object? value);
                rowValues[j] = value;
            }
            result[i + 1] = rowValues; // 첫 번째 행은 열 이름이므로, 데이터는 그 다음 행부터 저장
        }

        return result;
    }

    // 변수 값을 매핑하기 위한 매개 변수 평가 방법을 정의합니다.
    private static object? EvaluateParameter(string name, IDictionary<string, object?> parameters)
    {
        // 결과 딕셔너리에서 변수 값을 찾아 반환합니다.
        if (parameters.TryGetValue(name, out object? value))
        {
            // 값이 List<Dictionary<string, object?>> 타입인 경우, 객체 배열로 변환합니다.
            if (value is List<Dictionary<string, object?>> list)
            {
                return ListDictionaryToObjectArray(list);
            }
            return value;
        }

        // 변수 값이 없는 경우 null을 반환합니다.
        return null;
    }

    public static object? Evaluate(string expression, IDictionary<string, object?> parameters)
    {
        // 수식에서 $ 기호를 제거합니다. (NCalc 라이브러리에서는 변수 이름에 $ 기호를 사용하지 않습니다.)
        expression = expression.Replace("$", "");

        // NCalc의 Expression 객체를 생성합니다. 매개 변수를 사용하여 변수 값을 해석합니다.
        var e = new Expression(expression, EvaluateOptions.IgnoreCase);

        // 매개 변수 이벤트를 구독하여 결과 딕셔너리에서 변수 값을 조회합니다.
        e.EvaluateParameter += (name, args) =>
        {
            args.Result = EvaluateParameter(name, parameters);
        };

        e.EvaluateFunction += (name, args) =>
        {
            args.Result = name.ToUpper() switch
            {
                "SUM" => EvaluateSumFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "AVERAGE" => EvaluateAverageFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "MIN" => EvaluateMinFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "MAX" => EvaluateMaxFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "COUNT" => EvaluateCountFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "INDEX" => EvaluateIndexFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "IF" => EvaluateIfFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "CONCAT" => EvaluateConcatFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "LEFT" => EvaluateLeftFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "RIGHT" => EvaluateRightFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "MID" => EvaluateMidFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "LEN" => EvaluateLenFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                "TRIM" => EvaluateTrimFunction(args.Parameters.Select(p => p.Evaluate()).ToArray(), parameters),
                _ => throw new Exception($"Unsupported function '{name}'."),
            };
        };

        // 수식을 평가하고 결과를 반환합니다.
        return e.Evaluate();
    }

    // 합계를 계산하는 함수
    private static object EvaluateSumFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        double sum = 0;

        foreach (var obj in objects)
        {
            if (obj is IEnumerable<object> objArray)
            {
                foreach (var element in objArray)
                {
                    if (element is double || element is int || element is float || element is long)
                    {
                        sum += Convert.ToDouble(element);
                    }
                }
            }
            else if (obj is double || obj is int || obj is float || obj is long)
            {
                sum += Convert.ToDouble(obj);
            }
        }

        return sum;
    }

    // 평균을 계산하는 함수
    private static object EvaluateAverageFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        double sum = 0;
        int count = 0;
        foreach (var obj in objects)
        {
            if (obj is IEnumerable<object> objArray)
            {
                foreach (var element in objArray)
                {
                    if (element is double || element is int || element is float || element is long)
                    {
                        sum += Convert.ToDouble(element);
                        count++;
                    }
                }
            }
            else if (obj is double || obj is int || obj is float || obj is long)
            {
                sum += Convert.ToDouble(obj);
                count++;
            }
        }

        return count == 0 ? 0 : sum / count; // Avoid division by zero
    }

    // 최소값을 계산하는 함수
    private static object? EvaluateMinFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        double? min = null;
        foreach (var obj in objects)
        {
            if (obj is IEnumerable<object> objArray)
            {
                // 중첩된 객체 배열일 경우 재귀적으로 함수 호출하여 처리합니다.
                object? nestedMin = EvaluateMinFunction(objArray.ToArray(), parameters);
                if (nestedMin is double nestedValue)
                {
                    min = min.HasValue ? Math.Min(min.Value, nestedValue) : nestedValue;
                }
            }
            else if (obj is double || obj is int || obj is float || obj is long)
            {
                double value = Convert.ToDouble(obj);
                min = min.HasValue ? Math.Min(min.Value, value) : value;
            }
        }

        return min; // 변경: min이 null이더라도 null 반환
    }

    // 최대값을 계산하는 함수
    private static object? EvaluateMaxFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        double? max = null;
        foreach (var obj in objects)
        {
            if (obj is IEnumerable<object> objArray)
            {
                // 중첩된 객체 배열일 경우 재귀적으로 함수 호출하여 처리합니다.
                object? nestedMax = EvaluateMaxFunction(objArray.ToArray(), parameters);
                if (nestedMax is double nestedValue)
                {
                    max = max.HasValue ? Math.Max(max.Value, nestedValue) : nestedValue;
                }
            }
            else if (obj is double || obj is int || obj is float || obj is long)
            {
                double value = Convert.ToDouble(obj);
                max = max.HasValue ? Math.Max(max.Value, value) : value;
            }
        }

        return max; // 변경: max가 null이더라도 null 반환
    }

    // 항목 수를 계산하는 함수
    private static object EvaluateCountFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        int count = 0;
        foreach (var obj in objects)
        {
            if (obj is IEnumerable<object> objArray)
            {
                count += objArray.Count(element => IsCountableElement(element));
            }
            else if (IsCountableElement(obj))
            {
                count++;
            }
        }

        return count;
    }

    // 카운트 가능한 요소인지 확인하는 보조 메서드
    private static bool IsCountableElement(object element)
    {
        return element != null &&
               ((element is double || element is int || element is float || element is long) ||
                (element is string str && !string.IsNullOrWhiteSpace(str)));
    }

    // 지정된 범위에서 특정 행과 열의 값을 반환하는 함수
    private static object? EvaluateIndexFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        if (objects.Length < 3)
        {
            throw new ArgumentException("Index 함수는 최소한 3개의 매개 변수가 필요합니다.");
        }

        // 첫 번째 매개 변수는 범위입니다. 여기서는 이 함수가 호출될 때마다 같은 범위로 가정합니다.
        object? range = objects[0];
        if (range is not IEnumerable<object> rangeArray)
        {
            throw new ArgumentException("첫 번째 매개 변수는 범위여야 합니다.");
        }

        // 두 번째 매개 변수는 행 인덱스입니다.
        object? rowIndexObj = objects[1];
        if (rowIndexObj is not int rowIndex)
        {
            throw new ArgumentException("두 번째 매개 변수는 정수여야 합니다.");
        }

        // 세 번째 매개 변수는 열 인덱스 또는 열 이름입니다.
        object? columnIndexObj = objects[2];
        int columnIndex;
        if (columnIndexObj is int columnIndexInt)
        {
            columnIndex = columnIndexInt;
        }
        else if (columnIndexObj is string columnIndexStr)
        {
            // 열 이름에 해당하는 열 인덱스를 찾습니다.
            if (rangeArray.ElementAt(0) is string[] columnNames)
            {
                columnIndex = Array.IndexOf(columnNames, columnIndexStr);
                if (columnIndex == -1)
                {
                    throw new ArgumentException("열을 찾을 수 없습니다.");
                }
            }
            else
            {
                throw new ArgumentException("첫 번째 행은 열 이름을 포함해야 합니다.");
            }
        }
        else
        {
            throw new ArgumentException("세 번째 매개 변수는 정수 또는 문자열이어야 합니다.");
        }

        // 범위에서 특정 행과 열의 값을 찾아서 반환합니다.
        if (rowIndex >= 0 && rowIndex < rangeArray.Count())
        {
            var row = rangeArray.ElementAt(rowIndex);
            if (row is IEnumerable<object> rowArray && columnIndex >= 0 && columnIndex < rowArray.Count())
            {
                return rowArray.ElementAt(columnIndex);
            }
        }

        // 인덱스가 범위를 벗어난 경우 null을 반환합니다.
        return null;
    }

    // 조건을 평가하고 조건이 참이면 하나의 값을, 거짓이면 다른 값을 반환합니다.
    private static object? EvaluateIfFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        if (objects.Length < 2 || objects.Length > 3)
        {
            throw new ArgumentException("IF 함수는 2개 또는 3개의 매개 변수가 필요합니다.");
        }

        // 첫 번째 매개 변수는 조건식입니다.
        object? condition = objects[0];

        // 두 번째 매개 변수는 조건이 참일 때 반환할 값입니다.
        object? trueValue = objects[1];

        // 세 번째 매개 변수는 조건이 거짓일 때 반환할 값입니다. (선택 사항)
        object? falseValue = objects.Length == 3 ? objects[2] : null;

        // 조건식이 true로 평가되면 trueValue를 반환하고, 그렇지 않으면 falseValue를 반환합니다.
        if (IsConditionTrue(condition))
        {
            return trueValue;
        }
        else
        {
            return falseValue;
        }
    }

    // 조건식을 평가하여 true 또는 false를 반환하는 보조 메서드
    private static bool IsConditionTrue(object? condition)
    {
        // 조건식이 null이거나 boolean 형식이 아니면 false를 반환합니다.
        if (condition == null || condition is not bool)
        {
            return false;
        }

        return (bool)condition;
    }

    // 문자열을 연결하는 함수
    private static string EvaluateConcatFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        // StringBuilder를 사용하여 효율적으로 문자열을 연결합니다.
        StringBuilder resultBuilder = new();

        foreach (var obj in objects)
        {
            // 각 객체를 문자열로 변환하여 StringBuilder에 추가합니다.
            var str = Convert.ToString(obj);
            resultBuilder.Append(str);
        }

        // 연결된 문자열을 반환합니다.
        return resultBuilder.ToString();
    }

    // 문자열의 왼쪽에서 지정된 수의 문자를 반환합니다.
    private static string EvaluateLeftFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        if (objects.Length != 2 || objects[0] is not string str || objects[1] is not int length)
        {
            throw new ArgumentException("The LEFT function takes two parameters (string, length).");
        }

        // 지정된 길이가 문자열의 길이보다 크면 전체 문자열을 반환합니다.
        return str[..Math.Min(length, str.Length)];
    }

    // 문자열의 오른쪽에서 지정된 수의 문자를 반환합니다.
    private static object EvaluateRightFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        if (objects.Length != 2 || objects[0] is not string str || objects[1] is not int length)
        {
            throw new ArgumentException("The RIGHT function takes two parameters (string, length).");
        }

        // 지정된 길이가 문자열의 길이보다 크면 전체 문자열을 반환합니다.
        return str[Math.Max(0, str.Length - length)..];
    }

    // 문자열의 중간에서 지정된 위치부터 지정된 수의 문자를 반환합니다.
    private static string EvaluateMidFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        if (objects.Length != 3 || objects[0] is not string str || objects[1] is not int startIndex || objects[2] is not int length)
        {
            throw new ArgumentException("The MID function takes three parameters: a string, a starting index, and a length.");
        }

        // 시작 인덱스는 0부터가 아니라 1부터 시작하므로 1을 빼줍니다.
        startIndex = Math.Max(startIndex - 1, 0); // 음수가 아닌지 확인

        // 시작 인덱스가 문자열의 길이보다 크거나 같으면 빈 문자열을 반환합니다.
        if (startIndex >= str.Length) return "";

        // 지정된 길이가 문자열의 나머지 길이보다 크면 가능한 만큼의 문자열을 반환합니다.
        return str.Substring(startIndex, Math.Min(length, str.Length - startIndex));
    }

    // 문자열의 길이를 반환합니다.
    private static int EvaluateLenFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        if (objects.Length != 1 || objects[0] is not string str)
        {
            throw new ArgumentException("The LEN function takes one parameter (a string).");
        }

        return str.Length;
    }

    // 문자열의 앞뒤 공백을 제거합니다.
    private static string EvaluateTrimFunction(object[] objects, IDictionary<string, object?> parameters)
    {
        if (objects.Length != 1 || objects[0] is not string str)
        {
            throw new ArgumentException("The TRIM function takes one parameter (a string).");
        }

        return str.Trim();
    }
}