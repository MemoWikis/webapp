interface LicenseInfo {
    id: number
    nameShort: string
    nameLong: string
    displayTextShort: string
    displayTextFull: string
    licenseLink?: string
    licenseShortDescriptionLink?: string
    authorRequired: boolean
    licenseLinkRequired: boolean
    changesNotAllowed: boolean
    isDefault: boolean
}

export const useLicense = () => {
    const { t } = useI18n()

    const getLicenseInfo = (licenseId: number): LicenseInfo => {
        switch (licenseId) {
            case 1: // CC BY 4.0 - Default license
                return {
                    id: 1,
                    nameShort: 'CC BY 4.0',
                    nameLong: t('license.ccBy40.nameLong'),
                    displayTextShort: 'CC BY 4.0',
                    displayTextFull: t('license.ccBy40.displayTextFull'),
                    licenseLink: 'https://creativecommons.org/licenses/by/4.0/legalcode',
                    licenseShortDescriptionLink: 'https://creativecommons.org/licenses/by/4.0/deed.de',
                    authorRequired: true,
                    licenseLinkRequired: true,
                    changesNotAllowed: false,
                    isDefault: true
                }

            case 2: // BAMF Official Work
                return {
                    id: 2,
                    nameShort: t('license.bamf.nameShort'),
                    nameLong: t('license.bamf.nameLong'),
                    displayTextShort: t('license.bamf.displayTextShort'),
                    displayTextFull: t('license.bamf.displayTextFull'),
                    licenseLink: 'https://www.gesetze-im-internet.de/urhg/__5.html',
                    authorRequired: true,
                    licenseLinkRequired: false,
                    changesNotAllowed: true,
                    isDefault: false
                }

            case 3: // ELWIS
                return {
                    id: 3,
                    nameShort: 'ELWIS',
                    nameLong: t('license.elwis.nameLong'),
                    displayTextShort: t('license.elwis.displayTextShort'),
                    displayTextFull: t('license.elwis.displayTextFull'),
                    licenseLink: 'https://www.elwis.de/misc/disclaimer.html',
                    authorRequired: true,
                    licenseLinkRequired: false,
                    changesNotAllowed: true,
                    isDefault: false
                }

            case 4: // BLAC
                return {
                    id: 4,
                    nameShort: 'BLAC',
                    nameLong: t('license.blac.nameLong'),
                    displayTextShort: t('license.blac.displayTextShort'),
                    displayTextFull: t('license.blac.displayTextFull'),
                    licenseLink: 'http://blak-uis.server.de/servlet/is/2146/P-4a.pdf',
                    authorRequired: true,
                    licenseLinkRequired: false,
                    changesNotAllowed: true,
                    isDefault: false
                }

            default:
                // Fallback to default CC BY 4.0 license
                return getLicenseInfo(1)
        }
    }

    const getDefaultLicenseId = (): number => 1

    const isDefaultLicense = (licenseId: number): boolean => {
        return licenseId === getDefaultLicenseId()
    }

    const getLicenseImageUrl = (licenseId: number): string | null => {
        if (isDefaultLicense(licenseId)) {
            return '/Images/Licenses/cc-by 88x31.png'
        }
        return null
    }

    const formatLicenseForComponent = (licenseId: number) => {
        const licenseInfo = getLicenseInfo(licenseId)
        return {
            isDefault: licenseInfo.isDefault,
            shortText: licenseInfo.displayTextShort,
            fullText: licenseInfo.displayTextFull
        }
    }

    return {
        getLicenseInfo,
        getDefaultLicenseId,
        isDefaultLicense,
        getLicenseImageUrl,
        formatLicenseForComponent
    }
}
