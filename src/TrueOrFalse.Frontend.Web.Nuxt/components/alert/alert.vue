<script lang="ts" setup>
import { useAlertStore, AlertType } from './alertStore'

import successImage1 from '~/assets/images/illustrations/hummingbird_flipped 500 x 500.png';
import successImage2 from '~/assets/images/illustrations/parrot_flipped 500 x 500.png';
import successImage3 from '~/assets/images/illustrations/girl_success2.png';
import successImage4 from '~/assets/images/illustrations/bird_success 500 x 500.png';
import neutralImage from '~/assets/images/illustrations/butterfly 500 x 500.png';
import errorImage from '~/assets/images/illustrations/owl_error3 500 x 500.png';

const alertStore = useAlertStore()

const randomImageSuccess = ref('')
const successImages = [
    successImage1,
    // successImage2,
    successImage3,
    successImage4,
]

onMounted(() => {
    shuffleSuccessImage();
})

watch(() => alertStore.show, (show) => {
    if(!show) return

    shuffleSuccessImage();
})

function shuffleSuccessImage(): void {
    const randomIdx = Math.floor(Math.random() * successImages.length)
    var newImage = successImages[randomIdx]
    if(successImages.length < 2 || newImage != randomImageSuccess.value){
        randomImageSuccess.value = newImage
    } else {
        shuffleSuccessImage()
    }
}

</script>

<template>
    <VueFinalModal v-model="alertStore.show" @keydown.esc="alertStore.show = false" :z-index-auto="false"
        @close="alertStore.show = false">
        <div class="modal-dialog" :class="{ 'has-icon': alertStore.type != AlertType.Default }" role="document">
            <div class="modal-content">

                <div class="modal-body">
                    <h3 v-if="alertStore.title != null && alertStore.title.length > 0">
                        <font-awesome-icon v-if="alertStore.type == AlertType.Success" icon="fa-solid fa-circle-check"
                            class="success" />
                        <font-awesome-icon v-else-if="alertStore.type == AlertType.Error" icon="fa-solid fa-circle-xmark"
                            class="error" />
                        {{ alertStore.title }}
                    </h3>
                    <div class="alert-msg-container">
                        <template v-if="alertStore.title == null || alertStore.title.length == 0">
                            <img v-if="alertStore.type == AlertType.Error" width="200" :src="errorImage"/>
                            <img v-else-if="alertStore.type == AlertType.Success" width="200" :src="randomImageSuccess"/>
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
                    <div v-if="alertStore.msg != null && alertStore.msg.customBtn" v-html="alertStore.msg.customBtn"
                        @click="alertStore.closeAlert(false, alertStore.msg!.customBtnKey)">
                    </div>
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
    color: @memo-wuwi-red;
}

.cancel-alert {
    margin-right: 4px;
}

.alert-msg-container {
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

.modal-footer {
    padding-top: 0;
    margin-top: 0;
}

@media(min-width: 992px) {
    .modal-dialog {
        margin: 200px auto;
    }
}

</style>