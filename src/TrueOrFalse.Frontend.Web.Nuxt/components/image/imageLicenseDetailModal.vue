<script setup lang="ts">
import { useImageLicenseStore } from './imageLicenseStore'
const imageLicenseStore = useImageLicenseStore()
</script>

<template>
    <LazyModal>
        <template v-slot:header>
            Bilddetails
        </template>
        <template v-slot:body>
            <div class="ImageContainer">
                <img :src="imageLicenseStore.url" />
                <div class="ImageInfo">
                    <div v-if="imageLicenseStore.attributionHtmlString.length > 0"
                        v-html="imageLicenseStore.attributionHtmlString"> </div>
                    <div class="description" v-if="imageLicenseStore.description.length > 0">
                        <span class="InfoLabel">Beschreibung:</span>
                        {{ imageLicenseStore.description }}
                    </div>
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