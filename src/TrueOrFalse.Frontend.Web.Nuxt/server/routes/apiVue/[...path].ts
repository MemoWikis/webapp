import { CustomPino } from "~~/logs/logger"

// proxy for client requests, use client keys
export default eventHandler((event) => {
    const config = useRuntimeConfig()

    const log = new CustomPino(config.public.seqClientApiKey, config.public.serverBase + ':' + config.public.seqServerPort)
    log.info(`REQUEST ApiProxy: ${event.context.params!.path}`, [{ url: event.node.req.url }])

    return proxyRequest(event, `${config.public.serverBase}/${event.node.req.url}`)
})