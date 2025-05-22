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
                        <i18n-t keypath="page.analytics.includedQuestions" tag="span" :plural="pageStore.questionCount">
                            <template #count>
                                <b>{{ pageStore.questionCount }}</b>
                            </template>
                        </i18n-t>
                    </li>
                    <li>
                        <i18n-t keypath="page.analytics.directlyLinkedQuestions" tag="span" :plural="pageStore.directQuestionCount">
                            <template #count>
                                <b>{{ pageStore.directQuestionCount }}</b>
                            </template>
                        </i18n-t>
                    </li>
                    <li>
                        <i18n-t keypath="page.analytics.totalQuestionViews" tag="span" :plural="pageStore.totalQuestionViews">
                            <template #count>
                                <b>{{ pageStore.totalQuestionViews }}</b>
                            </template>
                        </i18n-t>
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
                        <i18n-t keypath="page.analytics.includedPages" tag="span" :plural="pageStore.childPageCount">
                            <template #count>
                                <b>{{ pageStore.childPageCount }}</b>
                            </template>
                        </i18n-t>
                    </li>
                    <li>
                        <i18n-t keypath="page.analytics.directlyLinkedChildPages" tag="span" :plural="pageStore.directVisibleChildPageCount">
                            <template #count>
                                <b>{{ pageStore.directVisibleChildPageCount }}</b>
                            </template>
                        </i18n-t>
                    </li>
                    <li>
                        <i18n-t keypath="page.analytics.parentPages" tag="span" :plural="pageStore.parentPageCount">
                            <template #count>
                                <b>{{ pageStore.parentPageCount }}</b>
                            </template>
                        </i18n-t>
                    </li>
                    <li>
                        <i18n-t keypath="page.analytics.totalPageViews" tag="span" :plural="pageStore.views">
                            <template #count>
                                <b>{{ pageStore.views }}</b>
                            </template>
                        </i18n-t>
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
            margin-bottom: 1.2rem;
        }

        .pagedata-content {
            font-size: 1.4rem;
            color: @memo-grey-dark;
            display: flex;
            flex-direction: column;
            gap: 2rem;

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
