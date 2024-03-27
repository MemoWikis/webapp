import { defineStore } from 'pinia'
import { TopicItem } from '../../search/searchHelper'
import { useTopicStore } from '../topicStore'
import { useUserStore } from '../../user/userStore'
import { useTabsStore, Tab } from '../tabs/tabsStore'
import { isEqual } from 'underscore'
import { AlertType, messages, useAlertStore } from '~/components/alert/alertStore'
import { TargetPosition } from '~/components/shared/dragStore'

export enum EditTopicRelationType {
    Create,
    Move,
    AddParent,
    AddChild,
    None,
    AddToPersonalWiki
}

export interface EditRelationData {
    parentId?: number | undefined
    childId?: number
    editCategoryRelation: EditTopicRelationType
    categoriesToFilter?: number[]
    selectedCategories?: any[]
    redirect?: boolean
    topicIdToRemove?: number
}

interface MoveTarget {
    movingTopicId: number,
    targetId: number,
    position: TargetPosition,
    newParentId: number,
    oldParentId: number
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
            personalWiki: null as TopicItem | null,
            recentlyUsedRelationTargetTopics: null as TopicItem[] | null,
            topicIdToRemove: 0,
            moveHistory: {} as MoveTarget
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
            this.topicIdToRemove = data.topicIdToRemove ?? 0
            this.redirect = data.redirect ?? false

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
                personalWiki: TopicItem,
                recentlyUsedRelationTargetTopics: TopicItem[]
            }
            const result = await $fetch<FetchResult<personalWikiDataResult>>(`/apiVue/EditTopicRelationStore/GetPersonalWikiData/${this.parentId}`, { method: 'GET', mode: 'cors', credentials: 'include' })

            if (!!result && result.success) {
                this.personalWiki = result.data.personalWiki
                this.categoriesToFilter = []
                this.categoriesToFilter.push(this.personalWiki.Id)

                this.recentlyUsedRelationTargetTopics = result.data.recentlyUsedRelationTargetTopics?.reverse()
                this.recentlyUsedRelationTargetTopics?.forEach((el) => {
                    this.categoriesToFilter.push(el.Id)
                })
            }
        },
        addParent(id: number, redirect: boolean = true) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            const editTopicRelationData: EditRelationData = {
                childId: id,
                redirect: redirect,
                editCategoryRelation: EditTopicRelationType.AddParent
            }

            this.openModal(editTopicRelationData)
        },
        addChild(id: number, redirect: boolean = true) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            const editTopicRelationData: EditRelationData = {
                parentId: id,
                redirect: redirect,
                editCategoryRelation: EditTopicRelationType.AddChild
            }

            this.openModal(editTopicRelationData)
        },
        async addToPersonalWiki(id: number) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            const result = await $fetch<any>(`/apiVue/EditTopicRelationStore/AddToPersonalWiki/${id}`, {
                method: "POST",
                mode: "cors",
                credentials: "include",
            })

            if (result.success == true) {
                return {
                    success: true,
                    id: id
                }
            } else if (result.success == false) {
                const alertStore = useAlertStore()
                alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
            }
        },
        async removeFromPersonalWiki(id: number) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            const result = await $fetch<any>(`/apiVue/EditTopicRelationStore/RemoveFromPersonalWiki/${id}`, {
                method: "POST",
                mode: "cors",
                credentials: "include",
            })

            if (result.success == true) {
                return {
                    success: true,
                    id: id
                }
            } else if (result.success == false) {
                const alertStore = useAlertStore()
                alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
            }
        },
        addTopic(childId: number) {
            return {
                parentId: this.parentId,
                childId: childId,
            }
        },
        removeTopic(childId: number, parentIdToRemove: number) {
            return {
                parentId: parentIdToRemove,
                childId: childId
            }
        },
        async removeChild(parentId: number, childId: number) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            const result = await this.removeChildren(parentId, [childId])
            if (result && isEqual(result.removedChildIds, [childId]))
                return {
                    parentId: this.parentId,
                    childId: childId,
                }
        },
        async removeChildren(parentId: number, childIds: number[]) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            const data = {
                parentId: parentId,
                childIds: childIds,
            }

            const result = await $fetch<FetchResult<number[]>>("/apiVue/EditTopicRelationStore/RemoveTopics", {
                method: "POST",
                body: data,
                mode: "cors",
                credentials: "include",
            })

            if (result.success == true) {
                return {
                    parentId: parentId,
                    removedChildIds: result.data
                }
            }
        },

        async moveTopic(movingTopicId: number, targetId: number, position: TargetPosition, newParentId: number, oldParentId: number) {

            const userStore = useUserStore()

            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            const data = {
                movingTopicId: movingTopicId,
                targetId: targetId,
                position: position,
                newParentId: newParentId,
                oldParentId: oldParentId
            }
            interface MoveTopicResult {
                oldParentId: number
                newParentId: number
                undoMove: MoveTarget
            }
            const result = await $fetch<MoveTopicResult>("/apiVue/EditTopicRelationStore/MoveTopic", {
                method: "POST",
                body: data,
                mode: "cors",
                credentials: "include",
            })

            if (result)
                this.moveHistory = result.undoMove

            return result
        },

        async undoMoveTopic() {

            return this.moveTopic(this.moveHistory.movingTopicId, this.moveHistory.targetId, this.moveHistory.position, this.moveHistory.newParentId, this.moveHistory.oldParentId)
        }
    },
})

