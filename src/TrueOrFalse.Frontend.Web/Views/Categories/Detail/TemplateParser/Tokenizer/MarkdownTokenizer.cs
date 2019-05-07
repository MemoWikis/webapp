using System.Collections.Generic;

public static class MarkdownTokenizer
{
    public static List<Token> Run(string markdown)
    {
        var tokens = new List<Token>();
        var currentPart = new Token();
        string inputText = markdown;
        char lastChar = ' ';
        char preLastChar = ' ';
        string previousPart = "";

        if (inputText.Trim().StartsWith("[["))
            currentPart.Type = TokenType.Template;

        var chars = inputText.ToCharArray();
        var charsLength = chars.Length;
        for (var i = 0; i < charsLength; i++ )
        {
            var character = chars[i];
            var hasNextChar = i < charsLength - 1;

            char nextChar = ' ';
            if (hasNextChar)
                nextChar = chars[i + 1];
            if (!hasNextChar)
                tokens.Add(currentPart);

            if (character == '[' && nextChar == '[')
            {
                if (currentPart.ToText().Trim().Length > 0)
                {
                    tokens.Add(currentPart);
                    previousPart = currentPart.ToText();
                    if (previousPart.EndsWith("]]"))
                    {
                        currentPart = new Token { Type = TokenType.Text };
                        currentPart.AddNewLine();
                        tokens.Add(currentPart);
                        previousPart = currentPart.ToText();
                    }

                    currentPart = new Token { Type = TokenType.Template };
                }
                else if (currentPart.ToText().Trim().Length == 0 && previousPart.EndsWith("]]"))
                {
                    currentPart = new Token { Type = TokenType.Text };
                    currentPart.AddNewLine();
                    tokens.Add(currentPart);
                    previousPart = currentPart.ToText();

                    currentPart = new Token { Type = TokenType.Template };
                }
                else
                    currentPart = new Token { Type = TokenType.Template };
            }
            else if (preLastChar == ']' && lastChar == ']' && !(character == '[' && nextChar == '['))
            {
                tokens.Add(currentPart);
                previousPart = currentPart.ToText();
                currentPart = new Token { Type = TokenType.Text };
            }

            preLastChar = lastChar;
            lastChar = character;

            currentPart.AddChar(character);

        }

        return tokens;
    }
}