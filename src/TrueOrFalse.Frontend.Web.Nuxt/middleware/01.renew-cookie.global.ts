
export default defineNuxtRouteMiddleware(async (to, from) => {
    const nuxtApp = useNuxtApp()
    if (import.meta.client && nuxtApp.isHydrating && nuxtApp.payload.serverRendered) {
        
        await $api<void>('/apiVue/MiddlewareRefreshCookie/Get',
            {
                credentials: 'include',
                mode: 'cors',
                onResponseError(context) {
                    nuxtApp.$logger.error('renew-cookie.global.ts: error', [{ response: context.response, host: context.request }])
                },
            })
    }
})
