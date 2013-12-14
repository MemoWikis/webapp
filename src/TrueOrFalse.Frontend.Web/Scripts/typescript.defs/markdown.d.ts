/// <reference path="jquery.d.ts"/>
interface MarkdownStatic {
    getSanitizingConverter(): any;
    Editor(converter : any) : void;
    Editor(converter : any, cssSuffix: string) : void;
}

declare var Markdown: MarkdownStatic;