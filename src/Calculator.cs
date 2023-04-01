using System;

public class Calculator {
    
    private static readonly char[] validChars = {
        '.',
        '+',
        '-',
        '*',
        '/',
        '^',
        '(',
        ')'
    };

    private const string OpenDelimeter = "(";
    private const string CloseDelimeter = ")";

    public void Start() {
        while (true) {
            string? str;
            do {
                Console.Write("Enter expression: ");
                str = Console.ReadLine();
            } while (str == null || !validCharacters(str));
            parse(str);
        }
    }

    private void parse(string str) {
        List<string> tokens = createTokens(str);
        if (!validExpresion(tokens)) {
            Console.WriteLine("SYNTAX ERROR");
            return;
        }
        Console.WriteLine(evaluate(tokens, 0, tokens.Count - 1));
    }

    private double evaluate(List<string> tokens, int startIndex, int endIndex) {
        for (var i = startIndex; i <= endIndex; i++) {
            if (tokens[i] == OpenDelimeter)
            {
                int oldCount = tokens.Count;
                evaluate(tokens, i + 1, IndexOfCloseDelimeter(tokens, i, endIndex) - 1);
                endIndex -= (oldCount - tokens.Count);
                tokens.RemoveAt(IndexOfCloseDelimeter(tokens, i, endIndex));
                tokens.RemoveAt(i);
                endIndex -= 2;
            }
        }
        executeOperations(tokens, startIndex, ref endIndex, createDictionary(new string[] {"^"}, 
            new Func<double, double, double>[] {(a, b) => Math.Pow(a, b)}));
        executeOperations(tokens, startIndex, ref endIndex, createDictionary(new string[] {"*", "/"}, 
            new Func<double, double, double>[] {(a, b) => a * b, (a, b) => a / b}));
        executeOperations(tokens, startIndex, ref endIndex, createDictionary(new string[] {"+", "-"}, 
            new Func<double, double, double>[] {(a, b) => a + b, (a, b) => a - b}));
        return Double.Parse(tokens[startIndex]);
    }

    private void executeOperations(List<string> tokens, int startIndex, ref int endIndex, 
        Dictionary<string, Func<double, double, double>> symbolsToOperations) {
        for (var i = startIndex; i < endIndex; i++) {
            foreach (string symbol in symbolsToOperations.Keys) {
                if (tokens[i] == symbol) {
                    tokens[i - 1] = symbolsToOperations[symbol].Invoke(Double.Parse(tokens[i - 1]), Double.Parse(tokens[i + 1])).ToString();
                    tokens.RemoveAt(i + 1);
                    tokens.RemoveAt(i);
                    endIndex -= 2;
                    i--;
                    break;
                }
            }
        }
    }

    private Dictionary<K, V> createDictionary<K, V>(K[] keys, V[] values) where K : notnull {
        if (values.Length != keys.Length) {
            throw new ArgumentException("values and keys must have same length");
        }
        var result = new Dictionary<K, V>();
        for (var i = 0; i < keys.Length; i++) {
            result.Add(keys[i], values[i]);
        }
        return result;
    }

    private int IndexOfCloseDelimeter(List<string> tokens, int delimIndex, int endIndex) {
        int delimTotal = 1;
        for (var j = delimIndex + 1; j <= endIndex; j++) {
            if (tokens[j] == OpenDelimeter) {
                delimTotal++;
            }
            else if (tokens[j] == CloseDelimeter) {
                delimTotal--;
            }
            if (delimTotal == 0) {
                return j;
            }
        }
        return -1;
    }

    private List<string> createTokens(string str) {
        for (var i = 0; i < str.Length; i++) {
            if (Char.IsWhiteSpace(str[i])) {
                str = str.Remove(i, 1);
            }
        }
        var result = new List<string>();
        var tokenStartIndex = 0;
        for (var i = 0; i < str.Length; i++) {
            if (isNumber(str.Substring(tokenStartIndex, i + 1 - tokenStartIndex))) {
                if (i == str.Length - 1) {
                    result.Add(str.Substring(tokenStartIndex));             
                }
                continue;
            }
            if (isNumber(str.Substring(tokenStartIndex, i - tokenStartIndex))) {
                result.Add(str.Substring(tokenStartIndex, i - tokenStartIndex));
                if (str[i].ToString() == OpenDelimeter) {
                    result.Add("*");
                }
            } 
            result.Add(str[i].ToString());
            if (i < str.Length - 1 && str[i].ToString() == CloseDelimeter && str[i + 1].ToString() == OpenDelimeter) {
                result.Add("*");
            }
            tokenStartIndex = i + 1;
        }
        return result;
    }

    private bool validExpresion(List<string> tokens) {
        if (tokens == null) {
            throw new NullReferenceException();
        }
        if (tokens.Find(str => isNumber(str)) == null) {
            return false;
        }
        int openCount = 0;
        int closeCount = 0;
        for (var i = 0; i < tokens.Count; i++) {
            string token = tokens[i];
            if (token == CloseDelimeter) {
                closeCount++;
                if (closeCount > openCount || (!isNumber(tokens[i - 1]) && tokens[i - 1] != CloseDelimeter)) {
                    return false;
                }
                continue;
            }
            if (token == OpenDelimeter) {
                if (i == tokens.Count) {
                    return false;
                }
                openCount++;
                if (!isNumber(tokens[i + 1]) && tokens[i + 1] != OpenDelimeter) {
                    return false;
                }
            }
            else if (i == tokens.Count || 
                (!isNumber(token) && !isNumber(tokens[i + 1]) && tokens[i + 1] != CloseDelimeter && tokens[i + 1] != OpenDelimeter)) {
                return false;
            }
        }
        return openCount == closeCount;
    }
    
    private bool isNumber(string str) =>
        Double.TryParse(str, out double x);

    private bool validCharacters(string str) {
        if (str.Length == 0) {
            return false;
        }
        foreach (char strChar in str) {
            if (!Char.IsNumber(strChar) && !Char.IsWhiteSpace(strChar) && !validChars.Contains(strChar)) {
                return false;
            }
        }
        return true;
    }
} 