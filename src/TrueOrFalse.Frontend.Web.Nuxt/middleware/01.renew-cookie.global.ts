
export default defineNuxtRouteMiddleware(async (to, from) => {

    const nuxtApp = useNuxtApp()
    if (import.meta.client && nuxtApp.isHydrating && nuxtApp.payload.serverRendered) {
        nuxtApp.$logger.debug('timing-test: renew-cookie.global.ts: Skipping renew-cookie middleware because of server-rendering')

        interface RefreshCookieResponse {
            success: boolean
            loginGuid?: string
            expiryDate?: string
        }

        const response = await $api<RefreshCookieResponse>('/apiVue/MiddlewareRefreshCookie/Get',
            {
                credentials: 'include',
                mode: 'cors',
                onResponseError(context) {
                    nuxtApp.$logger.debug('renew-cookie.global.ts: error', [{ response: context.response, host: context.request }])
                },
            })
        if (response.success) { 
            const loginGuid = response.loginGuid
            const expiryDate = response.expiryDate
            nuxtApp.$logger.debug('renew-cookie.global.ts: guid', [{ guid: loginGuid }])

            if (loginGuid && expiryDate) {
                useCookie('persistentLogin', {
                    expires: new Date(expiryDate),
                    sameSite: 'lax',
                    secure: nuxtApp.$config.public.environment != 'development',
                    httpOnly: true
                }).value = loginGuid

            refreshCookie('persistentLogin')    
            }
        }
    }

})