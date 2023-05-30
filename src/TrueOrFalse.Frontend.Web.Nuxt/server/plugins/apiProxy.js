import httpProxy from "http-proxy" // make sure to use package redirect to "http-proxy-node16" for fixing closing event: https://github.com/http-party/node-http-proxy/pull/1559
import { CustomPino } from "~~/logs/logger"

const apiProxy = httpProxy.createProxyServer({
  changeOrigin: true, // don't forget this, or you're going to chase your tail for hours
  target: useRuntimeConfig().public.serverBase + "/apiVue/",
})

export default defineNitroPlugin(nitroApp => {
  nitroApp.h3App.stack.unshift({
    route: "/apiVue/",
    handler: fromNodeMiddleware((req, res, _) => {
      const config = useRuntimeConfig()
      const log = new CustomPino(process.server ? config.seqServerApiKey : config.public.seqClientApiKey, config.public.seqServerPort ? "http://localhost:" + config.public.seqServerPort : config.public.seqServerUrl)

      log.info(`REQUEST ApiProxy: ${req.url}`, [{ url: req.url }, { headers: req.headers }])
      apiProxy.web(req, res)
    })
  })
})