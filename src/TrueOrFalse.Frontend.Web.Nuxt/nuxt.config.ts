// https://v3.nuxtjs.org/api/configuration/nuxt.config
export default defineNuxtConfig({
    // nitro: {
    //     devProxy: {
    //         '/api': {
    //             target: "http://memucho.local/apiVue",
    //             changeOrigin: true
    //         }
    //     }
    //   },
    //   runtimeConfig: {
    //     public: {
    //         apiBase: "http://memucho.local/apiVue/",
    //         base: "http://memucho.local/",
    //         apiParams: {
    //             baseURL: process.server ? "http://memucho.local/apiVue" : "http://memucho.local:3000/api",
    //             credentials: 'include',
    //             mode: 'no-cors',
    //         }
    //     },
    // },
    ssr: true,
    modules: [
        '@pinia/nuxt',
    ],
    css: [
        '@fortawesome/fontawesome-svg-core/styles.css',
      ]
})
