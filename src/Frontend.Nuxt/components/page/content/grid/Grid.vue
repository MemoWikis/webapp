<script lang="ts" setup>
import { usePageStore } from '~/components/page/pageStore'
import { ToggleState } from './toggleStateEnum'
import { GridPageItem } from './item/gridPageItem'
import { EditRelationData, EditPageRelationType, useEditPageRelationStore } from '~/components/page/relation/editPageRelationStore'
import { useUserStore } from '~/components/user/userStore'
import { AlertType, useAlertStore } from '~/components/alert/alertStore'
import { usePublishPageStore } from '~/components/page/publish/publishPageStore'
import { usePageToPrivateStore } from '~/components/page/toPrivate/pageToPrivateStore'
import { useDeletePageStore } from '~/components/page/delete/deletePageStore'
import { TargetPosition, useDragStore } from '~/components/shared/dragStore'
import { useAiCreatePageStore } from '~/components/page/content/ai/aiCreatePageStore'

const pageStore = usePageStore()
const editPageRelationStore = useEditPageRelationStore()
const userStore = useUserStore()
const alertStore = useAlertStore()
const publishPageStore = usePublishPageStore()
const pageToPrivateStore = usePageToPrivateStore()
const deletePageStore = useDeletePageStore()
const dragStore = useDragStore()
const aiCreatePageStore = useAiCreatePageStore()

interface Props {
    children: GridPageItem[]
}
const props = defineProps<Props>()

const toggleState = ref(ToggleState.Collapsed)

const pagesToFilter = computed<number[]>(() => {

    let pagesToFilter = import.meta.server ? props.children.map(c => c.id) : pageStore.gridItems.map(c => c.id)
    pagesToFilter.push(pageStore.id)

    return pagesToFilter
})

function addPage(newPage: boolean) {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    const parent: EditRelationData = {
        parentId: pageStore.id,
        editCategoryRelation: newPage
            ? EditPageRelationType.Create
            : EditPageRelationType.AddChild,
        pagesToFilter: pagesToFilter.value,
    }
    editPageRelationStore.openModal(parent)
}

function openAiCreatePage() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    aiCreatePageStore.openModal(pageStore.id)
}

editPageRelationStore.$onAction(({ after, name }) => {
    if (name === 'addPage') {
        after((result) => {
            if (result.parentId === pageStore.id) {
                addGridItem(result.childId)
            } else if (pageStore.gridItems.some(c => c.id === result.parentId)) {
                reloadGridItem(result.parentId)
            } else if (pageStore.gridItems.some(c => c.id === result.childId))
                reloadGridItem(result.childId)

        })
    }
    if (name === 'removePage') {
        after((result) => {
            if (result.parentId === pageStore.id) {
                removeGridItem(result.childId)
            }
        })
    }
    if (name === 'addToPersonalWiki' || name === 'removeFromPersonalWiki') {
        after((result) => {
            if (result) {
                reloadGridItem(result.id)
            }
        })
    }
})

publishPageStore.$onAction(({ after, name }) => {
    if (name === 'publish') {
        after((result) => {
            if (result?.success && result.id && pageStore.gridItems.some(c => c.id === result.id))
                reloadGridItem(result.id)
        })
    }
})

pageToPrivateStore.$onAction(({ after, name }) => {
    if (name === 'setToPrivate') {
        after((result) => {
            if (result?.success && result.id && pageStore.gridItems.some(c => c.id === result.id))
                reloadGridItem(result.id)
        })
    }
})

deletePageStore.$onAction(({ after, name }) => {
    if (name === 'deletePage') {
        after((result) => {
            if (result && result.id && pageStore.gridItems.some(c => c.id === result.id)) {
                removeGridItem(result.id)
            }
        })
    }
})
const { t } = useI18n()

async function addGridItem(id: number) {
    const result = await loadGridItem(id)

    if (result.success === true) {
        pageStore.gridItems.push(result.data)
    } else if (result.success === false)
        alertStore.openAlert(AlertType.Error, { text: t(result.messageKey) })
}

async function loadGridItem(id: number) {
    const result = await $api<FetchResult<GridPageItem>>(`/apiVue/Grid/GetItem/${id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include'
    })
    return result
}

async function reloadGridItem(id: number) {
    const result = await loadGridItem(id)
    if (result.success === true) {
        pageStore.gridItems = pageStore.gridItems.map(i => i.id === result.data.id ? result.data : i)
    } else if (result.success === false)
        alertStore.openAlert(AlertType.Error, { text: t(result.messageKey) })
}

function removeGridItem(id: number) {
    const filteredGridItems = pageStore.gridItems.filter(i => i.id != id)
    pageStore.gridItems = filteredGridItems
}

const { isMobile, isDesktop } = useDevice()

editPageRelationStore.$onAction(({ name, after }) => {
    if (name === 'movePage' || name === 'cancelMovePage') {

        after(async (result) => {
            if (result) {
                if (result?.oldParentId === pageStore.id || result?.newParentId === pageStore.id)
                    await pageStore.reloadGridItems()

                const parentHasChanged = result.oldParentId != result.newParentId

                if (props.children.find(c => c.id === result.oldParentId))
                    await reloadGridItem(result.oldParentId)
                if (props.children.find(c => c.id === result.newParentId) && parentHasChanged)
                    await reloadGridItem(result.newParentId)
            }
        })
    }

    if (name === 'tempInsert') {
        after((result) => {

            if (result.oldParentId === pageStore.id) {
                const index = pageStore.gridItems.findIndex(c => c.id === result.movePage.id)
                if (index !== -1) {
                    pageStore.gridItems.splice(index, 1)
                }
            }

            if (result.newParentId === pageStore.id) {
                const index = pageStore.gridItems.findIndex(c => c.id === result.targetId)
                if (result.position === TargetPosition.Before)
                    pageStore.gridItems.splice(index, 0, result.movePage)
                else if (result.position === TargetPosition.After)
                    pageStore.gridItems.splice(index + 1, 0, result.movePage)
                else if (result.position === TargetPosition.Inner)
                    pageStore.gridItems.push(result.movePage)
            }
        })
    }
})

</script>

<template>
    <div class="row grid-row" id="PageGrid">
        <div class="col-xs-12">
            <div class="grid-container">
                <div class="grid-header ">
                    <div class="grid-title no-line" :class="{ 'overline-m': !isMobile, 'overline-s': isMobile }">
                        {{ isMobile ? t('page.grid.childPagesMobile') : t('page.grid.childPages') }} ({{ pageStore.childPageCount }})
                    </div>

                    <div class="grid-options">
                        <div class="grid-option">
                            <button @click="addPage(true)">
                                <font-awesome-icon :icon="['fas', 'plus']" />
                            </button>
                        </div>
                        <div class="grid-option">
                            <button @click="addPage(false)">
                                <font-awesome-icon :icon="['fas', 'link']" />
                            </button>
                        </div>
                        <div class="grid-option">
                            <button @click="openAiCreatePage()" :title="t('page.ai.createPage.buttonTitle')">
                                <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" />
                            </button>
                        </div>
                    </div>
                </div>

                <div class="grid-items">
                    <template v-if="isDesktop">
                        <PageContentGridDragAndDropItem v-for="(page, index) in props.children" :page="page" :toggle-state="toggleState"
                            :parent-id="pageStore.id" :parent-name="pageStore.name"
                            :user-is-creator-of-parent="pageStore.currentUserIsCreator"
                            :parent-visibility="pageStore.visibility!"
                            :previous-page-id="props.children[index - 1]?.id"
                            :next-page-id="props.children[index + 1]?.id" />
                    </template>
                    <template v-else>
                        <PageContentGridTouchDragAndDropItem v-for="c in props.children" :page="c" :toggle-state="toggleState"
                            :parent-id="pageStore.id" :parent-name="pageStore.name"
                            :user-is-creator-of-parent="pageStore.currentUserIsCreator"
                            :parent-visibility="pageStore.visibility!" />
                    </template>
                </div>


                <div class="grid-footer">
                    <div class="grid-option overline-m no-line no-margin">
                        <button @click="addPage(true)">
                            <font-awesome-icon :icon="['fas', 'plus']" />
                            <span class="button-label" :class="{ 'is-mobile': isMobile }">
                                {{ isMobile ? t('page.grid.createChildPageMobile') : t('page.grid.createChildPage') }}
                            </span>
                        </button>
                    </div>
                    <div class="grid-divider" :class="{ 'is-mobile': isMobile }"></div>
                    <div class="grid-option overline-m no-line no-margin">
                        <button @click="addPage(false)">
                            <font-awesome-icon :icon="['fas', 'link']" />
                            <span class="button-label" :class="{ 'is-mobile': isMobile }">
                                {{ isMobile ? t('page.grid.linkChildPageMobile') : t('page.grid.linkChildPage') }}
                            </span>
                        </button>
                    </div>
                    <div class="grid-divider" :class="{ 'is-mobile': isMobile }"></div>
                    <div class="grid-option overline-m no-line no-margin">
                        <button @click="openAiCreatePage()">
                            <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" />
                            <span class="button-label" :class="{ 'is-mobile': isMobile }">
                                {{ isMobile ? t('page.ai.createPage.buttonTitleMobile') : t('page.ai.createPage.buttonTitle') }}
                            </span>
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <LazyClientOnly>
            <PageContentGridDragAndDropGhost v-show="dragStore.active" />
            <PageContentGridDragStartIndicator v-if="dragStore.showTouchSpinner" />
        </LazyClientOnly>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.grid-row {
    margin-top: 20px;
    max-width: calc(100vw - 20px);
    margin-bottom: 45px;

    .grid-container {
        margin-bottom: 45px;
    }

    .no-margin {
        margin-right: 0px;
        margin-left: 0px;
        margin-top: 0px;
        margin-bottom: 0px;
    }

    .grid-header {
        min-height: 40px;
        height: 40px;
        justify-content: space-between;

        .grid-options {
            display: flex;
            justify-content: flex-end;
            align-items: center;
        }

        .grid-title {
            margin-bottom: 0px;
        }
    }

    .grid-header,
    .grid-footer {
        display: flex;
        flex-wrap: nowrap;
        align-items: center;

        button {
            color: @memo-grey-darker;
            background: white;
            height: 32px;
            border-radius: 32px;
            min-width: 32px;
            display: flex;
            flex-wrap: nowrap;
            align-items: center;
            justify-content: center;

            .button-label {
                color: @memo-grey-dark;
                padding: 0 4px;
                text-wrap: nowrap;

                &.is-mobile {
                    line-height: 16px;
                }
            }

            &:hover {
                filter: brightness(0.95)
            }

            &:active {
                filter: brightness(0.85)
            }
        }

        .grid-divider {
            margin: 0 8px;
            height: 22px;
            border-left: solid 1px @memo-grey-light;
        }
    }

    .grid-footer {
        border-top: solid 1px @memo-grey-light;
        padding-top: 4px;
        overflow: hidden;

        .grid-divider {
            &.is-mobile {
                margin: 0 4px;
            }
        }
    }
}

#PageGrid {
    touch-action: pan-y;
}
</style>

<style lang="less">
.open-modal {

    .grid-container,
    .grid-header {
        .grid-option {
            button {
                background: none;
            }
        }
    }
}
</style>