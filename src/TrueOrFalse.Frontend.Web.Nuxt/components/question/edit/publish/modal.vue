<script lang="ts" setup>
import { usePublishQuestionStore } from './publishQuestionStore'
const publishQuestionStore = usePublishQuestionStore()
</script>

<template>
    <LazyModal
        :show="publishQuestionStore.showModal"
        :primary-btn-label="'Bestätigen'"
        @primary-btn="publishQuestionStore.confirmPublish"
        @close="publishQuestionStore.closeModal"
        :show-cancel-btn="true">
        <template v-slot:header>
            <h4 class="modal-title">
                Veröffentlichung bestätigen
            </h4>
        </template>
        <template v-slot:body>
            <p>
                Möchtest du wirklich die Frage "{{ publishQuestionStore.text }}" veröffentlichen?
            </p>
            <div class="alert alert-warning" role="alert" v-if="publishQuestionStore.showErrorMsg">
                {{ publishQuestionStore.errorMsg }}
            </div>
        </template>
    </LazyModal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';
</style>