import { defineStore } from "pinia"
import { PageItem } from "../../search/searchHelper"
import { usePageStore } from "../pageStore"
import { useUserStore } from "../../user/userStore"
import { useTabsStore, Tab } from "../tabs/tabsStore"
import { isEqual } from "underscore"
import { AlertType, useAlertStore } from "~/components/alert/alertStore"
import { TargetPosition } from "~/components/shared/dragStore"
import { GridPageItem } from "../content/grid/item/gridPageItem"
import {
    SnackbarData,
    useSnackbarStore,
} from "~/components/snackBar/snackBarStore"

export enum EditPageRelationType {
    Create,
    Move,
    AddParent,
    AddChild,
    None,
    AddToPersonalWiki,
}

export interface EditRelationData {
    parentId?: number | undefined
    childId?: number
    editCategoryRelation: EditPageRelationType
    pagesToFilter?: number[]
    selectedPages?: any[]
    redirect?: boolean
    pageIdToRemove?: number
}

interface MoveTarget {
    movingPage: GridPageItem
    targetId: number
    position: TargetPosition
    newParentId: number
    oldParentId: number
}

export const useEditPageRelationStore = defineStore("editPageRelationStore", {
    state: () => {
        return {
            showModal: false,
            type: null as EditPageRelationType | null,
            parentId: 0,
            childId: 0,
            redirect: false,
            addPageBtnExists: false,
            pagesToFilter: [] as number[],
            personalWiki: null as PageItem | null,
            recentlyUsedRelationTargetPages: null as PageItem[] | null,
            pageIdToRemove: 0,
            moveHistory: {} as MoveTarget,
        }
    },
    actions: {
        openModal(data: EditRelationData) {
            const tabsStore = useTabsStore()
            this.parentId = data.parentId ?? 0
            this.addPageBtnExists = tabsStore.activeTab == Tab.Text
            this.type = data.editCategoryRelation
            this.pagesToFilter = data.pagesToFilter ?? []
            this.childId = data.childId ?? 0
            this.pageIdToRemove = data.pageIdToRemove ?? 0
            this.redirect = data.redirect ?? false

            if (
                data.editCategoryRelation ==
                EditPageRelationType.AddToPersonalWiki
            )
                this.initWikiData()

            this.showModal = true
        },
        createPage() {
            const userStore = useUserStore()
            if (userStore.isLoggedIn) {
                const pageStore = usePageStore()
                this.parentId = pageStore.id
                this.type = EditPageRelationType.Create
                this.redirect = true
                this.showModal = true
            } else {
                userStore.openLoginModal()
            }
        },
        async initWikiData() {
            type personalWikiDataResult = {
                personalWiki: PageItem
                recentlyUsedRelationTargetPages: PageItem[]
            }
            const id = EditPageRelationType.AddParent
                ? this.childId
                : this.parentId
            const result = await $api<FetchResult<personalWikiDataResult>>(
                `/apiVue/EditPageRelationStore/GetPersonalWikiData/${id}`,
                { method: "GET", mode: "cors", credentials: "include" }
            )

            if (!!result && result.success) {
                this.personalWiki = result.data.personalWiki
                this.pagesToFilter = []
                this.pagesToFilter.push(this.personalWiki.id)

                this.recentlyUsedRelationTargetPages =
                    result.data.recentlyUsedRelationTargetPages?.reverse()
                this.recentlyUsedRelationTargetPages?.forEach((el) => {
                    this.pagesToFilter.push(el.id)
                })
            }
        },
        addParent(id: number, redirect: boolean = true) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            const editPageRelationData: EditRelationData = {
                childId: id,
                redirect: redirect,
                editCategoryRelation: EditPageRelationType.AddParent,
            }

            this.openModal(editPageRelationData)
        },
        addChild(id: number, redirect: boolean = true) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            const editPageRelationData: EditRelationData = {
                parentId: id,
                redirect: redirect,
                editCategoryRelation: EditPageRelationType.AddChild,
            }

            this.openModal(editPageRelationData)
        },
        async addToPersonalWiki(id: number) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            const result = await $api<any>(
                `/apiVue/EditPageRelationStore/AddToPersonalWiki/${id}`,
                {
                    method: "POST",
                    mode: "cors",
                    credentials: "include",
                }
            )
            const nuxtApp = useNuxtApp()
            const { $i18n } = nuxtApp

            if (result.success == true) {
                const snackbarStore = useSnackbarStore()
                const data: SnackbarData = {
                    type: "success",
                    text: {
                        message: $i18n.t("success.page.addedToPersonalWiki"),
                    },
                }
                snackbarStore.showSnackbar(data)

                return {
                    success: true,
                    id: id,
                }
            } else if (result.success == false) {
                const alertStore = useAlertStore()
                alertStore.openAlert(AlertType.Error, {
                    text: $i18n.t(result.messageKey),
                })
            }
        },
        async removeFromPersonalWiki(id: number) {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            const result = await $api<any>(
                `/apiVue/EditPageRelationStore/RemoveFromPersonalWiki/${id}`,
                {
                    method: "POST",
                    mode: "cors",
                    credentials: "include",
                }
            )

            if (result.success == true) {
                return {
                    success: true,
                    id: id,
                }
            } else if (result.success == false) {
                const alertStore = useAlertStore()
                const nuxtApp = useNuxtApp()
                const { $i18n } = nuxtApp

                alertStore.openAlert(AlertType.Error, {
                    text: $i18n.t(result.messageKey),
                })
            }
        },
        addPage(childId: number) {
            return {
                parentId: this.parentId,
                childId: childId,
            }
        },
        removePage(childId: number, parentIdToRemove: number) {
            return {
                parentId: parentIdToRemove,
                childId: childId,
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

            const result = await $api<FetchResult<number[]>>(
                "/apiVue/EditPageRelationStore/RemovePages",
                {
                    method: "POST",
                    body: data,
                    mode: "cors",
                    credentials: "include",
                }
            )

            if (result.success == true) {
                return {
                    parentId: parentId,
                    removedChildIds: result.data,
                }
            }
        },

        async movePage(
            movingPage: GridPageItem,
            targetId: number,
            position: TargetPosition,
            newParentId: number,
            oldParentId: number
        ) {
            const userStore = useUserStore()

            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }

            if (position == TargetPosition.Inner)
                newParentId = targetId

            this.tempInsert(
                movingPage,
                targetId,
                oldParentId,
                newParentId,
                position
            )

            const data = {
                movingPageId: movingPage.id,
                targetId: targetId,
                position: position,
                newParentId: newParentId,
                oldParentId: oldParentId,
            }

            interface MovePageResult {
                success: boolean
                data?: MovePageData
                error?: string
            }

            interface MovePageData {
                oldParentId: number
                newParentId: number
                undoMove: MoveTarget
            }

            const result = await $api<MovePageResult>(
                "/apiVue/EditPageRelationStore/MovePage",
                {
                    method: "POST",
                    body: data,
                    mode: "cors",
                    credentials: "include",
                }
            )

            if (result.success && result.data) {
                const data = result.data
                const newMoveTarget: MoveTarget = {
                    movingPage: movingPage,
                    targetId: data.undoMove.targetId,
                    position: data.undoMove.position,
                    newParentId: data.undoMove.newParentId,
                    oldParentId: data.undoMove.oldParentId,
                }
                this.moveHistory = newMoveTarget

                return result.data
            } else {
                this.cancelMovePage(oldParentId, newParentId, result.error)
            }
        },
        cancelMovePage(
            oldParentId: number,
            newParentId: number,
            messageKey?: string
        ) {
            if (messageKey) {
                const snackbarStore = useSnackbarStore()
                const nuxtApp = useNuxtApp()
                const { $i18n } = nuxtApp

                const data: SnackbarData = {
                    type: "error",
                    text: { message: $i18n.t(messageKey) },
                }
                snackbarStore.showSnackbar(data)
            }

            return {
                oldParentId: oldParentId,
                newParentId: newParentId,
            }
        },
        async undoMovePage() {
            return this.movePage(
                this.moveHistory.movingPage,
                this.moveHistory.targetId,
                this.moveHistory.position,
                this.moveHistory.newParentId,
                this.moveHistory.oldParentId
            )
        },
        tempInsert(
            movePage: GridPageItem,
            targetId: number,
            oldParentId: number,
            newParentId: number,
            position: TargetPosition
        ) {
            return {
                movePage,
                targetId,
                oldParentId,
                newParentId,
                position,
            }
        },
    },
})
