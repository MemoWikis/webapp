
export default eventHandler((event) => {
    const config = useRuntimeConfig()
    return proxyRequest(event, `${config.seqRawUrl}`)
})