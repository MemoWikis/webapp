/// <reference path="jquery.d.ts"/>
interface JQuery {
    defaultText();
    setCursorPosition(position : number);

    sparklines(p1: any);
    sparklines(p1: any, p2: any);
    sparklines(p1: any, p2: any, p3: any);
    sparkline(p1: any);
    sparkline(p1: any, p2: any);
    sparkline(p1: any, p2: any, p3: any);
    has(p1: any);
}

interface Window {
    ajaxUrl_SendAnswer: any;
    ajaxUrl_GetAnswer: any;
    questionId : any;
}