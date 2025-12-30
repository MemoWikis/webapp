// https://v3.nuxtjs.org/api/configuration/nuxt.config

export default defineNuxtConfig({
    devtools: { enabled: true },
    nitro: {
        compatibilityDate: '2025-05-08',
        preset: 'node-cluster',
        devProxy: {
            '/localCollab': {
                target: `http://localhost:3010`,
                ws: true,
            },
        },
    },
    runtimeConfig: {
        seqServerApiKey: '',
        seqRawUrl: 'http://localhost:5341/api/events/raw',
        sessionStartGuid: '',
        public: {
            clientBase: 'http://localhost:3000',
            serverBase: 'http://localhost:5069',
            officialBase: 'https://memowikis.net',
            gsiClientKey: '',
            discord: '',
            stripePlusPriceId: '',
            stripeTeamPriceId: '',
            stripeKey: '',
            seqClientApiKey: '',
            facebookAppId: '',
            environment: '',
            hocuspocusWebsocketUrl: 'ws://localhost:3000/localCollab',
            teamEmail: '',
        },
    },
    watchers: {
        chokidar: {
            usePolling: false,
            interval: 1000,
            ignored: [
                '**/node_modules/**',
                '**/.git/**',
                '**/.nuxt/**',
                '**/.output/**',
            ],
        },
    },
    ssr: true,
    modules: [
        '@pinia/nuxt',
        '@nuxtjs/device',
        '@nuxt/eslint',
        'nuxt-snackbar',
        '@nuxtjs/i18n',
        '~/server/modules/ws-proxy',
        '@nuxt/test-utils/module',
        'vue3-perfect-scrollbar/nuxt',
    ],
    eslint: {
        // options here
    },
    css: [
        '@fortawesome/fontawesome-svg-core/styles.css',
        '~/assets/bootstrap/bootstrap.less',
        // '~/assets/bootstrap/memoWikis_overrides.less',
        '~/assets/bootstrap/variables_custom.less',
        '~/assets/includes/shared.less',
        '~/assets/memo-bundle.less',
        '~/assets/fonts/googleFonts.less',
        '~/assets/shared/dropdown.less',
        'vue-final-modal/style.css',
        '~/assets/vue-transitions.less',
        '~/assets/shared/pagination.less',
        'vue-circle-flags/dist/vue-circle-flags.css',
        '~/components/shared/figure-extension.less',
    ],
    typescript: {
        shim: false,
        typeCheck: true,
        // strict: false
        tsConfig: {
            compilerOptions: {
                verbatimModuleSyntax: false,
            },
            exclude: ['**/*.less'],
        },
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
        ],
    },
    components: [
        {
            path: '~/components',
            extensions: ['.vue'],
        },
    ],
    snackbar: {
        bottom: true,
        duration: 5000,
        groups: false,
        success: '#AFD534',
        error: '#FF001F',
        warning: '#FDD648',
        info: '#555555',
    },
    router: {
        options: {
            scrollBehaviorType: 'smooth',
        },
    },
    app: {
        head: {
            link: [
                { rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' },
                {
                    rel: 'apple-touch-icon',
                    sizes: '180x180',
                    href: '/apple-touch-icon.png',
                },
                {
                    rel: 'icon',
                    type: 'image/png',
                    sizes: '32x32',
                    href: '/favicon-32x32.png',
                },
                {
                    rel: 'icon',
                    type: 'image/png',
                    sizes: '16x16',
                    href: '/favicon-16x16.png',
                },
            ],
        },
    },
    i18n: {
        locales: [
            {
                name: 'Deutsch',
                code: 'de',
                iso: 'de-DE',
                file: 'de.json',
                flag: 'de',
            },
            {
                name: 'English',
                code: 'en',
                iso: 'en-GB',
                file: 'en.json',
                flag: 'gb',
            },
            {
                name: 'Français',
                code: 'fr',
                iso: 'fr-FR',
                file: 'fr.json',
                flag: 'fr',
            },
            {
                name: 'Español',
                code: 'es',
                iso: 'es-ES',
                file: 'es.json',
                flag: 'es',
            },
        ],
        defaultLocale: 'en',
        strategy: 'no_prefix',
        compilation: {
            strictMessage: false,
            escapeHtml: false,
        },
        bundle: {
            optimizeTranslationDirective: false,
        },
    },
    // uncomment if you need sourcemaps in prod builds:
    // sourcemap: {
    //     server: true,
    //     client: true
    // },
    // debug: true,

    // devServer: {
    //     host: 'localhost:5069'
    // }
    // devServer: {
    //     https: {
    //         key: 'localhost-key.pem',
    //         cert: 'localhost.pem'
    //     }
    // },
})
