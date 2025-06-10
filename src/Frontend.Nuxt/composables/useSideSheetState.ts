import { useSideSheetStore } from '~/components/sideSheet/sideSheetStore'

/**
 * Composable for managing side sheet state with cookie persistence and mobile detection
 * Automatically handles the logic for showing/hiding the side sheet based on:
 * - Cookie persistence ('showSideSheet')
 * - Mobile device detection
 * - Side sheet store state changes
 *
 * @returns Object containing the reactive sideSheetOpen state
 */
export const useSideSheetState = () => {
    const sideSheetStore = useSideSheetStore()
    const { isMobile } = useDevice()

    // Get the cookie for side sheet state persistence
    const showSideSheetCookie = useCookie<boolean>('showSideSheet')

    // Reactive state for whether the side sheet is actually open (considering mobile)
    const sideSheetOpen = ref(false)

    // Initialize the state based on cookie and mobile detection
    sideSheetOpen.value = showSideSheetCookie.value && !isMobile

    // Watch for changes in the side sheet store state
    watch(
        () => sideSheetStore.showSideSheet,
        (newValue) => {
            sideSheetOpen.value = newValue && !isMobile
            // Update the cookie when the store state changes
            showSideSheetCookie.value = newValue
        }
    )

    return {
        sideSheetOpen: readonly(sideSheetOpen),
        sideSheetStore,
    }
}
