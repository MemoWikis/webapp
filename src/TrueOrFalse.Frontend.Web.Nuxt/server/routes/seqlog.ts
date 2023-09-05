
export default eventHandler((event: any) => {
    const config = useRuntimeConfig()
    return proxyRequest(event, `${config.seqRawUrl}`)
})