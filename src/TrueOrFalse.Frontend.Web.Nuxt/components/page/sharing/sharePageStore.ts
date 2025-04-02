import { defineStore } from "pinia"
import { useUserStore } from "~~/components/user/userStore"
import {
    useSnackbarStore,
    SnackbarType,
} from "~~/components/snackBar/snackBarStore"

// Define SharePermission enum based on backend counterpart
export enum SharePermission {
    View = 0,
    Edit = 1,
    ViewWithChildren = 2,
    EditWithChildren = 3,
}

// Define interfaces for API requests/responses
interface SharePageRequest {
    pageId: number
    userIds: number[]
    permission: SharePermission
}

interface SharePageResponse {
    success: boolean
    messageKey: string
}

interface RenewShareTokenRequest {
    pageId: number
}

interface RenewShareTokenResponse {
    success: boolean
    messageKey: string
}

export const useSharePageStore = defineStore("sharePageStore", () => {
    // State
    const showModal = ref(false)
    const pageId = ref(0)
    const userIds = ref<number[]>([])
    const permission = ref<SharePermission>(SharePermission.View)

    // Actions
    function openModal(id: number) {
        pageId.value = id
        userIds.value = []
        permission.value = SharePermission.View
        showModal.value = true
    }

    function closeModal() {
        showModal.value = false
    }

    async function sharePage() {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false }
        }

        const data: SharePageRequest = {
            pageId: pageId.value,
            userIds: userIds.value,
            permission: permission.value,
        }

        const result = await $api<SharePageResponse>(
            "/apiVue/SharePageStore/SharePage",
            {
                method: "POST",
                body: data,
                mode: "cors",
                credentials: "include",
            }
        )

        const snackbarStore = useSnackbarStore()
        const nuxtApp = useNuxtApp()
        const { $i18n } = nuxtApp

        if (result.success) {
            closeModal()
            snackbarStore.showSnackbar({
                type: SnackbarType.Success.toString(),
                text: { message: $i18n.t("success.page.shared") },
                duration: 4000,
            })
            return { success: true }
        } else {
            snackbarStore.showSnackbar({
                type: SnackbarType.Error.toString(),
                text: {
                    message: $i18n.t(result.messageKey || "error.general"),
                },
                duration: 6000, // Longer duration for errors
            })
            return { success: false }
        }
    }

    async function renewShareToken(id: number) {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false }
        }

        const data: RenewShareTokenRequest = {
            pageId: id,
        }

        const result = await $api<RenewShareTokenResponse>(
            "/apiVue/SharePageStore/RenewShareToken",
            {
                method: "POST",
                body: data,
                mode: "cors",
                credentials: "include",
            }
        )

        const snackbarStore = useSnackbarStore()
        const nuxtApp = useNuxtApp()
        const { $i18n } = nuxtApp

        if (result.success) {
            snackbarStore.showSnackbar({
                type: SnackbarType.Success.toString(),
                text: { message: $i18n.t("success.token.renewed") },
                duration: 4000,
            })
            return { success: true }
        } else {
            snackbarStore.showSnackbar({
                type: SnackbarType.Error.toString(),
                text: {
                    message: $i18n.t(result.messageKey || "error.general"),
                },
                duration: 6000,
            })
            return { success: false }
        }
    }

    // Setters
    function setUserIds(ids: number[]) {
        userIds.value = ids
    }

    function setPermission(newPermission: SharePermission) {
        permission.value = newPermission
    }

    // Return all state and functions
    return {
        // State
        showModal,
        pageId,
        userIds,
        permission,

        // Actions
        openModal,
        closeModal,
        sharePage,
        renewShareToken,
        setUserIds,
        setPermission,
    }
})
