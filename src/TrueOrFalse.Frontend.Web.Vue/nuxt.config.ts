import { defineNuxtConfig } from "nuxt";

// https://v3.nuxtjs.org/api/configuration/nuxt.config
export default defineNuxtConfig(
  {
    vite: {
      server: {
        proxy: {
          "/api": {
            target: "http://memucho.local/apiVue",
            changeOrigin: true,
            rewrite: (path) => path.replace(/^\/api/, ""),
          },
        },
      },
    },
    buildModules: [
        '@pinia/nuxt',
        'floating-vue/nuxt',
      ],
    typescript: {
        shim: false,
        typeCheck: true
    },
    runtimeConfig: {
        public: {
            apiBase: "http://memucho.local/apiVue/",
            base: "http://memucho.local/"
        },
    },
    components: {
        global: true,
        dirs: ['~/components'],
      },
    css: [
        '@fortawesome/fontawesome-svg-core/styles.css',
        '~/assets/bootstrap/bootstrap.less',
        '~/assets/bootstrap/memucho_overrides.css',
        '~/assets/bootstrap/variables_custom.less',
        '~/assets/site.less',
        '~/assets/top-header.less'   
      ]
  }
);