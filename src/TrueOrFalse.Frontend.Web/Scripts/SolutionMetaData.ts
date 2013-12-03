class SolutionMetaData {
    IsDate: bool;
    IsNumber: bool;
    IsText: bool;
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