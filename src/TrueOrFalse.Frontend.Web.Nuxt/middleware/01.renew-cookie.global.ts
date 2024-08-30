
export default defineNuxtRouteMiddleware(async (to, from) => {
    const nuxtApp = useNuxtApp()
    if (import.meta.client && nuxtApp.isHydrating && nuxtApp.payload.serverRendered) {

        interface RunResponse {
            success: boolean
            deleteCookie?: boolean
        }

        const result = await $api<RunResponse>('/apiVue/MiddlewareRefreshCookie/Run',
            {
                credentials: 'include',
                mode: 'cors',
                onResponseError(context) {
                    nuxtApp.$logger.warn('renew-cookie.global.ts: error', [{ response: context.response, host: context.request,  }])
                },
            })

        if (!result.success) {
            if (result.deleteCookie) {
                const persistentLoginCookie = useCookie('persistentLogin')
                nuxtApp.$logger.warn('renew-cookie.global.ts: invalidCookie', [{ locationHref: window.location.href, userAgent: window.navigator.userAgent, invalidCookie: persistentLoginCookie.value }])
                persistentLoginCookie.value = null
            }

            nuxtApp.$logger.warn('renew-cookie.global.ts: could not refresh cookie', [{ locationHref: window.location.href, userAgent: window.navigator.userAgent }])
        }
    }
})
