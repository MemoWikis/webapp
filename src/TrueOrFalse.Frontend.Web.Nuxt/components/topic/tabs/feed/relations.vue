<script lang="ts" setup>
import { RelatedTopic, RelationChanges } from './feedHelper'

interface Props {
    relationChanges: RelationChanges
}
const props = defineProps<Props>()
const setRelatedTopics = () => {

    if (props.relationChanges) {
        addedChildren.value = props.relationChanges.addedChildren
        removedChildren.value = props.relationChanges.removedChildren
        addedParents.value = props.relationChanges.addedParents
        removedParents.value = props.relationChanges.removedParents
    }
}

const addedParents = ref<RelatedTopic[]>([])
const removedParents = ref<RelatedTopic[]>([])
const addedChildren = ref<RelatedTopic[]>([])
const removedChildren = ref<RelatedTopic[]>([])


const clearRelatedTopics = () => {
    addedChildren.value = []
    removedChildren.value = []
    addedParents.value = []
    removedParents.value = []
}

onBeforeMount(() => {
    setRelatedTopics()
})

watch(() => props.relationChanges, () => {
    clearRelatedTopics()
    setRelatedTopics()
}, { deep: true })

</script>

<template>

    <div>
        <div class="feed-item-label-text-body">

            <div v-if="addedParents.length > 0">
                {{ addedParents.length > 1 ? 'Oberthemen hinzugefügt' : 'Oberthema hinzugefügt' }}

                <div class="feed-item-relation-list" v-for="addedParent in addedParents.slice(0, 3)">
                    <NuxtLink :to="$urlHelper.getTopicUrl(addedParent.name, addedParent.id)" @click.stop>
                        {{ addedParent.name }}
                    </NuxtLink>
                </div>
                <div v-if="addedParents.length > 3">...</div>
            </div>

            <div v-if="removedParents.length > 0">
                {{ removedParents.length > 1 ? 'Oberthemen entfernt' : 'Oberthema entfernt' }}
                <div class="feed-item-relation-list" v-for="removedParent in removedParents.slice(0, 3)">
                    <NuxtLink :to="$urlHelper.getTopicUrl(removedParent.name, removedParent.id)" @click.stop>
                        {{ removedParent.name }}
                    </NuxtLink>
                </div>
                <div v-if="removedParents.length > 3">...</div>

            </div>

            <div v-if="addedChildren.length > 0">
                {{ addedChildren.length > 1 ? 'Unterthemen hinzugefügt' : 'Unterthema hinzugefügt' }}

                <div class="feed-item-relation-list" v-for="addedChild in addedChildren.slice(0, 3)">
                    <NuxtLink :to="$urlHelper.getTopicUrl(addedChild.name, addedChild.id)" @click.stop>
                        {{ addedChild.name }}
                    </NuxtLink>
                </div>

                <div v-if="addedChildren.length > 3">...</div>
            </div>

            <div v-if="removedChildren.length > 0">
                {{ removedChildren.length > 1 ? 'Unterthemen entfernt' : 'Unterthema entfernt' }}
                <div class="feed-item-relation-list" v-for="removedChild in removedChildren.slice(0, 3)">
                    <NuxtLink :to="$urlHelper.getTopicUrl(removedChild.name, removedChild.id)" @click.stop>
                        {{ removedChild.name }}
                    </NuxtLink>
                </div>
                <div v-if="removedChildren.length > 3">...</div>
            </div>

        </div>

    </div>
</template>