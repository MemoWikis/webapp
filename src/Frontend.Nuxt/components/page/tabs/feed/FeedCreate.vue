<script lang="ts" setup>
import { RelatedPage, RelationChanges } from './feedHelper'

interface Props {
    addedParent: RelatedPage
}
const props = defineProps<Props>()

const { $urlHelper } = useNuxtApp()
const { t } = useI18n()

const setParentPage = () => {

    if (props.addedParent) {
        parent.value = props.addedParent
    }
}

const parent = ref<RelatedPage>()

onBeforeMount(() => {
    setParentPage()
})

watch(() => props.addedParent, () => {
    parent.value = undefined
    setParentPage()
}, { deep: true })

</script>

<template>
    <div class="feed-item-label-text-body">
        <div>
            {{ t('page.feed.create.parentPage') }}
            <NuxtLink :to="$urlHelper.getPageUrl(addedParent.name, addedParent.id)" @click.stop>
                {{ addedParent.name }}
            </NuxtLink>
        </div>
    </div>
</template>