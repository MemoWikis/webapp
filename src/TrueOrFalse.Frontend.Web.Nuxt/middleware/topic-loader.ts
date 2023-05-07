import { useUserStore } from "~/components/user/userStore"
import { Topic } from "~~/components/topic/topicStore"

export default defineNuxtRouteMiddleware(async (to, from) => {

    const config = useRuntimeConfig()
    const headers = useRequestHeaders(['cookie']) as HeadersInit
    console.log('middleware')
    const topic = await $fetch<Topic>(`/apiVue/TopicLoader/GetTopic/${to.params.id}`,
        {
            credentials: 'include',
            mode: 'cors',
            onRequest({ options }) {
                if (process.server) {
                    options.headers = headers
                    options.baseURL = config.public.serverBase
                }
            },
            onResponse(context) {
            },
            onResponseError(context) {
                throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
            }
        })

    useState<Topic>('topic', () => topic)
    return
})