<script lang="ts" setup>
import { useSharePageStore, SharePermission } from './sharePageStore'
import { useUserStore } from '~~/components/user/userStore'
import { SearchType, UserItem } from '~~/components/search/searchHelper'
import { useLoadingStore } from '~/components/loading/loadingStore'

const sharePageStore = useSharePageStore()
const userStore = useUserStore()
const loadingStore = useLoadingStore()
const { t } = useI18n()

const selectedUsers = ref<any[]>([])
const sharePermission = ref<SharePermission>(SharePermission.View)

const permissionOptions = [
    { value: SharePermission.View, label: t('page.sharing.permission.view') },
    { value: SharePermission.Edit, label: t('page.sharing.permission.edit') },
    { value: SharePermission.ViewWithChildren, label: t('page.sharing.permission.viewWithChildren') },
    { value: SharePermission.EditWithChildren, label: t('page.sharing.permission.editWithChildren') }
]

function addSelectedUser(user: UserItem) {
    if (!selectedUsers.value.some(u => u.id === user.id)) {
        selectedUsers.value.push({
            id: user.id,
            name: user.name,
            imageUrl: user.imageUrl,
            permission: sharePermission.value
        })
    }
}

function removeUser(userId: number) {
    selectedUsers.value = selectedUsers.value.filter(u => u.id !== userId)
}

function updateUserPermission(userId: number, permission: SharePermission) {
    const user = selectedUsers.value.find(u => u.id === userId)
    if (user) {
        user.permission = permission
    }
}

async function saveSharing() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    loadingStore.startLoading()

    // Map the user permissions to the format expected by the store
    sharePageStore.setUserIds(selectedUsers.value.map(u => u.id))
    sharePageStore.setPermission(sharePermission.value)

    const result = await sharePageStore.sharePage()

    loadingStore.stopLoading()

    if (result.success) {
        sharePageStore.closeModal()
    }
}

async function renewToken() {
    loadingStore.startLoading()
    const result = await sharePageStore.renewShareToken(sharePageStore.pageId)
    loadingStore.stopLoading()
}

watch(() => sharePageStore.showModal, (show) => {
    if (!show) {
        selectedUsers.value = []
        sharePermission.value = SharePermission.View
    }
})
</script>

<template>
    <LazyModal @close="sharePageStore.closeModal()" :show="sharePageStore.showModal"
        primary-btn-label="Share" @primary-btn="saveSharing()"
        :show-cancel-btn="true">

        <template v-slot:header>
            <h4 class="modal-title">{{ t('page.sharing.modal.title') }}</h4>
        </template>

        <template v-slot:body>
            <div class="sharing-container">
                <!-- User search -->
                <div class="search-container">
                    <Search
                        :search-type="SearchType.users"
                        :show-search="true"
                        :show-search-icon="true"
                        :placeholder-label="t('page.sharing.search.placeholder')"
                        @select-item="addSelectedUser" />
                </div>

                <!-- Selected users list -->
                <div class="selected-users" v-if="selectedUsers.length > 0">
                    <div class="selected-user-item" v-for="user in selectedUsers" :key="user.id">
                        <div class="user-info">
                            <img :src="user.imageUrl" class="user-avatar" alt="User avatar" />
                            <span class="user-name">{{ user.name }}</span>
                        </div>

                        <div class="user-actions">
                            <div class="permission-dropdown">
                                <VDropdown class="form-select permission-select">
                                    <template #trigger>
                                        <div class="select-trigger">
                                            {{permissionOptions.find(option => option.value === user.permission)?.label}}
                                            <font-awesome-icon icon="fa-solid fa-chevron-down" class="select-icon" />
                                        </div>
                                    </template>
                                    <template #default>
                                        <div class="dropdown-menu">
                                            <div
                                                v-for="option in permissionOptions"
                                                :key="option.value"
                                                @click="updateUserPermission(user.id, option.value)"
                                                class="dropdown-item"
                                                :class="{ 'active': option.value === user.permission }">
                                                {{ option.label }}
                                            </div>
                                        </div>
                                    </template>
                                </VDropdown>
                            </div>

                            <button class="btn-remove" @click="removeUser(user.id)">
                                <font-awesome-icon icon="fa-solid fa-xmark" />
                            </button>
                        </div>
                    </div>
                </div>

                <!-- Sharing link section -->
                <div class="sharing-link-section">
                    <div class="link-header">
                        <h5>{{ t('page.sharing.link.title') }}</h5>
                        <button class="btn-renew" @click="renewToken()">
                            {{ t('page.sharing.link.renew') }}
                        </button>
                    </div>
                    <div class="link-info">
                        <p>{{ t('page.sharing.link.info') }}</p>
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
    gap: 16px;
}

.search-container {
    padding-bottom: 16px;
    border-bottom: 1px solid @memo-grey-light;
}

.selected-users {
    display: flex;
    flex-direction: column;
    gap: 8px;
}

.selected-user-item {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 8px;
    border-radius: 4px;
    background-color: @memo-grey-lighter;

    &:hover {
        background-color: darken(@memo-grey-lighter, 3%);
    }
}

.user-info {
    display: flex;
    align-items: center;
    gap: 8px;
}

.user-avatar {
    width: 32px;
    height: 32px;
    border-radius: 50%;
    object-fit: cover;
}

.user-actions {
    display: flex;
    align-items: center;
    gap: 8px;
}

.permission-select {
    min-width: 150px;
}

.permission-dropdown {
    position: relative;

    .select-trigger {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 6px 12px;
        background: white;
        border: 1px solid @memo-grey-light;
        border-radius: 4px;
        cursor: pointer;
        min-width: 150px;

        &:hover {
            border-color: @memo-blue;
        }

        .select-icon {
            margin-left: 8px;
            font-size: 0.8rem;
        }
    }

    .dropdown-menu {
        min-width: 150px;
        background: white;
        border-radius: 4px;
        box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);

        .dropdown-item {
            padding: 8px 12px;
            cursor: pointer;

            &:hover {
                background-color: @memo-grey-lighter;
            }

            &.active {
                background-color: @memo-blue-lighter;
                font-weight: 500;
            }
        }
    }
}

.btn-remove {
    background: none;
    border: none;
    cursor: pointer;
    color: @memo-grey-dark;

    &:hover {
        color: @memo-blue;
    }
}

.sharing-link-section {
    margin-top: 16px;
    padding-top: 16px;
    border-top: 1px solid @memo-grey-light;
}

.link-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 8px;

    h5 {
        margin: 0;
    }
}

.btn-renew {
    background-color: @memo-blue;
    color: white;
    border: none;
    padding: 4px 12px;
    border-radius: 4px;
    cursor: pointer;

    &:hover {
        background-color: darken(@memo-blue, 10%);
    }
}

.link-info {
    color: @memo-grey-dark;
    font-size: 0.9rem;
}
</style>
