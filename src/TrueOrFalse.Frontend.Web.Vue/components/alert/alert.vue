<script lang="ts" setup>
import { AlertType } from './alertCollection';
import { useAlertStore } from './alertStore';
const alertStore = useAlertStore()

</script>

<template>
    <vue-final-modal>
        <div id="AlertModal" class="modal fade">
            <div class="modal-dialog" role="document">
                <div class="modal-content">

                    <div class="modal-body">
                        <h3>
                            <font-awesome-icon v-if="alertStore.type == AlertType.Success" icon="fa-solid fa-circle-check" />
                            <font-awesome-icon v-else-if="alertStore.type == AlertType.Error" icon="fa-solid fa-circle-xmark" />
                        </h3>

                        <div class="">{{alertStore.msg.text}}</div>
                        <div v-if="alertStore.msg.customHtml.length > 0" v-html="alertStore.msg.customHtml"></div>
                    </div>

                    <div class="modal-footer">
                        <div class="btn memo-button col-xs-4" 
                        :class="{ 
                            'btn-success': alertStore.type == AlertType.Success, 
                            'btn-error': alertStore.type == AlertType.Error, 
                            'btn-primary': alertStore.type == AlertType.Default }" 
                            data-dismiss="modal">Ok</div>
                        <div v-if="alertStore.msg.customBtn.length > 0" v-html="alertStore.msg.customBtn"></div>
                    </div>

                </div>
            </div>
        </div>
    </vue-final-modal>
        
</template>