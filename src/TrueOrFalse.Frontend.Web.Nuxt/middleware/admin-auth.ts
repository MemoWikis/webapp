import { useUserStore } from "~/components/user/userStore"

export default defineNuxtRouteMiddleware(async (to, from) => {
    if (process.client) {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
        }
    }

    const headers = useRequestHeaders(['cookie']) as HeadersInit
    const { $config } = useNuxtApp()

    const isAdmin = await $api<boolean>('/apiVue/MiddlewareAuth/Get',
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
    if (isAdmin)
        return true
    else (!isAdmin)
        throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
})