<script lang="ts" setup>
import { SharePermission, UserWithPermission } from './sharePageStore'

const props = defineProps<{
    show: boolean
    user: UserWithPermission | null
    currentPageName: string
}>()

const emit = defineEmits<{
    (e: 'close'): void
    (e: 'removeFromItem'): void
    (e: 'removeFromParent'): void
}>()

const { t } = useI18n()

const permissionLabel = computed(() => {
    if (!props.user) return ''

    switch (props.user.permission) {
        case SharePermission.View:
            return t('page.sharing.permission.view')
        case SharePermission.Edit:
            return t('page.sharing.permission.edit')
        case SharePermission.ViewWithChildren:
            return t('page.sharing.permission.viewWithChildren')
        case SharePermission.EditWithChildren:
            return t('page.sharing.permission.editWithChildren')
        default:
            return ''
    }
})
</script>

<template>
    <LazyModal :show="show" @close="emit('close')">
        <template v-slot:header>
            <h4 class="modal-title">{{ t('page.sharing.inherited.removeTitle') }}</h4>
        </template>

        <template v-slot:body>
            <div class="remove-inherited-container">
                <p class="description">
                    {{ t('page.sharing.inherited.description', {
                        userName: user?.name,
                        pageName: currentPageName,
                        parentName: user?.inheritedFrom?.name
                    }) }}
                </p>

                <div class="inheritance-tree">
                    <div class="tree-item parent-item">
                        <div class="file-icon">
                            <font-awesome-icon :icon="['fas', 'file']" class="icon-gray" />
                            <div class="dot-indicator">
                                <font-awesome-icon :icon="['fas', 'ellipsis-h']" class="icon-dots" />
                            </div>
                        </div>
                        <div class="item-details">
                            <div class="item-name">{{ user?.inheritedFrom?.name }}</div>
                            <div class="item-actions">
                                <div class="item-permission">{{ permissionLabel }}</div>
                                <font-awesome-icon :icon="['fas', 'chevron-right']" class="icon-chevron" />
                                <div class="item-remove-action">{{ t('page.sharing.permission.remove') }}</div>
                            </div>
                        </div>
                    </div>

                    <div class="tree-item child-item">
                        <div class="file-icon current-page">
                            <font-awesome-icon :icon="['fas', 'file']" class="icon-green" />
                        </div>
                        <div class="item-details">
                            <div class="item-name">{{ currentPageName }}</div>
                            <div class="item-actions">
                                <div class="item-permission">{{ permissionLabel }}</div>
                                <font-awesome-icon :icon="['fas', 'chevron-right']" class="icon-chevron" />
                                <div class="item-remove-action">{{ t('page.sharing.permission.remove') }}</div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </template>

        <template v-slot:footer>
            <div class="actions">
                <button class="btn btn-link memo-button" @click="emit('close')">
                    {{ t('label.cancel') }}
                </button>
                <div class="remove-options">
                    <button class="btn btn-link memo-button" @click="emit('removeFromItem')">
                        {{ t('page.sharing.inherited.removeAccess') }}
                    </button>
                    <button class="btn btn-primary memo-button" @click="emit('removeFromParent')">
                        {{ t('page.sharing.inherited.removeFromParent') }}
                    </button>
                </div>
            </div>
        </template>
    </LazyModal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.remove-inherited-container {
    display: flex;
    flex-direction: column;
    gap: 24px;
}

.description {
    font-size: 1.4rem;
    line-height: 1.5;
    color: @memo-grey-darker;
}

.inheritance-tree {
    display: flex;
    flex-direction: column;
    margin: 16px 0;
    border-radius: 8px;
    overflow: hidden;
}

.tree-item {
    display: flex;
    align-items: center;
    gap: 12px;
    padding: 12px 0px;
    position: relative;

    &.parent-item {}

    &.child-item {
        margin-left: 20px;
        position: relative;
        padding-left: 4px;

        &:before {
            content: '';
            position: absolute;
            left: -10px;
            top: -8px;
            height: 38px;
            width: 2px;
            background-color: @memo-grey-light;
            z-index: 1;
            border-bottom-left-radius: 24px;
            border-top-right-radius: 24px;
            border-top-left-radius: 24px;
        }

        &:after {
            content: '';
            position: absolute;
            left: -8px;
            top: 28px;
            height: 2px;
            width: 8px;
            background-color: @memo-grey-light;
            border-bottom-right-radius: 24px;
            border-top-right-radius: 24px;
        }
    }
}

.folder-icon,
.file-icon {
    width: 24px;
    height: 24px;
    position: relative;
    display: flex;
    align-items: center;
    justify-content: center;

    .icon-gray {
        color: @memo-grey-darker;
        font-size: 20px;
    }

    .icon-green {
        color: @memo-blue;
        font-size: 20px;
    }
}

.dot-indicator {
    position: absolute;
    right: 0px;
    bottom: -36px;
    background: @memo-grey-lighter;
    height: 16px;
    width: 16px;
    justify-content: center;
    align-items: center;
    display: flex;
    border-radius: 16px;
    left: 3px;
    z-index: 4;

    .icon-dots {
        color: @memo-grey-dark;
        font-size: 12px;
    }
}

.item-details {
    flex: 1;
    overflow: hidden;
}

.parent-item {
    .item-details {
        margin-left: 24px;
    }
}

.item-name {
    font-weight: 500;
    color: #202124;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    margin-bottom: 4px;
}

.item-actions {
    display: flex;
    align-items: center;
    gap: 8px;
    font-size: 1.25rem;
}

.item-permission {
    color: @memo-grey-dark;
    font-weight: 500;
    text-decoration: line-through;
}

.icon-chevron {
    color: @memo-grey-dark;
    font-size: 12px;
}

.item-remove-action {
    color: @memo-grey-dark;
}

.actions {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
    margin-top: 16px;
}

.remove-options {
    display: flex;
    gap: 12px;
}
</style>