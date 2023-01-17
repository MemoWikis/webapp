import { defineStore } from 'pinia'
import { useUserStore } from '../../user/userStore'
enum Type {
  Create,
  Edit
}

export const useEditQuestionStore = defineStore('editQuestionStore', {
  state: () => {
    return {
      showModal: false,
      id: 0,
      type: null as Type | null,
      edit: false,
      sessionIndex: 0
    }
  },
  actions: {
    createQuestion(q: {
      topicId: number
      edit: boolean,
      questionHtml: string,
      solution: string,
    }) {
      var question = {
        topicId: q.topicId,
        editQuestion: false,
        questionHtml: q.questionHtml,
        solution: q.solution,
      }
    },
    openModal() {
      this.showModal = true
    },
    editQuestion(id: number, sessionIndex: number | null = null) {
      this.id = id
      this.edit = true
      if (sessionIndex != null)
        this.sessionIndex = sessionIndex
      this.openModal()
    },
    create() {
      const userStore = useUserStore()
      if (userStore.isLoggedIn) {
        this.edit = false
        this.openModal()
      } else {
        userStore.openLoginModal()
      }
    }
  },
})