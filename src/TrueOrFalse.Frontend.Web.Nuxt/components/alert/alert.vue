<script lang="ts" setup>
import { useAlertStore, AlertType } from './alertStore'

const alertStore = useAlertStore()
</script>

<template>
    <VueFinalModal v-model="alertStore.show" @keydown.esc="alertStore.show = false" :z-index-auto="false"
        @close="alertStore.show = false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">

                <div class="modal-body">
                    <h3 v-if="alertStore.title != null && alertStore.title.length > 0">
                        <font-awesome-icon v-if="alertStore.type == AlertType.Success" icon="fa-solid fa-circle-check"
                            class="success" />
                        <font-awesome-icon v-else-if="alertStore.type == AlertType.Error" icon="fa-solid fa-circle-xmark"
                            class="error" />
                        {{ alertStore.title }}
                    </h3>
                    <div class="alert-msg-container" :class="{ 'has-icon': alertStore.type != AlertType.Default }">
                        <template v-if="alertStore.title == null || alertStore.title.length == 0">
                            <font-awesome-icon v-if="alertStore.type == AlertType.Success" icon="fa-solid fa-circle-check"
                                class="success msg-icon" />
                            <font-awesome-icon v-else-if="alertStore.type == AlertType.Error"
                                icon="fa-solid fa-circle-xmark" class="error msg-icon" />

                        </template>
                        <div class="alert-msg">
                            {{ alertStore.text }}

                        </div>
                    </div>
                    <div v-if="alertStore.msg != null" v-html="alertStore.msg.customHtml"></div>
                </div>

                <div class="modal-footer">
                    <button class="btn memo-button pull-right" :class="{
                        'btn-success': alertStore.type == AlertType.Success,
                        'btn-error': alertStore.type == AlertType.Error,
                        'btn-primary': alertStore.type == AlertType.Default
                    }" @click="alertStore.closeAlert()">{{ alertStore.label }}</button>
                    <button v-if="alertStore.showCancelButton" class="btn memo-button btn-link pull-right cancel-alert"
                        @click="alertStore.closeAlert(true)">
                        Abbrechen
                    </button>
                    <div v-if="alertStore.msg != null" v-html="alertStore.msg.customBtn"></div>
                </div>

            </div>
        </div>
    </VueFinalModal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.success {
    color: @memo-green;
}

.error {
    color: @memo-salmon;
}

.cancel-alert {
    margin-right: 4px;
}

.alert-msg-container {
    display: flex;
    margin-top: 20px;
    margin-bottom: 10px;
    justify-content: center;
    align-items: center;

    .msg-icon {
        font-size: 25px;
    }

    &.has-icon {
        .alert-msg {
            padding-left: 20px;
        }
    }
}
</style>