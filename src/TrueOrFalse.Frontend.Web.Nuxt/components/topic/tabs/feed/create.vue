<script lang="ts" setup>
import { RelatedTopic, RelationChanges } from './feedHelper'

interface Props {
    addedParent: RelatedTopic
}
const props = defineProps<Props>()
const setParentTopic = () => {

    if (props.addedParent) {
        parent.value = props.addedParent
    }
}

const parent = ref<RelatedTopic>()

onBeforeMount(() => {
    setParentTopic()
})

watch(() => props.addedParent, () => {
    parent.value = undefined
    setParentTopic()
}, { deep: true })

</script>

<template>
    <div class="feed-item-label-text-body">
        <div>
            Übergeordnete Seite: <NuxtLink :to="$urlHelper.getTopicUrl(addedParent.name, addedParent.id)" @click.stop>
                {{ addedParent.name }}
            </NuxtLink>
        </div>
    </div>
</template>