using System.Text;

public class Part
{
    public PartType Type;

    private readonly StringBuilder _sb = new StringBuilder();

    public bool IsTemplate => Type == PartType.Template;
    public bool IsText => Type == PartType.Text;

    public void AddChar(char character) => _sb.Append(character);
    public void AddNewLine() => _sb.Append("\r\n");
    public string ToText() => _sb.ToString();
}