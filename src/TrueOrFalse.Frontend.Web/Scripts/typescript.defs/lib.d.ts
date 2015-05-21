/// <reference path="jquery.d.ts"/>
interface JQuery {
    setCursorPosition(position : number);

    bind(p1: any, p2: any);
    has(p1: any);
    hide(p1: any, p2: any, p3: any);
    sparklines(p1: any);
    sparklines(p1: any, p2: any);
    sparklines(p1: any, p2: any, p3: any);
    sparkline(p1: any);
    sparkline(p1: any, p2: any);
    sparkline(p1: any, p2: any, p3: any);
    watch(p1: any);
    typeWatch(p1: any);

    countdown(finalDate: Date, callback: Function);
    countdown(finalDate: string, callback: Function);
    countdown(action: string);
}

interface Window {
    ajaxUrl_SendAnswer: any;
    ajaxUrl_GetAnswer: any;
    ajaxUrl_CountLastAnswerAsCorrect : any;
    questionId : any;
}

interface SignalR {
    brainWavesHub: any;
    gameHub: any;
}