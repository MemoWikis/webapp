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
        type: null as Type,
        edit: false
      }
    },
    actions: {
      createQuestion(q) {
        var question = {
          topicId: q.topicId,
          edit: false,
          questionHtml: q.questionHtml,
          solution: q.solution,
        }
      },
      openModal(id: Number){
          this.id = id
      },
      edit(id: Number, sessionIndex: Number = null) {
        this.edit = true
        if (sessionIndex != null)
          this.sessionIndex = sessionIndex
        this.openModal(id)
      }
    },
  })