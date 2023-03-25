using System;

public class Calculator {
    
    private readonly char[] validChars = {
        '.',
        '+',
        '-',
        '*',
        '/',
        '^',
        '(',
        ')'
    };

    public void Start() {
        string? str;
        do {
            Console.Write("Enter expression: ");
            str = Console.ReadLine();
        } while (str == null || !legalExpression(str));
        parse(str);
    }

    private void parse(string str) {
        
    }

    private List<string> createTokens(string str) {
        for (var i = 0; i < str.Length; i++) {
            if (Char.IsWhiteSpace(str[i])) {
                str = str.Remove(i);
            }
        }
        var result = new List<string>();
        var tokenStartIndex = 0;
        for (var i = 1; i < str.Length; i++) {
            if (Double.TryParse(str.Substring(tokenStartIndex, i), out double x)) {
                continue;
            }
            if (Double.TryParse(str.Substring(tokenStartIndex, i - 1), out double y)) {
                result.Add(str.Substring(tokenStartIndex, i - 1));    
            }
        }
        return result;
    }

    private bool legalExpression(string str) {
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