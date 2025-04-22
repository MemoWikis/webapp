import { defineStore } from "pinia"
import { useUserStore } from "~~/components/user/userStore"
import {
    useSnackbarStore,
    SnackbarType,
} from "~~/components/snackBar/snackBarStore"
import { useLoadingStore } from "~/components/loading/loadingStore"
import { usePageStore, TinyPageModel } from "../pageStore"

export enum SharePermission {
    View = 0,
    Edit = 1,
    ViewWithChildren = 2,
    EditWithChildren = 3,
    RestrictAccess = 10,
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
    creator: CreatorResponse
    users?: UserWithPermission[]
    shareToken?: string
    shareTokenPermission?: SharePermission
}

interface CreatorResponse {
    id: number
    name: string
    imageUrl?: string
}

interface RenewShareTokenRequest {
    pageId: number
    shareToken: string | null
}

interface RenewShareTokenResponse {
    success: boolean
    token: string | null
}

interface SharePageByTokenRequest {
    pageId: number
    permission: SharePermission
    shareToken: string | null
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
    parentRemovals?: {
        userId: number
        parentPageId: number
    }[]
    removeShareToken?: boolean
    tokenPermission?: SharePermission | null
}

interface BatchUpdatePermissionsResponse {
    success: boolean
    messageKey: string
}

interface UpdateTokenPermissionRequest {
    pageId: number
    permission: SharePermission
}

interface UpdateTokenPermissionResponse {
    success: boolean
    messageKey?: string
}

export interface UserWithPermission {
    id: number
    name?: string
    imageUrl?: string
    permission: SharePermission
    inheritedFrom?: TinyPageModel
}

interface RemoveShareTokenRequest {
    pageId: number
}

interface RemoveShareTokenResponse {
    success: boolean
}

export const useSharePageStore = defineStore("sharePageStore", () => {
    const showModal = ref(false)
    const pageId = ref(0)
    const pageName = ref("")
    const selectedUsers = ref<UserWithPermission[]>([])
    const existingShares = ref<UserWithPermission[]>([])
    const creator = ref<CreatorResponse | null>(null)
    const markShareViaToken = ref(false)
    const currentTokenPermission = ref<SharePermission>(SharePermission.View)
    const pendingTokenPermission = ref<SharePermission | null>(null)

    // New refs for handling inherited shares
    const showInheritedShareModal = ref(false)
    const selectedInheritedUser = ref<UserWithPermission | null>(null)

    const pendingPermissionChanges = ref<Map<number, SharePermission>>(
        new Map()
    )

    const pendingRemovals = ref<Set<number>>(new Set())
    const pendingTokenRemoval = ref(false)

    // Add parentRemovedUserIds to track users who should be removed from parent pages
    const parentRemovedUserIds = ref<
        { userId: number; parentPageId: number }[]
    >([])

    const hasPendingChanges = computed(
        () =>
            pendingPermissionChanges.value.size > 0 ||
            pendingRemovals.value.size > 0 ||
            parentRemovedUserIds.value.length > 0 ||
            pendingTokenRemoval.value ||
            pendingTokenPermission.value !== null
    )

    const openModal = (id: number, name: string) => {
        pageId.value = id
        pageName.value = name
        selectedUsers.value = []
        showModal.value = true
    }

    const closeModal = () => {
        showModal.value = false
        // reset all store values when closing modal
        resetPendingChanges()
        pageId.value = 0
        pageName.value = ""
        selectedUsers.value = []
        existingShares.value = []
        creator.value = null
        markShareViaToken.value = false
        currentToken.value = null
        currentTokenPermission.value = SharePermission.View
        // clear inherited share modal state
        showInheritedShareModal.value = false
        selectedInheritedUser.value = null

        const pageStore = usePageStore()
        pageStore.updateIsShared()
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

    const removeFromMarkUserForRemoval = (userId: number) => {
        pendingRemovals.value.delete(userId)
    }

    // Mark a user for removal from a parent page
    const markUserForParentRemoval = (userId: number, parentPageId: number) => {
        // Check if this specific parent removal is already in the list
        const existingRemoval = parentRemovedUserIds.value.find(
            (item) =>
                item.userId === userId && item.parentPageId === parentPageId
        )

        // Only add if it doesn't already exist
        if (!existingRemoval) {
            parentRemovedUserIds.value.push({ userId, parentPageId })
        }
    }

    const markTokenForRemoval = () => {
        markShareViaToken.value = false
        pendingTokenRemoval.value = true
    }

    const cancelTokenRemoval = () => {
        markShareViaToken.value = true
        pendingTokenRemoval.value = false
    }

    const resetPendingChanges = () => {
        pendingPermissionChanges.value.clear()
        pendingRemovals.value.clear()
        parentRemovedUserIds.value = []
        pendingTokenRemoval.value = false
        pendingTokenPermission.value = null
    }

    const updateTokenPermission = (permission: SharePermission) => {
        if (permission !== currentTokenPermission.value) {
            pendingTokenPermission.value = permission
        } else {
            pendingTokenPermission.value = null
        }
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
            const pageStore = usePageStore()
            const tokenString = pageStore.shareToken
                ? `?token=${pageStore.shareToken}`
                : ""

            const response = await $api<GetShareInfoResponse>(
                `/apiVue/SharePageStore/GetShareInfo/${pageId.value}${tokenString}`,
                {
                    method: "GET",
                    mode: "cors",
                    credentials: "include",
                }
            )

            if (response) {
                if (response.users) {
                    existingShares.value = response.users
                }

                if (response.creator) {
                    creator.value = response.creator
                }

                if (response.shareToken) {
                    currentToken.value = response.shareToken
                }

                if (
                    response.shareTokenPermission !== undefined &&
                    response.shareTokenPermission !== null
                ) {
                    currentTokenPermission.value = response.shareTokenPermission
                }

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

    /**
     * Returns true when a page is shared exclusively via token (no direct user shares)
     */
    const isExclusivelyTokenShared = (): boolean => {
        return existingShares.value?.length === 0 && Boolean(currentToken.value)
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

    const savePermissionChanges = async () => {
        // Validate user is logged in
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false }
        }

        // Check if there are any changes to save
        if (
            pendingPermissionChanges.value.size === 0 &&
            pendingRemovals.value.size === 0 &&
            parentRemovedUserIds.value.length === 0 &&
            !pendingTokenRemoval.value &&
            pendingTokenPermission.value === null
        ) {
            return { success: true }
        }

        // Prepare data for batch update
        const updates = Array.from(
            pendingPermissionChanges.value.entries()
        ).map(([userId, permission]) => ({
            userId,
            permission,
        }))

        const removedUserIds = Array.from(pendingRemovals.value)

        // Create the request with all changes - user permissions, removals, and token changes
        const data: BatchUpdatePermissionsRequest = {
            pageId: pageId.value,
            permissionUpdates: updates,
            removedUserIds: removedUserIds,
            parentRemovals:
                parentRemovedUserIds.value.length > 0
                    ? parentRemovedUserIds.value
                    : undefined,
            removeShareToken: pendingTokenRemoval.value,
            tokenPermission: pendingTokenPermission.value,
        }

        // Send all changes in a single request
        const result = await $api<BatchUpdatePermissionsResponse>(
            "/apiVue/SharePageStore/BatchUpdatePermissions",
            {
                method: "POST",
                body: data,
                mode: "cors",
                credentials: "include",
            }
        )

        // Handle response
        const snackbarStore = useSnackbarStore()
        const nuxtApp = useNuxtApp()
        const { $i18n } = nuxtApp

        if (result.success) {
            // Reset all pending changes
            resetPendingChanges()

            // Reload to show updated state
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

    const renewShareToken = async (
        id: number,
        shareToken: string | null = null
    ) => {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false }
        }

        const data: RenewShareTokenRequest = {
            pageId: id,
            shareToken: shareToken,
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

        if (result.success && result.token) {
            snackbarStore.showSnackbar({
                type: SnackbarType.Success.toString(),
                text: { message: $i18n.t("success.token.renewed") },
                duration: 4000,
            })
            updateShareToken(result.token)

            return { success: true }
        } else {
            snackbarStore.showSnackbar({
                type: SnackbarType.Error.toString(),
                text: {
                    message: $i18n.t("error.general"),
                },
                duration: 6000,
            })
            return { success: false }
        }
    }

    const currentToken = ref<string | null>(null)

    const sharePageByToken = async (
        id: number,
        permission: SharePermission,
        shareToken: string | null = null
    ) => {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false, token: null }
        }

        const data: SharePageByTokenRequest = {
            pageId: id,
            permission: permission,
            shareToken: shareToken,
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
            snackbarStore.showSnackbar({
                type: SnackbarType.Success.toString(),
                text: { message: $i18n.t("success.token.copied") },
                duration: 4000,
            })
            updateShareToken(result.token)

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

    const updateShareToken = (token: string | null) => {
        currentToken.value = token
        const pageStore = usePageStore()
        pageStore.setToken(currentToken.value)
    }

    const removeShareToken = async () => {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false }
        }

        const data: RemoveShareTokenRequest = {
            pageId: pageId.value,
        }

        const result = await $api<RemoveShareTokenResponse>(
            "/apiVue/SharePageStore/RemoveShareToken",
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
                text: { message: $i18n.t("success.token.removed") },
                duration: 4000,
            })
            currentToken.value = null
            return { success: true }
        } else {
            snackbarStore.showSnackbar({
                type: SnackbarType.Error.toString(),
                text: { message: $i18n.t("error.token.removal") },
                duration: 6000,
            })
            return { success: false }
        }
    }

    const updateLinkPermission = async (permission: SharePermission) => {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false }
        }

        const data: UpdateTokenPermissionRequest = {
            pageId: pageId.value,
            permission: permission,
        }

        const result = await $api<UpdateTokenPermissionResponse>(
            "/apiVue/SharePageStore/UpdateTokenPermission",
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
            currentTokenPermission.value = permission
            return { success: true }
        } else {
            snackbarStore.showSnackbar({
                type: SnackbarType.Error.toString(),
                text: {
                    message: $i18n.t(
                        result.messageKey || "error.token.permission"
                    ),
                },
                duration: 6000,
            })
            return { success: false }
        }
    }

    const shareViaToken = () => {
        return (
            (currentToken.value || markShareViaToken.value) &&
            !pendingTokenRemoval.value
        )
    }

    // Check if a share is inherited from parent
    const isInheritedShare = (userId: number) => {
        const user = existingShares.value.find((u) => u.id === userId)
        return user?.inheritedFrom !== undefined && user.inheritedFrom !== null
    }

    // Open the inherited share removal modal
    const openInheritedShareModal = (userId: number) => {
        const user = existingShares.value.find((u) => u.id === userId)
        if (user) {
            selectedInheritedUser.value = user
            showInheritedShareModal.value = true
        }
    }

    // Close the inherited share removal modal
    const closeInheritedShareModal = () => {
        showInheritedShareModal.value = false
        selectedInheritedUser.value = null
    }

    // Add a restriction for a user (block inherited access)
    const restrictUserAccess = async (userId: number) => {
        const userStore = useUserStore()
        if (!userStore.isLoggedIn) {
            userStore.openLoginModal()
            return { success: false }
        }

        // Create a new share with restriction permission
        const data: ShareToUserRequest = {
            pageId: pageId.value,
            userId: userId,
            permission: SharePermission.RestrictAccess,
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
            // Reload the shares to show the updated state
            await loadExistingShares()

            snackbarStore.showSnackbar({
                type: SnackbarType.Success.toString(),
                text: { message: $i18n.t("success.page.accessRestricted") },
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

    const markUserForRestriction = (userId: number) => {
        const user = existingShares.value.find((u) => u.id === userId)
        if (user) {
            pendingPermissionChanges.value.set(
                userId,
                SharePermission.RestrictAccess
            )
        }
    }

    const removeUserAccess = (userId: number) => {
        if (isInheritedShare(userId)) {
            openInheritedShareModal(userId)
        } else {
            markUserForRemoval(userId)
        }
    }

    return {
        showModal,
        pageId,
        pageName,
        selectedUsers,
        existingShares,
        hasPendingChanges,
        pendingRemovals,
        currentToken,
        pendingTokenRemoval,
        creator,
        markShareViaToken,
        currentTokenPermission,
        pendingTokenPermission,
        showInheritedShareModal,
        selectedInheritedUser,

        openModal,
        closeModal,
        addUser,
        removeUser,
        updateUserPermission,
        shareToUser,
        renewShareToken,
        sharePageByToken,
        loadExistingShares,

        updatePendingPermission,
        getEffectivePermission,
        resetPendingChanges,
        savePermissionChanges,
        markUserForRemoval,
        removeFromMarkUserForRemoval,
        removeShareToken,
        markTokenForRemoval,
        cancelTokenRemoval,
        updateTokenPermission,
        updateLinkPermission,

        shareViaToken,
        isExclusivelyTokenShared,
        isInheritedShare,
        openInheritedShareModal,
        closeInheritedShareModal,
        restrictUserAccess,
        markUserForRestriction,
        removeUserAccess,
        markUserForParentRemoval,
    }
})
