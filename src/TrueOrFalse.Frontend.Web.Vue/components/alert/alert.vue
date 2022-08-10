<script lang="ts" setup>
import { ref } from 'vue'
import { useAlertStore, AlertType } from './alertStore'
const alertStore = useAlertStore()

</script>

<template>
    <div id="AlertModal">

        <vue-final-modal v-model="alertStore.show" @keydown.esc="alertStore.show = false" classes="modal-container"
            content-class="modal-content">
            <div class="modal-dialog" role="document">
                <div class="modal-content">

                    <div class="modal-body">
                        <h3>
                            <font-awesome-icon v-if="alertStore.type == AlertType.Success"
                                icon="fa-solid fa-circle-check" />
                            <font-awesome-icon v-else-if="alertStore.type == AlertType.Error"
                                icon="fa-solid fa-circle-xmark" />
                        </h3>

                        <div class="">{{ alertStore.text }}</div>
                        <div v-if="alertStore.msg != null" v-html="alertStore.msg.customHtml"></div>
                    </div>

                    <div class="modal-footer">
                        <div class="btn memo-button col-xs-4" :class="{
                            'btn-success': alertStore.type == AlertType.Success,
                            'btn-error': alertStore.type == AlertType.Error,
                            'btn-primary': alertStore.type == AlertType.Default
                        }" data-dismiss="modal">Ok</div>
                        <div v-if="alertStore.msg != null" v-html="alertStore.msg.customBtn"></div>
                    </div>

                </div>
            </div>
        </vue-final-modal>
    </div>

</template>