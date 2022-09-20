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
      createQuestion(q) {
        var question = {
          topicId: q.topicId,
          edit: q.edit,
          questionHtml: q.questionHtml,
          solution: q.solution,
        }
      },
      openModal(id: number){
          this.id = id
          this.showModal = true
      },
      edit(id: number) {
        this.openModal(id)
      }
    },
  })