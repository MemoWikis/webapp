
export default defineNuxtRouteMiddleware(async (to, from) => {

    const headers = useRequestHeaders(['cookie']) as HeadersInit
    interface Result {
        name?: string
        id?: number
        isLoggedIn: boolean
    }

    const {$config, $urlHelper} = useNuxtApp()
    const result = await $api<Result>('/apiVue/MiddlewareStartpage/Get',
        {
            credentials: 'include',
            mode: 'cors',
            onRequest({ options }) {
                if (import.meta.server) {
                    options.headers = headers
                    options.baseURL = $config.public.serverBase
                }
            },
            onResponseError(context) {
                throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
            },
        })

    if (result.isLoggedIn && result.name && result.id)
        return navigateTo($urlHelper.getPageUrl(result.name, result.id))
    return
})