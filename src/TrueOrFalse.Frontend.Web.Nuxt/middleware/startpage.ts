
export default defineNuxtRouteMiddleware(async (to, from) => {

    const headers = useRequestHeaders(['cookie']) as HeadersInit
    interface Result {
        name: string
        id: number
    }
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
    return navigateTo($urlHelper.getTopicUrl(result.name, result.id))

})