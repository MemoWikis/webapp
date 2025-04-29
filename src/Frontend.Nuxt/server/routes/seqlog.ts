
export default defineEventHandler((event: any) => {
    const config = useRuntimeConfig()
    return proxyRequest(event, `${config.seqRawUrl}`)
})