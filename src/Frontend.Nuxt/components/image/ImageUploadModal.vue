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

const handlePaste = (e: ClipboardEvent) => {
    if (!props.show) {
        return
    }

    const items = e.clipboardData?.items
    if (!items) {
        return
    }

    for (const item of items) {
        if (imageTypes.includes(item.type)) {
            const file = item.getAsFile()
            if (file) {
                selectedImageUploadMode.value = ImageUploadMode.Custom
                showTypeError.value = false
                createImage(file)
                e.preventDefault()
                return
            }
        }
    }
}

watch(() => props.show, (visible) => {
    if (visible) {
        document.addEventListener('paste', handlePaste)
    } else {
        document.removeEventListener('paste', handlePaste)
    }
})

onUnmounted(() => {
    document.removeEventListener('paste', handlePaste)
})

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

const licenseConfirmed = ref(false)

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
        data.append('licenseOwner', 'Eigenes Werk / freie Lizenz')
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
    else if (selectedImageUploadMode.value === ImageUploadMode.Custom && imageLoaded.value && licenseConfirmed.value)
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
    licenseConfirmed.value = false
}
</script>

<template>
    <Modal :show="props.show" :show-cancel-btn="true" @close="emit('close')" @primary-btn="upload"
        :primary-btn-label="primaryLabel" :disabled="disablePrimaryButton" container-class="image-upload-modal">
        <template v-slot:header>
            {{ t('image.upload.header') }}
        </template>
        <template v-slot:body>
            <div class="upload-notice">
                <font-awesome-icon :icon="['fas', 'circle-info']" class="upload-notice-icon" />
                {{ t('image.upload.warnings.attention') }}
            </div>

            <div class="mode-tabs">
                <button class="mode-tab" :class="{ active: selectedImageUploadMode === ImageUploadMode.Wikimedia }"
                    @click="selectedImageUploadMode = ImageUploadMode.Wikimedia">
                    <font-awesome-icon icon="fa-solid fa-globe" class="mode-tab-icon" />
                    {{ t('image.upload.options.wikimedia') }}
                </button>
                <button class="mode-tab" :class="{ active: selectedImageUploadMode === ImageUploadMode.Custom }"
                    @click="selectedImageUploadMode = ImageUploadMode.Custom">
                    <font-awesome-icon icon="fa-solid fa-upload" class="mode-tab-icon" />
                    {{ t('image.upload.options.custom') }}
                </button>
            </div>

            <Transition name="fade">
                <div v-if="selectedImageUploadMode === ImageUploadMode.Wikimedia" class="content">
                    <p class="wikimedia-hint">
                        {{ t('image.upload.wikimedia.info') }}
                        <font-awesome-icon :icon="['fas', 'circle-info']"
                            v-tooltip="t('image.upload.wikimedia.tip')" class="wikimedia-tip-icon" />
                    </p>

                    <div class="form-group">
                        <label class="input-label">{{ t('image.upload.wikimedia.urlLabel') }}</label>
                        <input class="form-control wikimedia-url-input" v-model="wikimediaUrl" placeholder="https://commons.wikimedia.org/wiki/File:..." />
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
                    <div class="imageupload-dropzone" @drop.prevent="handleImageChange"
                        :class="{ 'active': onDragOver }" @dragover.prevent="onDragOver = true"
                        @dragleave.prevent="onDragOver = false">
                        <input type="file" class="imageupload-dropzone-input" :accept="imageTypes.join(', ')"
                            name="file" id="imageUpload" v-on:change="handleImageChange" />
                        <font-awesome-icon icon="fa-solid fa-cloud-arrow-up" class="dropzone-icon" />
                        <div class="dropzone-text">
                            {{ t('image.upload.dropzone.dragHere') }}
                        </div>
                        <div class="dropzone-actions">
                            <label for="imageUpload" class="btn memo-button btn-primary dropzone-btn">
                                {{ t('image.upload.buttons.chooseFile') }}
                            </label>
                            <div class="paste-hint">
                                {{ t('image.upload.dropzone.pasteHint') }}
                            </div>
                        </div>
                    </div>
                    <div v-if="showTypeError" class="alert alert-warning">
                        {{ t('image.upload.warnings.allowedFormats', { formats: allowedExtensions.join(', ') }) }}
                    </div>
                    <div v-if="imageLoaded" class="image-preview-container">
                        <b>{{ t('image.upload.preview') }}</b>
                        <Image :src="customImgUrl" :format="ImageFormat.Page" class="image-preview" :square="true" />
                    </div>
                    <div v-if="imageLoaded" class="license-container">
                        <label class="license-checkbox-label" @click.prevent="licenseConfirmed = !licenseConfirmed">
                            <input type="checkbox" v-model="licenseConfirmed" class="license-checkbox" />
                            {{ t('image.upload.license.confirmation') }}
                        </label>
                    </div>
                </div>
            </Transition>
        </template>
    </Modal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.upload-notice {
    display: flex;
    align-items: flex-start;
    gap: 8px;
    font-size: 13px;
    line-height: 1.5;
    color: @memo-grey-dark;
    background: @memo-grey-lightest;
    border-radius: 6px;
    padding: 10px 14px;
    margin-bottom: 20px;

    .upload-notice-icon {
        color: @memo-blue-link;
        margin-top: 3px;
        flex-shrink: 0;
    }
}

.mode-tabs {
    display: flex;
    gap: 0;
    border-bottom: 2px solid @memo-grey-light;
    margin-bottom: 20px;

    .mode-tab {
        flex: 1;
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
        padding: 10px 16px;
        border: none;
        background: none;
        color: @memo-grey-dark;
        font-size: 14px;
        font-weight: 500;
        cursor: pointer;
        position: relative;
        transition: color 0.2s;

        &::after {
            content: '';
            position: absolute;
            bottom: -2px;
            left: 0;
            right: 0;
            height: 2px;
            background: transparent;
            transition: background 0.2s;
        }

        &:hover {
            color: @memo-blue-link;
        }

        &.active {
            color: @memo-blue-link;

            &::after {
                background: @memo-blue-link;
            }
        }

        .mode-tab-icon {
            font-size: 15px;
        }
    }
}

.content {
    .wikimedia-hint {
        font-size: 14px;
        line-height: 1.6;
        color: @memo-grey-darker;
        margin-bottom: 16px;

        .wikimedia-tip-icon {
            color: @memo-blue-link;
            cursor: pointer;
            margin-left: 4px;
        }
    }

    .input-label {
        display: block;
        font-size: 13px;
        font-weight: 500;
        color: @memo-grey-darker;
        margin-bottom: 6px;
    }

    .wikimedia-url-input {
        border-radius: 6px;
        border: 1px solid @memo-grey-light;
        padding: 10px 14px;
        font-size: 14px;
        transition: border-color 0.2s;

        &:focus {
            border-color: @memo-blue-link;
            outline: none;
            box-shadow: 0 0 0 3px @memo-blue-light-transparent;
        }
    }
}

.imageupload-dropzone-container {
    .imageupload-dropzone {
        width: 100%;
        min-height: 180px;
        border: 2px dashed @memo-grey-light;
        border-radius: 12px;
        background: @memo-grey-lightest;
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
        text-align: center;
        padding: 28px 20px;
        gap: 8px;
        transition: border-color 0.2s, background 0.2s;
        cursor: default;

        &:hover {
            border-color: @memo-blue-link;
        }

        &.active {
            border-color: @memo-blue-link;
            background: @memo-blue-light-transparent;
        }

        .imageupload-dropzone-input {
            display: none;
        }

        .dropzone-icon {
            font-size: 36px;
            color: @memo-blue-link;
            opacity: 0.7;
        }

        .dropzone-text {
            font-size: 15px;
            color: @memo-grey-darker;
        }

        .dropzone-actions {
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 6px;
            margin-top: 4px;

            .dropzone-btn {
                font-size: 13px;
                padding: 6px 20px;
                border-radius: 6px;
                cursor: pointer;
                margin: 0;
            }
        }

        .paste-hint {
            font-size: 12px;
            color: @memo-grey;
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

.license-checkbox-label {
    display: flex;
    align-items: flex-start;
    gap: 8px;
    cursor: pointer;
    font-size: 14px;
    line-height: 1.4;
    color: @memo-grey-dark;

    .license-checkbox {
        margin-top: 3px;
        flex-shrink: 0;
    }
}
</style>