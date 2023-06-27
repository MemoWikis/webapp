
export default eventHandler((event) => {
    const config = useRuntimeConfig()
    return proxyRequest(event, `${config.public.serverBase}/${event.node.req.url}`)
})