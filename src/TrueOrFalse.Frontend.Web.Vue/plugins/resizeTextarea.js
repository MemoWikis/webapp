import ResizeTextarea from 'resize-textarea-vue3'

export default defineNuxtPlugin((nuxtApp) => {
    nuxtApp.vueApp.use(ResizeTextarea)
  })
