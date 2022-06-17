import { defineNuxtConfig } from "nuxt";

// https://v3.nuxtjs.org/api/configuration/nuxt.config
export default defineNuxtConfig({
    buildModules: [
        '@pinia/nuxt',
      ],
    typescript: {
        shim: false,
    },
    runtimeConfig: {
        public: {
            apiBase: "http://localhost:5211/apiVue/",
            base: "http://localhost:5211"
        },
    },
    components: {
        global: true,
        dirs: ['~/components'],
      },
      css: [
        '@fortawesome/fontawesome-svg-core/styles.css'
      ]
});
