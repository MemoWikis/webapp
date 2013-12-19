class SolutionMetaData {
    IsDate: boolean;
    IsNumber: boolean;
    IsText: boolean;
}

enum DatePrecision {
    Day = 1,
    Month = 2,
    Year = 3,
    Decade = 4,
    Century = 5,
    Millenium = 6
}

class SolutionMetadataDate extends SolutionMetaData
{
    constructor() {
        super();
        this.IsDate = true;
    }
    Precision: DatePrecision;

    GetPrecisionLabel(): string {
        switch (this.Precision) {
            case DatePrecision.Day: return "Tag"; 
            case DatePrecision.Month: return "Monat";
            case DatePrecision.Year: return "Jahr";
            case DatePrecision.Decade: return "Jahrzent";
            case DatePrecision.Century: return "Jahrhundert";
            case DatePrecision.Millenium: return "Jahrtausend";
        }

        return "-unknown-";
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