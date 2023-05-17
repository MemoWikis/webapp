
export default defineNuxtRouteMiddleware(async (to, from) => {

    const headers = useRequestHeaders(['cookie']) as HeadersInit
    const { $config } = useNuxtApp()
    interface Result {
        encodedName: string
        id: number
    }
    const result = await $fetch<Result>('/apiVue/MiddlewareStartpage/Get',
        {
            credentials: 'include',
            mode: 'cors',
            onRequest({ options }) {
                if (process.server) {
                    options.headers = headers
                    options.baseURL = $config.public.serverBase
                }
            },
            onResponseError(context) {
                throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
            },
        })
    return navigateTo(`/${result.encodedName}/${result.id}`)

})