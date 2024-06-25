import CodeDiff from 'v-code-diff'

export default defineNuxtPlugin((nuxtApp) => {
    nuxtApp.vueApp.use(CodeDiff)
})