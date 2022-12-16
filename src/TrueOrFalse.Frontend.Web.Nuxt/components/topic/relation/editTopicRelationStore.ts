import { defineStore } from 'pinia'
import { SearchTopicItem } from '../../search/searchHelper'
import { useTopicStore } from '../topicStore'
import { useUserStore } from '../../user/userStore'
import { useTabsStore, Tab } from '../tabs/tabsStore'

export enum EditTopicRelationType {
  Create,
  Move,
  AddParent,
  AddChild,
  None,
  AddToPersonalWiki
}

export interface EditRelationData {
  parentId?: number | undefined,
  childId?: number,
  editCategoryRelation: EditTopicRelationType,
  categoriesToFilter?: number[],
  selectedCategories?: any[],
  redirect?: boolean
}

export const useEditTopicRelationStore = defineStore('editTopicRelationStore', {
  state: () => {
    return {
      showModal: false,
      type: null as EditTopicRelationType | null,
      parentId: 0,
      childId: 0,
      redirect: false,
      addTopicBtnExists: false,
      categoriesToFilter: [] as number[],
      personalWiki: null as SearchTopicItem | null,
      addToWikiHistory: null as SearchTopicItem[] | null,
    }
  },
  actions: {
    openModal(data: EditRelationData) {
      const tabsStore = useTabsStore()
      this.parentId = data.parentId ?? 0
      this.addTopicBtnExists = tabsStore.activeTab == Tab.Topic
      this.type = data.editCategoryRelation
      this.categoriesToFilter = data.categoriesToFilter ?? []
      this.childId = data.childId ?? 0

      if (data.editCategoryRelation == EditTopicRelationType.AddToPersonalWiki)
        this.initWikiData()

      this.showModal = true
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
      type personalWikiDataResult = {
        success: boolean,
        personalWiki: SearchTopicItem,
        addToWikiHistory: SearchTopicItem[]
      }
      var result = await $fetch<personalWikiDataResult>('/apiVue/Search/GetPersonalWikiData', { method: 'POST', body: { id: this.parentId }, mode: 'cors', credentials: 'include' })

      if (!!result && result.success) {
        this.personalWiki = result.personalWiki
        this.addToWikiHistory = result.addToWikiHistory.reverse()
        this.categoriesToFilter = []
        this.categoriesToFilter.push(this.personalWiki.Id)
        this.addToWikiHistory.forEach((el) => {
          this.categoriesToFilter.push(el.Id)
        })
      }
    },
    addParent(id: number) {
      const userStore = useUserStore()
      if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
      }
      const editTopicRelationData: EditRelationData = {
        childId: id,
        redirect: true,
        editCategoryRelation: EditTopicRelationType.AddParent
      }

      this.openModal(editTopicRelationData)
    },
    addChild(id: number) {
      const userStore = useUserStore()
      if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
      }
      const editTopicRelationData: EditRelationData = {
        parentId: id,
        redirect: true,
        editCategoryRelation: EditTopicRelationType.AddChild
      }

      this.openModal(editTopicRelationData)
    },
    addToPersonalWiki(id: number) {
      const editTopicRelationData: EditRelationData = {
          childId: id,
          redirect: true,
          editCategoryRelation: EditTopicRelationType.AddToPersonalWiki
      }
  
      this.openModal(editTopicRelationData)
  }
  },
})
