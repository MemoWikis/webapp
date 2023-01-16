import { defineStore } from "pinia"
export enum PinState {
    Added,
    Loading,
    NotAdded
}
export enum PinType {

}
export const usePinStore = defineStore('pinStore', {
    actions: {
        async pin(id: number) {
            const data = {
                id: id
            }
            const result = await $fetch<boolean>('/apiVue/QuestionPinStore/Pin', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
            if (result)
                return { id: id, state: PinState.Added }
            else return null
        },

        async unpin(id: number) {
            const data = {
                id: id
            }
            const result = await $fetch<boolean>('/apiVue/QuestionPinStore/UnPin', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
            if (result)
                return { id: id, state: PinState.NotAdded }
            else return null
        }
    }
})