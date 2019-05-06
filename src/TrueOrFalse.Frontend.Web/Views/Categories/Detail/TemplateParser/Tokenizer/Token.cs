using System.Text;

public class Token
{
    public TokenType Type;

    private readonly StringBuilder _sb = new StringBuilder();

    public bool IsTemplate => Type == TokenType.Template;
    public bool IsText => Type == TokenType.Text;

    public void AddChar(char character) => _sb.Append(character);
    public void AddNewLine() => _sb.Append("\r\n");
    public string ToText() => _sb.ToString();
}