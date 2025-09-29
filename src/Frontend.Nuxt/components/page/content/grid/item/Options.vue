<script lang="ts" setup>
import { EditRelationData, EditPageRelationType, useEditPageRelationStore } from '~/components/page/relation/editPageRelationStore'
import { useUserStore } from '~/components/user/userStore'
import { usePublishPageStore } from '~/components/page/publish/publishPageStore'
import { useAlertStore, AlertType } from '~/components/alert/alertStore'
import { Visibility } from '~/components/shared/visibilityEnum'
import { GridPageItem } from './gridPageItem'
import { ImageFormat } from '~/components/image/imageFormatEnum'
import { usePageToPrivateStore } from '~/components/page/toPrivate/pageToPrivateStore'
import { useDeletePageStore } from '~/components/page/delete/deletePageStore'
import { useConvertStore } from '~/components/page/convert/convertStore'

const userStore = useUserStore()
const publishPageStore = usePublishPageStore()
const editPageRelationStore = useEditPageRelationStore()
const alertStore = useAlertStore()
const pageToPrivateStore = usePageToPrivateStore()
const deletePageStore = useDeletePageStore()
const convertStore = useConvertStore()

interface Props {
    page: GridPageItem
    parentId: number
    parentName: string
}

const props = defineProps<Props>()

const { $urlHelper } = useNuxtApp()

const { t } = useI18n()

async function removeParent() {
    const userStore = useUserStore()
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    const data = {
        parentIdToRemove: props.parentId,
        childId: props.page.id,
    }

    const result = await $api<FetchResult<null>>('/apiVue/PageRelationEdit/RemoveParent', {
        method: 'POST',
        body: data
    })
    if (result) {
        if (result.success === true) {
            alertStore.openAlert(AlertType.Success, {
                text: t(result.messageKey)
            })

            editPageRelationStore.removePage(data.childId, data.parentIdToRemove)
        }
        else {
            alertStore.openAlert(AlertType.Error, {
                text: t(result.messageKey)
            })
        }
    }
}

function openMovePageModal() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    const data = {
        pageIdToRemove: props.parentId,
        childId: props.page.id,
        editCategoryRelation: EditPageRelationType.Move,
        pagesToFilter: [props.parentId, props.page.id]
    } as EditRelationData

    editPageRelationStore.openModal(data)
}

const emit = defineEmits(['addPage'])
const showAllLinkOptions = ref<boolean>(false)
const ariaId = useId()
const ariaId2 = useId()

</script>

<template>
    <div class="grid-item-options-container">
        <!-- First dropdown - parent pages -->
        <div class="grid-item-option" v-if="props.page.parents.length > 1">
            <VDropdown :aria-id="ariaId" :distance="6">
                <button v-show="props.page.parents.length > 1" class="" @click.stop>
                    <font-awesome-icon :icon="['fas', 'sitemap']" rotation=180 />
                </button>
                <template #popper>
                    <div class="dropdown-row">
                        <div class="overline-s no-line">{{ t('page.grid.options.parentPages') }}</div>
                    </div>
                    <template v-for="parent in props.page.parents">
                        <LazyNuxtLink class="dropdown-row" v-if="parent.id > 0"
                            :to="$urlHelper.getPageUrl(parent.name, parent.id)">
                            <div class="dropdown-icon">
                                <Image :src="parent.imgUrl" :format="ImageFormat.Page" class="header-page-icon" />
                            </div>
                            <div class="dropdown-label">{{ parent.name }}</div>
                        </LazyNuxtLink>
                    </template>
                </template>
            </VDropdown>
        </div>

        <!-- Lock icon button -->
        <div class="grid-item-option" v-if="props.page.visibility != Visibility.Public">
            <button class="" @click.stop="publishPageStore.openModal(props.page.id)">
                <font-awesome-icon :icon="['fas', 'lock']" />
            </button>
        </div>

        <!-- Main options dropdown -->
        <div class="grid-item-option">
            <VDropdown :aria-id="ariaId2" :distance="1">
                <button class="" @click.stop>
                    <font-awesome-icon :icon="['fa-solid', 'ellipsis-vertical']" />
                </button>
                <template #popper="{ hide }">
                    <!-- Create subpage -->
                    <div @click="emit('addPage', true); hide()" class="dropdown-row">
                        <div class="dropdown-icon">
                            <font-awesome-icon :icon="['fas', 'plus']" />
                        </div>
                        <div class="dropdown-label">{{ t('page.grid.options.createSubpage') }}</div>
                    </div>
                    <div class="divider"></div>

                    <!-- Edit links section -->
                    <div @click="showAllLinkOptions = !showAllLinkOptions" class="dropdown-row"
                        :class="{ 'extended': showAllLinkOptions }">
                        <div class="dropdown-icon">
                            <font-awesome-icon icon="fa-solid fa-link" />
                        </div>
                        <div class="dropdown-label">
                            {{ t('page.grid.options.editLinks') }}
                        </div>
                        <div class="dropdown-icon collapse">
                            <font-awesome-icon :icon="['fas', 'chevron-up']" v-if="showAllLinkOptions" />
                            <font-awesome-icon :icon="['fas', 'chevron-down']" v-else />
                        </div>
                    </div>

                    <div v-if="showAllLinkOptions" class="link-options">
                        <!-- Link parent page -->
                        <div @click="editPageRelationStore.addParent(props.page.id, false); hide()"
                            class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon icon="fa-solid fa-link" />
                            </div>
                            <div class="dropdown-label">
                                {{ t('page.grid.options.linkParentPage') }}
                            </div>
                        </div>

                        <!-- Link subpage -->
                        <div @click="emit('addPage', false); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fas', 'link']" />
                            </div>
                            <div class="dropdown-label">{{ t('page.grid.linkChildPage') }}</div>
                        </div>

                        <!-- Remove parent page -->
                        <div @click="removeParent(); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fa-solid', 'link-slash']" />
                            </div>
                            <div class="dropdown-label">'{{ props.parentName }}' <br /> {{ t('page.grid.options.removeParentPage') }}</div>
                        </div>

                        <!-- Move page -->
                        <div @click="openMovePageModal(); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fa-solid', 'circle-right']" />
                            </div>
                            <div class="dropdown-label">{{ t('page.grid.options.movePage') }}</div>
                        </div>

                        <!-- Add to personal wiki -->
                        <div @click="editPageRelationStore.addToPersonalWiki(props.page.id); hide()"
                            class="dropdown-row"
                            v-if="!props.page.isChildOfPersonalWiki && props.page.id != userStore.personalWiki?.id">
                            <div class="dropdown-icon">
                                <font-awesome-layers>
                                    <font-awesome-icon :icon="['fas', 'house']" />
                                    <font-awesome-icon :icon="['fas', 'square']" transform="shrink-2 down-2 right-1" />
                                    <font-awesome-icon :icon="['fas', 'plus']" transform="shrink-3 down-1 right-1"
                                        style="color: white;" />
                                </font-awesome-layers>
                            </div>
                            <div class="dropdown-label">{{ t('page.grid.options.addToPersonalWiki') }}</div>
                        </div>
                    </div>

                    <!-- Admin options -->
                    <template v-if="userStore.id === props.page.creatorId || userStore.isAdmin">
                        <div class="divider"></div>

                        <!-- Make private -->
                        <div v-if="props.page.visibility === Visibility.Public"
                            @click="pageToPrivateStore.openModal(props.page.id); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fa-solid', 'lock']" />
                            </div>
                            <div class="dropdown-label">{{ t('page.grid.options.makePrivate') }}</div>
                        </div>

                        <!-- Publish page -->
                        <div v-else @click="publishPageStore.openModal(props.page.id); hide()" class="dropdown-row">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fa-solid', 'unlock']" />
                            </div>
                            <div class="dropdown-label">{{ t('page.grid.options.publish') }}</div>
                        </div>

                        <!-- Convert to wiki -->
                        <div v-if="props.page.creatorId === userStore.id && !props.page.isWiki"
                            class="dropdown-row" @click="convertStore.openModal(props.page.id); hide()">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fas', 'folder']" />
                            </div>
                            <div class="dropdown-label">
                                {{ t('page.grid.options.convertToWiki') }}
                            </div>
                        </div>
                        <div class="divider"></div>

                        <!-- Delete page -->
                        <div @click="deletePageStore.openModal(props.page.id); hide()" data-allowed="logged-in"
                            class="dropdown-row" v-if="props.page.canDelete">
                            <div class="dropdown-icon">
                                <font-awesome-icon :icon="['fas', 'trash']" />
                            </div>
                            <div class="dropdown-label">{{ t('page.grid.options.delete') }}</div>
                        </div>
                    </template>
                </template>
            </VDropdown>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.dropdown-row {
    &.extended {
        margin-bottom: 8px;
    }

    .dropdown-icon {
        &.collapse {
            margin-left: 8px;
        }
    }
}

.link-options {
    .dropdown-row {
        padding-left: 44px;
    }
}

.grid-item-options-container {
    display: flex;
    justify-content: center;
    align-items: center;
    flex-wrap: nowrap;
    height: 40px;
    padding-right: 4px;
    color: @memo-grey-dark;

    .grid-item-option {
        width: 32px;
        display: flex;
        justify-content: center;
        align-items: center;
        margin-left: -4px;

        button {
            background: white;
            width: 32px;
            height: 32px;
            border-radius: 32px;
            color: @memo-grey-dark;

            &:hover {
                filter: brightness(0.95)
            }

            &:active {
                filter: brightness(0.85)
            }
        }
    }

}
</style>

<style lang="less">
.open-modal {
    .grid-item-options-container {
        .grid-item-option {
            button {
                background: none;
            }
        }
    }
}
</style>