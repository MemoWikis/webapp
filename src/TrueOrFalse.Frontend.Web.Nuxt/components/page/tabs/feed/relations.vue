<script lang="ts" setup>
import { RelatedPage, RelationChanges } from './feedHelper'

interface Props {
    relationChanges: RelationChanges
}
const { $urlHelper } = useNuxtApp()
const props = defineProps<Props>()
const setRelatedPages = () => {

    if (props.relationChanges) {
        addedChildren.value = props.relationChanges.addedChildren
        removedChildren.value = props.relationChanges.removedChildren
        addedParents.value = props.relationChanges.addedParents
        removedParents.value = props.relationChanges.removedParents
    }
}

const addedParents = ref<RelatedPage[]>([])
const removedParents = ref<RelatedPage[]>([])
const addedChildren = ref<RelatedPage[]>([])
const removedChildren = ref<RelatedPage[]>([])


const clearRelatedPages = () => {
    addedChildren.value = []
    removedChildren.value = []
    addedParents.value = []
    removedParents.value = []
}

onBeforeMount(() => {
    setRelatedPages()
})

watch(() => props.relationChanges, () => {
    clearRelatedPages()
    setRelatedPages()
}, { deep: true })

</script>

<template>

    <div class="feed-item-label-text-body">

        <div v-if="addedParents.length > 0">
            {{ addedParents.length > 1 ? 'Übergeordnete Seiten hinzugefügt' : 'Übergeordnete Seite hinzugefügt' }}

            <div class="feed-item-relation-list" v-for="addedParent in addedParents.slice(0, 3)">
                <NuxtLink :to="$urlHelper.getPageUrl(addedParent.name, addedParent.id)" @click.stop>
                    {{ addedParent.name }}
                </NuxtLink>
            </div>
            <div v-if="addedParents.length > 3">...</div>
        </div>

        <div v-if="removedParents.length > 0">
            {{ removedParents.length > 1 ? 'Übergeordnete Seiten entfernt' : 'Übergeordnete Seite entfernt' }}
            <div class="feed-item-relation-list" v-for="removedParent in removedParents.slice(0, 3)">
                <NuxtLink :to="$urlHelper.getPageUrl(removedParent.name, removedParent.id)" @click.stop>
                    {{ removedParent.name }}
                </NuxtLink>
            </div>
            <div v-if="removedParents.length > 3">...</div>

        </div>

        <div v-if="addedChildren.length > 0">
            {{ addedChildren.length > 1 ? 'Unterseiten hinzugefügt' : 'Unterseite hinzugefügt' }}

            <div class="feed-item-relation-list" v-for="addedChild in addedChildren.slice(0, 3)">
                <NuxtLink :to="$urlHelper.getPageUrl(addedChild.name, addedChild.id)" @click.stop>
                    {{ addedChild.name }}
                </NuxtLink>
            </div>

            <div v-if="addedChildren.length > 3">...</div>
        </div>

        <div v-if="removedChildren.length > 0">
            {{ removedChildren.length > 1 ? 'Unterseiten entfernt' : 'Unterseite entfernt' }}
            <div class="feed-item-relation-list" v-for="removedChild in removedChildren.slice(0, 3)">
                <NuxtLink :to="$urlHelper.getPageUrl(removedChild.name, removedChild.id)" @click.stop>
                    {{ removedChild.name }}
                </NuxtLink>
            </div>
            <div v-if="removedChildren.length > 3">...</div>
        </div>

    </div>

</template>