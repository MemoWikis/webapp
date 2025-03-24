import { Tab } from "~/components/page/tabs/tabsStore"

class UrlHelper {
    public sanitizeUri(string: string = "", maxLength: number = 50): string {
        if (!string) {
            string = "_"
        }

        string = Array.from(string)
            .filter(this.isValidChar)
            .flatMap(this.transform)
            .slice(0, maxLength)
            .join("")

        while (string.includes("--")) {
            string = string.replaceAll("--", "-")
        }

        return encodeURIComponent(string)
    }

    private isValidChar(chr: string) {
        return /[a-zA-Z0-9-_ ÄäÜüÖöß]/.test(chr)
    }

    private transform(chr: string): string[] {
        switch (chr) {
            case "ä":
                return ["a", "e"]
            case "Ä":
                return ["A", "e"]
            case "ü":
                return ["u", "e"]
            case "Ü":
                return ["U", "e"]
            case "ö":
                return ["o", "e"]
            case "Ö":
                return ["O", "e"]
            case "ß":
                return ["s", "s"]
            case " ":
                return ["-"]
            case "_":
                return ["-"]
            case "?":
                return [""]
            default:
                return [chr]
        }
    }

    public getPageUrl(
        name: string,
        id: number | string,
        tab: Tab = Tab.Text
    ): string {
        const url = `/${this.sanitizeUri(name)}/${id}`

        switch (tab) {
            case Tab.Learning:
                return `${url}/Lernen`
            case Tab.Analytics:
                return `${url}/Analytics`
            case Tab.Feed:
                return `${url}/Feed`
            default:
                return url
        }
    }

    public getPageUrlWithQuestionId(
        name: string,
        pageId: number | string,
        questionId: number | string
    ) {
        return `${this.getPageUrl(name, pageId)}/Lernen/${questionId}`
    }
    public getQuestionUrl(title: string, id: number | string): string {
        return `/Fragen/${this.sanitizeUri(title)}/${id}`
    }
    public getUserUrl(name: string, id: number | string): string {
        return `/Nutzer/${this.sanitizeUri(name)}/${id}`
    }
    public getCanonicalUserUrl(name: string, id: number | string): string {
        return `/Nutzer/${this.sanitizeUri(name)}/${id}`
    }
}
export default defineNuxtPlugin(() => {
    return {
        provide: {
            urlHelper: new UrlHelper(),
        },
    }
})
