import { defineStore } from 'pinia'

export enum PinAction {
    Add,
    Remove
}
export const usePinStore = defineStore('pinStore', {
    state: () => {
            return {
                id: 0,
            action: null as unknown as PinAction
        }
      },
      actions: {
        update(id:number, action:PinAction){
            this.id = id
            this.action = action
        },
      },
})