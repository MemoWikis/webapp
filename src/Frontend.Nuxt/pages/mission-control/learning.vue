<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'
import { useUserStore } from '~/components/user/userStore'
import { useLearningSessionConfigurationStore } from '~/components/page/learning/learningSessionConfigurationStore'

const userStore = useUserStore()

const emit = defineEmits(['setPage'])
emit('setPage', SiteType.MissionControl)

const { t } = useI18n()

useHead({
    title: t('learning.wishknowledge.title', 'Wishknowledge Learning')
})

// Get wishknowledge question count via API
const wishknowledgeQuestionCount = ref(0)

const fetchWishknowledgeCount = async () => {
    try {
        // Use the same API that gets the question count for wishknowledge (pageId = 0)
        const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
        await learningSessionConfigurationStore.getQuestionCount(0)
        wishknowledgeQuestionCount.value = learningSessionConfigurationStore.maxSelectableQuestionCount
    } catch (error) {
        console.error('Failed to fetch wishknowledge question count:', error)
        wishknowledgeQuestionCount.value = 0
    }
}

const missionControlHeader = ref()
const missionControlHeaderHeight = ref(123) // Default height

const sideBarMarginTop = computed(() => {
    const defaultHeaderHeight = 123
    if (missionControlHeaderHeight.value > defaultHeaderHeight) {
        return missionControlHeaderHeight.value - defaultHeaderHeight + 25
    }
    return 25 // Default margin top if no header is available
})

// Set up component initialization
onMounted(async () => {
    // Fetch wishknowledge question count
    await fetchWishknowledgeCount()

    // Watch for header height changes
    if (missionControlHeader.value) {
        // Initial height measurement
        missionControlHeaderHeight.value = missionControlHeader.value.offsetHeight

        // Set up ResizeObserver to watch for height changes
        const resizeObserver = new ResizeObserver((entries) => {
            for (const entry of entries) {
                missionControlHeaderHeight.value = entry.contentRect.height
            }
        })

        resizeObserver.observe(missionControlHeader.value)

        // Cleanup on unmount
        onUnmounted(() => {
            resizeObserver.disconnect()
        })
    }
})

definePageMeta({
    key: 'learning-page',
})
</script>

<template>
    <div class="mission-control-container">
        <div class="mission-control-content">
            <div class="mission-control-header" ref="missionControlHeader">
                <h1>{{ t('learning.wishknowledge.heading', 'Learning Your Wishknowledge') }}</h1>
                <div v-if="wishknowledgeQuestionCount > 0" class="question-count-info">
                    {{ wishknowledgeQuestionCount }} {{ t('learning.wishknowledge.questionsAvailable', 'questions available') }}
                </div>
            </div>

            <div class="learning-content">
                <PageLearning :all-wishknowledge-mode="true" />
            </div>
        </div>

        <!-- <SidebarLearning :margin-top="sideBarMarginTop" /> -->
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.mission-control-container {
    display: flex;
    flex-wrap: nowrap;
    gap: 0 1rem;
    width: 100%;

    .mission-control-content {
        max-width: 1200px;
        width: calc(75% - 1rem);
        flex-grow: 2;

        .mission-control-header {
            margin: 25px 0;

            h1 {
                margin-bottom: 12px;
                font-weight: 600;
            }

            .question-count-info {
                color: @memo-grey-dark;
                font-size: 16px;
                margin-bottom: 12px;
            }
        }

        .learning-content {
            width: 100%;
        }
    }

    @media (max-width: 900px) {
        .mission-control-content {
            width: 100%;
        }
    }
}

// Ensure the learning components work properly within this layout
.mission-control-container {
    :deep(.page-container) {
        display: block;
        width: 100%;

        .page {
            width: 100%;
            max-width: none;
        }
    }

    // Override PageLearning styles to fit mission control layout
    :deep(.learning-page) {
        width: 100%;
        max-width: none;
    }

    // Ensure filter and learning components have proper spacing
    :deep(#PageTabContent) {
        margin-top: 20px;
    }

    // Adjust learning session components
    :deep(.learning-session-configuration) {
        margin-bottom: 20px;
    }
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

// Override some global styles for the mission control learning page to ensure proper layout
.sidesheet-open {
    .mission-control-container {
        @media (max-width: 1209px) {
            .mission-control-content {
                width: 100%;
            }
        }
    }
}
</style>