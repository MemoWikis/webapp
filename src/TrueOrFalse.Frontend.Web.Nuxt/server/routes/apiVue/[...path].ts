import { CustomPino } from "~~/logs/logger"

export default eventHandler((event) => {
    const config = useRuntimeConfig()

    const log = new CustomPino()
    log.info(`REQUEST ApiProxy: ${event.context.params!.path}`, [{ url: event.node.req.url }])

    return proxyRequest(event, `${config.public.serverBase}/${event.node.req.url}`)
})