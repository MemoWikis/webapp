import { createVfm, VueFinalModal } from 'vue-final-modal'
export default defineNuxtPlugin(({ vueApp }) => {
    const vfm = createVfm() as any

    vueApp.use(vfm)
    vueApp.component('VueFinalModal', VueFinalModal)
})