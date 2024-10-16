<script lang="ts" setup>

interface Props {
    show: boolean
    escToClose?: boolean
    disabled?: boolean
    showCancelBtn?: boolean
    id?: number
    showCloseButton?: boolean
    adminContent?: boolean
    iconClasses?: string
    primaryBtnLabel?: string
    secondaryBtnLabel?: string
    isFullSizeButtons?: boolean
}

const props = defineProps<Props>()

const isError = ref(false)
const isSuccess = ref(false)
const slots = useSlots()

const emit = defineEmits(['close', 'primary-btn', 'secondary-btn'])
const openModal = ref(false)
const hasHeader = ref(false)
watch(() => props.show, (val) => {
    hasHeader.value = slots.header !== undefined
    openModal.value = val
}, { immediate: true })
</script>


<template>
    <VueFinalModal v-model="openModal" class="modal-container" content-class="modal-content" :z-index-auto="false">

        <div class="modal-default">
            <div class="modal-default-mask" @click="emit('close')">
                <div class="modal-default-wrapper">
                    <div class="modal-default-container" v-on:click.stop
                        :class="{ 'no-close-button': !props.showCloseButton, 'has-header': hasHeader }">
                        <div>
                            <font-awesome-icon v-if="props.showCloseButton" icon="fa-solid fa-xmark"
                                class="pull-right pointer modal-close-button" @click="emit('close')" />
                            <div class="header-default-modal"
                                v-bind:class="{ errorHeaderModal: isError, successHeaderModal: isSuccess }">
                                <div class="modal-header-icon" v-if="isError || isSuccess || !!$slots.headerIcon">
                                    <font-awesome-icon v-if="isError" icon="fa-solid fa-circle-xmark modal-header-icon" />
                                    <font-awesome-icon v-else-if="isSuccess"
                                        icon="fa-solid fa-circle-check modal-header-icon" />
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

                            <div class="modal-default-footer"
                                v-if="props.primaryBtnLabel || props.secondaryBtnLabel || props.showCancelBtn || slots['footer-text']">
                                <slot name="footer"></slot>
                                <div class="row">
                                    <div class="col-xs-12">
                                        <button v-if="props.primaryBtnLabel"
                                            class="btn btn-primary memo-button pull-right modal-button col-sm" :class="{
                                                'primary-error-button': isError,
                                                'primary-success-button': isSuccess,
                                                'full-size-buttons': props.isFullSizeButtons
                                            }" @click="emit('primary-btn')" :disabled="disabled">
                                            {{ props.primaryBtnLabel }}
                                        </button>
                                        <button v-if="props.secondaryBtnLabel"
                                            class="btn btn-lg btn-link memo-button pull-right secondary-action-button modal-button col-sm"
                                            :class="{
                                                'secondary-error-button': isError,
                                                'secondary-success-button': isSuccess,
                                                'full-size-buttons': props.isFullSizeButtons
                                            }" @click="emit('secondary-btn')">
                                            {{ props.secondaryBtnLabel }}
                                        </button>
                                        <button v-if="props.showCancelBtn"
                                            class="btn btn-lg btn-link memo-button pull-right secondary-action-button modal-button col-sm"
                                            :class="{
                                                'full-size-buttons': props.isFullSizeButtons
                                            }" @click="emit('close')">
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

<style lang="less">
.modal-default-container  {
    &.has-header {
        h2 {
            margin-bottom: 36px;
        }
    }
}
</style>