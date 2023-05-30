import { CustomPino } from "~~/logs/logger"

export default defineNuxtPlugin(() => {
    const config = useRuntimeConfig()
    return {
        provide: {
            logger: new CustomPino(
                process.server ? config.seqServerApiKey : config.public.seqClientApiKey,
                config.public.seqServerPort ? "http://localhost:" + config.public.seqServerPort : config.public.seqServerUrl)
        }
    }
})