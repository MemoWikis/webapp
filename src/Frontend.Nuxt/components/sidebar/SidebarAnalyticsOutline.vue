<script lang="ts" setup>
import { usePageStore } from '~/components/page/pageStore'
import { Tab } from '../page/tabs/tabsStore'
import { PAGE_ANALYTICS_SECTIONS } from '~/constants/pageAnalyticsSections'

const pageStore = usePageStore()

const { $urlHelper } = useNuxtApp()
const { t } = useI18n()
const currentHeadingId = ref<string | null>(null)
</script>

<template>
    <div id="AnalyticsOutline">
        <perfect-scrollbar :suppressScrollX="true">
            <div class="outline-container">
                <div v-for="(heading, index) in PAGE_ANALYTICS_SECTIONS" :key="heading.id" class="outline-heading level-1">
                    <NuxtLink :to="`${$urlHelper.getPageUrl(pageStore.name, pageStore.id, Tab.Analytics)}#${heading.id}`"
                        class="outline-link" :class="{ 'current-heading': heading.id === currentHeadingId }">
                        {{ t(heading.translationKey) }}
                    </NuxtLink>
                </div>
            </div>
        </perfect-scrollbar>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#AnalyticsOutline {
    height: 100%;

    .outline-container {
        height: 100%;
    }

    .outline-heading {
        display: flex;
        flex-wrap: nowrap;
        align-items: center;
        transition: all 0.01s ease;

        &.level-2,
        &.level-3 {
            margin-top: 4px;
            margin-bottom: 4px;
        }

        &.next-step {
            margin-top: 8px;
        }

        &.level-1 {
            font-size: 16px;
            font-weight: 600;
            margin-top: 24px;

            .current-heading {
                font-weight: 700;
            }

            &.preceeding-section-is-empty {
                margin-top: 8px;
            }
        }

        &.level-2 {
            font-weight: 400;

            .current-heading {
                font-weight: 600;
            }
        }

        &.level-3 {
            font-weight: 300;

            .current-heading {
                font-weight: 600;
            }
        }

        &.first-outline {
            margin-top: 0px;
        }

        .outline-link {
            color: @memo-grey-dark;
            display: block;

            transition: all 0.1s ease-out;

            &:hover {
                color: @memo-blue-link;
            }

            &:visited,
            &:focus,
            &:active,
            &:hover {
                text-decoration: none;
            }

            &.current-heading {
                color: @memo-blue;
            }
        }
    }
}
</style>