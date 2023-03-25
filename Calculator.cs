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