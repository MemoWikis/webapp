import { useUserStore } from "~~/components/user/userStore"

export default defineNuxtRouteMiddleware(async () => {
    const headers = useRequestHeaders(['cookie']) as HeadersInit
    const { $config } = useNuxtApp()
    console.log('hello')
    const userStore = useUserStore()
    return userStore.isAdmin
    // return await $fetch<any>('/apiVue/AdminAuth/Get',
    //     {
    //         credentials: 'include',
    //         mode: 'cors',
    //         onRequest({ options }) {
    //             if (process.server) {
    //                 options.headers = headers
    //                 options.baseURL = $config.public.serverBase
    //             }
    //         },
    //     })
})