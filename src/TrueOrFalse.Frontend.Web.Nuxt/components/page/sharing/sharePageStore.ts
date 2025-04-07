import { defineStore } from "pinia"
import { useUserStore } from "~~/components/user/userStore"
import {
    useSnackbarStore,
    SnackbarType,
} from "~~/components/snackBar/snackBarStore"
import { useLoadingStore } from "~/components/loading/loadingStore"

export enum SharePermission {
    View = 0,
    Edit = 1,
    ViewWithChildren = 2,
    EditWithChildren = 3,
}

interface ShareInfoRequest {
    userId: number
    permission: SharePermission
}

interface EditRightsRequest {
    pageId: number
    users: ShareInfoRequest[]
}

interface EditRightsResponse {
    success: boolean
    messageKey: string
}

interface ShareToUserRequest {
    pageId: number
    userId: number
    permission: SharePermission
    customMessage?: string
}

interface ShareToUserResponse {
    success: boolean
    messageKey: string
}

interface GetShareInfoResponse {
    users?: UserWithPermission[]
}

interface RenewShareTokenRequest {
    pageId: number
}

interface RenewShareTokenResponse {
    success: boolean
    messageKey: string
}

interface SharePageByTokenRequest {
    pageId: number
    permission: SharePermission
}

interface SharePageByTokenResponse {
    success: boolean
    token: string | null
}

interface BatchUpdatePermissionsRequest {
    pageId: number
    permissionUpdates: {
        userId: number
        permission: SharePermission
    }[]
    removedUserIds: number[]
}

interface BatchUpdatePermissionsResponse {
    success: boolean
    messageKey: string
}

export interface UserWithPermission {
    id: number
    name?: string
    imageUrl?: string
    permission: SharePermission
}

export const useSharePageStore = defineStore("sharePageStore", () => {
    const showModal = ref(false)
    const pageId = ref(0)
    const selectedUsers = ref<UserWithPermission[]>([])
    const existingShares = ref<UserWithPermission[]>([])

    const pendingPermissionChanges = ref<Map<number, SharePermission>>(
        new Map()
    )

    const pendingRemovals = ref<Set<number>>(new Set())

    const hasPendingChanges = computed(
        () =>
            pendingPermissionChanges.value.size > 0 ||
            pendingRemovals.value.size > 0
    )

    const openModal = (id: number) => {
        pageId.value = id
        selectedUsers.value = []
        showModal.value = true
    }

    const closeModal = () => {
        showModal.value = false
    }

    const addUser = (user: UserWithPermission) => {
        const existingUserIndex = selectedUsers.value.findIndex(
            (u) => u.id === user.id
        )

        if (existingUserIndex >= 0) {
            selectedUsers.value[existingUserIndex].permission = user.permission
        } else {
            selectedUsers.value.push(user)
        }
    }

    const removeUser = (userId: number) => {
        const index = selectedUsers.value.findIndex((u) => u.id === userId)
        if (index >= 0) {
            selectedUsers.value.splice(index, 1)
        }
    }

    const updateUserPermission = (
        userId: number,
        permission: SharePermission
    ) => {
        const index = selectedUsers.value.findIndex((u) => u.id === userId)
        if (index >= 0) {
            selectedUsers.value[index].permission = permission
        }
    }

    const updatePendingPermission = (
        userId: number,
        permission: SharePermission
    ) => {
        const user = existingShares.value.find((u) => u.id === userId)

        if (user && user.permission !== permission) {
            pendingPermissionChanges.value.set(userId, permission)
        } else if (user && user.permission === permission) {
            pendingPermissionChanges.value.delete(userId)
        }
    }

    const getEffectivePermission = (
        userId: number
    ): SharePermission | undefined => {
        if (pendingPermissionChanges.value.has(userId)) {
            return pendingPermissionChanges.value.get(userId)
        }

        const user = existingShares.value.find((u) => u.id === userId)
        return user?.permission
    }

    const markUserForRemoval = (userId: number) => {
        pendingRemovals.value.add(userId)
    }

    const resetPendingChanges = () => {
        pendingPermissionChanges.value.clear()
        pendingRemovals.value.clear()
    }

    const loadExistingShares = async () => {
        const userStore = useUserStore()
        const loadingStore = useLoadingStore()

        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false }
        }

        loadingStore.startLoading()

        try {
            const response = await $api<GetShareInfoResponse>(
                `/apiVue/SharePageStore/GetShareInfo/${pageId.value}`,
                {
                    method: "GET",
                    mode: "cors",
                    credentials: "include",
                }
            )

            if (response && response.users) {
                existingShares.value = response.users
                return { success: true, users: response.users }
            } else {
                return { success: false }
            }
        } catch (error) {
            console.error("Failed to load existing shares:", error)

            const snackbarStore = useSnackbarStore()
            const nuxtApp = useNuxtApp()
            const { $i18n } = nuxtApp

            snackbarStore.showSnackbar({
                type: SnackbarType.Error.toString(),
                text: { message: $i18n.t("error.loading.shares") },
                duration: 4000,
            })

            return { success: false }
        } finally {
            loadingStore.stopLoading()
        }
    }

    const shareToUser = async (
        userId: number,
        permission: SharePermission = SharePermission.View,
        customMessage?: string
    ) => {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false }
        }

        const data: ShareToUserRequest = {
            pageId: pageId.value,
            userId: userId,
            permission: permission,
            customMessage: customMessage,
        }

        const result = await $api<ShareToUserResponse>(
            "/apiVue/SharePageStore/ShareToUser",
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
                duration: 6000,
            })
            return { success: false }
        }
    }

    const editRights = async () => {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false }
        }

        if (selectedUsers.value.length === 0) {
            const snackbarStore = useSnackbarStore()
            const nuxtApp = useNuxtApp()
            const { $i18n } = nuxtApp

            snackbarStore.showSnackbar({
                type: SnackbarType.Error.toString(),
                text: { message: $i18n.t("error.page.noUsersSelected") },
                duration: 4000,
            })
            return { success: false }
        }

        const users = selectedUsers.value.map((user) => ({
            userId: user.id,
            permission: user.permission,
        }))

        const data: EditRightsRequest = {
            pageId: pageId.value,
            users: users,
        }

        const result = await $api<EditRightsResponse>(
            "/apiVue/SharePageStore/EditRights",
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
                text: { message: $i18n.t("success.page.rightsUpdated") },
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

    const savePermissionChanges = async () => {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false }
        }

        if (
            pendingPermissionChanges.value.size === 0 &&
            pendingRemovals.value.size === 0
        ) {
            return { success: true }
        }

        const updates = Array.from(
            pendingPermissionChanges.value.entries()
        ).map(([userId, permission]) => ({
            userId,
            permission,
        }))

        const removedUserIds = Array.from(pendingRemovals.value)

        const data: BatchUpdatePermissionsRequest = {
            pageId: pageId.value,
            permissionUpdates: updates,
            removedUserIds: removedUserIds,
        }

        const result = await $api<BatchUpdatePermissionsResponse>(
            "/apiVue/SharePageStore/BatchUpdatePermissions",
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
            resetPendingChanges()

            await loadExistingShares()

            snackbarStore.showSnackbar({
                type: SnackbarType.Success.toString(),
                text: { message: $i18n.t("success.page.rightsUpdated") },
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

    const renewShareToken = async (id: number) => {
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

    const sharePageByToken = async (
        id: number,
        permission: SharePermission
    ) => {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false, token: null }
        }

        const data: SharePageByTokenRequest = {
            pageId: id,
            permission: permission,
        }

        const result = await $api<SharePageByTokenResponse>(
            "/apiVue/SharePageStore/SharePageByToken",
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

        if (result.success && result.token) {
            await navigator.clipboard.writeText(result.token)

            snackbarStore.showSnackbar({
                type: SnackbarType.Success.toString(),
                text: { message: $i18n.t("success.token.copied") },
                duration: 4000,
            })
            return { success: true, token: result.token }
        } else {
            snackbarStore.showSnackbar({
                type: SnackbarType.Error.toString(),
                text: { message: $i18n.t("error.token.generation") },
                duration: 6000,
            })
            return { success: false, token: null }
        }
    }

    return {
        showModal,
        pageId,
        selectedUsers,
        existingShares,
        hasPendingChanges,
        pendingRemovals,

        openModal,
        closeModal,
        addUser,
        removeUser,
        updateUserPermission,
        shareToUser,
        editRights,
        renewShareToken,
        sharePageByToken,
        loadExistingShares,

        updatePendingPermission,
        getEffectivePermission,
        resetPendingChanges,
        savePermissionChanges,
        markUserForRemoval,
    }
})
