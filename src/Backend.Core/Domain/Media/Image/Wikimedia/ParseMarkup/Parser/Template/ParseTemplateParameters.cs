using System.Text.RegularExpressions;

public class ParseTemplateParameters
{
    enum States
    {
        Detault,
        ParameterStarted
    }

    public static List<Parameter> Run(string section)
    {
        var textTokens = Regex.Split(section, "(\\||{{|}}|\\[\\[|\\]\\]|=)");

        int level = 0;

        var result = new List<Parameter>();
        var enumerator = textTokens.GetEnumerator();
        var currentParameterTokens = new List<string>();
        var state = States.Detault;
        while (enumerator.MoveNext())
        {
            var token = (string)enumerator.Current;

            //ignore parameters of subtemplates
            if (new[] { "[[", "{{" }.Any(x => x == token))
                level++;

            if (new[] { "]]", "}}" }.Any(x => x == token))
                level--;

            //parameter started
            if (token == "|" && level == 0)
            {
                if (currentParameterTokens.Any())
                    result.Add(new Parameter(currentParameterTokens));

                state = States.ParameterStarted;
                currentParameterTokens = new List<string>();

                continue;
            }

            //collect parameter data
            if (state == States.ParameterStarted)
                currentParameterTokens.Add(token);
        }

        if (currentParameterTokens.Any())
            result.Add(new Parameter(currentParameterTokens));

        return result;
    }
}