<script lang="ts" setup>
import { AlertType, useAlertStore } from '../alert/alertStore'
import { usePageStore } from '../page/pageStore'
import { ImageFormat } from './imageFormatEnum'

const pageStore = usePageStore()
const alertStore = useAlertStore()
const { t } = useI18n()

interface Props {
    show: boolean
}
const props = defineProps<Props>()
enum ImageUploadMode {
    Wikimedia,
    Custom
}
const selectedImageUploadMode = ref<ImageUploadMode>(ImageUploadMode.Wikimedia)

const emit = defineEmits(['close'])
const imageLoaded = ref(false)

const primaryLabel = ref(t('image.upload.buttons.loadPreview'))
watch(imageLoaded, (val) => {
    if (val)
        primaryLabel.value = t('image.upload.buttons.useImage')
    else primaryLabel.value = t('image.upload.buttons.loadPreview')
})

const wikimediaUrl = ref('')
const allowedExtensions = ['jpeg', 'jpg', 'png', 'svg', 'gif']
const showWikimediaError = ref(false)

interface WikimediaPreviewResult {
    imageFound: boolean
    imageThumbUrl: string
}
const wikiMediaPreviewUrl = ref('')
const { $logger } = useNuxtApp()

async function loadWikimediaImage() {
    const data = {
        url: wikimediaUrl.value
    }
    const result = await $api<WikimediaPreviewResult>('/apiVue/ImageUploadModal/GetWikimediaPreview', {
        mode: 'cors',
        credentials: 'include',
        method: 'POST',
        body: data,
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])

        }
    })
    if (result.imageFound) {
        wikiMediaPreviewUrl.value = result.imageThumbUrl
    } else {
        wikiMediaPreviewUrl.value = ''
    }
}
watch(wikimediaUrl, (url) => {
    if (allowedExtensions.some(end => url.endsWith(end))) {
        showWikimediaError.value = false
        loadWikimediaImage()
    } else {
        showWikimediaError.value = true
    }
})

watch(wikiMediaPreviewUrl, (url) => {
    if (selectedImageUploadMode.value === ImageUploadMode.Wikimedia && url?.length > 0)
        imageLoaded.value = true
    else if (selectedImageUploadMode.value === ImageUploadMode.Wikimedia && (url == null || url?.length <= 0))
        imageLoaded.value = false
})

const imgFile = ref<File>()
const customImgUrl = ref('')
const showTypeError = ref(false)
const imageTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/svg', 'image/gif']

function handleImageChange(e: any) {
    const files = e.target.files || e.dataTransfer.files

    if (files.length > 0 && imageTypes.some(end => files[0].type === end)) {
        showTypeError.value = false
        createImage(files[0])
    }
    else showTypeError.value = true
}
const onDragOver = ref(false)

function createImage(file: File) {
    imgFile.value = file
    const previewImgUrl = URL.createObjectURL(file)
    customImgUrl.value = previewImgUrl
}

watch(customImgUrl, (url) => {
    if (selectedImageUploadMode.value === ImageUploadMode.Custom && url?.length > 0)
        imageLoaded.value = true
    else if (selectedImageUploadMode.value === ImageUploadMode.Custom && (url == null || url?.length <= 0))
        imageLoaded.value = false
})

watch(selectedImageUploadMode, (mode) => {
    if ((selectedImageUploadMode.value === ImageUploadMode.Wikimedia && wikiMediaPreviewUrl.value.length > 0) || (selectedImageUploadMode.value === ImageUploadMode.Custom && customImgUrl.value.length > 0))
        imageLoaded.value = true
    else imageLoaded.value = false
})

const licenseGiverName = ref('')
const isPersonalCreation = ref<boolean>()

async function upload() {
    let url
    let data
    if (selectedImageUploadMode.value === ImageUploadMode.Wikimedia) {
        url = '/apiVue/ImageUploadModal/SaveWikimediaImage'
        data = {
            pageId: pageStore.id,
            url: wikimediaUrl.value
        }
    } else {
        url = '/apiVue/ImageUploadModal/SaveCustomImage'

        data = new FormData()
        if (imgFile.value == null)
            return

        data.append('file', imgFile.value)
        data.append('pageId', pageStore.id.toString())
        data.append('licenseOwner', licenseGiverName.value)
    }
    const result = await $api<boolean>(url, {
        body: data,
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])

        }
    })

    if (result) {
        emit('close')
        alertStore.openAlert(AlertType.Success, { text: t('success.page.saveImage') })
        pageStore.refreshPageImage()
        resetModal()
    } else {
        alertStore.openAlert(AlertType.Error, { text: t('error.page.saveImageError ') })
    }
}

const disablePrimaryButton = computed(() => {
    if (selectedImageUploadMode.value === ImageUploadMode.Wikimedia && imageLoaded.value)
        return false
    else if (selectedImageUploadMode.value === ImageUploadMode.Custom && imageLoaded.value && isPersonalCreation.value && licenseGiverName.value.length >= 3)
        return false
    else return true
})

function resetModal() {
    selectedImageUploadMode.value = ImageUploadMode.Wikimedia
    imageLoaded.value = false
    wikimediaUrl.value = ''
    showWikimediaError.value = false
    wikiMediaPreviewUrl.value = ''
    imgFile.value = undefined
    customImgUrl.value = ''
    showTypeError.value = false
    licenseGiverName.value = ''
    isPersonalCreation.value = undefined
}
</script>

<template>
    <Modal :show="props.show" :show-cancel-btn="true" @close="emit('close')" @primary-btn="upload"
        :primary-btn-label="primaryLabel" :disabled="disablePrimaryButton">
        <template v-slot:header>
            {{ t('image.upload.header') }}
        </template>
        <template v-slot:body>
            <div class="alert alert-info">
                <b>{{ t('label.attention') }}:</b> {{ t('image.upload.warnings.attention') }}
            </div>
            <div class="imagetype-select-container">
                <div @click="selectedImageUploadMode = ImageUploadMode.Wikimedia" class="imagetype-select">
                    <font-awesome-icon icon="fa-solid fa-circle-dot" class="imagetype-select-radio active"
                        v-if="selectedImageUploadMode === ImageUploadMode.Wikimedia" />
                    <font-awesome-icon icon="fa-regular fa-circle" class="imagetype-select-radio" v-else />
                    {{ t('image.upload.options.wikimedia') }}
                </div>

                <div @click="selectedImageUploadMode = ImageUploadMode.Custom" class="imagetype-select">
                    <font-awesome-icon icon="fa-solid fa-circle-dot" class="imagetype-select-radio active"
                        v-if="selectedImageUploadMode === ImageUploadMode.Custom" />
                    <font-awesome-icon icon="fa-regular fa-circle" class="imagetype-select-radio" v-else />
                    {{ t('image.upload.options.custom') }}
                </div>
            </div>
            <Transition name="fade">
                <div v-if="selectedImageUploadMode === ImageUploadMode.Wikimedia" class="content">
                    <p>{{ t('image.upload.wikimedia.info') }}</p>

                    <p>
                        {{ t('image.upload.wikimedia.tip') }}
                    </p>

                    <div class="form-group">
                        <input class="form-control wikimedia-url-input" v-model="wikimediaUrl" placeholder="http://" />
                        <small class="form-text text-muted">{{ t('image.upload.wikimedia.urlLabel') }} <font-awesome-icon
                                :icon="['fas', 'circle-info']"
                                v-tooltip="t('image.upload.wikimedia.urlTooltip')" /></small>
                    </div>
                    <div v-if="showWikimediaError" class="alert alert-warning">
                        {{ t('image.upload.warnings.allowedFormats', { formats: allowedExtensions.join(', ') }) }}
                    </div>

                    <div v-if="imageLoaded" class="image-preview-container">
                        <b>{{ t('image.upload.preview') }}</b>
                        <Image :src="wikiMediaPreviewUrl" :format="ImageFormat.Page" class="image-preview"
                            :square="true" />
                    </div>
                </div>
                <div v-else-if="selectedImageUploadMode === ImageUploadMode.Custom"
                    class="imageupload-dropzone-container">
                    <label for="imageUpload" class="imageupload-dropzone" @drop.prevent="handleImageChange"
                        :class="{ 'active': onDragOver }" @dragover.prevent="onDragOver = true"
                        @dragleave.prevent="onDragOver = false">
                        <input type="file" class="imageupload-dropzone-input" :accept="imageTypes.join(', ')"
                            name="file" id="imageUpload" v-on:change="handleImageChange" />
                        <div>
                            <h4>
                                <font-awesome-icon icon="fa-solid fa-upload" />
                                {{ t('image.upload.dropzone.title') }}
                            </h4>
                        </div>
                        <div>
                            {{ t('image.upload.dropzone.dragHere') }}
                            <br />
                            {{ t('image.upload.dropzone.or') }}
                            <br />
                            <div class="memo-button btn-link btn">
                                {{ t('image.upload.buttons.chooseFile') }}
                            </div>
                        </div>
                    </label>
                    <div v-if="showTypeError" class="alert alert-warning">
                        {{ t('image.upload.warnings.allowedFormats', { formats: allowedExtensions.join(', ') }) }}
                    </div>
                    <div v-if="imageLoaded" class="image-preview-container">
                        <b>{{ t('image.upload.preview') }}</b>
                        <Image :src="customImgUrl" :format="ImageFormat.Page" class="image-preview" :square="true" />
                    </div>
                    <div v-if="imageLoaded" class="license-container">
                        <b>{{ t('image.upload.license.title') }}</b>
                        <p>{{ t('image.upload.license.info') }}</p>

                        <div>
                            <div @click="isPersonalCreation = true" class="license-select">
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="license-select-radio active"
                                    v-if="isPersonalCreation === true" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="license-select-radio" v-else />
                                {{ t('image.upload.license.isOwnWork') }}
                            </div>
                            <p v-if="isPersonalCreation === true" class="license-info">
                                <i18n-t keypath="image.upload.license.declaration" tag="span">
                                    <template #name>
                                        <input v-model="licenseGiverName" :placeholder="t('image.upload.license.namePlaceholder')"
                                            class="creator-name-input" />
                                    </template>
                                    <template #licenseLink>
                                        <NuxtLink to="https://creativecommons.org/licenses/by/4.0/deed.de" :external="true">
                                            {{ t('image.upload.license.licenseText') }}
                                        </NuxtLink>
                                    </template>
                                </i18n-t>
                            </p>

                            <div @click="isPersonalCreation = false" class="license-select">
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="license-select-radio active" v-if="isPersonalCreation === false" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="license-select-radio" v-else />
                                {{ t('image.upload.license.notOwnWork') }}
                            </div>
                            <p v-if="isPersonalCreation === false" class="license-info">
                                {{ t('image.upload.license.useWikimedia') }}
                                <NuxtLink to="https://commons.wikimedia.org/wiki/Main_Page" :external="true">
                                    Wikimedia
                                </NuxtLink>
                                {{ t('label.toUpload') }}
                            </p>
                        </div>
                    </div>
                </div>
            </Transition>
        </template>
    </Modal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.imagetype-select-container {
    display: flex;
    align-items: center;
    flex-wrap: wrap;
}

.imagetype-select,
.license-select {
    display: flex;
    flex-wrap: nowrap;
    align-items: center;
    cursor: pointer;
    margin-right: 20px;
    padding-bottom: 8px;
    padding-top: 8px;

    .imagetype-select-radio,
    .license-select-radio {
        margin-right: 8px;

        &.active {
            color: @memo-blue-link;
        }
    }
}

.content {
    margin-top: 20px;
}

.wikimedia-url-input {
    border-radius: 0px
}

.imageupload-dropzone-container {
    padding-top: 20px;

    .imageupload-dropzone {
        height: 160px;
        width: 100%;
        border: dashed 1px silver;
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
        text-align: center;

        &.active {
            background: @memo-grey-lighter;
        }

        .imageupload-dropzone-input {
            display: none;

            &::-webkit-file-upload-button {
                border: none;
                font-size: 0px;
                width: 100%;
                min-height: 200px;
                color: white;
            }

            .imageupload-dropzone-input-visible {
                visibility: visible;
            }
        }
    }
}

.image-preview-container,
.license-container {
    padding-top: 20px;
    width: 100%;
}

.image-preview-container {
    .image-preview {
        display: flex;
        justify-content: center;
        align-items: center;
    }
}

.creator-name-input {
    border: solid 1px @memo-grey-light;
}

.license-info {
    padding-left: 22px;
}
</style>