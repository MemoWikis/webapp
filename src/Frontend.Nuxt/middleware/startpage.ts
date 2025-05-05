export default defineNuxtRouteMiddleware(async (to, from) => {
    const headers = useRequestHeaders(["cookie"]) as HeadersInit
    interface Result {
        name: string
        id: number
    }

    const { $config, $urlHelper } = useNuxtApp()
    const result = await $api<Result>("/apiVue/MiddlewareStartpage/Get", {
        credentials: "include",
        mode: "cors",
        onRequest({ options }) {
            if (import.meta.server) {
                options.headers = new Headers(headers)
                options.baseURL = $config.public.serverBase
            }
        },
        onResponseError(context) {
            throw createError({ statusCode: 404, statusMessage: "Not Found" })
        },
    })
    return navigateTo($urlHelper.getPageUrl(result.name, result.id))
})
