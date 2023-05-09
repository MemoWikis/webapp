<script lang="ts" setup>
import { useUserStore } from './userStore'
import { ImageFormat } from '../image/imageFormatEnum'
import { messages } from '../alert/messages'
const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit

interface Props {
    imageUrl?: string
}

const props = defineProps<Props>()

const userStore = useUserStore()
enum Content {
    //Profile Information
    EditProfile,
    Password,
    DeleteProfile,

    //Settings
    ShowWuwi,
    SupportLogin,
    Abonnement,

    //Notifications
    General,
    KnowledgeReport
}
const activeContent = ref<Content>(Content.EditProfile)

const userName = ref<string>(userStore.name)
const email = ref<string>(userStore.email)

const currentPassword = ref<string>('')
const newPassword = ref<string>('')
const repeatedPassword = ref<string>('')

const showWuwi = ref(false)
const allowSupportLogin = ref(false)
const postingDate = ref<Date>(new Date())

function calculatePostingDate() {
    if (userStore.isSubscriptionCanceled)
        return

    if (userStore.subscriptionStartDate !== null) {
        let postingDateInner = new Date();
        if (userStore.subscriptionStartDate.getDay() < new Date().getDay()) {
            postingDateInner.setMonth(postingDateInner.getMonth() + 1);
            postingDateInner.setDate(userStore.subscriptionStartDate.getDay())
            postingDate.value = postingDateInner
            return;
        } else {
            postingDateInner.setDate(userStore.subscriptionStartDate.getDay())
            postingDate.value = postingDateInner
            return
        }
    }
}
async function removeImage(){
    const fallbackImagaUrl = await $fetch<string>('/apiVue/VueUserSettings/DeleteUserImage', {
        mode: 'cors',
        method: 'GET',
    })
    console.log(fallbackImagaUrl)
    imageUrl.value = ""
    emit('updateProfile')
    userStore.imgUrl = fallbackImagaUrl
}
function onFileChange(e: any) {
    var files = e.target.files || e.dataTransfer.files
    if (!files.length)
        return
    createImage(files[0])
}
const imgFile = ref<File>()
const imageUrl = ref('')
onBeforeMount(() => {
    if (props.imageUrl)
        imageUrl.value = props.imageUrl

    calculatePostingDate()
})

function createImage(file: File) {
    imgFile.value = file
    const previewImgUrl = URL.createObjectURL(file)
    imageUrl.value = previewImgUrl
}

async function  cancelPlan() {
    const { data } = await useFetch<string>('/apiVue/StripeAdminstration/CancelPlan', {
        method: 'GET',
        credentials: 'include',
        mode: 'no-cors',
        onRequest({ options }) {
            if (process.server) {
                options.headers = headers
                options.baseURL = config.public.serverBase
            }
        }
    });
    if (data.value) {
        // Führen Sie die Umleitung im Browser durch.
        window.location.href = data.value;
    } else {
        console.log("kein Ergebnis"); 
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
    success: boolean
    message: string
    name?: string
    email?: string
    imgUrl?: string
    tinyImgUrl?: string
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

    const result = await $fetch<ChangeProfileInformationResult>('/apiVue/VueUserSettings/ChangeProfileInformation', {
        mode: 'cors',
        method: 'POST',
        body: formData,
        credentials: 'include'
    })

    if (result?.success) {
        userStore.name, userName.value = result.name!
        userStore.email, email.value = result.name!
        userStore.imgUrl = result.tinyImgUrl!
        emit('updateProfile')

        msg.value = messages.success.user[result.message]
        success.value = true
        showAlert.value = true
    } else {
        msg.value = messages.error.user[result.message]
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
        msg.value = messages.error.user['inputError']
        success.value = false
        showAlert.value = true
        return
    }

    if (newPassword.value != repeatedPassword.value) {
        msg.value = messages.error.user['passwordNotCorrectlyRepeated']
        success.value = false
        showAlert.value = true
        return
    }

    const result = await $fetch<DefaultResult>('/apiVue/VueUserSettings/ChangePassword', {
        mode: 'cors',
        method: 'POST',
        body: {
            currentPassword: currentPassword.value,
            newPassword: newPassword.value
        },
        credentials: 'include'
    })

    if (result.success) {
        msg.value = messages.success.user[result.message]
        success.value = true
        showAlert.value = true
    } else {
        msg.value = messages.error.user[result.message]
        success.value = false
        showAlert.value = true
    }
}

async function resetPassword() {

    const result = await $fetch<boolean>('/apiVue/VueUserSettings/ResetPassword', {
        mode: 'cors',
        method: 'POST',
        credentials: 'include'
    })

    if (result) {
        msg.value = messages.success.user['passwordReset']
        success.value = true
        showAlert.value = true
    }
}

async function saveWuwiVisibility() {

    const result = await $fetch<DefaultResult>('/apiVue/VueUserSettings/ChangeWuwiVisibility', {
        mode: 'cors',
        method: 'POST',
        body: {
            showWuwi: showWuwi.value
        },
        credentials: 'include'
    })

    if (result.success) {
        msg.value = messages.success.user[result.message]
        success.value = true
        showAlert.value = true
    } else {
        msg.value = messages.error.user[result.message]
        success.value = false
        showAlert.value = true
    }
}

async function saveSupportLoginRights() {

    const result = await $fetch<DefaultResult>('/apiVue/VueUserSettings/ChangeSupportLoginRights', {
        mode: 'cors',
        method: 'POST',
        body: {
            allowSupportiveLogin: allowSupportLogin.value
        },
        credentials: 'include'
    })

    if (result.success) {
        msg.value = messages.success.user[result.message]
        success.value = true
        showAlert.value = true
    } else {
        msg.value = messages.error.user[result.message]
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
            return 'Nie'
        case NotifcationInterval.Daily:
            return 'Täglich'
        case NotifcationInterval.Weekly:
            return 'Wöchentlich'
        case NotifcationInterval.Monthly:
            return 'Monatlich'
        case NotifcationInterval.Quarterly:
            return 'Vierteljährlich'
        default:
            return 'Nicht ausgewählt'
    }
})

const notificationIntervalChangeMsg = ref('')
async function saveNotificationIntervalPreferences() {

    const result = await $fetch<DefaultResult>('/apiVue/VueUserSettings/ChangeNotificationIntervalPreferences', {
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
        notificationIntervalChangeMsg.value = messages.error.default
        success.value = false
        showAlert.value = true
    }
}

const getSelectedSettingsPageLabel = computed(() => {
    switch (activeContent.value) {

        case Content.EditProfile:
            return 'Profil bearbeiten'
        case Content.Password:
            return 'Passwort'
        case Content.DeleteProfile:
            return 'Profil löschen'
        case Content.ShowWuwi:
            return 'Wunschwissen anzeigen'
        case Content.SupportLogin:
            return 'Support Login'
        case Content.General:
            return 'Allgemein'
        case Content.KnowledgeReport:
            return 'Wissensbericht'
    }
})

</script>

<template>
    <div class="row">
        <div class="col-lg-3 col-sm-3 hidden-xs navigation">
            <div class="overline-s no-line">Profil Informationen</div>
            <button @click="activeContent = Content.EditProfile">Profil bearbeiten</button>
            <button @click="activeContent = Content.Password">Passwort</button>
            <button @click="activeContent = Content.DeleteProfile">Profil löschen</button>

            <div class="divider"></div>
            <div class="overline-s no-line">Einstellungen</div>
            <button @click="activeContent = Content.ShowWuwi">Wunschwissen anzeigen</button>
            <button @click="activeContent = Content.SupportLogin">Support Login</button>
            <button @click="activeContent = Content.Abonnement">Abonnement</button>

            <div class="divider"></div>
            <div class="overline-s no-line">Benachrichtigungen</div>
            <!-- <button @click="activeContent = Content.General">Allgemein</button> -->
            <button @click="activeContent = Content.KnowledgeReport">Wissensbericht</button>

            <div class="divider"></div>
        </div>
        <div class="hidden-lg hidden-md hidden-sm col-xs-12">
            <div class="settings-dropdown">
                <VDropdown :distance="0">
                    <div class="settings-select">
                        <div>
                            {{ getSelectedSettingsPageLabel }}
                        </div>
                        <font-awesome-icon icon="fa-solid fa-chevron-down" class="chevron" />
                    </div>

                    <template #popper="p: any">
                        <div class="dropdown-row group-label">
                            Profil Informationen
                        </div>
                        <div class="dropdown-row select-row" @click="activeContent = Content.EditProfile; p.hide()"
                            :class="{'active': activeContent == Content.EditProfile}">
                            <div class="dropdown-label select-option">
                                Profil bearbeiten
                            </div>
                        </div>
                        <div class="dropdown-row select-row" @click="activeContent = Content.Password; p.hide()"
                            :class="{ 'active': activeContent == Content.Password }">
                            <div class="dropdown-label select-option">
                                Passwort
                            </div>
                        </div>
                        <div class="dropdown-row select-row" @click="activeContent = Content.DeleteProfile; p.hide()"
                            :class="{ 'active': activeContent == Content.DeleteProfile }">
                            <div class="dropdown-label select-option">
                                Profil löschen
                            </div>
                        </div>

                        <div class="dropdown-row group-label">
                            Einstellungen
                        </div>
                        <div class="dropdown-row select-row" @click="activeContent = Content.ShowWuwi; p.hide()"
                            :class="{ 'active': activeContent == Content.ShowWuwi }">
                            <div class="dropdown-label select-option">
                                Wunschwissen anzeigen
                            </div>
                        </div>
                        <div class="dropdown-row select-row" @click="activeContent = Content.SupportLogin; p.hide()"
                            :class="{ 'active': activeContent == Content.SupportLogin }">
                            <div class="dropdown-label select-option">
                                Support Login
                            </div>
                        </div>

                        <div class="dropdown-row group-label">
                            Benachrichtigungen
                        </div>
                        <div class="dropdown-row select-row" @click="activeContent = Content.KnowledgeReport; p.hide()"
                            :class="{ 'active': activeContent == Content.KnowledgeReport }">
                            <div class="dropdown-label select-option">
                                Wissensbericht
                            </div>
                        </div>

                    </template>
                </VDropdown>
            </div>

        </div>
        <div class="col-lg-9 col-sm-9 col-xs-12 settings-content">
            <Transition>
                <div v-if="activeContent == Content.EditProfile" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success">{{ msg }}</div>
                        <div class="alert alert-danger" v-else>{{ msg }}</div>
                    </div>
                    <div class="settings-section">
                        <div class="overline-s no-line">Profilbild</div>
                        <Image :src="imageUrl" :format="ImageFormat.Author" class="profile-picture" />
                        <div class="img-settings-btns">

                            <div>
                                <label class="img-upload-btn" for="imageUpload">
                                    <input type="file" accept="image/*" name="file" id="imageUpload"
                                        v-on:change="onFileChange" />
                                    <font-awesome-icon icon="fa-solid fa-upload" />
                                    Bild hochladen
                                </label>
                                <span>{{ imgFile?.name }}</span>
                            </div>

                            <div>
                                <button class="img-delete-btn" @click="removeImage()">
                                    <font-awesome-icon icon="fa-solid fa-trash" /> Profilbild entfernen
                                </button>
                            </div>

                        </div>
                    </div>
                    <div class="settings-section">
                        <div class="input-container">
                            <div class="overline-s no-line">Nutzername</div>
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-12 col-lg-6">
                                        <input name="username" placeholder="" type="text" width="0" v-model="userName"
                                            class="settings-input" id="username">
                                    </div>
                                </div>
                            </form>
                        </div>

                        <div class="input-container">
                            <div class="overline-s no-line">E-Mail</div>
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-12 col-lg-6">
                                        <input name="email" placeholder="" type="email" width="0" v-model="email"
                                            class="settings-input" id="email">
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                    <div class="settings-section">
                        <button class="memo-button btn btn-primary" @click="saveProfileInformation()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            Speichern
                        </button>
                    </div>
                </div>

                <div v-else-if="activeContent == Content.Password" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success">{{ msg }}</div>
                        <div class="alert alert-danger" v-else>{{ msg }}</div>
                    </div>
                    <div class="settings-section">
                        <div class="input-container">
                            <div class="overline-s no-line">Altes Passwort</div>
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-12 col-lg-6">
                                        <input placeholder="" type="password" width="0" v-model="currentPassword"
                                            class="settings-input">
                                    </div>
                                </div>
                            </form>
                        </div>

                        <div class="input-container">
                            <div class="overline-s no-line">Neues Passwort</div>
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-12 col-lg-6">
                                        <input placeholder="" type="password" width="0" v-model="newPassword"
                                            class="settings-input">
                                    </div>
                                </div>
                            </form>
                        </div>

                        <div class="input-container">
                            <div class="overline-s no-line">Neues Passwort wiederholen</div>
                            <form class="form-horizontal">
                                <div class="form-group">
                                    <div class="col-sm-12 col-lg-6">
                                        <input placeholder="" type="password" width="0" v-model="repeatedPassword"
                                            class="settings-input">
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>

                    <div class="settings-section password-section">
                        <button class="memo-button btn btn-primary" @click="saveNewPassword()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            Passwort ändern
                        </button>

                        <button class="memo-button btn btn-link" @click="resetPassword()">
                            Passwort vergessen
                        </button>
                    </div>
                </div>

                <div v-else-if="activeContent == Content.DeleteProfile" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success">{{ msg }}</div>
                        <div class="alert alert-danger" v-else>{{ msg }}</div>
                    </div>
                    <div class="settings-section">
                        <div class="">
                            <p>
                                Um dein Konto zu löschen sende eine E-Mail an die Adresse: <NuxtLink
                                    to="mailto:team@memucho.de" :external="true">team@memucho.de</NuxtLink>
                            </p>
                        </div>

                    </div>
                </div>

                <div v-else-if="activeContent == Content.ShowWuwi" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success">{{ msg }}</div>
                        <div class="alert alert-danger" v-else>{{ msg }}</div>
                    </div>
                    <div class="settings-section">
                        <label class="checkbox-section">
                            <div class="checkbox-container">
                                <input type="checkbox" name="answer" :value="true" v-model="showWuwi" class="hidden" />
                                <font-awesome-icon icon="fa-solid fa-square-check" v-if="showWuwi"
                                    class="checkbox-icon" />
                                <font-awesome-icon icon="fa-regular fa-square" v-else class="checkbox-icon" />
                            </div>
                            <div class="checkbox-label">
                                <div class="overline-s no-line">
                                    Wunschwissen zeigen
                                </div>
                                <p>
                                    Wenn ausgewählt, ist öffentlich sichtbar, welche Fragen in deinem Wunschwissen sind
                                    (außer
                                    private Fragen). Antwortstatistiken werden nicht angezeigt.
                                </p>
                            </div>
                        </label>
                    </div>

                    <div class="settings-section">
                        <button class="memo-button btn btn-primary" @click="saveWuwiVisibility()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            Speichern
                        </button>
                    </div>
                </div>

                <div v-else-if="activeContent == Content.SupportLogin" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success">{{ msg }}</div>
                        <div class="alert alert-danger" v-else>{{ msg }}</div>
                    </div>
                    <div class="settings-section">
                        <label class="checkbox-section">
                            <div class="checkbox-container">
                                <input type="checkbox" name="answer" :value="true" v-model="allowSupportLogin"
                                    class="hidden" />
                                <font-awesome-icon icon="fa-solid fa-square-check" v-if="allowSupportLogin"
                                    class="checkbox-icon" />
                                <font-awesome-icon icon="fa-regular fa-square" v-else class="checkbox-icon" />
                            </div>
                            <div class="checkbox-label">
                                <div class="overline-s no-line">
                                    Support-Login zulassen
                                </div>
                                <p>
                                    Achtung: Das ist nur nach Rücksprache mit dem memucho-Team nötig! Wenn du den
                                    Support-Login
                                    aktivierst, können sich Mitarbeiter von memucho zur Fehlerbehebung oder zu deiner
                                    Unterstützung in deinem Nutzerkonto einloggen, selbstverständlich ohne dein Passwort
                                    zu
                                    benötigen oder sehen zu können.
                                </p>
                            </div>
                        </label>

                    </div>
                    <div class="settings-section">
                        <button class="memo-button btn btn-primary" @click="saveSupportLoginRights()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            Speichern
                        </button>
                    </div>
                </div>
                <div v-else-if="activeContent == Content.Abonnement" class="content">


                    <div>Abotyp {{userStore.subscriptionType}} AblaufDatum {{userStore.subscriptionDuration}}
                        BuchungsDatum
                        {{postingDate}}
                    </div>
                    <div class="settings-section" v-if="userStore.isSubscriptionCanceled == false">
                        <button class="memo-button btn btn-primary" @click="cancelPlan()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            Abo Kündigen
                        </button>
                    </div>
                </div>

                <div v-else-if="activeContent == Content.General" class="content"></div>

                <div v-else-if="activeContent == Content.KnowledgeReport" class="content">
                    <div class="settings-section" v-if="showAlert">
                        <div class="alert alert-success" v-if="success" v-html="notificationIntervalChangeMsg"></div>
                        <div class="alert alert-danger" v-else v-html="notificationIntervalChangeMsg"></div>
                    </div>
                    <div class="settings-section">
                        <div class="overline-s no-line">
                            Wissensbericht per E-Mail:
                        </div>
                        <div class="interval-dropdown">
                            <VDropdown :distance="0">
                                <div class="interval-select">
                                    <div>
                                        {{ getNotificationIntervalText }}
                                    </div>
                                    <font-awesome-icon icon="fa-solid fa-chevron-down" class="chevron" />
                                </div>

                                <template #popper="p: any">
                                    <div class="dropdown-row select-row"
                                        @click="selectedNotificationInterval = NotifcationInterval.Quarterly; p.hide()"
                                        :class="{ 'active': selectedNotificationInterval == NotifcationInterval.Quarterly }">
                                        <div class="dropdown-label select-option">
                                            Vierteljährlich
                                        </div>
                                    </div>
                                    <div class="dropdown-row"
                                        @click="selectedNotificationInterval = NotifcationInterval.Monthly; p.hide()"
                                        :class="{ 'active': selectedNotificationInterval == NotifcationInterval.Monthly }">
                                        <div class="dropdown-label select-option">
                                            Monatlich
                                        </div>
                                    </div>
                                    <div class="dropdown-row select-row"
                                        @click="selectedNotificationInterval = NotifcationInterval.Weekly; p.hide()"
                                        :class="{ 'active': selectedNotificationInterval == NotifcationInterval.Weekly }">
                                        <div class="dropdown-label select-option">
                                            Wöchentlich
                                        </div>
                                    </div>
                                    <div class="dropdown-row select-row"
                                        @click="selectedNotificationInterval = NotifcationInterval.Daily; p.hide()"
                                        :class="{ 'active': selectedNotificationInterval == NotifcationInterval.Daily }">
                                        <div class="dropdown-label select-option">
                                            Täglich
                                        </div>
                                    </div>
                                    <div class="dropdown-row select-row"
                                        @click="selectedNotificationInterval = NotifcationInterval.Never; p.hide()"
                                        :class="{ 'active': selectedNotificationInterval == NotifcationInterval.Never }">
                                        <div class="dropdown-label select-option">
                                            Nie
                                        </div>
                                    </div>

                                </template>
                            </VDropdown>
                        </div>

                        <p>
                            Der Wissensreport informiert dich über deinen aktuellen Wissensstand von deinem
                            <font-awesome-icon icon="fa-solid fa-heart" class="wuwi-icon" />
                            Wunschwissen und über neue Inhalte bei memucho. Er wird nur
                            verschickt, wenn du Wunschwissen hast.
                        </p>
                    </div>

                    <div class="settings-section">
                        <button class="memo-button btn btn-primary" @click="saveNotificationIntervalPreferences()">
                            <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                            Speichern
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

p {
    margin: 10px 0;
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
}

.password-section {
    display: flex;
    flex-direction: row;
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



.divider {
    margin-top: 20px;
    height: 1px;
    background: @memo-grey-lighter;
    width: 100%;
    margin-bottom: 10px;
}



.navigation {
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
</style>