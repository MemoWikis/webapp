export function getCountryCode(language: string): string {
    if (language === "en") {
        return "gb"
    }
    return language
}
