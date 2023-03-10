
// https://v3.nuxtjs.org/api/configuration/nuxt.config
export default defineNuxtConfig({

    runtimeConfig: {
        public: {
            clientBase: "http://memucho.local:3000",
            serverBase: "http://memucho.local",
            //Include .env file (is ignored by git) in root dir with variable "NUXT_PUBLIC_EXAMPLE_VAR=ExampleVarEnvOverwritten" to overwrite:
            exampleVar: "ExampleVarNuxtConfig"
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
})
