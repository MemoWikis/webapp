export default defineNuxtRouteMiddleware(async (to, from) => {
    const headers = useRequestHeaders(['cookie']) as HeadersInit
    const { $config } = useNuxtApp()
    const isAdmin = await $fetch<boolean>('/apiVue/MiddlewareAuth/Get',
        {
            credentials: 'include',
            mode: 'cors',
            onRequest({ options }) {
                if (process.server) {
                    options.headers = headers
                    options.baseURL = $config.public.serverBase
                }
            },
        })
    console.log(isAdmin)
    if (isAdmin)
        return true
    else (!isAdmin)
    return false
    throw abortNavigation()
})