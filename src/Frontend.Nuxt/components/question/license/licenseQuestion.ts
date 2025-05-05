const DefaultLicenseId = -1

export class LicenseQuestion {
    id: number = -1
    nameLong: string = ''
    nameShort: string = ''
    displayTextShort:  string = ''
    displayTextFull: string = ''

    licenseLink: string = ''
    licenseShortDescriptionLink: string = ''

    authorRequired: boolean = false
    licenseLinkRequired: boolean = false
    changesNotAllowed: boolean = true

    IsDefault():boolean {
        return this.id == DefaultLicenseId
    }
}

