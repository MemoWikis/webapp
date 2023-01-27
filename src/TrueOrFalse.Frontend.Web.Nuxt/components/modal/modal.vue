<script lang="ts" setup>
import { ref } from 'vue'
const props = defineProps([
    'id',
    'showCloseButton',
    'adminContent',
    'modalType',
    'iconClasses',
    'primaryBtn',
    'secondaryBtn',
    'modalWidth',
    'isFullSizeButtons',
    'show',
    'escToClose',
    'disabled',
    'cancelBtn'
])

const isError = ref(false)
const isSuccess = ref(false)
const modalWidthData = ref(props.modalWidth + 'px')
const slots = useSlots()
</script>


<template>

    <VueFinalModal v-model="props.show" class="modal-container" content-class="modal-content" :z-index-auto="false">
        <div id="defaultModal" class="modal-default">
            <div class="modal-default-mask" @click="$emit('close')">
                <div class="modal-default-wrapper">
                    <div class="modal-default-container" :style="{ width: modalWidthData }" v-on:click.stop
                        :class="{ 'no-close-button': !props.showCloseButton }">
                        <div>
                            <font-awesome-icon v-if="props.showCloseButton" icon="fa-solid fa-xmark"
                                class="pull-right pointer modal-close-button" @click="$emit('close')" />
                            <div class="header-default-modal"
                                v-bind:class="{ errorHeaderModal: isError, successHeaderModal: isSuccess }">
                                <div class="iconHeaderModal" v-if="isError || isSuccess || !!$slots.headerIcon">
                                    <font-awesome-icon v-if="isError" icon="fa-solid fa-circle-xmark iconHeaderModal" />
                                    <font-awesome-icon v-else-if="isSuccess"
                                        icon="fa-solid fa-circle-check iconHeaderModal" />
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
                                        <button v-if="props.primaryBtn != null"
                                            class="btn btn-primary memo-button pull-right modal-button" v-bind:class="{
                                                'errorButton1Modal': isError,
                                                'successButton1Modal': isSuccess,
                                                'fullSizeButtons': props.isFullSizeButtons
                                            }" @click="$emit('main-btn')" :disabled="disabled">
                                            {{ props.primaryBtn }}
                                        </button>
                                        <button v-if="props.secondaryBtn != null"
                                            class="btn btn-lg btn-link memo-button pull-right modalSecondActionButton modal-button"
                                            v-bind:class="{
                                                'errorButton2Modal': isError,
                                                'successButton2Modal': isSuccess,
                                                'fullSizeButtons': props.isFullSizeButtons
                                            }" @click="$emit('sub-btn')">
                                            {{ props.secondaryBtn }}
                                        </button>
                                        <button v-if="props.cancelBtn"
                                            class="btn btn-lg btn-link memo-button pull-right modalSecondActionButton modal-button"
                                            @click="$emit('close')">
                                            Abbrechen
                                        </button>
                                    </div>
                                </div>
                                <div class="modal-default-footer-text" v-if="slots['footer-text']">
                                    <slot name="footer-text"></slot>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>


    </VueFinalModal>



</template>

<style scoped lang="less">
.no-close-button {
    padding-top: 24px;
}
</style>