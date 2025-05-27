import { describe, it, expect } from 'vitest'
import { useFormatNumber } from '~/composables/useFormatNumber'

// Note: We're using the actual i18n functionality instead of mocking it
// This relies on the Nuxt i18n module's useI18n composable from #imports

describe('useFormatNumber', () => {
    describe('formattedNumber', () => {
        const { formattedNumber } = useFormatNumber()

        it('should return the input if it is not a number', () => {
            expect(formattedNumber('not a number')).toBe('not a number')
            expect(formattedNumber('123')).toBe('123')
        })

        it('should format numbers less than 1000 as they are', () => {
            expect(formattedNumber(0)).toBe('0')
            expect(formattedNumber(1)).toBe('1')
            expect(formattedNumber(999)).toBe('999')
        })

        it('should format numbers between 1000 and 999999 with K suffix', () => {
            expect(formattedNumber(1000)).toBe('1k')
            expect(formattedNumber(1500)).toBe('1.5k')
            expect(formattedNumber(10000)).toBe('10k')
            expect(formattedNumber(999999)).toBe('999.9k')
        })

        it('should format numbers 1000000 and above with M suffix', () => {
            expect(formattedNumber(1000000)).toBe('1M')
            expect(formattedNumber(1500000)).toBe('1.5M')
            expect(formattedNumber(10000000)).toBe('10M')
        })

        it('should not show decimal places if they are zero', () => {
            expect(formattedNumber(2000)).toBe('2k') // Not '2.0k'
            expect(formattedNumber(3000000)).toBe('3M') // Not '3.0M'
        })
    })
})
