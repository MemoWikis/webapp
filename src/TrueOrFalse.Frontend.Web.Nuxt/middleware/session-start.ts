
export default defineNuxtRouteMiddleware(async (to, from) => {
    console.log('sessionStart')
    const hasRun = useState<boolean>('middlewareRun', () => false)
    // if (hasRun.value)
    //     navigateTo(to.path, { replace: false })

    if (process.client) {
        // const userStore = useUserStore()
        // if (!userStore.isLoggedIn) {
        //     throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
        // }
    }
    

    const headers = useRequestHeaders(['cookie']) as HeadersInit
    const { $config } = useNuxtApp()

    await $fetch<boolean>('/apiVue/App/SessionStart',
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
    hasRun.value = true
    // navigateTo(to.path, { replace: false })
})