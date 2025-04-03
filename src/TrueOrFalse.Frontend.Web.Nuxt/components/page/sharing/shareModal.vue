<script lang="ts" setup>
import { useSharePageStore, SharePermission, UserWithPermission } from './sharePageStore'
import { useUserStore } from '~~/components/user/userStore'
import { SearchType, UserItem } from '~~/components/search/searchHelper'
import { useLoadingStore } from '~/components/loading/loadingStore'

const sharePageStore = useSharePageStore()
const userStore = useUserStore()
const loadingStore = useLoadingStore()
const { t } = useI18n()

// Track the current UI mode (search or edit)
const currentMode = ref('search') // 'search' or 'edit'
const currentUser = ref()
const notifyUser = ref(true)
const customMessage = ref('')

const permissionOptions = reactive([
    { value: SharePermission.View, label: t('page.sharing.permission.view') },
    { value: SharePermission.Edit, label: t('page.sharing.permission.edit') },
    { value: SharePermission.ViewWithChildren, label: t('page.sharing.permission.viewWithChildren') },
    { value: SharePermission.EditWithChildren, label: t('page.sharing.permission.editWithChildren') }
])

function selectUserToShare(user: UserItem) {
    currentUser.value = {
        id: user.id,
        name: user.name,
        avatarUrl: user.imageUrl,
        permission: SharePermission.View
    }
    currentMode.value = 'edit'
}

function goBackToSearch() {
    currentUser.value = null
    currentMode.value = 'search'
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
        // Reload existing shares to show updated list
        await sharePageStore.loadExistingShares()

        // Reset to search mode
        goBackToSearch()
    }
}

function removeExistingShare(userId: number) {
    // Implement removal of sharing rights
    // This would need an additional backend endpoint
}

async function renewToken() {
    loadingStore.startLoading()
    const result = await sharePageStore.renewShareToken(sharePageStore.pageId)
    loadingStore.stopLoading()
}

async function generateShareToken() {
    loadingStore.startLoading()
    const result = await sharePageStore.sharePageByToken(sharePageStore.pageId)
    loadingStore.stopLoading()
}

watch(() => sharePageStore.showModal, async (show) => {
    if (show) {
        // When modal opens, load existing shares
        currentMode.value = 'search'
        currentUser.value = null
        notifyUser.value = true
        customMessage.value = ''
        await sharePageStore.loadExistingShares()
    }
})

const ariaId = useId()

</script>

<template>
    <LazyModal @close="sharePageStore.closeModal()" :show="sharePageStore.showModal"
        :primary-btn-label="currentMode === 'search' ? t('page.sharing.modal.close') : t('page.sharing.modal.shareWithUser')"
        :secondary-btn-label="currentMode === 'edit' ? t('page.sharing.modal.cancel') : ''"
        @primary-btn="currentMode === 'search' ? sharePageStore.closeModal() : shareWithCurrentUser()"
        @secondary-btn="currentMode === 'edit' ? goBackToSearch() : null"
        :show-cancel-btn="false">

        <template v-slot:header>
            <h4 class="modal-title">
                {{ currentMode === 'search'
                    ? t('page.sharing.modal.titleManage')
                    : t('page.sharing.modal.titleShareWith', { user: currentUser?.name })
                }}
            </h4>
        </template>

        <template v-slot:body>
            <div class="sharing-container">
                <!-- User search mode -->
                <div v-if="currentMode === 'search'">
                    <!-- User search -->
                    <div class="search-container">
                        <Search
                            :search-type="SearchType.users"
                            :show-search="true"
                            :show-search-icon="true"
                            :placeholder-label="t('page.sharing.search.placeholder')"
                            @select-item="selectUserToShare" />
                    </div>

                    <!-- Existing shares list -->
                    <template v-if="sharePageStore.existingShares.length > 0">

                        <div class="section-heading">
                            <h5>{{ t('page.sharing.existingShares.title') }}</h5>
                        </div>

                        <div class="existing-shares">
                            <div class="user-item" v-for="user in sharePageStore.existingShares" :key="user.id">
                                <div class="user-info">
                                    <img :src="user.imageUrl" class="user-avatar" alt="User avatar" />
                                    <span class="user-name">{{ user.name }}</span>
                                </div>

                                <div class="user-detail">
                                    <span class="user-permission">
                                        {{permissionOptions.find(option => option.value === user.permission)?.label}}
                                    </span>
                                    <button class="btn-remove" @click="removeExistingShare(user.id)">
                                        <font-awesome-icon icon="fa-solid fa-xmark" />
                                    </button>
                                </div>
                            </div>
                        </div>

                    </template>

                    <!-- Sharing link section -->
                    <div class="sharing-link-section">
                        <div class="section-heading">
                            <h5>{{ t('page.sharing.link.title') }}</h5>
                        </div>

                        <div class="link-actions">
                            <button class="btn btn-primary btn-generate" @click="generateShareToken()">
                                {{ t('page.sharing.link.generate') }}
                            </button>
                            <button class="btn btn-secondary btn-renew" @click="renewToken()">
                                {{ t('page.sharing.link.renew') }}
                            </button>
                        </div>

                        <div class="link-info">
                            <p>{{ t('page.sharing.link.info') }}</p>
                        </div>
                    </div>
                </div>

                <!-- Edit permissions mode -->
                <div v-else-if="currentMode === 'edit' && currentUser" class="edit-container">
                    <!-- Selected user info -->
                    <div class="selected-user">
                        <img :src="currentUser.avatarUrl" class="user-avatar large" alt="User avatar" />
                        <span class="user-name">{{ currentUser.name }}</span>
                    </div>

                    <!-- Permission selection -->
                    <div class="permission-selection">
                        <div class="form-group">
                            <label>{{ t('page.sharing.permission.label') }}</label>
                            <VDropdown class="permission-dropdown" :distance="5" :aria-id="ariaId">
                                <div class="permission-dropdown-trigger">
                                    {{permissionOptions.find(option => option.value === currentUser.permission)?.label}}
                                    <font-awesome-icon icon="fa-solid fa-chevron-down" />
                                </div>

                                <template #popper="{ hide }">
                                    <div class="permission-dropdown-menu">
                                        <div
                                            @click="updatePermission(SharePermission.View); hide()"
                                            class="dropdown-item"
                                            :class="{ 'active': currentUser.permission === SharePermission.View }">
                                            {{ t('page.sharing.permission.view') }}
                                        </div>

                                        <div
                                            @click="updatePermission(SharePermission.Edit); hide()"
                                            class="dropdown-item"
                                            :class="{ 'active': currentUser.permission === SharePermission.Edit }">
                                            {{ t('page.sharing.permission.edit') }}
                                        </div>

                                        <div
                                            @click="updatePermission(SharePermission.ViewWithChildren); hide()"
                                            class="dropdown-item"
                                            :class="{ 'active': currentUser.permission === SharePermission.ViewWithChildren }">
                                            {{ t('page.sharing.permission.viewWithChildren') }}
                                        </div>

                                        <div
                                            @click="updatePermission(SharePermission.EditWithChildren); hide()"
                                            class="dropdown-item"
                                            :class="{ 'active': currentUser.permission === SharePermission.EditWithChildren }">
                                            {{ t('page.sharing.permission.editWithChildren') }}
                                        </div>
                                    </div>
                                </template>
                            </VDropdown>

                            <!-- Keep the description for selected permission -->
                            <div class="permission-description">
                                {{ t(`page.sharing.permission.description.${currentUser.permission}`) }}
                            </div>
                        </div>
                    </div>

                    <!-- Notification settings -->
                    <div class="notification-settings">
                        <div class="form-group">
                            <div class="notification-checkbox">
                                <input type="checkbox" id="notify-user" v-model="notifyUser">
                                <label for="notify-user">{{ t('page.sharing.notification.send') }}</label>
                            </div>

                            <div v-if="notifyUser" class="custom-message">
                                <label for="custom-message">{{ t('page.sharing.notification.message') }}</label>
                                <textarea id="custom-message" v-model="customMessage" :placeholder="t('page.sharing.notification.placeholder')" rows="3"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </template>
    </LazyModal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

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

    h5 {
        font-weight: 600;
        margin: 0;
        color: @memo-blue;
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
    padding: 10px;
    border-radius: 4px;
    background-color: @memo-grey-lighter;

    &:hover {
        background-color: darken(@memo-grey-lighter, 3%);
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
    font-weight: 500;
}

.user-detail {
    display: flex;
    align-items: center;
    gap: 12px;
}

.user-permission {
    color: @memo-grey-dark;
    font-size: 0.9rem;
}

.btn-remove {
    background: none;
    border: none;
    cursor: pointer;
    color: @memo-grey-dark;
    padding: 4px 8px;

    &:hover {
        color: @memo-wuwi-red;
    }
}

.sharing-link-section {
    margin-top: 12px;
    padding-top: 12px;
    border-top: 1px solid @memo-grey-light;
}

.link-actions {
    display: flex;
    gap: 12px;
    margin-bottom: 12px;
}

.btn-generate {
    background-color: @memo-blue;
    color: white;

    &:hover {
        background-color: darken(@memo-blue, 10%);
    }
}

.btn-renew {
    background-color: transparent;
    color: @memo-blue;
    border: 1px solid @memo-blue;

    &:hover {
        background-color: fade(@memo-blue, 10%);
    }
}

.link-info {
    color: @memo-grey-dark;
    font-size: 0.9rem;
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
        /* Add this for proper positioning context */

        .permission-dropdown-trigger {
            display: flex;
            justify-content: space-between;
            align-items: center;
            padding: 10px 16px;
            border: 1px solid @memo-grey-light;
            border-radius: 4px;
            background-color: white;
            cursor: pointer;

            &:hover {
                border-color: @memo-blue;
            }
        }

        :deep(.v-popper__popper) {
            /* Add these to ensure proper z-index and width */
            z-index: 1000 !important;
            width: 100% !important;
        }

        .permission-dropdown-menu {
            width: 100%;
            background-color: white;
            border: 1px solid @memo-grey-light;
            border-radius: 4px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            max-width: none !important;
            /* Ensure menu takes full width */
            z-index: 200;
        }
    }

    .permission-description {
        padding: 12px;
        border-radius: 4px;
        background-color: fade(@memo-blue, 5%);
        color: @memo-grey-dark;
        font-size: 0.9rem;
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
            border-radius: 4px;

            &:focus {
                outline: none;
                border-color: @memo-blue;
            }
        }
    }
}
</style>
