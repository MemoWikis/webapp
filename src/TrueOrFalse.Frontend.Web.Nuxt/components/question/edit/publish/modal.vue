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
            <div class="license-info">
                <b>Achtung!</b>
                <br />
                Du stellst diesen Eintrag unter die Lizenz "Creative Commons - Namensnennung 4.0 International" (CC BY 4.0,
                <NuxtLink :external="true" to="https://creativecommons.org/licenses/by/4.0/legalcode.de"
                    target="_blank">Lizenztext, deutsche Zusammenfassung
                </NuxtLink>).
                <br />
                Der Eintrag kann bei angemessener Namensnennung ohne Einschränkung weiter genutzt werden.
                <br />
                Du bestätigst, dass die Texte und gegebenenfalls auch Bilder Deine eigene Arbeit sind und nicht aus urheberrechtlich geschützten
                Quellen kopiert wurden.
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