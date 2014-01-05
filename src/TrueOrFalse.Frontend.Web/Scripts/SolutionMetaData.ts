class SolutionMetaData {
    IsDate: boolean;
    IsNumber: boolean;
    IsText: boolean;
}

enum DatePrecision {
    Day = 1,
    Month = 5,
    Year = 10,
    Decade = 15,
    Century = 20,
    Millenium = 25
}

class SolutionMetadataDate extends SolutionMetaData
{
    constructor() {
        super();
        this.IsDate = true;
    }
    Precision: DatePrecision;

    static GetPrecisionLabel(precision : DatePrecision): string {
        switch (precision) {
            case DatePrecision.Day: return "Tag"; 
            case DatePrecision.Month: return "Monat";
            case DatePrecision.Year: return "Jahr";
            case DatePrecision.Decade: return "Jahrzent";
            case DatePrecision.Century: return "Jahrhundert";
            case DatePrecision.Millenium: return "Jahrtausend";
        }

        return "";
    }
}

class SolutionMetadataNumber extends SolutionMetaData {
    constructor() {
        super();
        this.IsNumber = true;
    }
}

class SolutionMetadataText extends SolutionMetaData {
    constructor() {
        super();
        this.IsText = true;
    }
}