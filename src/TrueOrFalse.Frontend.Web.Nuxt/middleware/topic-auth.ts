import { useUserStore } from "~/components/user/userStore"

export default defineNuxtRouteMiddleware((to, from) => {

    // const userStore = useUserStore()
    // if (!userStore.isLoggedIn) {
    //     throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
    // }
    // console.log(to.params)
    if (Object.keys(to.params).length == 0) {

        const userStore = useUserStore()
        if (userStore.isLoggedIn)
            return `/apiVue/Topic/GetTopic/${userStore.personalWiki?.Id}`
        else return `/apiVue/Topic/GetTopic/1`
    }

    return
    // const headers = useRequestHeaders(['cookie']) as HeadersInit
    // const { $config } = useNuxtApp()

    // const canView = await $fetch<boolean>('/apiVue/MiddlewareTopicAuth/Get',
    //     {
    //         credentials: 'include',
    //         mode: 'cors',
    //         onRequest({ options }) {
    //             if (process.server) {
    //                 options.headers = headers
    //                 options.baseURL = $config.public.serverBase
    //             }
    //         },
    //         onResponseError(context) {
    //             throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
    //         },
    //     })
    // if (canView)
    //     return true
    // else (!canView)
    // throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
})