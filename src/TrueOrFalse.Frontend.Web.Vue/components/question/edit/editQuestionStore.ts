import { defineStore } from 'pinia'
enum Type {
  Create,
  Edit
}

export const useEditQuestionStore = defineStore('editQuestionStore', {
    state: () => {
      return {
        showModal: false,
        id: 0,
        type: null as Type
      }
    },
    actions: {
        openModal(id: number){
            this.id = id
            this.showModal = true
        },
        edit(id: number) {
          this.openModal(id)
        }
    },
  })