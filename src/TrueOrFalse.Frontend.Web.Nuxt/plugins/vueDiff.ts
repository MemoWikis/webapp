import VueDiff from 'vue-diff'
import 'vue-diff/dist/index.css'

// Register the package
export default defineNuxtPlugin((nuxtApp) => {
    nuxtApp.vueApp.use(VueDiff)
})