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


export const useEditTopicRelationStore = defineStore('topicStore', {
    state: () => {
      return {
        showModal: false,
        type: null as EditTopicRelationType,
        parentId: 0,
        redirect: false,
      }
    },
    actions: {
        openModal(parentId) {
            this.parentId = parentId
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