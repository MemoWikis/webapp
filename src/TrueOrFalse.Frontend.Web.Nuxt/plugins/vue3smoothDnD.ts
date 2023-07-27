import { Container, Draggable } from 'vue3-smooth-dnd'

export default defineNuxtPlugin(({ vueApp }) => {
    vueApp.component('Container', Container)
    vueApp.component('Draggable', Draggable)
})