const DefaultLicenseId = -1

export class LicenseQuestion {
    Id: number = -1
    NameLong: string = ''
    NameShort: string = ''
    DisplayTextShort:  string = ''
    DisplayTextFull: string = ''

    LicenseLink: string = ''
    LicenseShortDescriptionLink: string = ''

    AuthorRequired: boolean = false
    LicenseLinkRequired: boolean = false
    ChangesNotAllowed: boolean = true

    IsDefault():boolean {
        return this.Id == DefaultLicenseId
    }
}

