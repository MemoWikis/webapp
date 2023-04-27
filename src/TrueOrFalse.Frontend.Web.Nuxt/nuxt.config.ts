
// https://v3.nuxtjs.org/api/configuration/nuxt.config
export default defineNuxtConfig({

    runtimeConfig: {
        public: {
            clientBase: "http://memucho.local:3000",
            serverBase: "http://memucho.local",
            gsiClientKey: "290065015753-gftdec8p1rl8v6ojlk4kr13l4ldpabc8.apps.googleusercontent.com",
            discord: "https://discord.com/invite/nXKwGrN",
            stripePlusPriceId: "price_1MqspiCAfoBJxQhotlUCv5Y4",
            stripeTeamPriceId: "",
            stripeKey: "pk_test_51MoR45CAfoBJxQhoJL2c0l4Z1Xghwfu7fgD67EGce4zLn8Nm5s1XN4XvDHOVMBIWIF7z2UOXYY0yoGNoF8eCMT6700yChY9qA2"
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
        '~/assets/includes/shared.less',
        '~/assets/memo-bundle.less',
        '~/assets/fonts/googleFonts.less',
        '~/assets/shared/dropdown.less',
        'vue-final-modal/style.css',
        '~/assets/vue-transitions.less',
        '~/assets/shared/pagination.less',
    ],
    typescript: {
        shim: false,
        typeCheck: true,
        // strict: false
    },
    build: {
        transpile: [
            '@fortawesome/fontawesome-svg-core',
            '@fortawesome/free-solid-svg-icons',
            '@fortawesome/free-regular-svg-icons',
            '@fortawesome/free-brands-svg-icons',
            '@fortawesome/vue-fontawesome',
            '@fortawesome',
            'underscore',
        ]
    },
    //Einkommentieren, wenn Sourcemaps im ProdBuild benötigt:
    // sourcemap: {
    //     client: false
    // },

    // devServer: {
    //     host: 'memucho.local'
    // }
    // hooks: {
    //     'pages:extend'(pages) {
    //         // add a route
    //         pages.push({
    //             file: '~/pages/maintenance.vue',
    //             name: 'maintenantePace',
    //             path: '/Maintenance',
    //         })

    //         async function auth() {
    //             const isAdmin = await $fetch<any>('/apiVue/AdminAuth/Get',
    //                 {
    //                     credentials: 'include',
    //                     mode: 'cors'
    //                 })

    //             if (isAdmin)
    //                 return
    //             else throw createError({ statusCode: 404, statusMessage: 'Seite nicht gefunden' })
    //         }

    //         auth()
    //     }
    // }
})
