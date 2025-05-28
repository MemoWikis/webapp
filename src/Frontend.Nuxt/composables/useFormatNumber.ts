export const useFormatNumber = () => {
    const { $i18n } = useNuxtApp()
    const { t, locale, localeProperties } = $i18n

    const getLocaleISO = (): string => {
        return (localeProperties.value.iso as string) || 'en-GB'
    }

    const formatNumber = (value: number): string => {
        const isoLocale = getLocaleISO()

        if (value >= 1000000) {
            // Format as millions
            // Use Math.floor to avoid rounding up to the next million
            const millionsValue = Math.floor(value / 100000) / 10
            const millions = millionsValue.toLocaleString(isoLocale, {
                minimumFractionDigits: millionsValue % 1 === 0 ? 0 : 1,
                maximumFractionDigits: 1,
            })
            return t('counter.millions', { value: millions })
        } else if (value >= 1000 && locale.value !== 'de') {
            // Format as thousands
            // Use Math.floor to avoid rounding up to the next thousand
            const thousandsValue = Math.floor(value / 100) / 10
            const thousands = thousandsValue.toLocaleString(isoLocale, {
                minimumFractionDigits: thousandsValue % 1 === 0 ? 0 : 1,
                maximumFractionDigits: 1,
            })
            return t('counter.thousands', { value: thousands })
        }

        if (locale.value !== 'de' || (locale.value === 'de' && value >= 10000))
            return value.toLocaleString(isoLocale)

        return value.toString()
    }

    const getFormattedNumber = (value: string | number): string => {
        if (typeof value !== 'number') {
            return value
        }

        return formatNumber(value)
    }

    return {
        formatNumber,
        getFormattedNumber,
    }
}

export const getFormattedNumber = (value: string | number): string =>
    useFormatNumber().getFormattedNumber(value)
