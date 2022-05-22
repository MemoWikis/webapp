import { defineNuxtConfig } from "nuxt";

// https://v3.nuxtjs.org/api/configuration/nuxt.config
export default defineNuxtConfig({
    typescript: {
        shim: false,
    },
    runtimeConfig: {
        public: {
            apiBase: "http://localhost:5211",
        },
    },
});
