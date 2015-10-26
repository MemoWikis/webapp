public class ForgettingCurveJson
{
    public static object Load()
    {
        return new {};
    }

    public static object GetSample()
    {
        return new
        {
            Data =
                new
                {
                    cols = new[]
                    {
                        new {id = "X", label = "X", type = "number"},
                        new {id = "Leichte", label = "Blau", type = "number"},
                        new {id = "Schwer", label = "Schwer", type = "number"},
                        new {id = "Mittel", label = "Mittel___", type = "number"},
                        new {id = "Nobrainer", label = "Nobrainer", type = "number"},
                    },
                    rows = new[]
                    {
                        new {c = new[] {new {v = "1"}, new {v = "10"}, new {v = "10"}, new {v = "10"}, new {v = "10"}}},
                        new {c = new[] {new {v = "2"}, new {v = "10"}, new {v = "9"}, new {v = "8"}, new {v = "7"}}},
                        new {c = new[] {new {v = "3"}, new {v = "10"}, new {v = "8"}, new {v = "6"}, new {v = "4"}}},
                        new {c = new[] {new {v = "4"}, new {v = "10"}, new {v = "7"}, new {v = "6"}, new {v = "3"}}},
                        new {c = new[] {new {v = "5"}, new {v = "10"}, new {v = "6"}, new {v = "6"}, new {v = "3"}}},
                        new {c = new[] {new {v = "6"}, new {v = "1"}, new {v = "5"}, new {v = "6"}, new {v = "3"}}},
                    }
                }
        };
    }
}