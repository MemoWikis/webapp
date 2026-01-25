<script lang="ts" setup>
import { useUserStore } from '../userStore'
import { ImageFormat } from '~~/components/image/imageFormatEnum'

interface Props {
    imageUrl?: string
}

const props = defineProps<Props>()

const { t } = useI18n()
const userStore = useUserStore()

const userName = ref<string>(userStore.name)
const email = ref<string>(userStore.email)

// Sync values after client-side hydration to ensure store data is available
onMounted(() => {
    userName.value = userStore.name
    email.value = userStore.email
})

const showAlert = ref(false)
const msg = ref('')
const success = ref(false)

const imgFile = ref<File>()
const currentImageUrl = ref('')

if (props.imageUrl) {
    currentImageUrl.value = props.imageUrl
}

const emit = defineEmits<{
    updateProfile: []
    resetAlert: []
}>()

watch(() => showAlert.value, (value) => {
    if (value) {
        emit('resetAlert')
    }
})

async function removeImage() {
    const fallbackImageUrl = await $api<string>('/apiVue/VueUserSettings/DeleteUserImage', {
        mode: 'cors',
        method: 'GET',
    })
    currentImageUrl.value = ""
    emit('updateProfile')
    userStore.imgUrl = fallbackImageUrl
}

function onFileChange(e: Event) {
    const target = e.target as HTMLInputElement | DataTransfer
    const files = (target as HTMLInputElement).files || (e as DragEvent).dataTransfer?.files
    if (!files?.length) {
        return
    }
    createImage(files[0])
}

function createImage(file: File) {
    imgFile.value = file
    const previewImgUrl = URL.createObjectURL(file)
    currentImageUrl.value = previewImgUrl
}

interface ChangeProfileInformationResult {
    name: string
    email: string
    imgUrl: string
    tinyImgUrl: string
}

async function saveProfileInformation() {
    const formData = new FormData()

    if (imgFile.value != null) {
        formData.append('file', imgFile.value)
    }

    if (email.value.trim() != '' && email.value != userStore.email) {
        formData.append('email', email.value)
    }

    if (userName.value.trim() != '' && userName.value != userStore.name) {
        formData.append('username', userName.value)
    }

    formData.append('id', userStore.id.toString())

    try {
        const result = await $api<FetchResult<ChangeProfileInformationResult>>('/apiVue/VueUserSettings/ChangeProfileInformation', {
            mode: 'cors',
            method: 'POST',
            body: formData,
            credentials: 'include'
        })

        if (result?.success) {
            userStore.name = result.data.name
            userName.value = result.data.name
            userStore.email = result.data.email
            email.value = result.data.email
            userStore.imgUrl = result.data.tinyImgUrl
            emit('updateProfile')

            msg.value = t(result.messageKey)
            success.value = true
            showAlert.value = true
        } else {
            msg.value = result?.messageKey ? t(result.messageKey) : t('error.default')
            success.value = false
            showAlert.value = true
        }
    } catch (error) {
        console.error('Error saving profile information:', error)
        msg.value = t('error.default')
        success.value = false
        showAlert.value = true
    }
}

async function requestVerificationMail() {
    const result = await userStore.requestVerificationMail()
    msg.value = t(result.messageKey)
    success.value = true
    showAlert.value = true
}

function resetAlert() {
    showAlert.value = false
    msg.value = ''
    success.value = false
}

defineExpose({
    resetAlert
})
</script>

<template>
    <div class="content">
        <div v-if="showAlert" class="settings-section">
            <div v-if="success" class="alert alert-success">{{ msg }}</div>
            <div v-else class="alert alert-danger">{{ msg }}</div>
        </div>
        <div class="settings-section">
            <div class="overline-s no-line">{{ t('settings.profile.profilePicture') }}</div>
            <Image :src="currentImageUrl" :format="ImageFormat.Author" class="profile-picture"
                :custom-style="'object-fit: cover;'" />
            <div class="img-settings-btns">
                <div>
                    <label class="img-upload-btn" for="imageUpload">
                        <input id="imageUpload" type="file" accept="image/*" name="file" @change="onFileChange" />
                        <font-awesome-icon icon="fa-solid fa-upload" />
                        {{ t('settings.profile.uploadImage') }}
                    </label>
                    <span>{{ imgFile?.name }}</span>
                </div>
                <div>
                    <button class="img-delete-btn" @click="removeImage()">
                        <font-awesome-icon icon="fa-solid fa-trash" /> {{
                            t('settings.profile.removeProfilePicture')
                        }}
                    </button>
                </div>
            </div>
        </div>
        <div class="settings-section">
            <div class="input-container">
                <div class="overline-s no-line">{{ t('settings.profile.username') }}</div>
                <form class="form-horizontal">
                    <div class="form-group">
                        <div class="col-sm-12 col-lg-6">
                            <input id="username" v-model="userName" name="username" placeholder="" type="text" width="0"
                                class="settings-input" />
                        </div>
                    </div>
                </form>
            </div>

            <div class="input-container">
                <div class="overline-s no-line">{{ t('settings.profile.email') }}</div>
                <div class="col-xs-12" />
                <form class="form-horizontal">
                    <div class="form-group">
                        <div class="col-sm-12 col-lg-6">
                            <input id="email" v-model="email" name="email" placeholder="" type="email" width="0"
                                class="settings-input" />
                        </div>
                        <div class="col-lg-12" />
                        <div class="col-sm-12 col-lg-6 ">
                            <div class="email-confirmation-container">
                                <div v-if="userStore.isEmailConfirmed"
                                    class="email-verification-label verified overline-s no-line">
                                    <font-awesome-icon :icon="['fas', 'check']" /> {{
                                        t('settings.profile.verified')
                                    }}
                                </div>
                                <template v-else>
                                    <div class="email-verification-label not-verified overline-s no-line">
                                        <font-awesome-icon :icon="['fas', 'xmark']" /> {{
                                            t('settings.profile.notVerified') }}
                                    </div>
                                    <button class="btn-link generic-btn-link"
                                        @click.prevent="requestVerificationMail()">
                                        <font-awesome-icon :icon="['fas', 'envelope-circle-check']" />{{
                                            t('settings.profile.sendVerificationEmail') }}
                                    </button>
                                </template>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>
        <div class="settings-section">
            <button class="memo-button btn btn-primary" @click="saveProfileInformation()">
                <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                {{ t('settings.button.save') }}
            </button>
        </div>
    </div>
</template>
