<script setup lang="ts">
import { Tab } from '../page/tabs/tabsStore'
import { PageData } from '~/composables/missionControl/pageData'


interface Props {
    page: PageData
}

const props = defineProps<Props>()

const { t } = useI18n()
const { $urlHelper } = useNuxtApp()

</script>

<template>
    <div class="page-card">
        <div class="page-image" v-if="page.imgUrl">
            <Image
                :src="page.imgUrl"
                :alt="page.name"
                :customStyle="'object-fit: cover; height: 100%; width: 100%;'" />
        </div>
        <div class="page-image placeholder" v-else>
            <Image
                src="/Images/Placeholders/placeholder-page-206.png"
                :alt="page.name"
                :customStyle="'object-fit: cover; height: 100%; width: 100%;'" />
        </div>
        <div class="page-content">
            <NuxtLink :to="$urlHelper.getPageUrl(props.page.name, props.page.id)">
                <h3 class="page-title">{{ page.name }}</h3>
            </NuxtLink>
            <div class="page-details" v-if="page.questionCount > 0">
                <div>
                    <div class="question-count">
                        <span class="count">{{ t('missionControl.pageGrid.questions', page.questionCount) }}</span>
                    </div>
                    <div class="knowledge-status">
                        <PageContentGridKnowledgebar v-if="page.knowledgebarData" :knowledgebarData="page.knowledgebarData" />
                    </div>
                </div>

                <div class="page-details-action" v-if="page.questionCount > 0">
                    <NuxtLink
                        v-if="page.questionCount > 0"
                        :to="{ path: $urlHelper.getPageUrl(page.name, page.id, Tab.Learning), query: { inWuWi: (page.knowledgebarData != null).toString() } }"
                        class="action-button"
                        :title="t('missionControl.pageTable.learnNow')"
                        v-tooltip="t('missionControl.pageTable.learnNow')">
                        <font-awesome-icon :icon="['fas', 'play']" />
                    </NuxtLink>

                </div>

            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.page-card {
    display: flex;
    background-color: white;
    border-radius: 8px;
    overflow: hidden;
    box-shadow: 0 1px 4px rgba(0, 0, 0, 0.1);
    transition: transform 0.2s ease-in-out, box-shadow 0.2s ease-in-out;
    height: 100%;
    cursor: pointer;
    height: 160px;

    .page-image {
        width: 100%;
        width: 160px;
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
            background-color: @memo-grey-light;
        }
    }

    .page-content {
        padding: 16px;
        flex: 1;
        display: flex;
        flex-direction: column;

        .page-title {
            margin: 0 0 8px;
            font-size: 16px;
            font-weight: 600;
            color: @memo-blue-link;
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

        .page-details {
            margin-top: 8px;
            display: flex;
            justify-content: space-between;
            height: 100%;
            flex-direction: column;

            .question-count {
                font-size: 14px;
                color: @memo-grey-dark;

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

            .page-details-action {
                display: flex;
                justify-content: flex-end;
                align-items: center;

                .action-button {
                    display: inline-flex;
                    align-items: center;
                    justify-content: center;
                    width: 36px;
                    height: 36px;
                    border-radius: 50%;
                    background-color: @memo-green;
                    color: white;
                    cursor: pointer;
                    transition: all 0.2s ease;

                    &:hover {
                        background-color: darken(@memo-green, 10%);
                    }

                    &:active {
                        background-color: darken(@memo-green, 20%);
                    }

                    .fa-play {
                        font-size: 14px;
                        padding-left: 2px;
                    }
                }


            }
        }
    }
}
</style>