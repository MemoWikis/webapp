import { useUserStore } from "~/components/user/userStore"

export default defineNuxtRouteMiddleware(async (to, from) => {

    if (process.client) {
        const userStore = useUserStore()
        if (userStore.isLoggedIn) {

        }
    }

    const headers = useRequestHeaders(['cookie']) as HeadersInit
    const { $config } = useNuxtApp()

    const canView = await $fetch<boolean>('/apiVue/MiddlewareTopicAuth/Get',
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
    if (canView)
        return true
    else (!canView)
    throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
})