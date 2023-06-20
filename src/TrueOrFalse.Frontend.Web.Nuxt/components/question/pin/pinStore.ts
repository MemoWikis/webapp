import { defineStore } from "pinia"
export enum PinState {
    Added,
    Loading,
    NotAdded
}
export enum PinType {

}
export interface PinData {
    id: number,
    state: PinState,
} 

export const usePinStore = defineStore('pinStore', {
    actions: {
        async pin(id: number): Promise<FetchResult<PinData>> {
            const data = {
                id: id
            }
            const result =  await $fetch<FetchResult<PinData>>('/apiVue/QuestionPinStore/Pin', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
            if (result.success) {
                result.data = { id: id, state: PinState.Added}
            }
            return result;
        },

        async unpin(id: number): Promise<FetchResult<PinData>> {
            const data = {
                id: id
            }
            const result = await $fetch<FetchResult<PinData>>('/apiVue/QuestionPinStore/UnPin', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
            if (result.success){
                result.data = { id: id, state: PinState.NotAdded}
            }
            return result
        }
    }
})