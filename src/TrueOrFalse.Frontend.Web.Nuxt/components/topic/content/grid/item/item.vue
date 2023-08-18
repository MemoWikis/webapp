<script lang="ts" setup>
import { useAlertStore, AlertType, messages } from 'components/alert/alertStore'
import { Visibility } from 'components/shared/visibilityEnum'
import { TinyTopicModel } from 'components/topic/topicStore'
import { ImageFormat } from 'components/image/imageFormatEnum'

import { KnowledgebarData } from '../knowledgebar/knowledgebar.vue'

const alertStore = useAlertStore()

export interface GridTopicItem {
    id: number
    name: string
    questionCount: number
    childrenCount: number
    imageUrl: string
    visibility: Visibility
    parents: TinyTopicModel[]
    knowledgebarData: KnowledgebarData
}

enum ToggleState {
    Collapse,
    Expand,
    None
}

interface Props {
    topic: GridTopicItem
    toggleState: ToggleState
    parentId: number
}

const props = defineProps<Props>()

watch(() => props.toggleState, (state) => {
    if (state == ToggleState.Collapse)
        expanded.value = false
    else if (state == ToggleState.Expand)
        expanded.value = true
})

const expanded = ref<boolean>(false)
watch(expanded, (val) => {
    if (val && !childrenLoaded)
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
            <font-awesome-icon :icon="['fas', 'caret-right']" :class="{ 'expanded': expanded }" />
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
            <div v-if="props.topic.childrenCount > 0" v-show="expanded && children.length > 0">
                <TopicContentGridItem v-for="child in children" :topic="child" :toggle-state="props.toggleState"
                    :parent-id="props.topic.id" />
            </div>
        </div>

        <div class="grid-item-options-container">

            <TopicContentGridItemOptions :topic="props.topic" :parent-id="props.parentId" />

        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';
</style>