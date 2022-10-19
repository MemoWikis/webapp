import { defineStore } from 'pinia'
import { SearchTopicItem } from '~~/components/search/searchHelper'
import { useTopicStore } from '../topicStore'
import { useUserStore } from '~~/components/user/userStore'

export enum EditTopicRelationType {
    Create,
    Move,
    AddParent,
    AddChild,
    None,
    AddToWiki
}

export type EditRelationData = {
  parentId: number,
  childId: number, 
  addCategoryBtnId: string,
  editCategoryRelation: EditTopicRelationType,
  categoriesToFilter: number[],
}

export const useEditTopicRelationStore = defineStore('editTopicRelationStore', {
    state: () => {
      return {
        showModal: false,
        type: null as EditTopicRelationType,
        parentId: 0,
        childId: 0,
        redirect: false,
        addTopicBtnId: '',
        categoriesToFilter: [] as number[],
        personalWiki: null,
        addToWikiHistory: null,
      }
    },
    actions: {
        openModal(data: EditRelationData) {
          console.log('openmodal')

            console.log(data)

            this.parentId = data.parentId
            this.addTopicBtnId = data.addCategoryBtnId
            this.type = data.editCategoryRelation
            this.categoriesToFilter = data.categoriesToFilter
            this.childId = data.childId
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

  