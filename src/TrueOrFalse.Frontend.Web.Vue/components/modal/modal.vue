<script lang="ts" setup>
import {ref, reactive } from 'vue'
import { useModalStore } from './modalStore'
const props = defineProps([
            'id', 
            'showCloseButton', 
            'adminContent', 
            'modalType', 
            'iconClasses', 
            'button1Text', 
            'button2Text',
            'modalWidth', 
            'isFullSizeButtons',
            'show'
        ])

const isError = ref(false)
const isSuccess = ref(false)
const modalWidthData = ref(props.modalWidth + 'px')

</script>


<template>
<Teleport to="body">
<div class="modal-bd" @click.self="$emit('close')" v-if="props.show">
    <div class="modal-c">
        <div id="defaultModal" class="modal-default">
            <div class="modal-default-mask" @click="$emit('close')">
                <div class="modal-default-wrapper">
                    <div class="modal-default-container" :style="{width: modalWidthData}" v-on:click.stop>
                        <div>
                            <img v-if="props.showCloseButton" src="/img/close_black.svg" class="pull-right pointer modal-close-button" @click="$emit('close')"/>
                        <div class="header-default-modal" v-bind:class="{ errorHeaderModal: isError, successHeaderModal: isSuccess }">
                            <div class="iconHeaderModal" v-if="isError || isSuccess || !!$slots.headerIcon">
                                <font-awesome-icon v-if="isError" icon="fa-solid fa-circle-xmark iconHeaderModal" />
                                <font-awesome-icon v-else-if="isSuccess" icon="fa-solid fa-circle-check iconHeaderModal" />
                                <slot name="headerIcon"></slot>
                            </div>
                            <slot name="header">
                            </slot>
                            <font-awesome-icon v-if="props.adminContent" icon="fa-solid fa-users-gear" />
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
                                        'errorButton1Modal': isError, 
                                        'successButton1Modal': isSuccess, 
                                        'fullSizeButtons': props.isFullSizeButtons }" @click="$emit('main-btn')">
                                        {{props.button1Text}}
                                    </a>
                                    <a v-if="button2Text != null" class="btn btn-lg btn-link memo-button pull-right modalSecondActionButton modal-button" v-bind:class="{ 
                                        'errorButton2Modal': isError, 
                                        'successButton2Modal': isSuccess, 
                                        'fullSizeButtons': props.isFullSizeButtons }" @click="$emit('sub-btn')">
                                        {{props.button2Text}}
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
    </div>
        
  </div>

</Teleport>
  
</template>

<style scoped>
.modal-bd {
    height: 100vw;
    width: 100vw;
    position: fixed;
    z-index: 998;
    background: rgba(0,0,0,0.5)
}
.modal-c {
    position: fixed;
    min-height: 300px;
    z-index: 999;
    width: 300px;
    left: 50%;
    top: 20%;
    background: white;
    margin-left: -150px;
    box-shadow: 0px 5px 10px 5px rgba(0,0,0,0.4)

}
</style>