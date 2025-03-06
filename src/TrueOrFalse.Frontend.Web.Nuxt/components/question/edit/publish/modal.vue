<script lang="ts" setup>
import { usePublishQuestionStore } from './publishQuestionStore'
const publishQuestionStore = usePublishQuestionStore()
const { t } = useI18n()
</script>

<template>
    <LazyModal
        :show="publishQuestionStore.showModal"
        :primary-btn-label="t('page.publishQuestionModal.confirmButton')"
        @primary-btn="publishQuestionStore.confirmPublish"
        @close="publishQuestionStore.closeModal"
        :show-cancel-btn="true">
        <template v-slot:header>
            <h4 class="modal-title">
                {{ t('page.publishQuestionModal.title') }}
            </h4>
        </template>
        <template v-slot:body>
            <p>
                {{ t('page.publishQuestionModal.confirmMessage', { text: publishQuestionStore.text }) }}
            </p>
            <div class="license-info">
                <b>{{ t('page.publishQuestionModal.warning') }}</b>
                <br />
                {{ t('page.publishQuestionModal.licenseInfo') }}
                <NuxtLink :external="true" to="https://creativecommons.org/licenses/by/4.0/legalcode.de"
                    target="_blank">{{ t('page.publishQuestionModal.licenseLink') }}
                </NuxtLink>).
                <br />
                {{ t('page.publishQuestionModal.licenseUsage') }}
                <br />
                {{ t('page.publishQuestionModal.licenseConfirmation') }}
            </div>

            <div class="alert alert-warning" role="alert" v-if="publishQuestionStore.showErrorMsg">
                {{ publishQuestionStore.errorMsg }}
            </div>
        </template>
    </LazyModal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.license-info {
    background-color: @background-grey;
    margin-top: 24px;
    margin-bottom: 48px;
    padding: 16px 24px;
    font-size: 12px;
}
</style>