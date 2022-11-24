import { defineStore } from 'pinia'
import { SearchTopicItem } from '../../search/searchHelper'
import { useTopicStore } from '../topicStore'
import { useUserStore } from '../../user/userStore'

export enum EditTopicRelationType {
    Create,
    Move,
    AddParent,
    AddChild,
    None,
    AddToWiki
}

export interface EditRelationData {
  parentId?: number | undefined,
  childId?: number, 
  addCategoryBtnId?: string,
  editCategoryRelation: EditTopicRelationType,
  categoriesToFilter?: number[],
  selectedCategories?: any[],
}

export const useEditTopicRelationStore = defineStore('editTopicRelationStore', {
    state: () => {
      return {
        showModal: false,
        type: null as unknown as EditTopicRelationType,
        parentId: 0,
        childId: 0,
        redirect: false,
        addTopicBtnId: '',
        categoriesToFilter: [] as number[],
        personalWiki: null as unknown as SearchTopicItem,
        addToWikiHistory: null as unknown as SearchTopicItem[],
      }
    },
    actions: {
        openModal(data: EditRelationData) {
          this.parentId = data.parentId ?? 0
          this.addTopicBtnId = data.addCategoryBtnId ?? ''
          this.type = data.editCategoryRelation
          this.categoriesToFilter = data.categoriesToFilter ?? []
          this.childId = data.childId ?? 0
          this.showModal = true

          if (data.editCategoryRelation == EditTopicRelationType.AddToWiki)
            this.initWikiData()
        },
        createTopic() {
          const userStore = useUserStore()
          if (userStore.isLoggedIn) {
            const topicStore = useTopicStore()
            this.parentId = topicStore.id
            this.type = EditTopicRelationType.Create
            this.redirect = true
            this.showModal = true
          } else {
            userStore.openLoginModal()
          }
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

  