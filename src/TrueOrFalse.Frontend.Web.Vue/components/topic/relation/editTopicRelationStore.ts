import { defineStore } from 'pinia'
import { SearchTopicItem } from '~~/components/search/searchHelper'
import { useTopicStore } from '../topicStore'

export enum EditTopicRelationType {
    Create,
    Move,
    AddParent,
    AddChild,
    None,
    AddToWiki
}

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

            if (parent.editCategoryRelation == EditTopicRelationType.AddToWiki)
              this.initWikiData()
        },
        createTopic() {
          const topicStore = useTopicStore()
          this.parentId = topicStore.id
          this.type = EditTopicRelationType.Create
          this.redirect = true
          this.showModal = true
        },
        async initWikiData() {
          type personalWikiDataResult =  {
            success: boolean,
            personalWiki: SearchTopicItem,
            addToWikiHistory: SearchTopicItem[]
          }
          var result = await $fetch<personalWikiDataResult>('/api/Search/GetPersonalWikiData', { method: 'POST', body: {id: this.parentId}, mode: 'cors', credentials: 'include'})

          if (!!result && result.success){
            this.personalWiki = result.personalWiki
            this.addToWikiHistory = result.addToWikiHistory.reverse()
            this.categoriesToFilter = []
            this.categoriesToFilter.push(this.personalWiki.Id)
            this.addToWikiHistory.forEach((el) => {
                this.categoriesToFilter.push(el.Id)
            })
          }
        }
    },
  })

  