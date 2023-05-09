import { Topic } from "~~/components/topic/topicStore"
// import { logger } from "~/logger/logger"

export default defineNuxtRouteMiddleware(async (to, from) => {

    return
    const config = useRuntimeConfig()
    const headers = useRequestHeaders(['cookie']) as HeadersInit
    // if (process.client)
    //     logger.info("nuxt = middleware:client", { date: new Date().getTime() })
    // else
    //     logger.info("nuxt = middleware:server {date}", { date: new Date().getTime() })

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
                // logger.info("nuxt = middleware:response", { date: new Date().getTime() })

            },
            onResponseError(context) {
                // logger.info("nuxt = middleware:responseError", { date: new Date().getTime() })
                throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })

            }
        })
    // logger.info("nuxt = middleware:afterFetch", { date: new Date().getTime() })
    useState<Topic>('topic', () => topic)
    return
})