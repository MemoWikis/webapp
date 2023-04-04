<script lang="ts" setup>
interface Props {
    show: boolean
}
const props = defineProps<Props>()
enum SelectedImageType {
    Wikimedia,
    Custom
}
const selectedImageType = ref<SelectedImageType>(SelectedImageType.Wikimedia)

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
async function loadWikimediaImage() {
    const data = {
        url: wikimediaUrl.value
    }
    const result = await $fetch<WikimediaPreviewResult>('/apiVue/ImageUploadModal/GetWikimediaPreview', {
        mode: 'cors',
        credentials: 'include',
        method: 'POST',
        body: data
    })
    if (result.imageFound) {
        wikiMediaPreviewUrl.value = result.imageThumbUrl
        imageLoaded.value = true
    } else {
        imageLoaded.value = false
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

function upload() { }
</script>

<template>
    <Modal :show="props.show" :show-cancel-btn="true" @close="emit('close')" @primary-btn="upload"
        :primary-btn-label="primaryLabel" :disabled="!imageLoaded">
        <template v-slot:header>
            Themenbild hochladen
        </template>
        <template v-slot:body>
            <div class="alert alert-info">
                <b>Achtung:</b> Bildrechte sind ein sensibles Thema. Bitte lade nur Bilder hoch, die gemeinfrei sind, die
                unter einer entsprechenden Lizenz stehen oder die du selbst erstellt hast.
            </div>
            <div class="imagetype-select-container">
                <div @click="selectedImageType = SelectedImageType.Wikimedia" class="imagetype-select">
                    <font-awesome-icon icon="fa-solid fa-circle-dot" class="imagetype-select-radio active"
                        v-if="selectedImageType == SelectedImageType.Wikimedia" />
                    <font-awesome-icon icon="fa-regular fa-circle" class="imagetype-select-radio" v-else />
                    Bilder von Wikimedia verwenden.
                </div>

                <div @click="selectedImageType = SelectedImageType.Custom" class="imagetype-select">
                    <font-awesome-icon icon="fa-solid fa-circle-dot" class="imagetype-select-radio active"
                        v-if="selectedImageType == SelectedImageType.Custom" />
                    <font-awesome-icon icon="fa-regular fa-circle" class="imagetype-select-radio" v-else />
                    Eigene Bilder
                </div>
            </div>
            <Transition fade>
                <div v-if="selectedImageType == SelectedImageType.Wikimedia" class="content">
                    <p>
                        Bei Wikipedia/ Wikimedia sind viele Millionen Bilder zu finden, die frei genutzt werden können. Auf
                        <NuxtLink to="https://commons.wikimedia.org/wiki/Hauptseite?uselang=de" :external="true">
                            Wikimedia-Commons</NuxtLink> kannst du gezielt nach Inhalten suchen.
                    </p>

                    <p>
                        Tipp: Wenn du bei <NuxtLink to="https://de.wikipedia.org/" :external="true">Wikipedia</NuxtLink>
                        oder
                        <NuxtLink to="https://commons.wikimedia.org/wiki/Hauptseite?uselang=de" :external="true">
                            Wikimedia-Commons</NuxtLink> auf das gewünschte Bild klickst, kommst du zur
                        Detailansicht. (Bei manchen Karten musst du auf das "i"-Logo in der rechten unteren Ecke klicken.)
                        Kopiere einfach die Url dieser Seite.
                    </p>

                    <div class="form-group">
                        <input class="form-control wikimedia-url-input" v-model.lazy="wikimediaUrl" placeholder="http://" />
                        <small class="form-text text-muted"></small>
                    </div>

                    <div v-if="imageLoaded">
                        <img :src="wikiMediaPreviewUrl" />
                    </div>
                </div>
                <div v-else-if="selectedImageType == SelectedImageType.Custom">

                </div>
            </Transition>
            <div></div>
        </template>
    </Modal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.imagetype-select-container {
    display: flex;
    align-items: center;

    .imagetype-select {
        display: flex;
        flex-wrap: nowrap;
        align-items: center;
        cursor: pointer;
        margin-right: 20px;

        .imagetype-select-radio {
            margin-right: 8px;

            &.active {
                color: @memo-blue-link;
            }
        }
    }
}

.content {
    margin-top: 20px;
}

.wikimedia-url-input {
    border-radius: 0px
}
</style>