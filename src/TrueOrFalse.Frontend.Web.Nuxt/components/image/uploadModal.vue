<script lang="ts" setup>
import { AlertType, messages, useAlertStore } from '../alert/alertStore'
import { usePageStore } from '../page/pageStore'
import { ImageFormat } from './imageFormatEnum'

const pageStore = usePageStore()
const alertStore = useAlertStore()

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

const primaryLabel = ref('Vorschau laden')
watch(imageLoaded, (val) => {
    if (val)
        primaryLabel.value = 'Bild übernehmen'
    else primaryLabel.value = 'Vorschau laden'
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
    if (selectedImageUploadMode.value == ImageUploadMode.Wikimedia && url?.length > 0)
        imageLoaded.value = true
    else if (selectedImageUploadMode.value == ImageUploadMode.Wikimedia && (url == null || url?.length <= 0))
        imageLoaded.value = false
})

const imgFile = ref<File>()
const customImgUrl = ref('')
const showTypeError = ref(false)
const imageTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/svg', 'image/gif']

function handleImageChange(e: any) {
    const files = e.target.files || e.dataTransfer.files

    if (files.length > 0 && imageTypes.some(end => files[0].type == end)) {
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
    if (selectedImageUploadMode.value == ImageUploadMode.Custom && url?.length > 0)
        imageLoaded.value = true
    else if (selectedImageUploadMode.value == ImageUploadMode.Custom && (url == null || url?.length <= 0))
        imageLoaded.value = false
})

watch(selectedImageUploadMode, (mode) => {
    if ((selectedImageUploadMode.value == ImageUploadMode.Wikimedia && wikiMediaPreviewUrl.value.length > 0) || (selectedImageUploadMode.value == ImageUploadMode.Custom && customImgUrl.value.length > 0))
        imageLoaded.value = true
    else imageLoaded.value = false
})

const licenseGiverName = ref('')
const isPersonalCreation = ref<boolean>()

async function upload() {
    let url
    let data
    if (selectedImageUploadMode.value == ImageUploadMode.Wikimedia) {
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
        alertStore.openAlert(AlertType.Success, { text: messages.success.page.saveImage })
        pageStore.refreshPageImage()
        resetModal()
    } else {
        alertStore.openAlert(AlertType.Error, { text: messages.error.page.saveImageError })
    }
}

const disablePrimaryButton = computed(() => {
    if (selectedImageUploadMode.value == ImageUploadMode.Wikimedia && imageLoaded.value)
        return false
    else if (selectedImageUploadMode.value == ImageUploadMode.Custom && imageLoaded.value && isPersonalCreation.value && licenseGiverName.value.length >= 3)
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
            Themenbild hochladen
        </template>
        <template v-slot:body>
            <div class="alert alert-info">
                <b>Achtung:</b> Bildrechte sind ein sensibles Thema. Bitte lade nur Bilder hoch, die gemeinfrei sind,
                die
                unter einer entsprechenden Lizenz stehen oder die du selbst erstellt hast.
            </div>
            <div class="imagetype-select-container">
                <div @click="selectedImageUploadMode = ImageUploadMode.Wikimedia" class="imagetype-select">
                    <font-awesome-icon icon="fa-solid fa-circle-dot" class="imagetype-select-radio active"
                        v-if="selectedImageUploadMode == ImageUploadMode.Wikimedia" />
                    <font-awesome-icon icon="fa-regular fa-circle" class="imagetype-select-radio" v-else />
                    Bilder von Wikimedia verwenden.
                </div>

                <div @click="selectedImageUploadMode = ImageUploadMode.Custom" class="imagetype-select">
                    <font-awesome-icon icon="fa-solid fa-circle-dot" class="imagetype-select-radio active"
                        v-if="selectedImageUploadMode == ImageUploadMode.Custom" />
                    <font-awesome-icon icon="fa-regular fa-circle" class="imagetype-select-radio" v-else />
                    Eigene Bilder
                </div>
            </div>
            <Transition name="fade">
                <div v-if="selectedImageUploadMode == ImageUploadMode.Wikimedia" class="content">
                    <p>
                        Bei Wikipedia/ Wikimedia sind viele Millionen Bilder zu finden, die frei genutzt werden können.
                        Auf
                        <NuxtLink to="https://commons.wikimedia.org/wiki/Hauptseite?uselang=de" :external="true">
                            Wikimedia-Commons</NuxtLink> kannst du gezielt nach Inhalten suchen.
                    </p>

                    <p>
                        Tipp: Wenn du bei <NuxtLink to="https://de.wikipedia.org/" :external="true">Wikipedia</NuxtLink>
                        oder
                        <NuxtLink to="https://commons.wikimedia.org/wiki/Hauptseite?uselang=de" :external="true">
                            Wikimedia-Commons</NuxtLink> auf das gewünschte Bild klickst, kommst du zur
                        Detailansicht. (Bei manchen Karten musst du auf das "i"-Logo in der rechten unteren Ecke
                        klicken.)
                        Kopiere einfach die Url dieser Seite.
                    </p>

                    <div class="form-group">
                        <input class="form-control wikimedia-url-input" v-model="wikimediaUrl" placeholder="http://" />
                        <small class="form-text text-muted">Wikimedia-URL <font-awesome-icon
                                :icon="['fas', 'circle-info']"
                                v-tooltip="'Hier kann für Bilder von Wikipedia/ Wikimedia wahlweise die Url der Detailseite, die Url der Bildanzeige im Media Viewer, die Url der Bilddatei oder der Dateiname (inkl. Dateiendung) angegeben werden.'" /></small>
                    </div>
                    <div v-if="showWikimediaError" class="alert alert-warning">
                        Nur folgende Formate sind erlaubt: {{
                            allowedExtensions.join(', ') }}
                    </div>

                    <div v-if="imageLoaded" class="image-preview-container">
                        <b>Bildvorschau:</b>
                        <Image :src="wikiMediaPreviewUrl" :format="ImageFormat.Page" class="image-preview"
                            :square="true" />
                    </div>
                </div>
                <div v-else-if="selectedImageUploadMode == ImageUploadMode.Custom"
                    class="imageupload-dropzone-container">
                    <label for="imageUpload" class="imageupload-dropzone" @drop.prevent="handleImageChange"
                        :class="{ 'active': onDragOver }" @dragover.prevent="onDragOver = true"
                        @dragleave.prevent="onDragOver = false">
                        <input type="file" class="imageupload-dropzone-input" :accept="imageTypes.join(', ')"
                            name="file" id="imageUpload" v-on:change="handleImageChange" />
                        <div>
                            <h4>
                                <font-awesome-icon icon="fa-solid fa-upload" />
                                Bild Hochladen
                            </h4>
                        </div>
                        <div>
                            Zieh ein Bild hierher
                            <br />
                            oder..
                            <br />
                            <div class="memo-button btn-link btn">
                                Datei auswählen
                            </div>
                        </div>
                    </label>
                    <div v-if="showTypeError" class="alert alert-warning">
                        Nur folgende Formate sind erlaubt: {{
                            allowedExtensions.join(', ') }}
                    </div>
                    <div v-if="imageLoaded" class="image-preview-container">
                        <b>Bildvorschau:</b>
                        <Image :src="customImgUrl" :format="ImageFormat.Page" class="image-preview" :square="true" />
                    </div>
                    <div v-if="imageLoaded" class="license-container">
                        <b>Urheberrechtsinformation:</b>
                        <p>
                            Wir benötigen Urheberrechtsinformationen für dieses Bild, damit wir sicherstellen können,
                            dass
                            Inhalte auf memucho frei weiterverwendet werden können. memucho folgt dem Wikipedia Prinzip.
                        </p>

                        <div>
                            <div @click="isPersonalCreation = true" class="license-select">
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="license-select-radio active"
                                    v-if="isPersonalCreation == true" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="license-select-radio" v-else />
                                Dieses Bild ist meine eigene Arbeit.
                            </div>
                            <p v-if="isPersonalCreation == true" class="license-info">
                                Ich, <input v-model="licenseGiverName" type="text" placeholder="Name"
                                    class="creator-name-input" />, der Rechteinhaber
                                dieses Werks gewähre unwiderruflich jedem das Recht, es gemäß der
                                „Creative Commons“-Lizenz „Namensnennung 4.0 International" (CC BY 4.0) <NuxtLink
                                    to="https://creativecommons.org/licenses/by/4.0/deed.de" :external="true">(Text der
                                    Lizenz)</NuxtLink> zu
                                nutzen.
                            </p>

                            <div @click="isPersonalCreation = false" class="license-select">
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="license-select-radio active"
                                    v-if="isPersonalCreation == false" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="license-select-radio" v-else />
                                Dieses Bild ist nicht meine eigene Arbeit.
                            </div>
                            <p v-if="isPersonalCreation == false" class="license-info">
                                Wir bitten dich das Bild auf <NuxtLink to="https://commons.wikimedia.org/wiki/Main_Page"
                                    :external="true">Wikimedia </NuxtLink> hochzuladen und so einzubinden.
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