import { defineStore } from 'pinia'
export enum SolutionType
{
    Text = 1,
    MultipleChoice_SingleSolution = 3,
    Numeric = 4,
    Sequence = 5,
    Date = 6,
    MultipleChoice = 7,
    MatchList = 8,
    FlashCard = 9
}

export const useEditQuestionStore = defineStore('editQuestionStore', {
    state: () => {
      return {
        showModal: false,
        id: 0
      }
    },
    actions: {
        openModal(id: number){
            this.id = id
            this.showModal = true
        }
    },
  })