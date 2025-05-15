<script setup lang="ts">
import { KnowledgebarData } from '~/components/page/content/grid/knowledgebar/knowledgebarData'
import { Tab } from '../page/tabs/tabsStore'

interface WikiData {
    id: number
    name: string
    imgUrl?: string
    questionCount: number
    knowledgebarData: KnowledgebarData
}

interface Props {
    wiki: WikiData
}

const props = defineProps<Props>()

const { t } = useI18n()
const { $urlHelper } = useNuxtApp()

</script>

<template>
    <div class="wiki-card">
        <div class="wiki-image" v-if="wiki.imgUrl">
            <Image
                :src="wiki.imgUrl"
                :alt="wiki.name"
                :customStyle="'object-fit: cover; height: 100%; width: 100%;'" />
        </div>
        <div class="wiki-image placeholder" v-else>
            <Image
                src="/Images/Placeholders/placeholder-page-206.png"
                :alt="wiki.name"
                :customStyle="'object-fit: cover; height: 100%; width: 100%;'" />
        </div>
        <div class="wiki-content">
            <NuxtLink :navigateTo="$urlHelper.getPageUrl(props.wiki.name, props.wiki.id)">
                <h3 class="wiki-title">{{ wiki.name }}</h3>
            </NuxtLink>
            <div class="wiki-details" v-if="wiki.questionCount > 0">
                <div class="question-count">
                    <span class="count">{{ t('missionControl.wiki.questionCount', wiki.questionCount) }}</span>
                </div>
                <div class="knowledge-status">
                    <PageContentGridKnowledgebar v-if="wiki.knowledgebarData" :knowledgebarData="wiki.knowledgebarData" />
                </div>
                <div class="wiki-details-action" v-if="wiki.questionCount > 0">
                    <NuxtLink :navigateTo="$urlHelper.getPageUrl(props.wiki.name, props.wiki.id, Tab.Learning)">
                        <div class="memo-button btn-default btn-primary btn" type="button">Jetzt lernen</div>
                    </NuxtLink>

                </div>

            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
.wiki-card {
    display: flex;
    background-color: white;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 1px 4px rgba(0, 0, 0, 0.1);
    transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
    height: 100%;
    cursor: pointer;
    flex-direction: column;
    margin-top: 24px;

    @media (min-width: 576px) {
        flex-direction: row;
    }

    .wiki-image {
        width: 100%;
        height: 160px;
        flex-shrink: 0;
        overflow: hidden;

        @media (min-width: 576px) {
            width: 120px;
            height: 120px;
        }

        &.placeholder {
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: #f5f5f5;
        }
    }

    .wiki-content {
        padding: 16px;
        flex: 1;
        display: flex;
        flex-direction: column;

        .wiki-title {
            margin: 0 0 12px;
            font-size: 16px;
            font-weight: 600;
            color: #333;
            overflow: hidden;
            text-overflow: ellipsis;
            display: -webkit-box;
            -webkit-line-clamp: 2;
            line-clamp: 2;
            -webkit-box-orient: vertical;

            @media (min-width: 576px) {
                font-size: 18px;
            }
        }

        .wiki-details {
            margin-top: auto;

            .question-count {
                margin-bottom: 8px;
                font-size: 14px;
                color: #666;

                .count {
                    font-weight: 600;
                    margin-right: 5px;
                }
            }

            .knowledge-status {
                margin-top: 4px;
                width: 100%;

                @media (min-width: 576px) {
                    max-width: 180px;
                }
            }

            .wiki-details-action {
                display: flex;
                justify-content: flex-end;
                align-items: center;

                .btn-primary {
                    color: white;
                }

                .memo-button {
                    border-radius: 24px;
                }

            }
        }
    }
}
</style>