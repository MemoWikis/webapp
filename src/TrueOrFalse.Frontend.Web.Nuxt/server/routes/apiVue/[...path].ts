import { CustomPino } from "~~/logs/logger"

export default eventHandler((event: any) => {
    const config = useRuntimeConfig()

    const log = new CustomPino()
    log.info(`REQUEST ApiProxy: ${event.context.params!.path}`, [{ url: event.node.req.url }])
    console.log("Proxy log", `${config.public.serverBase}${event.node.req.url}`)
    return proxyRequest(event, `${config.public.serverBase}${event.node.req.url}`, {
        async onResponse(e, r) {
            if (!r.ok) {

                log.error(`REQUEST ApiProxy - failed: ${event.context.params!.path}`, [{ url: event.node.req.url, content: e.node.res.statusMessage, status: r.status }])
            }
        }
    })
})
