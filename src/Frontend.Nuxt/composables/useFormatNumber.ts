export const useFormatNumber = () => {
    const { $i18n } = useNuxtApp()
    const t = $i18n.t

    const formatNumber = (value: number): string => {
        if (value >= 1000000) {
            // Format as millions
            // Use Math.floor to avoid rounding up to the next million
            const millionsValue = Math.floor(value / 100000) / 10
            const millions = millionsValue.toFixed(1).replace(/\.0$/, '')
            return t('counter.millions', { value: millions })
        } else if (value >= 1000) {
            // Format as thousands
            // Use Math.floor to avoid rounding up to the next thousand
            const thousandsValue = Math.floor(value / 100) / 10
            const thousands = thousandsValue.toFixed(1).replace(/\.0$/, '')
            return t('counter.thousands', { value: thousands })
        }

        return value.toString()
    }

    const formattedNumber = (value: string | number): string | number => {
        if (typeof value !== 'number') {
            return value
        }

        return formatNumber(value)
    }

    return {
        formatNumber,
        formattedNumber,
    }
}
