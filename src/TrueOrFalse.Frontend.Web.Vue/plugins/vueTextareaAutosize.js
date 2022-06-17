import TextareaAutosize from 'vue-textarea-autosize'

export default defineNuxtPlugin((nuxtApp) => {
    nuxtApp.vueApp.use(TextareaAutosize)
  })