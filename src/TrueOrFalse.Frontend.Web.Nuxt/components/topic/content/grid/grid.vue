<script lang="ts" setup>
import { useTopicStore } from '~/components/topic/topicStore'
import { ToggleState } from './toggleStateEnum'
import { GridTopicItem } from './item/gridTopicItem'

const topicStore = useTopicStore()

interface Props {
    children: GridTopicItem[]
}
const props = defineProps<Props>()

const toggleState = ref(ToggleState.Collapsed)

</script>

<template>
    <div class="row">
        <div class="col-xs-12">
            <TopicContentGridItem v-for="c in props.children" :topic="c" :toggle-state="toggleState"
                :parent-id="topicStore.id" />
        </div>
    </div>
</template>

<style lang="less" scoped></style>