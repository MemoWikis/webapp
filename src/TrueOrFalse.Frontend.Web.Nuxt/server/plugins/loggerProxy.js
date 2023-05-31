import httpProxy from "http-proxy" // make sure to use package redirect to "http-proxy-node16" for fixing closing event: https://github.com/http-party/node-http-proxy/pull/1559
import { CustomPino } from "~~/logs/logger"

const loggerProxy = httpProxy.createProxyServer({
	changeOrigin: true, // don't forget this, or you're going to chase your tail for hours
	target: useRuntimeConfig().public.seqServerUrl,
})

export default defineNitroPlugin(nitroApp => {
	nitroApp.h3App.stack.unshift({
		route: "/logger ",
		handler: fromNodeMiddleware((req, res, _) => {
			const config = useRuntimeConfig()
			const log = new CustomPino(process.server ? config.seqServerApiKey : config.public.seqClientApiKey, config.public.seqServerUrl)

			log.info(`LoggerProxy: ${req.url}`, [{ url: req.url }, { headers: req.headers }])
			loggerProxy.web(req, res)
		})
	})
})