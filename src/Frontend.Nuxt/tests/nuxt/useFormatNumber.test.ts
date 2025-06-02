import { describe, it, expect, beforeEach } from 'vitest'
import { getFormattedNumber } from '~/composables/useFormatNumber'

// Note: We're using the actual i18n functionality instead of mocking it
// This relies on the Nuxt i18n module's useI18n composable from #imports

describe('getgetFormattedNumber', () => {
    describe('English locale formatting', () => {
        beforeEach(async () => {
            const { $i18n } = useNuxtApp()
            await $i18n.setLocale('en')
        })

        it('should return the input if it is not a number', () => {
            expect(getFormattedNumber('not a number')).toBe('not a number')
            expect(getFormattedNumber('123')).toBe('123')
        })

        it('should format numbers less than 1000 using locale formatting', () => {
            expect(getFormattedNumber(0)).toBe('0')
            expect(getFormattedNumber(1)).toBe('1')
            expect(getFormattedNumber(999)).toBe('999')
        })

        it('should format numbers between 1000 and 999999 with translated thousands suffix', () => {
            // These should use the translation key 'counter.thousands' with value formatted using en-GB locale
            expect(getFormattedNumber(1000)).toBe('1k')
            expect(getFormattedNumber(1500)).toBe('1.5k')
            expect(getFormattedNumber(10000)).toBe('10k')
            expect(getFormattedNumber(999999)).toBe('999.9k')
        })

        it('should format numbers 1000000 and above with translated millions suffix', () => {
            // These should use the translation key 'counter.millions' with value formatted using en-GB locale
            expect(getFormattedNumber(1000000)).toBe('1M')
            expect(getFormattedNumber(1500000)).toBe('1.5M')
            expect(getFormattedNumber(10000000)).toBe('10M')
        })

        it('should not show decimal places if they are zero', () => {
            expect(getFormattedNumber(2000)).toBe('2k') // Not '2.0k'
            expect(getFormattedNumber(3000000)).toBe('3M') // Not '3.0M'
        })
    })

    describe('German locale formatting', () => {
        beforeEach(async () => {
            const { $i18n } = useNuxtApp()
            await $i18n.setLocale('de')
        })

        it('should format small numbers using German locale', () => {
            expect(getFormattedNumber(1234)).toBe('1234') // German uses . as thousands separator
        })

        it('should not apply thousands formatting for German locale', () => {
            // German locale skips thousands formatting due to locale.value !== 'de' condition
            expect(getFormattedNumber(5000)).toBe('5000')
            expect(getFormattedNumber(32500)).toBe('32.500') // German uses . as thousands separator
        })

        it('should format millions with German translation', () => {
            expect(getFormattedNumber(1000000)).toBe('1 Mio.')
            expect(getFormattedNumber(2500000)).toBe('2,5 Mio.') // German uses , as decimal separator
        })
    })

    describe('French locale formatting', () => {
        beforeEach(async () => {
            const { $i18n } = useNuxtApp()
            await $i18n.setLocale('fr')
        })

        it('should format thousands with French translation and locale', () => {
            expect(getFormattedNumber(1500)).toBe('1,5 k') // French uses , as decimal separator
            expect(getFormattedNumber(10000)).toBe('10 k')
        })

        it('should format millions with French translation and locale', () => {
            expect(getFormattedNumber(1500000)).toBe('1,5 M')
        })
    })

    describe('Spanish locale formatting', () => {
        beforeEach(async () => {
            const { $i18n } = useNuxtApp()
            await $i18n.setLocale('es')
        })

        it('should format thousands with Spanish translation and locale', () => {
            expect(getFormattedNumber(1500)).toBe('1,5 mil') // Spanish uses , as decimal separator
            expect(getFormattedNumber(10000)).toBe('10 mil')
        })

        it('should format millions with Spanish translation and locale', () => {
            expect(getFormattedNumber(1500000)).toBe('1,5M')
        })
    })
})
