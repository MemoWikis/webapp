<script lang="ts" setup>
import { useAlertStore, AlertType } from '~/components/alert/alertStore'
import { ImageFormat } from '~/components/image/imageFormatEnum'
import { ToggleState } from '../toggleStateEnum'
import { GridPageItem } from './gridPageItem'
import { useUserStore } from '~/components/user/userStore'
import { EditRelationData, EditPageRelationType, useEditPageRelationStore } from '~/components/page/relation/editPageRelationStore'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { usePublishPageStore } from '~/components/page/publish/publishPageStore'
import { usePageToPrivateStore } from '~/components/page/toPrivate/pageToPrivateStore'
import { useDeletePageStore } from '~/components/page/delete/deletePageStore'
import { TargetPosition } from '~/components/shared/dragStore'

const userStore = useUserStore()
const alertStore = useAlertStore()
const editPageRelationStore = useEditPageRelationStore()
const loadingStore = useLoadingStore()
const publishPageStore = usePublishPageStore()
const pageToPrivateStore = usePageToPrivateStore()
const deletePageStore = useDeletePageStore()

interface Props {
    page: GridPageItem
    toggleState: ToggleState
    parentId: number
    parentName: string
    isDragging: boolean
    dropExpand: boolean
}

const props = defineProps<Props>()

watch(() => props.page.id, async () => {
    children.value = []
    if (childrenLoaded.value)
        await loadChildren(true)
    expanded.value = false
})

watch(() => props.toggleState, (state) => {
    if (state === ToggleState.Collapsed)
        expanded.value = false
    else if (state === ToggleState.Expanded)
        expanded.value = true
})

const expanded = ref<boolean>(false)
watch(expanded, async (val) => {
    if (val && !childrenLoaded.value && props.page.childrenCount > 0)
        await loadChildren()
})

watch(() => props.dropExpand, val => {
    if (val && expanded.value === false)
        expanded.value = true
})

const children = ref<GridPageItem[]>([])
const childrenLoaded = ref<boolean>(false)
const { t } = useI18n()

async function loadChildren(force: boolean = false) {

    if (childrenLoaded.value && !force)
        return

    loadingStore.startLoading()
    const result = await $api<FetchResult<GridPageItem[]>>(`/apiVue/GridItem/GetChildren/${props.page.id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success === true) {
        children.value = result.data
    } else if (result.success === false) {
        alertStore.openAlert(AlertType.Error, { text: t(result.messageKey) })
    }

    childrenLoaded.value = true
    loadingStore.stopLoading()
}

const { $urlHelper } = useNuxtApp()

const detailLabel = computed(() => {
    const { questionCount, childrenCount } = props.page

    const childrenLabel = `${childrenCount} ${t(`page.grid.item.childPage.${childrenCount === 1 ? 'one' : 'other'}`)}`
    const questionLabel = `${questionCount} ${t(`page.grid.item.question.${questionCount === 1 ? 'one' : 'other'}`)}`

    if (childrenCount > 0 && questionCount > 0)
        return `${childrenLabel} ${t('page.grid.item.and')} ${questionLabel}`

    if (childrenCount > 0)
        return childrenLabel

    if (questionCount > 0)
        return questionLabel

    return ''
})

const pagesToFilter = computed(async () => {
    if (!childrenLoaded.value)
        await loadChildren()

    let pagesToFilter = children.value.map(c => c.id)
    pagesToFilter.push(props.page.id)

    return pagesToFilter
})

async function addPage(newPage: boolean) {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    const parent: EditRelationData = {
        parentId: props.page.id,
        editCategoryRelation: newPage
            ? EditPageRelationType.Create
            : EditPageRelationType.AddChild,
        pagesToFilter: await pagesToFilter.value,
    }
    editPageRelationStore.openModal(parent)
}

editPageRelationStore.$onAction(({ after, name }) => {
    if (name === 'addPage') {
        after((result) => {

            if (result.parentId === props.page.id) {
                if (children.value.some(c => c.id === result.childId))
                    reloadGridItem(result.childId)
                else
                    addGridItem(result.childId)
            } else if (children.value.some(c => c.id === result.childId))
                reloadGridItem(result.childId)
        })
    }
    if (name === 'removePage') {
        after((result) => {
            if (result.parentId === props.page.id) {
                removeGridItem(result.childId)
            }
        })
    }
    if (name === 'addToPersonalWiki' || name === 'removeFromPersonalWiki') {
        after((result) => {
            if (result?.success && result.id && children.value.some(c => c.id === result.id))
                reloadGridItem(result.id)
        })
    }
})

publishPageStore.$onAction(({ after, name }) => {
    if (name === 'publish') {
        after((result) => {
            if (result?.success && result.id && children.value.some(c => c.id === result.id))
                reloadGridItem(result.id)
        })
    }
})

pageToPrivateStore.$onAction(({ after, name }) => {
    if (name === 'setToPrivate') {
        after((result) => {
            if (result?.success && result.id && children.value.some(c => c.id === result.id))
                reloadGridItem(result.id)
        })
    }
})

deletePageStore.$onAction(({ after, name }) => {
    if (name === 'deletePage') {
        after((result) => {
            if (result && result.id && children.value.some(c => c.id === result.id)) {
                removeGridItem(result.id)
            }
        })
    }
})

function removeGridItem(id: number) {
    const filteredGridItems = children.value.filter(i => i.id != id)
    children.value = filteredGridItems
}

async function addGridItem(id: number) {

    if (!childrenLoaded.value) {
        await loadChildren()
    }
    await nextTick()
    if (children.value.findIndex(c => c.id === id) > 0)
        return

    const result = await loadGridItem(id)

    if (result.success === true) {
        if (children.value.some(c => c.id === result.data.id))
            reloadGridItem(result.data.id)
        else
            children.value.push(result.data)
    } else if (result.success === false)
        alertStore.openAlert(AlertType.Error, { text: t(result.messageKey) })
}

async function loadGridItem(id: number) {
    const result = await $api<FetchResult<GridPageItem>>(`/apiVue/GridItem/GetItem/${id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include'
    })
    return result
}
async function reloadGridItem(id: number) {
    const result = await loadGridItem(id)

    if (result.success === true) {
        children.value = children.value.map(i => i.id === result.data.id ? result.data : i)
    } else if (result.success === false)
        alertStore.openAlert(AlertType.Error, { text: t(result.messageKey) })
}

const dragActive = ref(false)
watch(() => props.isDragging, (val) => {
    dragActive.value = val
}, { immediate: true })

editPageRelationStore.$onAction(({ name, after }) => {
    if (name === 'movePage' || name === 'cancelMovePage') {

        after(async (result) => {
            if (result) {
                if (result.oldParentId === props.page.id || result.newParentId === props.page.id)
                    await loadChildren(true)

                const parentHasChanged = result.oldParentId != result.newParentId

                if (children.value.find(c => c.id === result.oldParentId))
                    await reloadGridItem(result.oldParentId)
                if (children.value.find(c => c.id === result.newParentId) && parentHasChanged)
                    await reloadGridItem(result.newParentId)
            }
        })
    }

    if (name === 'tempInsert') {
        after((result) => {

            if (result.oldParentId === props.page.id) {
                const index = children.value.findIndex(c => c.id === result.movePage.id)
                if (index !== -1) {
                    children.value.splice(index, 1)
                }
            }

            if (result.newParentId === props.page.id) {
                const index = children.value.findIndex(c => c.id === result.targetId)
                if (result.position === TargetPosition.Before)
                    children.value.splice(index, 0, result.movePage)
                else if (result.position === TargetPosition.After)
                    children.value.splice(index + 1, 0, result.movePage)
                else if (result.position === TargetPosition.Inner)
                    children.value.push(result.movePage)
            }
        })
    }
})
const { isDesktop } = useDevice()
</script>

<template>
    <div class="grid-item" @click="expanded = !expanded"
        :class="{ 'no-children': props.page.childrenCount <= 0 && children.length <= 0 }">

        <slot name="topdropzone"></slot>
        <slot name="bottomdropzone"
            v-if="!expanded || ((children.length === 0 && childrenLoaded) || props.page.childrenCount === 0)"></slot>
        <slot name="dropinzone"></slot>

        <div class="grid-item-caret-container">
            <font-awesome-icon :icon="['fas', 'caret-right']" class="expand-caret" v-if="props.page.childrenCount > 0"
                :class="{ 'expanded': expanded }" />
        </div>

        <div class="grid-item-body-container">
            <div class="item-img-container">
                <Image :src="props.page.imageUrl" :format="ImageFormat.Page" />
            </div>
            <div class="item-body">

                <div class="item-name">
                    <NuxtLink :to="$urlHelper.getPageUrl(props.page.name, props.page.id)" @click.stop>
                        {{ props.page.name }}
                    </NuxtLink>
                </div>

                <template v-if="detailLabel.length > 0">

                    <div class="item-detaillabel">
                        {{ detailLabel }}
                    </div>
                    <PageContentGridKnowledgebar :knowledgebar-data="props.page.knowledgebarData"
                        v-if="props.page.questionCount > 0" />

                </template>

            </div>
        </div>

        <PageContentGridItemOptions :page="props.page" :parent-id="props.parentId" @add-page="addPage" :parent-name="props.parentName" />
    </div>

    <div v-if="props.page.childrenCount > 0 && expanded && !dragActive" class="grid-item-children">
        <template v-if="isDesktop">
            <PageContentGridDndItem v-for="child in children" :page="child" :toggle-state="props.toggleState"
                :parent-id="props.page.id" :parent-name="props.page.name"
                :user-is-creator-of-parent="props.page.creatorId === userStore.id"
                :parent-visibility="props.page.visibility" />
        </template>
        <template v-else>
            <PageContentGridTouchDndItem v-for="child in children" :page="child" :toggle-state="props.toggleState"
                :parent-id="props.page.id" :parent-name="props.page.name"
                :user-is-creator-of-parent="props.page.creatorId === userStore.id"
                :parent-visibility="props.page.visibility" />
        </template>
    </div>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.grid-item {
    user-select: none;
    display: flex;
    justify-content: space-between;
    flex-wrap: nowrap;
    padding: 10px 0px;
    background: white;
    border-top: solid 1px @memo-grey-light;
    position: relative;

    &:hover {
        filter: brightness(0.975)
    }

    &:active {
        filter: brightness(0.95)
    }

    .grid-item-caret-container {
        width: 32px;
        height: 100%;
        min-height: 40px;
        min-width: 32px;

        display: flex;
        justify-content: center;
        align-items: center;
        color: @memo-grey-dark;

        .expand-caret {
            // transition: 0.1s ease-in-out;

            &.expanded {
                transform: rotate(90deg)
            }

            &.no-children {
                color: @memo-grey-light;
            }
        }
    }

    .grid-item-body-container {
        display: flex;
        justify-content: flex-start;
        flex-wrap: nowrap;
        width: 100%;

        .item-img-container {
            width: 40px;
            height: 40px;
            min-width: 40px;
        }

        .item-body {
            padding-left: 8px;
            width: 100%;

            .item-name {
                font-size: 18px;
                word-break: break-all;

                a {
                    cursor: pointer;
                }
            }

            .item-detaillabel {
                color: @memo-grey-dark;
                font-size: 12px;
                min-height: 18px;
            }
        }
    }
}

.grid-item-children {
    user-select: none;
    padding-left: 16px;
}
</style>

<style lang="less">
.open-modal {
    .grid-item {
        background: none;
    }
}
</style>