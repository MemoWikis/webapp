import { defineStore } from 'pinia'
import { useTopicStore } from '../topicStore'

export enum EditTopicRelationType {
    Create,
    Move,
    AddParent,
    AddChild,
    None,
    AddToWiki
};

export type EditRelationParentData = {
  id: number, 
  addCategoryBtnId: string,
  editCategoryRelation: EditTopicRelationType,
  categoriesToFilter: number[]
}

export const useEditTopicRelationStore = defineStore('editTopicRelationStore', {
    state: () => {
      return {
        showModal: false,
        type: null as EditTopicRelationType,
        parentId: 0,
        redirect: false,
        addTopicBtnId: '',
        categoriesToFilter: [] as number[]
      }
    },
    actions: {
        openModal(parent: EditRelationParentData) {

            this.parentId = parent.id
            this.addTopicBtnId = parent.addCategoryBtnId
            this.type = parent.editCategoryRelation
            this.categoriesToFilter = parent.categoriesToFilter
            this.showModal = true
        },
        createTopic() {
          const topicStore = useTopicStore()
          this.parentId = topicStore.id
          this.type = EditTopicRelationType.Create
          this.redirect = true
          this.showModal = true
        }
    },
  })