<script lang="ts" setup>
import { PageData } from '~/composables/missionControl/pageData'

interface Props {
    skill: PageData
    size?: LayoutCardSize
    canEdit?: boolean
}

const props = withDefaults(defineProps<Props>(), {
    size: LayoutCardSize.Small
})

const { t } = useI18n()
const { $urlHelper } = useNuxtApp()

const emit = defineEmits<{
    removeSkill: [skill: PageData]
}>()

const hasKnowledgebarData = computed(() => {
    return props.skill.knowledgebarData != null && props.skill.knowledgebarData.totalCount > 0
})
const hover = ref(false)
const { isMobile } = useDevice()
const showDeleteButton = computed(() => {
    return props.canEdit && (hover.value || isMobile)
})
</script>

<template>
    <LayoutCard :size="size" :url="$urlHelper.getPageUrl(skill.name, skill.id)" @mouseover="hover = true" @mouseleave="hover = false">
        <div class="user-skill-card">
            <div v-if="showDeleteButton" class="delete-button-container" v-tooltip="t('userSkillCard.deleteSkill')">
                <font-awesome-icon icon="fa-solid fa-trash" class="delete-icon" @click.prevent.stop="$emit('removeSkill', skill)" />
            </div>
            <div class="skill-image" v-if="skill.imgUrl">
                <Image :src="skill.imgUrl" :alt="skill.name" />
            </div>
            <div class="skill-details">
                <font-awesome-icon v-if="!skill.isPublic" icon="fa-solid fa-lock" v-tooltip="t('userSkillCard.private')" class="lock-icon" />
                <font-awesome-icon v-else-if="skill.questionCount < 1 || !hasKnowledgebarData" v-tooltip="t('userSkillCard.noQuestions')" class="visibility-icon" icon="fa-solid fa-eye-slash" />
                <h4>
                    {{ skill.name }}
                </h4>
                <p class="creator">{{ t('userSkillCard.by') }}: {{ skill.creatorName }}</p>
                <div class="knowledge-container" v-if="skill.questionCount > 0">
                    <p class="question-count">{{ t('userSkillCard.questionCount') }}: {{ skill.questionCount }}</p>
                    <PageContentGridKnowledgebar class="skill-bar" v-if="hasKnowledgebarData" :knowledgebarData="props.skill.knowledgebarData" />
                </div>

            </div>
        </div>
    </LayoutCard>
</template>

<style lang="less" scoped>
@import '~~/assets/includes/imports.less';

.user-skill-card {
    display: flex;
    align-items: flex-start;
    gap: 2rem;
    width: 100%;

    .delete-button-container {
        position: absolute;
        bottom: 12px;
        right: 12px;
        z-index: 10;
        background: white;
        border-radius: 36px;
        padding: 12px;
        height: 36px;
        width: 36px;
        display: flex;
        justify-content: center;
        align-items: center;
        cursor: pointer;
        color: @memo-grey;
        font-size: 1.2em;
        transition: all 0.1s ease-in;

        &:hover {
            background: darken(@memo-grey-light, 5%);
        }
    }

    .skill-image {
        border-radius: 8px;
        overflow: hidden;
    }

    .skill-details {
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        width: 100%;
        height: 100%;

        position: relative;

        .lock-icon,
        .visibility-icon {
            position: absolute;
            top: 4px;
            right: 4px;
            padding-top: 2px;
            padding-left: 1px; // padding added for alignment issue with v-tooltip
            font-size: 1.2em;
            color: @memo-grey;
        }

        h4 {
            margin: 0.5rem 0;
            // font-size: 1.2em;
        }

        p {
            // margin: 4px 0 0;
            font-size: 1em;
            color: @memo-grey;

            &.creator {
                // font-style: italic;
            }

            &.question-count {
                margin-bottom: 0;
            }
        }

        .knowledge-container {
            width: 100%;

            .skill-bar {
                margin-top: -7px;
            }
        }
    }
}
</style>