<script lang="ts" setup>
import { useUserStore } from '../userStore'
import { ImageFormat } from '~~/components/image/imageFormatEnum'
import * as Subscription from '~~/components/user/membership/subscription'
import { Content } from './contentEnum'
import { AlertType, useAlertStore } from '~/components/alert/alertStore'

const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const alertStore = useAlertStore()
const { t } = useI18n()

interface Props {
    imageUrl?: string
    content?: Content
}

const props = defineProps<Props>()

const userStore = useUserStore()

const activeContent = ref<Content>(Content.EditProfile)

const userName = ref<string>(userStore.name)
const email = ref<string>(userStore.email)

const currentPassword = ref<string>('')
const newPassword = ref<string>('')
const repeatedPassword = ref<string>('')

const showWishKnowledge = ref(false)
const allowSupportLogin = ref(false)
const postingDate = ref<Date>(new Date())

function calculatePostingDate() {
    if (userStore.isSubscriptionCanceled)
        return

    if (userStore.subscriptionStartDate !== null) {
        const postingDateInner = new Date()
        if (userStore.subscriptionStartDate.getDay() < new Date().getDay()) {
            postingDateInner.setMonth(postingDateInner.getMonth() + 1)
            postingDateInner.setDate(userStore.subscriptionStartDate.getDay())
            postingDate.value = postingDateInner
            return
        } else {
            postingDateInner.setDate(userStore.subscriptionStartDate.getDay())
            postingDate.value = postingDateInner
            return
        }
    }
}
async function removeImage() {
    const fallbackImageUrl = await $api<string>('/apiVue/VueUserSettings/DeleteUserImage', {
        mode: 'cors',
        method: 'GET',
    })
    currentImageUrl.value = ""
    emit('updateProfile')
    userStore.imgUrl = fallbackImageUrl
}
function onFileChange(e: any) {
    var files = e.target.files || e.dataTransfer.files
    if (!files.length)
        return
    createImage(files[0])
}
const imgFile = ref<File>()
const currentImageUrl = ref('')
if (props.imageUrl) {
    currentImageUrl.value = props.imageUrl
}
onBeforeMount(() => {
    if (props.content === Content.Membership) {
        activeContent.value = Content.Membership
    }
    calculatePostingDate()
})

function createImage(file: File) {
    imgFile.value = file
    const previewImgUrl = URL.createObjectURL(file)
    currentImageUrl.value = previewImgUrl
}
const { $logger } = useNuxtApp()

async function cancelPlan() {
    const { data } = await useFetch<string>('/apiVue/StripeAdminstration/CancelPlan', {
        method: 'GET',
        credentials: 'include',
        mode: 'no-cors',
        onRequest({ options }) {
            if (import.meta.server) {
                options.headers = new Headers(headers)
                options.baseURL = config.public.serverBase
            }
        },
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])

        },
    })
    if (data.value) {
        // Führen Sie die Umleitung im Browser durch.
        await navigateTo(data.value, { external: true })
    } else {
        alertStore.openAlert(AlertType.Error, { text: t('settings.error.redirectFailed') })
    }
}

const showAlert = ref(false)
const msg = ref('')
const success = ref(false)

function resetAlert() {
    showAlert.value = false
    msg.value = ''
    success.value = false
}

watch(activeContent, () => resetAlert())

interface ChangeProfileInformationResult {
    name: string
    email: string
    imgUrl: string
    tinyImgUrl: string
}

async function saveProfileInformation() {
    let formData = new FormData()

    if (imgFile.value != null)
        formData.append('file', imgFile.value)

    if (email.value.trim() != '' && email.value != userStore.email)
        formData.append('email', email.value)

    if (userName.value.trim() != '' && userName.value != userStore.name)
        formData.append('username', userName.value)

    formData.append('id', userStore.id.toString())

    const result = await $api<FetchResult<ChangeProfileInformationResult>>('/apiVue/VueUserSettings/ChangeProfileInformation', {
        mode: 'cors',
        method: 'POST',
        body: formData,
        credentials: 'include'
    })

    if (result?.success) {
        userStore.name, userName.value = result.data.name
        userStore.email, email.value = result.data.email
        userStore.imgUrl = result.data.tinyImgUrl
        emit('updateProfile')

        msg.value = t(result.messageKey)
        success.value = true
        showAlert.value = true
    } else {
        msg.value = t(result.messageKey)
        success.value = false
        showAlert.value = true
    }
}

const emit = defineEmits(['updateProfile'])

interface DefaultResult {
    success: boolean
    message: string
}

async function saveNewPassword() {

    if (currentPassword.value.length <= 0 || newPassword.value.length <= 0 || repeatedPassword.value.length <= 0) {
        msg.value = t('error.user.inputError')
        success.value = false
        showAlert.value = true
        return
    }

    if (newPassword.value != repeatedPassword.value) {
        msg.value = t('error.user.passwordNotCorrectlyRepeated')
        success.value = false
        showAlert.value = true
        return
    }

    const result = await $api<DefaultResult>('/apiVue/VueUserSettings/ChangePassword', {
        mode: 'cors',
        method: 'POST',
        body: {
            currentPassword: currentPassword.value,
            newPassword: newPassword.value
        },
        credentials: 'include'
    })

    if (result.success) {
        msg.value = t(result.message)
        success.value = true
        showAlert.value = true
    } else {
        msg.value = t(result.message)
        success.value = false
        showAlert.value = true
    }
}

async function resetPassword() {

    const result = await $api<boolean>('/apiVue/VueUserSettings/ResetPassword', {
        mode: 'cors',
        method: 'POST',
        credentials: 'include'
    })

    if (result) {
        msg.value = t('success.user.passwordReset')
        success.value = true
        showAlert.value = true
    }
}

const canDeleteProfile = ref(false)

const checkIfProfileCanBeDeleted = async () => {
    const result = await $api<boolean>('/apiVue/VueUserSettings/CanDeleteUser', {
        mode: 'cors',
        method: 'GET',
        credentials: 'include'
    })

    canDeleteProfile.value = result
}

watch(activeContent, (content) => {
    if (content === Content.DeleteProfile) {
        checkIfProfileCanBeDeleted()
    }
})

async function deleteProfile() {

    const result = await $api<boolean>('/apiVue/VueUserSettings/DeleteProfile', {
        mode: 'cors',
        method: 'POST',
        credentials: 'include'
    })

    if (result) {
        userStore.reset()
        alertStore.openAlert(AlertType.Success, { text: t('success.user.deleted') })

        alertStore.$onAction(({ name, after }) => {
            if (name === 'closeAlert') {
                after(() => {
                    userStore.deleteUser()
                })
            }
        })
    } else {
        msg.value = t('error.default')
        success.value = false
        showAlert.value = true
    }
}

async function saveWishKnowledgeVisibility() {

    const result = await $api<DefaultResult>('/apiVue/VueUserSettings/ChangeWishKnowledgeVisibility', {
        mode: 'cors',
        method: 'POST',
        body: {
            showWishKnowledge: showWishKnowledge.value
        },
        credentials: 'include'
    })

    if (result.success) {
        msg.value = t(result.message)
        success.value = true
        showAlert.value = true
    } else {
        msg.value = t(result.message)
        success.value = false
        showAlert.value = true
    }
}

async function saveSupportLoginRights() {

    const result = await $api<DefaultResult>('/apiVue/VueUserSettings/ChangeSupportLoginRights', {
        mode: 'cors',
        method: 'POST',
        body: {
            allowSupportiveLogin: allowSupportLogin.value
        },
        credentials: 'include'
    })

    if (result.success) {
        msg.value = t(result.message)
        success.value = true
        showAlert.value = true
    } else {
        msg.value = t(result.message)
        success.value = false
        showAlert.value = true
    }
}

// NotificationInterval is UserSettingNotificationInterval in backend
enum NotifcationInterval {
    NotSet = 0,
    Never = 1,
    Daily = 2,
    Weekly = 3,
    Monthly = 4,
    Quarterly = 5
}
const selectedNotificationInterval = ref<NotifcationInterval>(NotifcationInterval.Weekly)

const getNotificationIntervalText = computed(() => {
    switch (selectedNotificationInterval.value) {
        case NotifcationInterval.Never:
            return t('settings.knowledgeReport.interval.never')
        case NotifcationInterval.Daily:
            return t('settings.knowledgeReport.interval.daily')
        case NotifcationInterval.Weekly:
            return t('settings.knowledgeReport.interval.weekly')
        case NotifcationInterval.Monthly:
            return t('settings.knowledgeReport.interval.monthly')
        case NotifcationInterval.Quarterly:
            return t('settings.knowledgeReport.interval.quarterly')
        default:
            return t('settings.knowledgeReport.interval.notSelected')
    }
})

const notificationIntervalChangeMsg = ref('')
async function saveNotificationIntervalPreferences() {

    const result = await $api<DefaultResult>('/apiVue/VueUserSettings/ChangeNotificationIntervalPreferences', {
        mode: 'cors',
        method: 'POST',
        body: {
            notificationInterval: selectedNotificationInterval.value
        },
        credentials: 'include'
    })

    if (result.success) {
        notificationIntervalChangeMsg.value = result.message
        success.value = true
        showAlert.value = true
    } else {
        notificationIntervalChangeMsg.value = t('error.default')
        success.value = false
        showAlert.value = true
    }
}

const getSelectedSettingsPageLabel = computed(() => {
    switch (activeContent.value) {
        case Content.EditProfile:
            return t('settings.navigation.editProfile')
        case Content.Password:
            return t('settings.navigation.password')
        case Content.DeleteProfile:
            return t('settings.navigation.deleteProfile')
        case Content.ShowWishKnowledge:
            return t('settings.navigation.showWishKnowledge')
        case Content.SupportLogin:
            return t('settings.navigation.supportLogin')
        case Content.Membership:
            return t('settings.navigation.membership')
        case Content.General:
            return t('settings.navigation.general')
        case Content.KnowledgeReport:
            return t('settings.navigation.knowledgeReport')
    }
})

async function requestVerificationMail() {
    const result = await userStore.requestVerificationMail()
    msg.value = t(result.messageKey)
    success.value = true
    showAlert.value = true
}
const ariaId = useId()
const ariaId2 = useId()


</script>

<template>
    <div class="user-settings-container">
        <div class="navigation">
            <div class="overline-s no-line">{{ t('settings.navigation.profileInfo') }}</div>
            <button @click="activeContent = Content.EditProfile"
                :class="{ 'active': activeContent === Content.EditProfile }">{{ t('settings.navigation.editProfile') }}</button>
            <button @click="activeContent = Content.Password"
                :class="{ 'active': activeContent === Content.Password }">{{ t('settings.navigation.password') }}</button>
            <button @click="activeContent = Content.DeleteProfile"
                :class="{ 'active': activeContent === Content.DeleteProfile }">{{ t('settings.navigation.deleteProfile') }}</button>

            <div class="divider"></div>
            <div class="overline-s no-line">{{ t('settings.navigation.settings') }}</div>
            <button @click="activeContent = Content.ShowWishKnowledge"
                :class="{ 'active': activeContent === Content.ShowWishKnowledge }">{{ t('settings.navigation.showWishKnowledge') }}</button>
            <button @click="activeContent = Content.SupportLogin"
                :class="{ 'active': activeContent === Content.SupportLogin }">{{ t('settings.navigation.supportLogin') }}</button>
            <button @click="activeContent = Content.Membership"
                :class="{ 'active': activeContent === Content.Membership }">{{ t('settings.navigation.membership') }}</button>

            <div class="divider"></div>
            <div class="overline-s no-line">{{ t('settings.navigation.notifications') }}</div>
            <!-- <button @click="activeContent = Content.General">{{ t('settings.navigation.general') }}</button> -->
            <button @click="activeContent = Content.KnowledgeReport"
                :class="{ 'active': activeContent === Content.KnowledgeReport }">{{ t('settings.navigation.knowledgeReport') }}</button>
        </div>
        <div class="navigation-mobile">
            <div class="settings-dropdown">
                <VDropdown :aria-id="ariaId" :distance="0">
                    <div class="settings-select">
                        <div>
                            {{ getSelectedSettingsPageLabel }}
                        </div>
                        <font-awesome-icon :icon="['fas', 'bars']" />
                    </div>

                    <template #popper="{ hide }">
                        <div class="mobile-dropdown">
                            <div class="dropdown-row group-label">
                                {{ t('settings.navigation.profileInfo') }}
                            </div>
                            <div class="dropdown-row select-row" @click="activeContent = Content.EditProfile; hide()"
                                :class="{ 'active': activeContent === Content.EditProfile }">
                                <div class="dropdown-label select-option">
                                    {{ t('settings.navigation.editProfile') }}
                                </div>
                            </div>
                            <div class="dropdown-row select-row" @click="activeContent = Content.Password; hide()"
                                :class="{ 'active': activeContent === Content.Password }">
                                <div class="dropdown-label select-option">
                                    {{ t('settings.navigation.password') }}
                                </div>
                            </div>
                            <div class="dropdown-row select-row" @click="activeContent = Content.DeleteProfile; hide()"
                                :class="{ 'active': activeContent === Content.DeleteProfile }">
                                <div class="dropdown-label select-option">
                                    {{ t('settings.navigation.deleteProfile') }}
                                </div>
                            </div>
                            <div class="divider"></div>
                            <div class="dropdown-row group-label">
                                {{ t('settings.navigation.settings') }}
                            </div>
                            <div class="dropdown-row select-row" @click="activeContent = Content.ShowWishKnowledge; hide()"
                                :class="{ 'active': activeContent === Content.ShowWishKnowledge }">
                                <div class="dropdown-label select-option">
                                    {{ t('settings.navigation.showWishKnowledge') }}
                                </div>
                            </div>
                            <div class="dropdown-row select-row" @click="activeContent = Content.SupportLogin; hide()"
                                :class="{ 'active': activeContent === Content.SupportLogin }">
                                <div class="dropdown-label select-option">
                                    {{ t('settings.navigation.supportLogin') }}
                                </div>
                            </div>
                            <div class="dropdown-row select-row" @click="activeContent = Content.Membership; hide()"
                                :class="{ 'active': activeContent === Content.Membership }">
                                <div class="dropdown-label select-option">
                                    {{ t('settings.navigation.membership') }}
                                </div>
                            </div>
                            <div class="divider"></div>
                            <div class="dropdown-row group-label">
                                {{ t('settings.navigation.notifications') }}
                            </div>
                            <div class="dropdown-row select-row"
                                @click="activeContent = Content.KnowledgeReport; hide()"
                                :class="{ 'active': activeContent === Content.KnowledgeReport }">
                                <div class="dropdown-label select-option">
                                    {{ t('settings.navigation.knowledgeReport') }}
                                </div>
                            </div>
                        </div>
                    </template>
                </VDropdown>
            </div>
        </div>
        <div class="settings-content">
            <Transition>
                <div v-if="activeContent === Content.EditProfile" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success">{{ msg }}</div>
                        <div class="alert alert-danger" v-else>{{ msg }}</div>
                    </div>
                    <div class="settings-section">
                        <div class="overline-s no-line">{{ t('settings.profile.profilePicture') }}</div>
                        <Image :src="currentImageUrl" :format="ImageFormat.Author" class="profile-picture" :custom-style="'object-fit: cover;'" />
                        <div class="img-settings-btns">
                            <div>
                                <label class="img-upload-btn" for="imageUpload">
                                    <input type="file" accept="image/*" name="file" id="imageUpload" v-on:change="onFileChange" />
                                    <font-awesome-icon icon="fa-solid fa-upload" />
                                    {{ t('settings.profile.uploadImage') }}
                                </label>
                                <span>{{ imgFile?.name }}</span>
                            </div>
                            <div>
                                <button class="img-delete-btn" @click="removeImage()">
                                    <font-awesome-icon icon="fa-solid fa-trash" /> {{ t('settings.profile.removeProfilePicture') }}
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
                                        <input name="username" placeholder="" type="text" width="0" v-model="userName" class="settings-input" id="username">
                                    </div>
                                </div>
                            </form>
                        </div>

                        <div class="input-container">
                            <div class="overline-s no-line">{{ t('settings.profile.email') }}</div>
                            <div class="col-xs-12"></div>
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-12 col-lg-6">
                                        <input name="email" placeholder="" type="email" width="0" v-model="email" class="settings-input" id="email">
                                    </div>
                                    <div class="col-lg-12"></div>
                                    <div class="col-sm-12 col-lg-6 ">
                                        <div class="email-confirmation-container">
                                            <div v-if="userStore.isEmailConfirmed" class="email-verification-label verified overline-s no-line">
                                                <font-awesome-icon :icon="['fas', 'check']" /> {{ t('settings.profile.verified') }}
                                            </div>
                                            <template v-else>
                                                <div class="email-verification-label not-verified overline-s no-line">
                                                    <font-awesome-icon :icon="['fas', 'xmark']" /> {{ t('settings.profile.notVerified') }}
                                                </div>
                                                <button class="btn-link generic-btn-link"
                                                    @click.prevent="requestVerificationMail()">
                                                    <font-awesome-icon :icon="['fas', 'envelope-circle-check']" />{{ t('settings.profile.sendVerificationEmail') }}
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

                <div v-else-if="activeContent === Content.Password" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success">{{ msg }}</div>
                        <div class="alert alert-danger" v-else>{{ msg }}</div>
                    </div>
                    <div class="settings-section">
                        <div class="input-container">
                            <div class="overline-s no-line">{{ t('settings.password.currentPassword') }}</div>
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-12 col-lg-6">
                                        <input placeholder="" type="password" width="0" v-model="currentPassword" class="settings-input">
                                    </div>
                                </div>
                            </form>
                        </div>

                        <div class="input-container">
                            <div class="overline-s no-line">{{ t('settings.password.newPassword') }}</div>
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-12 col-lg-6">
                                        <input placeholder="" type="password" width="0" v-model="newPassword" class="settings-input">
                                    </div>
                                </div>
                            </form>
                        </div>

                        <div class="input-container">
                            <div class="overline-s no-line">{{ t('settings.password.repeatNewPassword') }}</div>
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-12 col-lg-6">
                                        <input placeholder="" type="password" width="0" v-model="repeatedPassword" class="settings-input">
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>

                    <div class="settings-section password-section">
                        <button class="memo-button btn btn-primary" @click="saveNewPassword()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            {{ t('settings.password.changePassword') }}
                        </button>

                        <button class="memo-button btn btn-link" @click="resetPassword()">
                            {{ t('settings.password.forgotPassword') }}
                        </button>
                    </div>
                </div>

                <div v-else-if="activeContent === Content.DeleteProfile" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success">{{ msg }}</div>
                        <div class="alert alert-danger" v-else>{{ msg }}</div>
                    </div>
                    <div class="settings-section">
                        <div class="">
                            <div class="alert alert-info">
                                <p>
                                    <b>{{ t('settings.deleteProfile.warning') }}</b> {{ t('settings.deleteProfile.onlyIf') }}
                                </p>
                                <ul>
                                    <li>{{ t('settings.deleteProfile.condition1') }}</li>
                                    <li>{{ t('settings.deleteProfile.condition2') }}</li>
                                </ul>
                            </div>

                            <button @click.prevent="deleteProfile()" class="memo-button btn btn-danger" v-if="canDeleteProfile">
                                {{ t('settings.deleteProfile.deleteButton') }}
                            </button>
                            <div v-else class="alert alert-warning">
                                <p>
                                    {{ t('settings.deleteProfile.notPossible') }}
                                    <NuxtLink :to="`mailto:${config.public.teamEmail}`" :external="true">{{ config.public.teamEmail }}</NuxtLink>,
                                    {{ t('settings.deleteProfile.contactReason') }}
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

                <div v-else-if="activeContent === Content.ShowWishKnowledge" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success">{{ msg }}</div>
                        <div class="alert alert-danger" v-else>{{ msg }}</div>
                    </div>
                    <div class="settings-section">
                        <label class="checkbox-section">
                            <div class="checkbox-container">
                                <input type="checkbox" name="answer" :value="true" v-model="showWishKnowledge" class="hidden" />
                                <font-awesome-icon icon="fa-solid fa-square-check" v-if="showWishKnowledge" class="checkbox-icon" />
                                <font-awesome-icon icon="fa-regular fa-square" v-else class="checkbox-icon" />
                            </div>
                            <div class="checkbox-label">
                                <div class="overline-s no-line">
                                    {{ t('settings.wuwi.showWishKnowledge') }}
                                </div>
                                <p>
                                    {{ t('settings.wuwi.explanation') }}
                                </p>
                            </div>
                        </label>
                    </div>

                    <div class="settings-section">
                        <button class="memo-button btn btn-primary" @click="saveWishKnowledgeVisibility()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            {{ t('settings.button.save') }}
                        </button>
                    </div>
                </div>

                <div v-else-if="activeContent === Content.SupportLogin" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success">{{ msg }}</div>
                        <div class="alert alert-danger" v-else>{{ msg }}</div>
                    </div>
                    <div class="settings-section">
                        <label class="checkbox-section">
                            <div class="checkbox-container">
                                <input type="checkbox" name="answer" :value="true" v-model="allowSupportLogin" class="hidden" />
                                <font-awesome-icon icon="fa-solid fa-square-check" v-if="allowSupportLogin" class="checkbox-icon" />
                                <font-awesome-icon icon="fa-regular fa-square" v-else class="checkbox-icon" />
                            </div>
                            <div class="checkbox-label">
                                <div class="overline-s no-line">
                                    {{ t('settings.supportLogin.allow') }}
                                </div>
                                <p>
                                    {{ t('settings.supportLogin.explanation') }}
                                </p>
                            </div>
                        </label>
                    </div>
                    <div class="settings-section">
                        <button class="memo-button btn btn-primary" @click="saveSupportLoginRights()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            {{ t('settings.button.save') }}
                        </button>
                    </div>
                </div>

                <div v-else-if="activeContent === Content.Membership" class="content">
                    <div class="settings-section" v-if="userStore.subscriptionType != Subscription.Type.Basic">
                        <button class="memo-button btn btn-primary" v-if="userStore.isSubscriptionCanceled === false" @click="cancelPlan()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            {{ t('settings.membership.manageOrCancel') }}
                        </button>
                        <button class="memo-button btn btn-primary" v-else-if="userStore.isSubscriptionCanceled === true && userStore.subscriptionType === Subscription.Type.Plus" @click="cancelPlan()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            {{ t('settings.membership.resume') }}
                        </button>
                    </div>

                    <div class="settings-section plans">
                        <UserMembershipPlans />
                    </div>
                </div>

                <div v-else-if="activeContent === Content.General" class="content"></div>

                <div v-else-if="activeContent === Content.KnowledgeReport" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success" v-html="notificationIntervalChangeMsg"></div>
                        <div class="alert alert-danger" v-else v-html="notificationIntervalChangeMsg"></div>
                    </div>
                    <div class="settings-section">
                        <div class="overline-s no-line">
                            {{ t('settings.knowledgeReport.emailReport') }}
                        </div>
                        <div class="interval-dropdown">
                            <VDropdown :aria-id="ariaId2" :distance="0">
                                <div class="interval-select">
                                    <div>
                                        {{ getNotificationIntervalText }}
                                    </div>
                                    <font-awesome-icon icon="fa-solid fa-chevron-down" class="chevron" />
                                </div>

                                <template #popper="{ hide }">
                                    <div class="dropdown-row select-row"
                                        @click="selectedNotificationInterval = NotifcationInterval.Quarterly; hide()"
                                        :class="{ 'active': selectedNotificationInterval === NotifcationInterval.Quarterly }">
                                        <div class="dropdown-label select-option">
                                            {{ t('settings.knowledgeReport.interval.quarterly') }}
                                        </div>
                                    </div>
                                    <div class="dropdown-row"
                                        @click="selectedNotificationInterval = NotifcationInterval.Monthly; hide()"
                                        :class="{ 'active': selectedNotificationInterval === NotifcationInterval.Monthly }">
                                        <div class="dropdown-label select-option">
                                            {{ t('settings.knowledgeReport.interval.monthly') }}
                                        </div>
                                    </div>
                                    <div class="dropdown-row select-row"
                                        @click="selectedNotificationInterval = NotifcationInterval.Weekly; hide()"
                                        :class="{ 'active': selectedNotificationInterval === NotifcationInterval.Weekly }">
                                        <div class="dropdown-label select-option">
                                            {{ t('settings.knowledgeReport.interval.weekly') }}
                                        </div>
                                    </div>
                                    <div class="dropdown-row select-row"
                                        @click="selectedNotificationInterval = NotifcationInterval.Daily; hide()"
                                        :class="{ 'active': selectedNotificationInterval === NotifcationInterval.Daily }">
                                        <div class="dropdown-label select-option">
                                            {{ t('settings.knowledgeReport.interval.daily') }}
                                        </div>
                                    </div>
                                    <div class="dropdown-row select-row"
                                        @click="selectedNotificationInterval = NotifcationInterval.Never; hide()"
                                        :class="{ 'active': selectedNotificationInterval === NotifcationInterval.Never }">
                                        <div class="dropdown-label select-option">
                                            {{ t('settings.knowledgeReport.interval.never') }}
                                        </div>
                                    </div>
                                </template>
                            </VDropdown>
                        </div>

                        <p>
                            {{ t('settings.knowledgeReport.description') }}
                            <font-awesome-icon icon="fa-solid fa-heart" class="wuwi-icon" />
                            {{ t('settings.knowledgeReport.additionalInfo') }}
                        </p>
                    </div>

                    <div class="settings-section">
                        <button class="memo-button btn btn-primary" @click="saveNotificationIntervalPreferences()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            {{ t('settings.button.save') }}
                        </button>
                    </div>
                </div>
            </Transition>
        </div>
    </div>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.group-label {
    background-color: @memo-blue;
    color: white;
}
</style>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.user-settings-container {
    display: flex;
    flex-direction: row;

    gap: 1rem;

    .email-confirmation-container {
        display: flex;
        justify-content: space-between;
        align-items: center;

        .email-verification-label {

            &.verified {
                svg {
                    color: @memo-green;

                }
            }

            &.not-verified {
                svg {
                    color: @memo-wuwi-red;
                }
            }
        }

        svg {
            margin-right: 2px;
        }
    }

    .mobile-dropdown {
        width: calc(100vw - 20px);

    }

    .group-label {
        font-weight: 600;
    }

    p {
        margin: 10px 0;
        margin-top: 5px;
    }

    .settings-dropdown {
        padding-top: 30px;
        width: 100%;

        .settings-select {
            width: 100%;
            color: @memo-blue-link;
            font-weight: 600;
        }
    }

    .interval-dropdown {
        width: 190px;
    }


    .v-popper--shown {

        .settings-select,
        .interval-select {

            .chevron {
                transform: rotate(180deg)
            }
        }
    }

    .settings-select,
    .interval-select {
        padding: 6px 12px;
        height: 34px;
        cursor: pointer;
        border: solid 1px @memo-grey-light;
        background: white;
        display: flex;
        justify-content: space-between;
        align-items: center;
        user-select: none;

        &:hover {
            color: @memo-blue;
            filter: brightness(0.95)
        }

        &:active {
            filter: brightness(0.85)
        }
    }

    .interval-select {
        width: 190px;
    }

    .checkbox-section {
        margin-top: -5px;
        cursor: pointer;
        display: flex;
        flex-wrap: nowrap;

        .checkbox-label {
            .overline-s {
                margin-top: 0;
                padding-top: 10px;
                margin-bottom: 10px;
            }
        }
    }

    .settings-input {
        resize: none;
        height: 44px;
        overflow: hidden;
        width: 100%;
        padding: 0 15px 0;
        border: solid @memo-grey-light 1px;
        box-shadow: none;
        color: @memo-grey-dark !important;
        outline: none;

        &:focus {
            border: solid 1px @memo-green;
            box-shadow: none;
        }

        &:active {
            border: solid 1px @memo-green;
            box-shadow: none;
        }
    }

    .settings-section {
        margin-bottom: 40px;

        &.plans {
            margin-left: -10px;
            margin-right: -10px;
            margin-bottom: 0;
        }
    }

    .password-section {
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
    }

    .profile-picture {
        width: 166px;
        min-width: 166px;
        height: 166px;
        margin: 10px 0;
    }

    .img-settings-btns {
        display: flex;
        flex-direction: column;
        flex-wrap: nowrap;

        .img-upload-btn,
        .img-delete-btn {
            input[type="file"] {
                display: none;
            }

            border-radius: 24px;

            color: @memo-blue-link;
            cursor: pointer;
            background: white;
            padding: 6px 12px;
            border: none;
            text-align: left;

            &:hover {
                color: @memo-blue;
                // filter: brightness(0.95)
            }

            &:active {
                // filter: brightness(0.85)
            }
        }
    }

    .generic-btn-link {
        padding: 0px;
    }

    .divider {
        margin-top: 20px;
        height: 1px;
        background: @memo-grey-lighter;
        width: 100%;
        margin-bottom: 10px;
    }



    .navigation {
        width: 25%;
        margin-left: -20px;
        display: flex;
        flex-direction: column;
        flex-wrap: nowrap;

        button {
            background: white;
            text-align: left;
            color: @memo-grey-dark;
            padding: 12px 20px;
            border-radius: 24px;
            outline: none;

            &.active {
                color: @memo-blue-link;
                font-weight: 600;
            }

            &:hover {
                color: @memo-blue;
                filter: brightness(0.95)
            }

            &:active {
                filter: brightness(0.85)
            }
        }

        .overline-s,
        button {
            padding-left: 20px;
            padding-right: 20px;
        }

    }

    .overline-s {
        margin: 5px 0;
    }


    .navigation,
    .settings-content {
        padding-top: 50px;
    }

    .wuwi-icon {
        color: @memo-wuwi-red;
    }

    .settings-content {
        max-width: 1200px;
        width: calc(75% - 1rem);
        flex-grow: 2;
    }
}

.user-settings-container {
    @media (max-width: 900px) {

        flex-direction: column;

        .navigation {
            display: none;
        }

        .settings-content {
            width: 100%;
        }
    }

    @media (min-width: 901px) {
        .navigation {
            width: 25%;
        }

        .navigation-mobile {
            display: none;
        }

        .settings-content {
            width: calc(75% - 1rem);
        }
    }
}
</style>

<style lang="less">
.sidesheet-open {
    .user-settings-container {
        @media (max-width: 1209px) {

            flex-direction: column;

            .navigation {
                display: none;
            }

            .settings-content {
                width: 100%;
            }
        }

        @media (min-width: 1210px) {
            .navigation {
                width: 25%;
            }

            .navigation-mobile {
                display: none;
            }

            .settings-content {
                width: calc(75% - 1rem);
            }
        }
    }
}
</style>