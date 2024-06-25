
export default defineNuxtRouteMiddleware(async (to, from) => {

    const headers = useRequestHeaders(['cookie']) as HeadersInit
    const { $config, $urlHelper } = useNuxtApp()
    interface Result {
        name: string
        id: number
    }
    const result = await $fetch<Result>('/apiVue/MiddlewareStartpage/Get',
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
    return navigateTo($urlHelper.getTopicUrl(result.name, result.id))

})