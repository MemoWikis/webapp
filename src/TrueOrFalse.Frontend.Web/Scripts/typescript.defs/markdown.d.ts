/// <reference path="jquery.d.ts"/>
interface MarkdownStatic {
    getSanitizingConverter(): any;
    Editor(converter : any) : any;
}

declare var Markdown: MarkdownStatic;