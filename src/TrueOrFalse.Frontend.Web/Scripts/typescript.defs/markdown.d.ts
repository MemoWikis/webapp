/// <reference path="jquery.d.ts"/>
interface MarkdownStatic {
    getSanitizingConverter(): any;
    Editor(converter : any) : any;
    Editor(converter : any, cssSuffix: string) : any;
}

declare var Markdown: MarkdownStatic;