<script setup lang="ts">
import { MISSION_CONTROL_SECTIONS } from '~/constants/missionControlSections'

interface Props {
    showWikis?: boolean
    showFavorites?: boolean
    marginTop?: number
}

const props = withDefaults(defineProps<Props>(), {
    showWikis: false,
    showFavorites: false,
    marginTop: 25
})

const getVisibleSections = computed(() => {
    const visibleSectionIds = ['KnowledgeStatus']

    if (props.showWikis)
        visibleSectionIds.push('Wikis')
    if (props.showFavorites)
        visibleSectionIds.push('Favorites')

    // Always show learn calendar section
    visibleSectionIds.push('LearnCalendar')

    return visibleSectionIds
})
</script>

<template>
    <LayoutSidebar site-class="is-mission-control" :hide-divider="true">
        <template #outline>
            <SidebarCard id="OutlineSection" :style="{ marginTop: props.marginTop + 'px' }">
                <template #body>
                    <SidebarOutline
                        :sections="MISSION_CONTROL_SECTIONS"
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

:deep(#DefaultSidebar) {
    height: 25px;
}
</style>