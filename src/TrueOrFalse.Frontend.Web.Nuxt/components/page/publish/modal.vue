<script lang="ts" setup>
import { usePublishPageStore } from './publishPageStore'

const publishPageStore = usePublishPageStore()
const { t } = useI18n()

const blinkTimer = ref<ReturnType<typeof setTimeout>>()
const blink = ref(false)

async function publish() {
    if (!publishPageStore.confirmLicense) {
        blinkTimer.value = undefined
        blink.value = true
        blinkTimer.value = setTimeout(() => {
            blink.value = false
        }, 2000)
        return
    }
    publishPageStore.publish()
}

</script>

<template>
    <LazyModal :show="publishPageStore.showModal" :primary-btn-label="t('page.publishModal.button.publish')" v-if="publishPageStore.showModal"
        @close="publishPageStore.showModal = false" @primary-btn="publish()"
        @keydown.esc="publishPageStore.showModal = false" :disabled="!publishPageStore.confirmLicense"
        :show-cancel-btn="true">

        <template v-slot:header>
            <h4>{{ t('page.publishModal.title', { name: publishPageStore.name }) }}</h4>
        </template>

        <template v-slot:body>
            <div class="subHeader">
                {{ t('page.publishModal.intro.public') }} <br />
                {{ t('page.publishModal.intro.license') }}
            </div>
            <div class="checkbox-container"
                @click="publishPageStore.includeQuestionsToPublish = !publishPageStore.includeQuestionsToPublish"
                v-if="publishPageStore.questionCount > 0">
                <div class="checkbox-icon">
                    <font-awesome-icon icon="fa-solid fa-square-check" v-if="publishPageStore.includeQuestionsToPublish" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else />
                </div>
                <div class="checkbox-label">
                    {{ t('page.publishModal.questions.publish', { count: publishPageStore.questionCount }) }}
                </div>

            </div>
            <div class="checkbox-container license-info"
                @click="publishPageStore.confirmLicense = !publishPageStore.confirmLicense">
                <div class="checkbox-icon" :class="{ blink: blink }">
                    <font-awesome-icon icon="fa-solid fa-square-check" v-if="publishPageStore.confirmLicense" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else />
                </div>
                <div class="checkbox-label" :class="{ blink: blink }">
                    <i18n-t keypath="page.publishModal.license.confirmation" tag="span">
                        <template #0>
                            <NuxtLink :external="true" to="https://creativecommons.org/licenses/by/4.0/legalcode.de"
                                target="_blank">{{ t('page.publishModal.license.linkText') }}</NuxtLink>
                        </template>
                    </i18n-t>
                    {{ t('page.publishModal.license.usage') }}
                </div>
            </div>
        </template>

    </LazyModal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.subHeader {
    line-height: 18px;
    margin-bottom: 16px;
}

.checkbox-container {
    cursor: pointer;
    padding: 16px;
    display: flex;
    justify-content: flex-start;

    .checkbox-icon {
        font-size: 24px;

        .fa-check-square {
            color: @memo-blue-link;
        }
    }

    .checkbox-label {
        padding-top: 4px;
        line-height: 20px;
        padding-left: 10px;
    }

    &.license-info {
        background-color: @background-grey;
        user-select: none;

        .checkbox-label {
            line-height: 18px;
            font-size: 12px;
        }

        margin-bottom: 40px;

        .blink {
            animation: blinker 1s linear infinite;
        }

        @keyframes blinker {
            50% {
                opacity: 0;
            }
        }
    }
}
</style>