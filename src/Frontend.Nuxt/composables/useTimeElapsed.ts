import { useI18n } from "#imports"

export const useTimeElapsed = () => {
    const { t } = useI18n()

    const getTimeElapsedAsText = (
        dateTime: Date | string | undefined
    ): string => {
        if (!dateTime) {
            return t("timeElapsed.notAvailable")
        }

        const dateTimeBegin = new Date(dateTime)
        const dateTimeEnd = new Date()

        const elapsedTime = dateTimeEnd.getTime() - dateTimeBegin.getTime()
        const elapsedSeconds = elapsedTime / 1000
        const elapsedMinutes = elapsedSeconds / 60
        const elapsedHours = elapsedMinutes / 60
        const elapsedDays = elapsedHours / 24

        const calDaysPassed = Math.floor(
            (dateTimeEnd.setHours(0, 0, 0, 0) -
                dateTimeBegin.setHours(0, 0, 0, 0)) /
                (24 * 60 * 60 * 1000)
        )

        if (elapsedSeconds < 60) {
            return t("timeElapsed.lessThanAMinute")
        }

        if (elapsedSeconds < 90) {
            return t("timeElapsed.oneMinute")
        }

        if (elapsedMinutes < 60) {
            return t("timeElapsed.minutes", {
                count: Math.round(elapsedMinutes),
            })
        }

        if (elapsedHours <= 24) {
            if (Math.round(elapsedHours) === 1) {
                return t("timeElapsed.oneHour")
            } else {
                return t("timeElapsed.hours", {
                    count: Math.round(elapsedHours),
                })
            }
        }

        if (elapsedDays < 30) {
            if (calDaysPassed <= 1) {
                return t("timeElapsed.oneDay")
            } else {
                return t("timeElapsed.days", { count: calDaysPassed })
            }
        }

        if (elapsedDays < 365) {
            const months = Math.round(elapsedDays / 30)
            if (months === 1) {
                return t("timeElapsed.oneMonth")
            } else {
                return t("timeElapsed.months", { count: months })
            }
        }

        if (elapsedDays < 365 * 1.5) {
            return t("timeElapsed.oneYear")
        }

        return t("timeElapsed.years", { count: Math.round(elapsedDays / 365) })
    }

    return {
        getTimeElapsedAsText,
    }
}
