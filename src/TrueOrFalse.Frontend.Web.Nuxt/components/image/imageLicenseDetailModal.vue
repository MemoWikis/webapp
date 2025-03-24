<script setup lang="ts">
import { handleNewLine } from '../shared/utils'
import { useImageLicenseStore } from './imageLicenseStore'
const imageLicenseStore = useImageLicenseStore()
const { t } = useI18n()
</script>

<template>
    <LazyModal :show="imageLicenseStore.show" @close="imageLicenseStore.show = false" :show-close-button="true"
        @keydown.esc="imageLicenseStore.show = false">
        <template v-slot:header>
            <h2>
                {{ t('image.license.modal.title') }}
            </h2>
        </template>
        <template v-slot:body>
            <div class="ImageContainer">
                <img :src="imageLicenseStore.url" />
                <div class="ImageInfo">
                    <div v-if="imageLicenseStore.attributionHtmlString.length > 0"
                        v-html="handleNewLine(imageLicenseStore.attributionHtmlString)">
                    </div>
                    <p class="description" v-if="imageLicenseStore.description.length > 0">
                        <span class="InfoLabel">{{ t('image.license.modal.description') }}</span>
                        {{ imageLicenseStore.description }}
                    </p>
                </div>
            </div>
        </template>
        <template v-slot:footer>

        </template>
    </LazyModal>
</template>

<style lang="less" scoped>
.description {
    margin-top: 10px;
}
</style>