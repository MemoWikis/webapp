<script lang="ts" setup>
import {ref} from 'vue'
const props = defineProps([
            'id', 
            'showCloseButton', 
            'adminContent', 
            'modalType', 
            'iconClasses', 
            'button1Text', 
            'button2Text',
            'modalWidth', 
            'isFullSizeButtons'
        ])

const isError = ref(false)
const isSuccess = ref(false)
const isAdminContent = ref(props.adminContent.value == true)
const modalWidthData = ref(props.modalWidth + 'px')

function closeModal() {
    document.body.classList.remove('no-scroll');
}
</script>


<template>
<div id="defaultModal" class="modal-default">
    <div class="modal-default-mask" @click="closeModal()">
        <div class="modal-default-wrapper">
            <div class="modal-default-container" :style="{width: modalWidthData}" v-on:click.stop>
                <div><img v-if="showCloseButton" src="/img/close_black.svg" class="pull-right pointer modal-close-button" @click="closeModal()"/>
                <div class="header-default-modal" v-bind:class="{ errorHeaderModal: isError, successHeaderModal: isSuccess }">
                    <div class="iconHeaderModal" v-if="isError || isSuccess || !!$slots.headerIcon">
                        <font-awesome-icon v-if="isError" icon="fa-solid fa-circle-xmark iconHeaderModal" />
                        <font-awesome-icon v-else-if="isSuccess" icon="fa-solid fa-circle-check iconHeaderModal" />
                        <slot name="headerIcon"></slot>
                    </div>
                    <slot name="header">
                    </slot>
                    
                    <i v-if="isAdminContent" class="fas fa-users-cog adminIconHeaderModal"></i>
                </div>
                <div class="modal-default-body">
                    <slot name="body">
                    </slot>
                </div>

                <div class="modal-default-footer">
                    <slot name="footer"></slot>
                    <div class="row">
                        <div class="col-xs-12">
                            <a v-if="button1Text != null" class="btn btn-primary memo-button pull-right modal-button" v-bind:class="{ 
                                errorButton1Modal: isError, 
                                successButton1Modal: isSuccess, 
                                fullSizeButtons: isFullSizeButtons }" @click="$emit('main-btn')">
                                {{button1Text}}
                            </a>
                            <a v-if="button2Text != null" class="btn btn-lg btn-link memo-button pull-right modalSecondActionButton modal-button" v-bind:class="{ 
                                errorButton2Modal: isError, 
                                successButton2Modal: isSuccess, 
                                fullSizeButtons: isFullSizeButtons }" @click="$emit('sub-btn')">
                                {{button2Text}}
                            </a>
                        </div>
                    </div>
                    <div class="modal-default-footer-text">
                        <slot name="footer-text"></slot>
                    </div>
                </div>

                </div>
            </div>
        </div>
    </div>
</div>
</template>