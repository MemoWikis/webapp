<script setup lang="ts">
import { USER_SECTIONS } from '~/constants/userSections'

interface Props {
    user?: {
        id: number
        name: string
        imageUrl: string
        reputationPoints: number
        rank: number
    }
    showSkills?: boolean
    showWikis?: boolean
    showPages?: boolean
    showQuestions?: boolean
    marginTop?: number
}

const props = withDefaults(defineProps<Props>(), {
    showSkills: false,
    showWikis: false,
    showPages: false,
    showQuestions: false,
    marginTop: 25
})

const getVisibleSections = computed(() => {
    const visibleSectionIds = ['Stats']

    // First, push sections that have content
    if (props.showSkills)
        visibleSectionIds.push('Skills')
    if (props.showWikis)
        visibleSectionIds.push('Wikis')
    if (props.showPages)
        visibleSectionIds.push('Pages')
    if (props.showQuestions)
        visibleSectionIds.push('Questions')

    // Then, push placeholder sections for empty ones
    if (!props.showSkills)
        visibleSectionIds.push('Skills-placeholder')
    if (!props.showWikis)
        visibleSectionIds.push('Wikis-placeholder')
    if (!props.showPages)
        visibleSectionIds.push('Pages-placeholder')
    if (!props.showQuestions)
        visibleSectionIds.push('Questions-placeholder')

    return visibleSectionIds
})
</script>

<template>
    <LayoutSidebar site-class="is-user" :hide-divider="true">
        <template #outline>
            <SidebarCard id="OutlineSection" :style="{ marginTop: props.marginTop + 'px' }">
                <template #body>
                    <SidebarOutline
                        :sections="USER_SECTIONS"
                        :visible-sections="getVisibleSections"
                        container-id="Outline" />
                </template>
            </SidebarCard>
        </template>
    </LayoutSidebar>
</template>

<style lang="less" scoped>
@import '~~/assets/sidebar.less';

#OutlineSection {
    margin-top: 25px;
}
</style>
