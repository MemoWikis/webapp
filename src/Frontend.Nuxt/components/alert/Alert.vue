<script lang="ts" setup>
import { useAlertStore, AlertType } from './alertStore'

import successImage1 from '~/assets/images/Illustrations/hummingbird_flipped 500 x 500.png'
import successImage3 from '~/assets/images/Illustrations/girl_success2.png'
import successImage4 from '~/assets/images/Illustrations/bird_success 500 x 500.png'
import errorImage from '~/assets/images/Illustrations/owl_error3 500 x 500.png'

const alertStore = useAlertStore()

const randomImageSuccess = ref('')
const successImages = [
    successImage1,
    successImage3,
    successImage4,
]

onMounted(() => {
    shuffleSuccessImage()
})

watch(() => alertStore.show, (show) => {
    if (!show) return

    shuffleSuccessImage()
})

function shuffleSuccessImage(): void {
    const randomIdx = Math.floor(Math.random() * successImages.length)
    const newImage = successImages[randomIdx]
    if (successImages.length < 2 || newImage != randomImageSuccess.value) {
        randomImageSuccess.value = newImage
    } else {
        shuffleSuccessImage()
    }
}

const showDetails = ref(false)
const hasImage = computed(() => {
    return alertStore.msg?.customImg || alertStore.type === AlertType.Error || alertStore.type === AlertType.Success
})

async function copyToClipboard() {
    if (alertStore.msg?.customDetails) {
        const text = typeof alertStore.msg.customDetails === 'string'
            ? alertStore.msg.customDetails
            : JSON.stringify(alertStore.msg.customDetails, null, 2)
        await navigator.clipboard.writeText(text)
    }
}

watch(() => alertStore.show, (show) => {
    if (!show) {
        showDetails.value = false
    }
})

const { t } = useI18n()
</script>

<template>
    <VueFinalModal v-model="alertStore.show" :z-index-auto="false" @keydown.esc="alertStore.closeAlert(true)"
        @close="alertStore.closeAlert(true)">
        <div class="modal-dialog" :class="{ 'has-icon': alertStore.type != AlertType.Default }" role="document">
            <div class="modal-content">

                <div class="modal-body">
                    <div class="alert-body">
                        <div v-if="hasImage" class="alert-img-container">
                            <img v-if="alertStore.msg?.customImg" width="200" :src="alertStore.msg?.customImg" />
                            <img v-else-if="alertStore.type === AlertType.Error" width="200" :src="errorImage" />
                            <img v-else-if="alertStore.type === AlertType.Success" width="200"
                                :src="randomImageSuccess" />
                        </div>
                        <div>
                            <h3 v-if="alertStore.title != null && alertStore.title.length > 0"
                                :class="{ 'has-image': hasImage }">
                                <font-awesome-icon v-if="alertStore.type === AlertType.Success"
                                    icon="fa-solid fa-circle-check" class="success" />
                                <font-awesome-icon v-else-if="alertStore.type === AlertType.Error"
                                    icon="fa-solid fa-circle-xmark" class="error" />
                                {{ alertStore.title }}
                            </h3>
                            <div v-if="alertStore.text || alertStore.texts" class="alert-msg-container">
                                <div class="alert-msg">
                                    <template v-if="alertStore.texts.length > 0">
                                        <template v-for="(text, i) in alertStore.texts" :key="i">
                                            {{ text }}
                                            <br v-if="i + 1 < alertStore.texts.length" />
                                        </template>
                                    </template>
                                    <template v-else-if="alertStore.text">
                                        {{ alertStore.text }}
                                    </template>
                                </div>
                            </div>
                            <div v-if="alertStore.msg != null" v-html="alertStore.msg.customHtml" />
                        </div>
                    </div>
                    <div v-if="alertStore.msg?.customDetails" class="alert-details">
                        <div v-if="!showDetails" class="alert-details-label" @click="showDetails = !showDetails">
                            {{ t('alert.showDetails') }}
                        </div>
                        <div v-if="showDetails" class="alert-details-code">
                            <div class="code-container">
                                <code> {{ alertStore.msg.customDetails }} </code>
                            </div>
                            <div class="copy-container">
                                <button class="btn btn-primary btn-sm" @click="copyToClipboard">
                                    <font-awesome-icon :icon="['fas', 'copy']" />
                                    {{ t('alert.copySourceCode') }}
                                </button>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <button class="btn memo-button pull-right" :class="{
                        'btn-success': alertStore.type === AlertType.Success,
                        'btn-primary': alertStore.type === AlertType.Error || alertStore.type === AlertType.Default
                    }" @click="alertStore.closeAlert()">{{ alertStore.label }}</button>
                    <button v-if="alertStore.showCancelButton" class="btn memo-button btn-link pull-right cancel-alert"
                        @click="alertStore.closeAlert(true)">
                        {{ alertStore.cancelLabel }}
                    </button>
                    <div v-if="alertStore.msg != null && alertStore.msg.customBtn"
                        @click="alertStore.closeAlert(false, alertStore.msg!.customBtnKey)"
                        v-html="alertStore.msg.customBtn" />
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
    color: @memo-wish-knowledge-red;
}

.cancel-alert {
    margin-right: 4px;
}

.modal-footer {
    padding-top: 0;
    margin-top: 0;
}

@media(min-width: 992px) {
    .modal-dialog {
        margin: 200px auto;
    }
}

.alert-body {
    display: flex;

    h3 {
        &.has-image {
            padding-left: 20px;
        }
    }

    .alert-img-container {
        min-width: 200px;
        width: 200px;
    }
}

.alert-details {
    margin-top: 24px;

    .alert-details-label {
        cursor: pointer;
        color: @memo-blue-link;
        text-align: right;
    }

    .alert-details-code {
        display: block;
        padding: 9.5px;
        margin: 0 0 10px;
        font-size: 13px;
        line-height: 1.42857143;
        word-break: break-all;
        word-wrap: break-word;
        background-color: @memo-grey-lighter;
        border: 1px solid @memo-grey-light;
        border-radius: 4px;

        .code-container {
            max-height: 400px;
            overflow-y: scroll;

            code {
                font-family: Menlo,
                    Monaco,
                    Consolas,
                    "Courier New",
                    monospace;

                color: @memo-grey-darker;
                background-color: transparent;
                white-space: pre;

            }
        }

        .copy-container {
            display: flex;
            justify-content: flex-end;
            margin-top: 16px;
        }
    }
}

:deep(.alert-msg-container) {
    padding: 25px 0 0;
    display: flex;
    justify-content: flex-start;
    align-items: center;
    font-size: 18px;
    line-height: 1.5;

    .msg-icon {
        font-size: 25px;
    }

    .has-icon & {

        .alert-msg {
            padding-left: 20px;
            flex-grow: 1;
        }
    }
}
</style>