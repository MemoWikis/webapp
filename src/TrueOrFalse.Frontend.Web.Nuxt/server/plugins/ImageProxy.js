import httpProxy from "http-proxy" // make sure to use package redirect to "http-proxy-node16" for fixing closing event: https://github.com/http-party/node-http-proxy/pull/1559

const apiProxy = httpProxy.createProxyServer({
  changeOrigin: true, // don't forget this, or you're going to chase your tail for hours
  target: "https://stage.memucho.de/Images/",
})

export default defineNitroPlugin(nitroApp => {
  nitroApp.h3App.stack.unshift({
    route: "/Images/",
    handler: fromNodeMiddleware((req, res, _) => apiProxy.web(req, res))
  })
})