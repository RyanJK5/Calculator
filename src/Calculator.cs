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
    }

    private List<string> createTokens(string str) {
        for (var i = 0; i < str.Length; i++) {
            if (Char.IsWhiteSpace(str[i])) {
                str = str.Remove(i);
            }
        }
        var result = new List<string>();
        var tokenStartIndex = 0;
        for (var i = 1; i <= str.Length; i++) {
            if (i != str.Length && isNumber(str.Substring(tokenStartIndex, i - tokenStartIndex))) {
                continue;
            }
            if (isNumber(str.Substring(tokenStartIndex, i - 1 - tokenStartIndex))) {
                result.Add(str.Substring(tokenStartIndex, i - 1 - tokenStartIndex));
            }
            result.Add(str[i - 1].ToString());
            tokenStartIndex = i;
        }
        result.Add(str.Substring(tokenStartIndex));
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
        for (var i = 0; i < tokens.Count - 1; i++) {
            string token = tokens[i];
            string nextToken = tokens[i + 1];
            if (token == CloseDelimeter) {
                closeCount++;
                if (closeCount > openCount || !isNumber(tokens[i - 1])) {
                    return false;
                }
            }
            else if (token == OpenDelimeter) {
                openCount++;
                if (!isNumber(nextToken)) {
                    return false;
                }
            }
            else if (!isNumber(token) && !isNumber(nextToken) && nextToken != CloseDelimeter && nextToken != OpenDelimeter) {
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