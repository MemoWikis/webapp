<script setup lang="ts">
import { usePageStore } from '../../page/pageStore'

const pageStore = usePageStore()
const { t } = useI18n()
</script>

<template>
    <div class="pagedata-section">
        <h3>{{ t('page.analytics.pageData') }}</h3>

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
                </ul>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.pagedata-section {
    margin-bottom: 40px;
    font-size: 18px;
    width: 100%;

    .pagedata-container {
        .pagedata-content {
            margin-bottom: 24px;
        }

        .pagedata-sub-label {
            margin-bottom: 16px;
        }
    }
}

.pagedata-section {
    width: 50%;

    @media screen and (max-width: 991px) {
        width: 100%;
    }
}

h3 {
    margin-top: 36px;
}
</style>
