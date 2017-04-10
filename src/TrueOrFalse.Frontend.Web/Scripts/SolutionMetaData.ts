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

enum SolutionType {
    Text = 1,
    Numeric = 4,
    Date = 6,
    MultipleChoice_SingleSolution = 3,
    MultipleChoice = 7,
    Sequence = 5,
    MatchList = 8,
    FlashCard = 9
}