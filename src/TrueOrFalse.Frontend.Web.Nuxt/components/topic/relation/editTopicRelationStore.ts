import { defineStore } from 'pinia'
import { TopicItem } from '../../search/searchHelper'
import { useTopicStore } from '../topicStore'
import { useUserStore } from '../../user/userStore'
import { useTabsStore, Tab } from '../tabs/tabsStore'
import { isEqual } from 'underscore'
import { AlertType, messages, useAlertStore } from '~/components/alert/alertStore'
import { TargetPosition } from '~/components/shared/dragStore'
import { GridTopicItem } from '../content/grid/item/gridTopicItem'
import { SnackbarData, useSnackbarStore } from '~/components/snackBar/snackBarStore'

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
    movingTopic: GridTopicItem,
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
            const id = EditTopicRelationType.AddParent ? this.childId : this.parentId
            const result = await $fetch<FetchResult<personalWikiDataResult>>(`/apiVue/EditTopicRelationStore/GetPersonalWikiData/${id}`, { method: 'GET', mode: 'cors', credentials: 'include' })

            if (!!result && result.success) {
                this.personalWiki = result.data.personalWiki
                this.categoriesToFilter = []
                this.categoriesToFilter.push(this.personalWiki.id)

                this.recentlyUsedRelationTargetTopics = result.data.recentlyUsedRelationTargetTopics?.reverse()
                this.recentlyUsedRelationTargetTopics?.forEach((el) => {
                    this.categoriesToFilter.push(el.id)
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
                editCategoryRelation: EditTopicRelationType.AddParent,
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

        async moveTopic(movingTopic: GridTopicItem, targetId: number, position: TargetPosition, newParentId: number, oldParentId: number) {

            const userStore = useUserStore()

            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            this.tempInsert(movingTopic, targetId, oldParentId, newParentId, position)

            const data = {
                movingTopicId: movingTopic.id,
                targetId: targetId,
                position: position,
                newParentId: newParentId,
                oldParentId: oldParentId
            }
            
            interface MoveTopicResult {
                success: boolean
                data?: MoveTopicData
                error?: string
            }

            interface MoveTopicData {
                oldParentId: number
                newParentId: number
                undoMove: MoveTarget
            }
        
            const result = await $fetch<MoveTopicResult>("/apiVue/EditTopicRelationStore/MoveTopic", {
                method: "POST",
                body: data,
                mode: "cors",
                credentials: "include"
            })

            if (result.success && result.data) {
                const data = result.data
                const newMoveTarget: MoveTarget = {
                    movingTopic: movingTopic,
                    targetId: data.undoMove.targetId,
                    position: data.undoMove.position,
                    newParentId: data.undoMove.newParentId,
                    oldParentId: data.undoMove.oldParentId
                }
                this.moveHistory = newMoveTarget

                return result.data
            } else {
                this.cancelMoveTopic(oldParentId, newParentId, result.error)
            }
        },
        cancelMoveTopic(oldParentId: number, newParentId: number, errorMsg?: string) {
            if (errorMsg) {
                const snackbarStore = useSnackbarStore()
                const data: SnackbarData = {
                    type: 'error',
                    text: messages.getByCompositeKey(errorMsg)
                }
                snackbarStore.showSnackbar(data)
            }

            return {
                oldParentId: oldParentId,
                newParentId: newParentId,
            }
        },
        async undoMoveTopic() {
            return this.moveTopic(this.moveHistory.movingTopic, this.moveHistory.targetId, this.moveHistory.position, this.moveHistory.newParentId, this.moveHistory.oldParentId)
        },
        tempInsert(moveTopic: GridTopicItem, targetId: number, oldParentId: number, newParentId: number, position: TargetPosition) {
            return {
                moveTopic,
                targetId,
                oldParentId,
                newParentId,
                position
            }
        }
    },
})

