import { CustomPino } from "~~/logs/logger"

export default defineNuxtPlugin(() => {
    const config = useRuntimeConfig()
    return {
        provide: {
            logger: new CustomPino(process.server ? config.seqServerApiKey : config.public.seqClientApiKey, process.server ? config.public.seqServerUrl : '/logger')
        }
    }
})