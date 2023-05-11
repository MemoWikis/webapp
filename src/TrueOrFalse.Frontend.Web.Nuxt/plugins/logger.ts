import { CustomPino } from "~~/logs/logger"

export default defineNuxtPlugin(() => {
    const config = useRuntimeConfig()
    return {
        provide: {
            logger: new CustomPino(config.public.seqApiKey, config.public.seqServerUrl)
        }
    }
})