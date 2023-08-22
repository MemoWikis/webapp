<script lang="ts" setup>
import { useAlertStore, AlertType, messages } from '~/components/alert/alertStore'
import { ImageFormat } from '~/components/image/imageFormatEnum'
import { ToggleState } from '../toggleStateEnum'
import { GridTopicItem } from './gridTopicItem'

const alertStore = useAlertStore()

interface Props {
    topic: GridTopicItem
    toggleState: ToggleState
    parentId: number
}

const props = defineProps<Props>()

watch(() => props.toggleState, (state) => {
    if (state == ToggleState.Collapsed)
        expanded.value = false
    else if (state == ToggleState.Expanded)
        expanded.value = true
})

const expanded = ref<boolean>(false)
watch(expanded, (val) => {
    if (val && !childrenLoaded.value)
        loadChildren()
})
const children = ref<GridTopicItem[]>([])
const childrenLoaded = ref<boolean>(false)

async function loadChildren() {

    if (childrenLoaded.value)
        return

    const result = await $fetch<FetchResult<GridTopicItem[]>>(`/apiVue/GridItem/GetChildren?id=${props.topic.id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include'
    })

    if (result.success == true) {
        children.value = result.data
    } else if (result.success == false) {
        alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
    }

    childrenLoaded.value = true
}

const { $urlHelper } = useNuxtApp()

const optionsText = computed(() => {
    return ''
})
</script>

<template>
    <div class="grid-item" @click="expanded = !expanded">

        <div class="grid-item-caret-container">
            <font-awesome-icon :icon="['fas', 'caret-right']" class="expand-caret" :class="{ 'expanded': expanded }" />
        </div>

        <div class="grid-item-body-container">
            <div class="item-img-container">
                <Image :src="props.topic.imageUrl" :format="ImageFormat.Topic" />
            </div>
            <div class="item-body">

                <div class="item-name">
                    <NuxtLink :to="$urlHelper.getTopicUrl(props.topic.name, props.topic.id)">
                        {{ props.topic.name }}
                    </NuxtLink>
                </div>

                <div class="item-data" v-if="props.topic.childrenCount > 0 || props.topic.questionCount > 0">
                    {{ optionsText }}
                </div>

                <div class="item-knowledgebar">
                </div>

                <TopicContentGridKnowledgebar :knowledgebar-data="props.topic.knowledgebarData" />

            </div>
        </div>

        <TopicContentGridItemOptions :topic="props.topic" :parent-id="props.parentId" />
    </div>

    <div v-if="props.topic.childrenCount > 0" v-show="expanded && children.length > 0" class="grid-item-children">
        <TopicContentGridItem v-for="child in children" :topic="child" :toggle-state="props.toggleState"
            :parent-id="props.topic.id" />
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

    &:hover {
        filter: brightness(0.975)
    }

    &:active {
        filter: brightness(0.95)
    }

    .grid-item-caret-container {
        cursor: pointer;
        width: 40px;
        height: 100%;
        min-height: 40px;

        display: flex;
        justify-content: center;
        align-items: center;
        color: @memo-grey-dark;

        .expand-caret {
            // transition: 0.1s ease-in-out;

            &.expanded {
                transform: rotate(90deg)
            }
        }
    }

    .grid-item-body-container {
        cursor: pointer;
        display: flex;
        justify-content: flex-start;
        flex-wrap: nowrap;
        width: 100%;

        .item-img-container {
            width: 40px;
            height: 40px;
        }

        .item-body {
            padding-left: 8px;
        }
    }
}

.grid-item-children {
    user-select: none;
    padding-left: 40px;
}
</style>