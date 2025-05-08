import { defineNuxtModule } from "@nuxt/kit";
import { createProxyServer } from "httpxy";

// Custom nuxt module for adding websocket proxy support locally. The official support does not work yet,
// see https://github.com/nuxt/cli/issues/107 and https://github.com/nuxt/cli/issues/108.
export default defineNuxtModule({
    setup(_, nuxt) {
        if (!nuxt.options.dev) return;

        // Take rules from the nitro devProxy configuration that have the ws flag set.
        const wsProxyRules = Object.entries(
            nuxt.options.nitro?.devProxy ?? {}
        ).filter(([, rule]) => typeof rule === "object" && rule.ws);

        // We have no rules with websocket proxying, return early.
        if (!wsProxyRules) return;

        // Create proxy servers for each rule.
        const proxies = wsProxyRules.map(([key, rule]) => {
            return [key, createProxyServer(rule)];
        });

        nuxt.hook("ready", () => {
            // Replace the nuxt server with our own version that uses the configured proxy servers or delegates.
            nuxt.server = {
                ...nuxt.server,
                upgrade: (req, socket, head) => {
                    const proxy = proxies.find(([key]) =>
                        req.url.startsWith(key)
                    )?.[1];

                    if (proxy) {
                        return proxy.ws(req, socket, {}, head);
                    } else {
                        return nuxt.server.upgrade(req, socket, head);
                    }
                },
            };
        });
    },
});
