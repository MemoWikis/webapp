<script setup lang="ts">
import { usePageStore } from '../../page/pageStore'

const pageStore = usePageStore()
const { t } = useI18n()
</script>

<template>
    <div class="pagedata-section">
        <div class="pagedata-container">
            <div class="pagedata-sub-label">
                {{ t('page.analytics.questionsLabel') }}
            </div>
            <div class="pagedata-content">
                <ul>
                    <li>
                        <b>{{ pageStore.questionCount }}</b> {{ t('page.analytics.includedQuestions') }}
                    </li>
                    <li>
                        <b>{{ pageStore.directQuestionCount }}</b> {{ t('page.analytics.directlyLinkedQuestions') }}
                    </li>
                    <li>
                        <b>{{ pageStore.totalQuestionViews }}</b> {{ t('page.analytics.totalQuestionViews') }}
                    </li>
                </ul>
            </div>
        </div>

        <div class="pagedata-container">
            <div class="pagedata-sub-label">
                {{ t('page.analytics.pagesLabel') }}
            </div>
            <div class="pagedata-content">
                <ul>
                    <li>
                        <b>{{ pageStore.childPageCount }} </b> {{ t('page.analytics.includedPages') }}
                    </li>
                    <li>
                        <b>{{ pageStore.directVisibleChildPageCount }}</b> {{ t('page.analytics.directlyLinkedChildPages') }}
                    </li>
                    <li>
                        <b> {{ pageStore.parentPageCount }} </b> {{ t('page.analytics.parentPages') }}
                    </li>
                    <li>
                        <b>{{ pageStore.views }}</b> {{ t('page.analytics.totalPageViews') }}
                    </li>
                </ul>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.pagedata-section {
    font-size: 1.6rem;
    width: 100%;

    .pagedata-container {
        .pagedata-sub-label {
            font-size: 1.6rem;
            color: @memo-grey-darker;
            font-weight: 600;
        }

        .pagedata-content {
            padding: 20px 0;
            font-size: 1.4rem;
            color: @memo-grey-dark;

            ul {
                list-style-type: none;
                padding-left: 0;

                li {
                    color: @memo-grey-dark;
                    margin-bottom: 12px;
                    position: relative;
                    padding-left: 15px;

                    &::before {
                        content: "";
                        position: absolute;
                        left: 0;
                        top: 8px;
                        width: 6px;
                        height: 6px;
                        border-radius: 50%;
                        background-color: @memo-blue;
                    }
                }
            }


        }
    }

    h3 {
        margin-top: 36px;
    }
}
</style>
