using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public static class QuestionDemoData
    {
        public static QuestionList All()
        {
            var result = new QuestionList();
            result.Add(new Question { Id=  1, Text = "Wie alt bin ich", Answer = new Answer("23")});
            result.Add(new Question { Id = 2, Text = "Wie alt bist Du", Answer = new Answer("42") });
            result.Add(new Question { Id = 3, Text = "Was ist BDD", Answer = new Answer("Behaviour Driven Development") });
            result.Add(new Question { Id = 4, Text = "Wann ist MVC2 RC geworden", Answer = new Answer("16. Dezemer 2009")});
            result.Add(new Question { Id = 5, Text = "Wann ist Releasedate für VS 2010", Answer = new Answer("12. April 2010") });

            return result;
        }
		
/*

Weitere Fragen: 

----
Frage: Ist JSON eine Untermenge von YAML
Antwort: Ja
Antwort-Beschreibung: JSON ist eine echte Untermenge von YAML. Jedes JSON-Dokument ist ein valides YAML-Dokument.
Tags: Softwareentwicklung {YAML, JSON, Datenformate}
----


*/
    }
}
