const DefaultLicenseId = -1

export class LicenseQuestion {
    Id: number
    NameLong: string
    NameShort: string
    DisplayTextShort:  string
    DisplayTextFull: string

    LicenseLink: string
    LicenseShortDescriptionLink: string

    AuthorRequired: boolean
    LicenseLinkRequired: boolean
    ChangesNotAllowed: boolean

    IsDefault():boolean {
        return this.Id == DefaultLicenseId
    }
}

