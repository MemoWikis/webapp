
// https://v3.nuxtjs.org/api/configuration/nuxt.config
export default defineNuxtConfig({
    nitro: {
        preset: 'node-cluster'
    },
    runtimeConfig: {
        seqServerApiKey: "",
        seqRawUrl: "http://localhost:5341/api/events/raw",
        public: {
            clientBase: "http://localhost:3000",
            serverBase: "http://localhost",
            gsiClientKey: "290065015753-gftdec8p1rl8v6ojlk4kr13l4ldpabc8.apps.googleusercontent.com",
            discord: "https://discord.com/invite/nXKwGrN",
            stripePlusPriceId: "price_1MqspiCAfoBJxQhotlUCv5Y4",
            stripeTeamPriceId: "",
            stripeKey: "pk_test_51MoR45CAfoBJxQhoJL2c0l4Z1Xghwfu7fgD67EGce4zLn8Nm5s1XN4XvDHOVMBIWIF7z2UOXYY0yoGNoF8eCMT6700yChY9qA2",
            seqServerPort: undefined,
            seqServerUrl: undefined,
            seqClientApiKey: "",
            showTestLogButton: false
        },
    },
    ssr: true,
    modules: [
        '@pinia/nuxt',
        '@nuxtjs/device',
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
    components: [
        {
            path: '~/components',
            extensions: ['.vue'],
        }
    ],
    // Einkommentieren, wenn Sourcemaps im ProdBuild benötigt:
    // sourcemap: {
    //     server: true,
    //     client: true
    // },
    // debug: true,


    // devServer: {
    //     host: 'memucho.local'
    // }
    // devServer: {
    //     https: {
    //         key: 'localhost-key.pem',
    //         cert: 'localhost.pem'
    //     }
    // },

})
