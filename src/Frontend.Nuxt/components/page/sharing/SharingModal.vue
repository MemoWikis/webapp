<script lang="ts" setup>
import { useSharePageStore, SharePermission } from './sharePageStore'
import { SearchType, UserItem } from '~~/components/search/searchHelper'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { Tab } from '../tabs/tabsStore'
import { usePageStore } from '../pageStore'
import { useSnackbarStore } from '~/components/snackBar/snackBarStore'
import { useUserStore } from '~/components/user/userStore'

const sharePageStore = useSharePageStore()
const loadingStore = useLoadingStore()
const pageStore = usePageStore()
const snackbarStore = useSnackbarStore()
const userStore = useUserStore()

const { t } = useI18n()

enum CurrentMode {
    Search = 'search',
    AddNew = 'edit'
}

const currentMode = ref<CurrentMode>(CurrentMode.Search)
const currentUser = ref()
const notifyUser = ref(true)
const customMessage = ref('')
const includeToken = ref(true)  // Ref to track if token should be included in URL

// Instead of a local ref, use the store's tokenPermission value
const linkPermission = computed({
    get: () => sharePageStore.currentTokenPermission,
    set: (value: SharePermission) => {
        sharePageStore.updateTokenPermission(value)
    }
})

const permissionOptions = reactive([
    { value: SharePermission.View, key: 'page.sharing.permission.view' },
    { value: SharePermission.Edit, key: 'page.sharing.permission.edit' },
    { value: SharePermission.ViewWithChildren, key: 'page.sharing.permission.viewWithChildren' },
    { value: SharePermission.EditWithChildren, key: 'page.sharing.permission.editWithChildren' }
])

function selectUserToShare(user: UserItem) {

    if (!user) return
    if (user.id === userStore.id) return

    currentUser.value = {
        id: user.id,
        name: user.name,
        avatarUrl: user.imageUrl,
        permission: SharePermission.View
    }
    currentMode.value = CurrentMode.AddNew
}

function goBackToSearch() {
    currentUser.value = null
    currentMode.value = CurrentMode.Search
}

function updatePermission(permission: SharePermission) {
    if (currentUser.value) {
        currentUser.value.permission = permission
    }
}

async function shareWithCurrentUser() {
    if (!currentUser.value) return

    loadingStore.startLoading()

    const result = await sharePageStore.shareToUser(
        currentUser.value.id,
        currentUser.value.permission,
        customMessage.value,
    )

    loadingStore.stopLoading()

    if (result.success) {
        await sharePageStore.loadExistingShares()

        goBackToSearch()
    }
}

function removeExistingShare(userId: number) {
    sharePageStore.removeUserAccess(userId)
}

async function handleRemoveFromItem() {
    if (sharePageStore.selectedInheritedUser) {
        const userId = sharePageStore.selectedInheritedUser.id

        sharePageStore.closeInheritedShareModal()

        sharePageStore.markUserForRestriction(userId)

        snackbarStore.showSnackbar({
            type: 'info',
            text: { message: t('page.sharing.permission.pendingRestriction') },
            duration: 3000,
        })
    }
}

async function handleRemoveFromParent() {
    if (sharePageStore.selectedInheritedUser && sharePageStore.selectedInheritedUser.inheritedFrom) {
        const parentId = sharePageStore.selectedInheritedUser.inheritedFrom.id
        const userId = sharePageStore.selectedInheritedUser.id

        sharePageStore.closeInheritedShareModal()

        sharePageStore.markUserForParentRemoval(userId, parentId)
    }
}

async function renewToken() {
    loadingStore.startLoading()
    const result = await sharePageStore.renewShareToken(sharePageStore.pageId, pageStore.shareToken)
    loadingStore.stopLoading()

    if (result.success)
        await navigator.clipboard.writeText(currentTokenUrl.value)
}

watch(() => sharePageStore.showModal, (show) => {
    if (show) {
        currentMode.value = CurrentMode.Search
        currentUser.value = null
        notifyUser.value = true
        customMessage.value = ''
        sharePageStore.resetPendingChanges()
        sharePageStore.loadExistingShares()
    } else {
        sharePageStore.resetPendingChanges()
    }
})

const ariaId = useId()

const updateExistingSharePermission = (userId: number, permission: SharePermission) => {
    sharePageStore.removeFromMarkUserForRemoval(userId)
    sharePageStore.updatePendingPermission(userId, permission)
}

const primaryButtonLabel = computed(() => {
    if (currentMode.value === 'edit') {
        return t('page.sharing.modal.shareWithUser')
    } else if (sharePageStore.hasPendingChanges) {
        return t('page.sharing.modal.saveChanges')
    } else {
        return t('page.sharing.modal.close')
    }
})

const handlePrimaryButtonClick = async () => {
    if (currentMode.value === CurrentMode.AddNew) {
        await shareWithCurrentUser()
    } else if (sharePageStore.hasPendingChanges) {
        loadingStore.startLoading()
        const result = await sharePageStore.savePermissionChanges()
        loadingStore.stopLoading()

        if (result.success) {
            // Don't close modal - let user see updated list
        }
    } else {
        sharePageStore.closeModal()
    }
}

const copyBaseUrl = async () => {
    const pageUrl = `${config.public.officialBase}${$urlHelper.getPageUrl(sharePageStore.pageName, sharePageStore.pageId, Tab.Text)}`
    await navigator.clipboard.writeText(pageUrl)
}

const copyShareUrl = async () => {

    if (!includeToken.value || !sharePageStore.shareViaToken()) {
        await copyBaseUrl()
        return
    }

    loadingStore.startLoading()
    const result = await sharePageStore.sharePageByToken(sharePageStore.pageId, linkPermission.value, pageStore.shareToken)
    loadingStore.stopLoading()

    if (result.success)
        await navigator.clipboard.writeText(currentTokenUrl.value)
}

const { $urlHelper } = useNuxtApp()
const config = useRuntimeConfig()

const currentTokenUrl = computed(() => {
    if (sharePageStore.currentToken) {
        return `${config.public.officialBase}${$urlHelper.getPageUrl(sharePageStore.pageName, sharePageStore.pageId, Tab.Text, sharePageStore.currentToken)}`
    }
    return ''
})

const permissionIsActive = (userId: number, permission: SharePermission) => {
    if (sharePageStore.pendingRemovals.has(userId))
        return false
    return sharePageStore.getEffectivePermission(userId) === permission
}

const currentLinkPermission = computed(() => {
    return sharePageStore.pendingTokenPermission !== null
        ? sharePageStore.pendingTokenPermission
        : linkPermission.value
})

const currentLinkPermissionLabel = computed(() => {
    return permissionOptions.find(option => option.value === currentLinkPermission.value)?.key
})

const getPermissionLabel = (userId: number, permission: SharePermission) => {
    const key = permissionOptions.find(option => option.value === (sharePageStore.getEffectivePermission(userId) ?? permission))?.key
    if (key) return key
    return ''
}

</script>

<template>
    <LazyModal @close="sharePageStore.closeModal()" :show="sharePageStore.showModal">

        <template v-slot:header>
            <h4 class="modal-title">
                {{ currentMode === CurrentMode.Search
                    ? t('page.sharing.modal.titleManage')
                    : t('page.sharing.modal.titleShareWith', { user: currentUser?.name })
                }}
            </h4>
        </template>

        <template v-slot:body>
            <div class="sharing-container">
                <template v-if="sharePageStore.canEdit">
                    <div v-if="currentMode === CurrentMode.Search">
                        <!-- User search -->
                        <div class="search-container">
                            <Search :search-type="SearchType.users" :show-search="true" :show-search-icon="true"
                                :placeholder-label="t('page.sharing.search.placeholder')"
                                @select-item="selectUserToShare" :hide-current-user="true" />
                        </div>

                        <!-- Existing shares list -->
                        <template v-if="sharePageStore.existingShares.length > 0 || sharePageStore.creator">
                            <div class="section-heading">
                                <h5>{{ t('page.sharing.existingShares.title') }}</h5>
                            </div>

                            <div class="existing-shares">
                                <!-- Display creator first -->
                                <div class="user-item creator" v-if="sharePageStore.creator">
                                    <div class="user-info">
                                        <img :src="sharePageStore.creator.imageUrl" class="user-avatar"
                                            alt="Creator avatar" />
                                        <div class="user-name-container">
                                            <span class="user-name">{{ sharePageStore.creator.name }}</span>
                                        </div>
                                    </div>

                                    <div class="user-detail no-hover">
                                        <div class="permission-display">
                                            <span class="user-permission">{{ t('page.sharing.permission.creator')
                                                }}</span>
                                        </div>
                                    </div>
                                </div>

                                <!-- Display other users -->
                                <div class="user-item" v-for="user in sharePageStore.existingShares" :key="user.id">
                                    <div class="user-info"
                                        :class="{ 'pending-removal': sharePageStore.pendingRemovals.has(user.id) }">
                                        <img :src="user.imageUrl" class="user-avatar" alt="User avatar" />
                                        <div class="user-name-container">
                                            <span class="user-name">{{ user.name }}</span>
                                            <span v-if="user.inheritedFrom" class="inherited-badge">
                                                {{ t('page.sharing.permission.inheritedFrom') }}
                                                <NuxtLink
                                                    :to="$urlHelper.getPageUrl(user.inheritedFrom.name, user.inheritedFrom.id)"
                                                    target="_blank" class="link">
                                                    {{ user.inheritedFrom.name }}
                                                </NuxtLink>
                                            </span>
                                        </div>

                                    </div>

                                    <div class="user-detail">
                                        <!-- Permission dropdown -->
                                        <VDropdown class="share-permission-dropdown" :distance="5"
                                            :aria-id="`existing-share-${user.id}`">
                                            <div class="permission-dropdown-trigger" :class="{
                                                'pending-change': sharePageStore.getEffectivePermission(user.id) !== user.permission,
                                                'pending-removal-trigger': sharePageStore.pendingRemovals.has(user.id),
                                            }">
                                                <span class="user-permission">
                                                    <template v-if="sharePageStore.pendingRemovals.has(user.id)">
                                                        {{ t('page.sharing.permission.removing') }}
                                                    </template>
                                                    <template v-else>
                                                        {{ t(getPermissionLabel(user.id, user.permission)) }}
                                                    </template>
                                                </span>
                                                <font-awesome-icon icon="fa-solid fa-chevron-down"
                                                    class="permission-dropdown-trigger-icon" />
                                            </div>

                                            <!-- Only show dropdown for non-inherited permissions -->
                                            <template #popper="{ hide }">
                                                <div class="permission-dropdown-menu">
                                                    <div @click="updateExistingSharePermission(user.id, SharePermission.View); hide()"
                                                        class="permission-dropdown-item"
                                                        :class="{ 'active': permissionIsActive(user.id, SharePermission.View) }">
                                                        {{ t('page.sharing.permission.view') }}
                                                    </div>

                                                    <div @click="updateExistingSharePermission(user.id, SharePermission.Edit); hide()"
                                                        class="permission-dropdown-item"
                                                        :class="{ 'active': permissionIsActive(user.id, SharePermission.Edit) }">
                                                        {{ t('page.sharing.permission.edit') }}
                                                    </div>

                                                    <div @click="updateExistingSharePermission(user.id, SharePermission.ViewWithChildren); hide()"
                                                        class="permission-dropdown-item"
                                                        :class="{ 'active': permissionIsActive(user.id, SharePermission.ViewWithChildren) }">
                                                        {{ t('page.sharing.permission.viewWithChildren') }}
                                                    </div>

                                                    <div @click="updateExistingSharePermission(user.id, SharePermission.EditWithChildren); hide()"
                                                        class="permission-dropdown-item"
                                                        :class="{ 'active': permissionIsActive(user.id, SharePermission.EditWithChildren) }">
                                                        {{ t('page.sharing.permission.editWithChildren') }}
                                                    </div>

                                                    <div class="divider"></div>

                                                    <div @click="removeExistingShare(user.id); hide()"
                                                        class="permission-dropdown-item permission-dropdown-item-danger"
                                                        :class="{ 'active': sharePageStore.pendingRemovals.has(user.id) }">
                                                        {{ t('page.sharing.permission.remove') }}
                                                    </div>
                                                </div>
                                            </template>
                                        </VDropdown>
                                    </div>
                                </div>
                            </div>
                        </template>

                        <!-- Sharing link section -->
                        <div class="sharing-link-section">
                            <div class="section-heading"
                                :class="{ 'has-renew-icon': sharePageStore.shareViaToken() && sharePageStore.currentToken != null }">
                                <h5>{{ t('page.sharing.link.title') }}</h5>
                                <button class="btn btn-icon btn-renew btn-secondary memo-button" @click="renewToken()"
                                    v-if="sharePageStore.shareViaToken() && sharePageStore.currentToken != null"
                                    v-tooltip="t('page.sharing.link.renewTooltip')">
                                    <font-awesome-icon :icon="['fas', 'rotate']" />
                                </button>
                            </div>

                            <div class="user-item share-link-item">
                                <div class="link-info">
                                    <div class="link-icon">
                                        <font-awesome-icon v-if="sharePageStore.shareViaToken()" :icon="(linkPermission === SharePermission.Edit || linkPermission === SharePermission.EditWithChildren)
                                            ? ['fas', 'pen']
                                            : ['fas', 'eye']" class="access-icon granted" />

                                        <font-awesome-icon v-else :icon="['fas', 'lock']" class="access-icon" />
                                    </div>
                                    <div class="link-name-container">
                                        <div>
                                            <VDropdown class="share-link-dropdown" :distance="5"
                                                :aria-id="'share-link'">
                                                <div class="permission-dropdown-trigger share-trigger"
                                                    :class="{ 'pending-change': sharePageStore.pendingTokenRemoval }">
                                                    <span class="user-permission">
                                                        <span class="link-name" v-if="sharePageStore.shareViaToken()">{{
                                                            t('page.sharing.link.shareViaLink') }}</span>
                                                        <span class="link-name" v-else>{{
                                                            t('page.sharing.link.restrict') }}</span>
                                                    </span>
                                                    <font-awesome-icon icon="fa-solid fa-chevron-down"
                                                        class="permission-dropdown-trigger-icon" />
                                                </div>

                                                <template #popper="{ hide }">
                                                    <div class="permission-dropdown-menu">
                                                        <div @click="sharePageStore.cancelTokenRemoval(); hide()"
                                                            class="permission-dropdown-item"
                                                            :class="{ 'active': sharePageStore.shareViaToken() }">
                                                            {{ t('page.sharing.link.shareViaLink') }}
                                                        </div>

                                                        <div @click="sharePageStore.markTokenForRemoval(); hide()"
                                                            class="permission-dropdown-item permission-dropdown-item-danger"
                                                            :class="{ 'active': !sharePageStore.shareViaToken() }">
                                                            {{ t('page.sharing.link.restrict') }}
                                                        </div>
                                                    </div>
                                                </template>
                                            </VDropdown>
                                        </div>
                                        <span class="link-description">
                                            {{ sharePageStore.shareViaToken()
                                                ? t('page.sharing.link.publicDescription')
                                                : t('page.sharing.link.restrictedDescription') }}
                                        </span>
                                    </div>
                                </div>

                                <div class="link-detail">
                                    <!-- Permission dropdown -->
                                    <VDropdown class="share-link-dropdown" :distance="5"
                                        :aria-id="'share-link-permission'" v-if="sharePageStore.shareViaToken()">
                                        <div class="permission-dropdown-trigger" :class="{
                                            'pending-change': sharePageStore.pendingTokenRemoval || sharePageStore.pendingTokenPermission !== null
                                        }">
                                            <span class="user-permission">
                                                {{ currentLinkPermissionLabel ? t(currentLinkPermissionLabel) : '' }}
                                            </span>
                                            <font-awesome-icon icon="fa-solid fa-chevron-down"
                                                class="permission-dropdown-trigger-icon" />
                                        </div>

                                        <template #popper="{ hide }">
                                            <div class="permission-dropdown-menu">
                                                <div @click="linkPermission = SharePermission.View; hide()"
                                                    class="permission-dropdown-item"
                                                    :class="{ 'active': currentLinkPermission === SharePermission.View }">
                                                    {{ t('page.sharing.permission.view') }}
                                                </div>

                                                <div @click="linkPermission = SharePermission.Edit; hide()"
                                                    class="permission-dropdown-item"
                                                    :class="{ 'active': currentLinkPermission === SharePermission.Edit }">
                                                    {{ t('page.sharing.permission.edit') }}
                                                </div>

                                                <div @click="linkPermission = SharePermission.ViewWithChildren; hide()"
                                                    class="permission-dropdown-item"
                                                    :class="{ 'active': currentLinkPermission === SharePermission.ViewWithChildren }">
                                                    {{ t('page.sharing.permission.viewWithChildren') }}
                                                </div>

                                                <div @click="linkPermission = SharePermission.EditWithChildren; hide()"
                                                    class="permission-dropdown-item"
                                                    :class="{ 'active': currentLinkPermission === SharePermission.EditWithChildren }">
                                                    {{ t('page.sharing.permission.editWithChildren') }}
                                                </div>
                                            </div>
                                        </template>
                                    </VDropdown>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- AddNew permissions mode -->
                    <div v-else-if="currentMode === CurrentMode.AddNew && currentUser" class="edit-container">

                        <div class="selected-user">
                            <img :src="currentUser.avatarUrl" class="user-avatar large" alt="User avatar" />
                            <NuxtLink :to="$urlHelper.getUserUrl(currentUser.name, currentUser.id)" target="_blank">
                                <span class=" user-name link">{{ currentUser.name }}</span>
                            </NuxtLink>
                        </div>

                        <!-- Permission selection -->
                        <div class="permission-selection">
                            <div class="form-group">
                                <label>{{ t('page.sharing.permission.label') }}</label>
                                <VDropdown class="permission-dropdown" :distance="5" :aria-id="ariaId">
                                    <div class="permission-dropdown-trigger new-user">
                                        {{permissionOptions.find(option => option.value === currentUser.permission) ?
                                            t(permissionOptions.find(option => option.value ===
                                        currentUser.permission)!.key) : ''}}
                                        <font-awesome-icon icon="fa-solid fa-chevron-down"
                                            class="permission-dropdown-trigger-icon" />
                                    </div>

                                    <template #popper="{ hide }">
                                        <div class="permission-dropdown-menu">
                                            <div @click="updatePermission(SharePermission.View); hide()"
                                                class="permission-dropdown-item"
                                                :class="{ 'active': currentUser.permission === SharePermission.View }">
                                                {{ t('page.sharing.permission.view') }}
                                            </div>

                                            <div @click="updatePermission(SharePermission.Edit); hide()"
                                                class="permission-dropdown-item"
                                                :class="{ 'active': currentUser.permission === SharePermission.Edit }">
                                                {{ t('page.sharing.permission.edit') }}
                                            </div>

                                            <div @click="updatePermission(SharePermission.ViewWithChildren); hide()"
                                                class="permission-dropdown-item"
                                                :class="{ 'active': currentUser.permission === SharePermission.ViewWithChildren }">
                                                {{ t('page.sharing.permission.viewWithChildren') }}
                                            </div>

                                            <div @click="updatePermission(SharePermission.EditWithChildren); hide()"
                                                class="permission-dropdown-item"
                                                :class="{ 'active': currentUser.permission === SharePermission.EditWithChildren }">
                                                {{ t('page.sharing.permission.editWithChildren') }}
                                            </div>
                                        </div>
                                    </template>
                                </VDropdown>

                                <div class="alert alert-light permission-description">
                                    {{ t(`page.sharing.permission.description.${currentUser.permission}`,
                                        { count: pageStore.directVisibleChildPageCount }) }}
                                </div>
                            </div>
                        </div>

                        <!-- Notification settings -->
                        <div class="notification-settings">
                            <div class="form-group">
                                <div class="notification-checkbox selectable-item" @click="notifyUser = !notifyUser">
                                    <font-awesome-icon icon="fa-solid fa-square-check" class="session-select active"
                                        v-if="notifyUser" />
                                    <font-awesome-icon icon="fa-regular fa-square" class="session-select" v-else />
                                    <div class="notification-label">
                                        {{ t('page.sharing.notification.send') }}
                                    </div>
                                </div>

                                <div v-if="notifyUser" class="custom-message">
                                    <textarea id="custom-message" v-model="customMessage"
                                        :placeholder="t('page.sharing.notification.placeholder')" rows="3"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </template>

                <template v-else>
                    <div class="alert alert-info">
                        {{ t('page.sharing.modal.noManagementRights') }}
                    </div>

                </template>
            </div>
        </template>

        <template v-slot:footer>
            <div class="alert alert-info sharing-info" v-if="sharePageStore.canEdit">
                <ul class="modal-info-list">
                    <li>{{ t('page.sharing.modal.info') }}</li>
                    <li>{{ t('page.sharing.modal.loginInfo') }}</li>
                </ul>
            </div>
            <div class="sharemodal-footer">
                <div class="token-toggle selectable-item" @click="includeToken = !includeToken"
                    v-if="sharePageStore.shareViaToken()">
                    <font-awesome-icon icon="fa-solid fa-square-check" class="session-select active"
                        v-if="includeToken" />
                    <font-awesome-icon icon="fa-regular fa-square" class="session-select" v-else />
                    <div class="token-toggle-label">
                        {{ t('page.sharing.link.includeToken') }}
                    </div>
                </div>
                <div class="sharemodal-footer-actions">
                    <div class="footer-left">
                        <div class="link-actions" v-if="currentMode != CurrentMode.AddNew">
                            <button class="btn btn-copy memo-button" @click="copyShareUrl()">
                                <font-awesome-icon :icon="['fas', 'link']" /> {{ t('page.sharing.link.copy') }}
                            </button>

                        </div>
                    </div>
                    <div class="footer-right">
                        <div v-if="sharePageStore.hasPendingChanges" class="pending-changes-text">
                            <em>{{ t('page.sharing.modal.pendingChanges') }}</em>
                        </div>
                        <button v-if="currentMode === CurrentMode.AddNew" class="btn btn-link memo-button"
                            @click="currentMode = CurrentMode.Search">
                            {{ t('label.back') }}
                        </button>
                        <button class="btn btn-primary memo-button" @click="handlePrimaryButtonClick()">
                            {{ primaryButtonLabel }}
                        </button>
                    </div>
                </div>
            </div>
        </template>
    </LazyModal>

    <!-- Add the RemoveInheritedShareModal -->
    <PageSharingRemoveInheritedShareModal :show="sharePageStore.showInheritedShareModal"
        :user="sharePageStore.selectedInheritedUser" :current-page-name="sharePageStore.pageName"
        @close="sharePageStore.closeInheritedShareModal" @remove-from-item="handleRemoveFromItem"
        @remove-from-parent="handleRemoveFromParent" />
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.permission-description {
    margin-bottom: 0px;
}

.sharing-container {
    display: flex;
    flex-direction: column;
    gap: 24px;
}

.search-container {
    margin-bottom: 24px;
}

.section-heading {
    margin-bottom: 12px;
    padding-top: 12px;

    h5 {
        font-weight: 600;
        margin: 0;
        color: @memo-blue;
    }

    &.has-renew-icon {
        padding-top: 2px;
        margin-bottom: 2px;
        display: flex;
        align-items: center;
    }
}

.existing-shares {
    display: flex;
    flex-direction: column;
    gap: 8px;
    margin-bottom: 24px;
}

.user-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 0 50px;
    min-height: 48px;
    background: white;
    margin: 0 -40px;

    &:hover {
        filter: brightness(0.925);
    }
}

.user-info {
    display: flex;
    align-items: center;
    gap: 12px;
}

.user-avatar {
    width: 32px;
    height: 32px;
    border-radius: 50%;
    object-fit: cover;

    &.large {
        width: 64px;
        height: 64px;
    }
}

.user-name {
    font-weight: 600;
    color: @memo-grey-darkest;

    &.link {
        color: @memo-blue-link;
    }
}

.user-name-container {
    display: flex;
    flex-direction: column;
}

.user-detail {
    display: flex;
    align-items: center;
    gap: 12px;
}

.btn-remove {
    background: none;
    border: none;
    cursor: pointer;
    color: @memo-grey-dark;
    padding: 4px 8px;

    &:hover {
        color: @memo-grey-darkest;
    }
}


.link-actions {
    display: flex;
    align-items: center;
    gap: 12px;
}

.link-info {
    color: @memo-grey-dark;
    font-size: 1.25rem;
}

.no-shares {
    color: @memo-grey-dark;
    font-style: italic;
    padding: 12px;
    background: @memo-grey-lighter;
    border-radius: 4px;
    text-align: center;
}

/* Edit mode styles */
.edit-container {
    display: flex;
    flex-direction: column;
    gap: 24px;
}

.selected-user {
    display: flex;
    flex-direction: column;
    align-items: center;
    gap: 12px;
    padding: 16px;
    background-color: fade(@memo-blue, 5%);
    border-radius: 8px;

    .user-name {
        font-size: 1.2rem;
    }
}

.permission-selection {
    .form-group {
        margin-bottom: 0px;

        label {
            display: block;
            margin-bottom: 8px;
            font-weight: 500;
        }
    }

    .permission-dropdown {
        width: 100%;
        margin-bottom: 12px;
        position: relative;

        .permission-dropdown-trigger {
            display: flex;
            align-items: center;
            padding: 12px 16px;
            border: 1px solid @memo-grey-light;
            border-radius: 4px;
            background-color: white;
            cursor: pointer;

            &:hover {
                filter: brightness(0.95);
            }

            &:active {
                filter: brightness(0.9);
            }

            &.share-trigger {
                padding: 4px 16px;
                justify-content: space-between;
            }

            &.new-user {
                border-radius: 0px;
                justify-content: space-between;
                border-top: none;
                border-left: none;
                border-right: none;
            }
        }

        :deep(.v-popper__popper) {
            /* Add these to ensure proper z-index and width */
            z-index: 1000 !important;
            width: 100% !important;
        }

        .permission-dropdown-menu {
            max-width: none !important;
            /* Ensure menu takes full width */
            z-index: 200;
        }
    }
}

.notification-settings {
    .notification-checkbox {
        display: flex;
        align-items: center;
        gap: 8px;
        margin-bottom: 12px;

        input[type="checkbox"] {
            width: 18px;
            height: 18px;
        }
    }

    .custom-message {
        label {
            display: block;
            margin-bottom: 8px;
        }

        textarea {
            width: 100%;
            padding: 8px;
            border: 1px solid @memo-grey-light;
            border-radius: 0;

            &:focus {
                outline: none;
                border-color: @memo-green;
            }
        }
    }
}

.share-permission-dropdown {

    .permission-dropdown-trigger {
        display: flex;
        align-items: center;
        gap: 8px;
        padding: 4px 8px;
        border-radius: 4px;
        cursor: pointer;
        height: 36px;
        background: white;
        user-select: none;

        &:hover {
            filter: brightness(0.925)
        }

        .user-permission {
            color: @memo-grey-dark;
            font-size: 1.4rem;
            white-space: nowrap;
        }

        &:active {
            filter: brightness(0.85)
        }
    }

    :deep(.v-popper__popper) {
        z-index: 1000 !important;
    }


}

.share-link-dropdown {
    .permission-dropdown-trigger {
        display: flex;
        align-items: center;
        gap: 8px;
        padding: 4px 8px;
        border-radius: 4px;
        cursor: pointer;
        height: 36px;
        background: white;
        user-select: none;
        width: 100%;

        &:hover {
            filter: brightness(0.925);
        }

        &:active {
            filter: brightness(0.85);
        }

        &.pending-removal-trigger {
            color: @memo-grey-darkest;
        }

        .permission-dropdown-trigger-icon {
            transition: transform 0.2s ease;
        }

        &.share-trigger {
            height: 28px;
        }

        .user-permission {
            text-align: right;
            white-space: nowrap;
        }
    }

    :deep(.v-popper__popper) {
        z-index: 1000 !important;
    }
}

.link-actions-section {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 12px;
}

.inherited-shares {
    display: flex;
    flex-direction: column;
    gap: 16px;
    margin-bottom: 24px;
}

.inherited-page-group {
    background-color: fade(@memo-grey-lighter, 50%);
    border-radius: 8px;
    padding: 12px;
}

.page-heading {
    display: flex;
    align-items: center;
    gap: 8px;
    margin-bottom: 8px;
    padding-bottom: 8px;
    border-bottom: 1px solid @memo-grey-light;
}

.page-icon {
    width: 24px;
    height: 24px;
    border-radius: 4px;
    object-fit: cover;
}

.page-name {
    font-weight: 600;
    color: @memo-blue;
    font-size: 0.9rem;
}

.inherited-permission {
    display: flex;
    align-items: center;
    gap: 8px;
    padding: 4px 8px;
    border-radius: 4px;
    height: 36px;
    background: white;
}

.permission-icon {
    display: flex;
    align-items: center;
    gap: 2px;
    color: @memo-grey-dark;
}

.children-icon {
    font-size: 0.8em;
    margin-left: 2px;
}

.inherited-badge {
    font-size: 1.25rem;
    color: @memo-grey-dark;
    border-radius: 4px;
    white-space: nowrap;
}

.permission-display {
    display: flex;
    align-items: center;
    padding: 4px 8px;
    border-radius: 4px;
    height: 36px;
    user-select: none;
}

.permission-dropdown-trigger {
    &.pending-change {

        .user-permission {
            font-weight: 600;
            color: @memo-blue;
            white-space: nowrap;
            font-size: 1.4rem;
        }
    }

    /* Add transition for smoother icon rotation */
    .permission-dropdown-trigger-icon {
        transition: transform 0.2s ease;
    }
}

.user-item {
    .pending-removal {
        .user-name {
            text-decoration: line-through;
            color: @memo-grey-dark;
        }
    }
}

.pending-removal-trigger {
    opacity: 0.8;
}

.v-popper--shown {

    .permission-dropdown-trigger-icon {
        transform: rotate3d(1, 0, 0, 180deg);
    }
}

.sharemodal-footer {
    display: flex;
    flex-direction: column;
    width: 100%;
    padding-top: 16px;

    .sharemodal-footer-actions {
        display: flex;
        align-items: center;
        justify-content: space-between;
        flex-direction: row;
    }
}

.footer-actions {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
}

.token-toggle {
    display: flex;
    align-items: center;
    gap: 6px;
    margin-bottom: 8px;

    input[type="checkbox"] {
        width: 16px;
        height: 16px;
        margin-right: 4px;
    }

    .token-toggle-label {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 6px;
        margin: 0;
        cursor: pointer;
        user-select: none;
        font-size: 1.25rem;
        color: @memo-grey-dark;
        line-height: 1.25rem;
    }

    .token-toggle-text {
        font-weight: 500;
    }

    .token-toggle-icon {
        color: @memo-blue;
    }
}

.footer-left {
    display: flex;
    align-items: center;
}

.footer-right {
    display: flex;
    align-items: center;
    flex-grow: 2;
    gap: 12px;
    justify-content: flex-end;
}

.link-actions {
    display: flex;
    align-items: center;
}

.btn-copy {

    color: @memo-blue-link;
    background: white;
    border: solid 1px @memo-grey-light;

    &:hover {
        filter: brightness(0.95);
    }

    &:active {
        filter: brightness(0.9);
    }
}


.btn-renew {

    display: flex;
    justify-content: center;
    align-items: center;
    margin: 0;
    color: @memo-grey-darker;
    border-radius: 24px;
    width: 35px;
    height: 35px;
    margin-left: 4px;
    background: white;

    svg {
        margin: 0;
    }

    &:hover {
        color: @memo-blue-link;
        filter: brightness(0.95);
    }

    &:active {
        color: @memo-blue-link;
        filter: brightness(0.9);
    }
}

.pending-changes-text {
    color: @memo-grey-dark;
    font-size: 1.25rem;
}

.button-group {
    display: flex;
    gap: 12px;
}

.pending-changes-indicator {
    display: flex;
    align-items: center;
    gap: 8px;
    color: @memo-grey-darker;
    font-size: 1.25rem;
    font-weight: 500;

    .pending-icon {
        color: @memo-blue;
    }
}

.link-icon {
    width: 32px;
    height: 32px;
    display: flex;
    align-items: center;
    justify-content: center;
    border-radius: 48px;
    background: @memo-grey-lighter;
    min-width: 32px;
    min-height: 32px;

    .access-icon {
        font-size: 1.25em;
        color: @memo-grey-dark;

        &.granted {
            color: @memo-blue;
        }

        &.fa-eye {
            margin-left: 1px;
        }
    }
}

.share-link-item {
    padding: 12px 50px;
}

.link-info {
    display: flex;
    align-items: center;
    gap: 12px;
}

.link-name-container {
    display: flex;
    flex-direction: column;
    margin-top: -8px;
}

.link-name {
    font-weight: 600;
    color: @memo-grey-darkest;
}

.link-description {
    font-size: 1.25rem;
    color: @memo-grey-dark;
    padding-left: 8px;
}

.link-detail {
    display: flex;
    align-items: center;
    gap: 12px;
}

.session-select {
    font-size: 18px;
    color: @memo-grey-dark;

    &.active {
        color: @memo-blue-link;
    }
}

.selectable-item {
    cursor: pointer;
    user-select: none;
    display: flex;
    align-items: center;

    &:hover {
        i {
            color: @memo-blue;
        }
    }
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';


.permission-dropdown-item {
    padding: 10px 25px;
    cursor: pointer;
    background: white;

    &:hover {
        filter: brightness(0.95);
    }

    &.active {
        filter: brightness(0.9);
        font-weight: 500;
    }

    &.permission-dropdown-item-danger {
        color: @memo-grey-darkest;
        font-weight: 500;

        &:hover {
            background-color: fade(@memo-grey-dark, 15%);
        }
    }
}

.divider {
    height: 1px;
    background-color: @memo-grey-light;
    margin: 4px 0;
}

.sharing-info {
    margin-top: 24px;
    margin-bottom: 12px;
}
</style>
