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
    hasWikis?: boolean
    hasQuestions?: boolean
    marginTop?: number
}

const props = withDefaults(defineProps<Props>(), {
    hasWikis: false,
    hasQuestions: false,
    marginTop: 25
})

const getVisibleSections = computed(() => {
    const visibleSectionIds = ['Stats']
    if (props.showSkills) {
        visibleSectionIds.push('Skills')
    }
    if (props.hasWikis) {
        visibleSectionIds.push('Wikis')
    }
    if (props.hasQuestions) {
        visibleSectionIds.push('Questions')
    }
    return visibleSectionIds
})
</script>

<template>
    <LayoutSidebar site-class="is-user">
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
