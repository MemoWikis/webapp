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
            const result = await $api<FetchResult<PinData>>(`/apiVue/QuestionPinStore/Pin/${id}`, { method: 'POST', mode: 'cors', credentials: 'include' })
            result.data = { id: id, state: result.success ? PinState.Added : PinState.NotAdded }
            return result;
        },

        async unpin(id: number): Promise<FetchResult<PinData>> {
            const result = await $api<FetchResult<PinData>>(`/apiVue/QuestionPinStore/UnPin/${id}`, { method: 'POST', mode: 'cors', credentials: 'include' })
            result.data = { id: id, state: result.success ? PinState.NotAdded : PinState.Added }
            return result
        }
    }
})