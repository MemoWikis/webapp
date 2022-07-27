import { defineStore } from 'pinia'

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
        parent: null as any
      }
    },
    actions: {
        openModal(parent) {
            this.parent = parent
            this.showModal = true
        }
    },
  })