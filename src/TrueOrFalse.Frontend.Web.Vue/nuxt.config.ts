import { defineNuxtConfig } from "nuxt";

// https://v3.nuxtjs.org/api/configuration/nuxt.config
export default defineNuxtConfig(
  {
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
        '@fortawesome/fontawesome-svg-core/styles.css',
        '~/assets/bootstrap/bootstrap.css',
        '~/assets/bootstrap/memucho_overrides.css',
        '~/assets/bootstrap/variables_custom.css',
        '~/assets/site.css'
      ],
    less: [
        // '~/assets/bootstrap/bootstrap.less',
        // '~/assets/bootstrap/memucho_overrides.less',
        // '~/assets/bootstrap/variables_custom.less',
      ]
  }
);
