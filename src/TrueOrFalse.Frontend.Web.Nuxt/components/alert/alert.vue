<script lang="ts" setup>
import { useAlertStore, AlertType } from './alertStore'

const alertStore = useAlertStore()
</script>

<template>
    <div id="AlertModal">

        <VueFinalModal v-model="alertStore.show" @keydown.esc="alertStore.show = false" :z-index-auto="false">
            <div class="modal-dialog" role="document">
                <div class="modal-content">

                    <div class="modal-body">
                        <h3>
                            <font-awesome-icon v-if="alertStore.type == AlertType.Success"
                                icon="fa-solid fa-circle-check" class="success" />
                            <font-awesome-icon v-else-if="alertStore.type == AlertType.Error"
                                icon="fa-solid fa-circle-xmark" class="error" />
                            {{ alertStore.title }}
                        </h3>

                        <div class="">{{ alertStore.text }}</div>
                        <div v-if="alertStore.msg != null" v-html="alertStore.msg.customHtml"></div>
                    </div>

                    <div class="modal-footer">
                        <div class="btn memo-button col-xs-4 pull-right" :class="{
                            'btn-success': alertStore.type == AlertType.Success,
                            'btn-error': alertStore.type == AlertType.Error,
                            'btn-primary': alertStore.type == AlertType.Default
                        }" @click="alertStore.closeAlert()">{{ alertStore.label }}</div>
                        <div v-if="alertStore.showCancelButton" class="btn memo-button col-xs-4 btn-default pull-right"
                            id="CancelAlert" @click="alertStore.closeAlert(true)">
                            Abbrechen
                        </div>
                        <div v-if="alertStore.msg != null" v-html="alertStore.msg.customBtn"></div>
                    </div>

                </div>
            </div>
        </VueFinalModal>
    </div>

</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#AlertModal {
    .success {
        color: @memo-green;
    }

    .error {
        color: @memo-salmon;
    }

    #CancelAlert {
        margin-right: 4px;
    }
}
</style>