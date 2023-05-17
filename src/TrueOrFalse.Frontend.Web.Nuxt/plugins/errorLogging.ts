import { CustomPino } from "~~/logs/logger"

export default defineNuxtPlugin((nuxtApp) => {
    nuxtApp.vueApp.config.errorHandler = (error, context) => {
        const config = useRuntimeConfig()
        const logger = new CustomPino(process.server ? config.seqServerApiKey : config.public.seqClientApiKey, config.public.seqServerUrl)

        logger.error(`NUXT ERROR`, [{ error: error, context: context }])
    }
})