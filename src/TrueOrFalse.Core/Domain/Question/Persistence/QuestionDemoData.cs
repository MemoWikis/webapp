using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TrueOrFalse.Core
{
    public static class QuestionDemoData
    {
        public static List<Question> All()
        {
            var result = new List<Question>();
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
Frage: Wofür steht YAML
Antwort: "YAML Ain't Markup Language"
Antwort-Lang: YAML ist ein rekursives Akronym für „YAML Ain't Markup Language“ (ursprünglich „Yet Another Markup Language“). 
Tags: Softwareentwicklung {YAML, Datenformate}

----
Frage: Wofür steht SUT
Antwort: "System under test"
Antwort-Lang: "". 
Antwort-Wikipedia-URL: http://en.wikipedia.org/wiki/System_under_test
Tags: Softwareentwicklung {Testing, Acronyme}

Frage: Wofür steth CQRS
Antwort: Command Query Responsibility Segregation
Antwort-Url: http://elegantcode.com/2009/11/11/cqrs-la-greg-young/
Antwort-Url: http://jonathan-oliver.blogspot.com/2009/03/dddd-and-cqs-getting-started.html
Antwort-Url: http://www.vimeo.com/8944337
Tags: Softwareentwicklung {DDD, Acronyme}

*/
    }
}
