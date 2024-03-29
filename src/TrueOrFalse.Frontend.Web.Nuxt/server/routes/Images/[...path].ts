
export default eventHandler((event: any) => {
    const config = useRuntimeConfig()
    return proxyRequest(event, `${config.public.serverBase}${event.node.req.url}`)
})