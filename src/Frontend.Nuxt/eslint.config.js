import js from "@eslint/js";
import tseslint from "typescript-eslint";
import pluginVue from "eslint-plugin-vue";
import tsParser from "@typescript-eslint/parser";

export default [
    { ignores: ["*.less", "**/*.less"] },

    ...tseslint.config(js.configs.recommended, tseslint.configs.recommended),

    ...pluginVue.configs["flat/essential"],

    {
        files: ["**/*.{ts,tsx,vue,js}"],
        languageOptions: {
            parser: tsParser,
            parserOptions: { ecmaVersion: "latest", sourceType: "module" },
        },
        plugins: {
            vue: pluginVue,
        },
        rules: {
            "no-undef": "off",
            "no-unused-vars": "off",
            "vue/multi-word-component-names": "off",
            "vue/valid-v-for": "off",
            "vue/require-v-for-key": "off",
            "vue/no-use-v-if-with-v-for": "off",
            "@typescript-eslint/no-unused-vars": "off",
            "@typescript-eslint/no-explicit-any": "off",
            "@typescript-eslint/no-non-null-asserted-optional-chain": "off",
        },
    },
];
