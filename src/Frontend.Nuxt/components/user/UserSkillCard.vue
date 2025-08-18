<script lang="ts" setup>
import { PageData } from '~/composables/missionControl/pageData'

interface Props {
    skill: PageData
    size?: LayoutCardSize
}

const props = withDefaults(defineProps<Props>(), {
    size: LayoutCardSize.Small
})

const { t } = useI18n()
const { $urlHelper } = useNuxtApp()

const hasKnowledgebarData = computed(() => {
    return props.skill.knowledgebarData != null && props.skill.knowledgebarData.total > 0
})

const knowledgebarData = computed(() => {
    return props.skill.knowledgebarData
})

</script>

<template>
    <LayoutCard :size="size" :url="$urlHelper.getPageUrl(skill.name, skill.id)">
        <div class="user-skill-card">
            <div class="skill-image" v-if="skill.imgUrl">
                <Image :src="skill.imgUrl" :alt="skill.name" />
            </div>
            <div class="skill-details">
                <h4>{{ skill.name }}</h4>
                <p class="creator">{{ t('userSkillCard.by') }}: {{ skill.creatorName }}</p>
                <div class="skill-bar" v-if="skill.questionCount > 0">
                    <p class="question-count">{{ t('userSkillCard.questionCount') }}: {{ skill.questionCount }}</p>
                    <PageContentGridKnowledgebar
                        v-if="hasKnowledgebarData"
                        :knowledgebarData="knowledgebarData" />
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

    .skill-image {
        border-radius: 8px;
        overflow: hidden;
    }

    .skill-details {
        display: flex;
        flex-direction: column;
        align-items: flex-start;
        width: 100%;

        h4 {
            margin: 0.5rem 0;
            // font-size: 1.2em;
        }

        p {
            // margin: 4px 0 0;
            font-size: 1em;
            color: @memo-grey;

            &.creator {
                font-style: italic;
            }

            &.question-count {
                margin-bottom: 0;
            }
        }

        .skill-bar {
            width: 100%;
        }
    }
}
</style>