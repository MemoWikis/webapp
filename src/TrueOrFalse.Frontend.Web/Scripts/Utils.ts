class Utils {
    static Random(minVal: any, maxVal: any, floatVal: any = 'undefined'): number {
        var randVal = minVal + (Math.random() * (maxVal - minVal));
        return <number>(typeof floatVal == 'undefined' ? Math.round(randVal) : randVal.toFixed(floatVal));
    }
}

