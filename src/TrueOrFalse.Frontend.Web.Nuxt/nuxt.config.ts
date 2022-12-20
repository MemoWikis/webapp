
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
    runtimeConfig: {
        public: {
            clientBase: "http://memucho.local:3000",
            serverBase: "http://memucho.local",
        },
    },
    ssr: true,
    modules: [
        '@pinia/nuxt',
        '@nuxtjs/device'
    ],
    css: [
        '@fortawesome/fontawesome-svg-core/styles.css',
        '~/assets/bootstrap/bootstrap.less',
        // '~/assets/bootstrap/memucho_overrides.less',
        '~/assets/bootstrap/variables_custom.less',
        '~/assets/site.less',
        '~/assets/top-header.less',
        '~/assets/fonts/googleFonts.less',
        '~/assets/shared/dropdown.less'
    ],
    typescript: {
        shim: false,
        typeCheck: true,
        // strict: false
    },
    build: {
        transpile: [
            '@fortawesome'
        ]
    },
    devServer: {
        host: 'memucho.local'
    }
})
