import Vue3TouchEvents, {
    type Vue3TouchEventsOptions,
  } from "vue3-touch-events"
  
export default defineNuxtPlugin(({ vueApp }) => {

    vueApp.use<Vue3TouchEventsOptions>(Vue3TouchEvents, {
        disableClick: false
        // any other global options...
      })
})