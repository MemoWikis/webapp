<script setup lang="ts">
import { AlertType, useAlertStore } from '../alert/alertStore'
import { useUserStore } from '../user/userStore'

const alertStore = useAlertStore()
const userStore = useUserStore()
const { t } = useI18n()

const eMail = ref('')
const password = ref('')
const persistentLogin = ref(true)
async function login() {

    errorMessage.value = ''

    const data = {
        EmailAddress: eMail.value,
        Password: password.value,
        PersistentLogin: persistentLogin.value
    }

    const result = await userStore.login(data)
    if (!result.success)
        errorMessage.value = result.msg!
}
const passwordInputType = ref('password')

const googleLoginComponent = ref()

function googleLogin() {
    showLoginIsInProgress.value = true
    googleLoginComponent.value.login()
}
const errorMessage = ref('')
const showLoginIsInProgress = ref(false)

const showGooglePluginInfo = ref(false)
const allowGooglePlugin = ref(false)

function loadGooglePlugin() {
    document.cookie = "allowGooglePlugin=true"
    allowGooglePlugin.value = true
    showLoginIsInProgress.value = true
    googleLoginComponent.value.loadPlugin()
}

const facebookLoginComponent = ref()

function facebookLogin() {
    showLoginIsInProgress.value = true
    facebookLoginComponent.value.login()
}

const showFacebookPluginInfo = ref(false)
const allowFacebookPlugin = ref(false)

function loadFacebookPlugin(login = false) {
    document.cookie = "allowFacebookPlugin=true"
    allowFacebookPlugin.value = true
    showLoginIsInProgress.value = true
    facebookLoginComponent.value.loadPlugin(login)
}
const primaryBtnLabel = ref<string | undefined>('Anmelden')
const showPasswordReset = ref(false)

watch([showLoginIsInProgress, showGooglePluginInfo, showFacebookPluginInfo, showPasswordReset], ([inProgress, googleInfo, fbInfo, pwReset]) => {
    if (inProgress || googleInfo || fbInfo)
        primaryBtnLabel.value = undefined
    else if (pwReset)
        primaryBtnLabel.value = t('login.button.requestLink')
    else primaryBtnLabel.value = t('login.button.signIn')
})

async function primaryAction() {
    if (showLoginIsInProgress.value || showGooglePluginInfo.value || showFacebookPluginInfo.value)
        return
    else if (showPasswordReset.value) {
        const result = await userStore.resetPassword(eMail.value)
        userStore.showLoginModal = false
        if (result.success) {
            alertStore.openAlert(AlertType.Default, {
                text: t('info.passwordResetRequested', { email: eMail.value })
            })
        } else {
            alertStore.openAlert(AlertType.Error, { text: t('error.default') })
        }
    }
    else login()
}

watch(() => userStore.showLoginModal, () => {
    showLoginIsInProgress.value = false
})

onMounted(() => {
    var googleCookie = document.cookie.match('(^|;)\\s*' + "allowGooglePlugin" + '\\s*=\\s*([^;]+)')?.pop() || ''
    if (googleCookie === "true")
        loadGooglePlugin()

    var facebookCookie = document.cookie.match('(^|;)\\s*' + "allowFacebookPlugin" + '\\s*=\\s*([^;]+)')?.pop() || ''
    if (facebookCookie === "true")
        loadFacebookPlugin()
})

</script>

<template>
    <div id="LoginModalComponent">
        <LazyModal :show-close-button="true" :primary-btn-label="primaryBtnLabel" :is-full-size-buttons="true"
            @close="userStore.showLoginModal = false" @primary-btn="primaryAction" :show="userStore.showLoginModal"
            @keydown.esc="userStore.showLoginModal = false">
            <template v-slot:header>
                <h2 v-if="showGooglePluginInfo && !allowGooglePlugin">{{ t('login.title.google') }}</h2>
                <h2 v-else-if="showFacebookPluginInfo && !allowFacebookPlugin">{{ t('login.title.facebook') }}</h2>
                <h2 v-else-if="showPasswordReset">{{ t('login.title.resetPassword') }}</h2>
                <h2 v-else>{{ t('login.title.default') }}</h2>
            </template>
            <template v-slot:body>
                <div v-if="showLoginIsInProgress">
                    {{ t('login.message.loginInProgress') }}
                </div>
                <div v-else-if="(showGooglePluginInfo && !allowGooglePlugin) || (showFacebookPluginInfo && !allowFacebookPlugin)"
                    class="row">
                    <div v-if="showGooglePluginInfo && !allowGooglePlugin" class="col-xs-12">
                        <p>
                            {{ t('login.message.googlePrivacy') }}
                            <NuxtLink href="/Impressum">{{ t('common.privacyPolicy') }}</NuxtLink>.
                        </p>
                    </div>

                    <div v-else-if="showFacebookPluginInfo && !allowFacebookPlugin" class="col-xs-12">
                        <p>
                            {{ t('login.message.facebookPrivacy') }}
                            <NuxtLink href="/Impressum">{{ t('common.privacyPolicy') }}</NuxtLink>.
                        </p>
                    </div>
                </div>
                <template v-else-if="showPasswordReset">
                    <div>
                        <p>
                            {{ t('login.message.resetPasswordInfo') }}
                        </p>
                    </div>

                    <div>
                        <form class="form-horizontal">
                            <div class="input-container">
                                <div class="overline-s no-line">{{ t('login.form.email') }}</div>
                                <div class="form-group">
                                    <div class="col-sm-12">
                                        <input name="passwordReset" placeholder="" type="email" width="100%"
                                            class="login-inputs" v-model="eMail" @keydown.enter="primaryAction"
                                            @click="errorMessage = ''" />
                                    </div>
                                </div>
                            </div>
                        </form>
                    </div>
                </template>
                <template v-else>
                    <div class="form-group omb_login row">
                        <LazyClientOnly>
                            <div class="col-sm-12 omb_socialButtons">

                                <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                                    <div class="btn btn-block cursor-hand socialMediaBtn" id="GoogleLogin"
                                        v-if="allowGooglePlugin" @click="googleLogin()">
                                        <img src="~/assets/images/SocialMediaIcons/Google__G__Logo.svg"
                                            alt="socialMediaBtnContainer" class="socialMediaLogo">
                                        <div class="socialMediaLabel">{{ t('login.button.continueWithGoogle') }}</div>
                                    </div>
                                    <div class="btn btn-block cursor-hand socialMediaBtn" v-else
                                        @click="showGooglePluginInfo = true">
                                        <img src="~/assets/images/SocialMediaIcons/Google__G__Logo.svg"
                                            alt="socialMediaBtnContainer" class="socialMediaLogo">
                                        <div class="socialMediaLabel">{{ t('login.button.continueWithGoogle') }}</div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 socialMediaBtnContainer">
                                    <div class="btn btn-block cursor-hand socialMediaBtn" id="FacebookLogin"
                                        v-if="allowFacebookPlugin" @click="facebookLogin()">
                                        <img src="~/assets/images/SocialMediaIcons/Facebook_logo_F.svg"
                                            alt="FacebookLogin" class="socialMediaLogo">
                                        <div class="socialMediaLabel">{{ t('login.button.continueWithFacebook') }}</div>
                                    </div>
                                    <div class="btn btn-block cursor-hand socialMediaBtn" v-else
                                        @click="showFacebookPluginInfo = true">
                                        <img src="~/assets/images/SocialMediaIcons/Facebook_logo_F.svg"
                                            alt="FacebookLogin" class="socialMediaLogo">
                                        <div class="socialMediaLabel">{{ t('login.button.continueWithFacebook') }}</div>
                                    </div>
                                </div>
                            </div>
                        </LazyClientOnly>
                    </div>

                    <i18n-t keypath="register.registerNote">
                        <template #termsOfUse>
                            <NuxtLink :to="`/${t('url.termsOfUse')}`">
                                {{ t('label.termsOfUse') }}
                            </NuxtLink>
                        </template>
                        <template #privacyPolicy>
                            <NuxtLink :to="`/${t('url.legalNotice')}`">
                                {{ t('label.privacyPolicy') }}
                            </NuxtLink>
                        </template>
                        <template #hereMoreInfos>
                            <NuxtLink :to="`/${t('url.legalNotice')}#under16`">
                                {{ t('register.hereMoreInfos') }}
                            </NuxtLink>
                        </template>
                    </i18n-t>
                    <form class="form-horizontal">
                        <div class="row" style="margin-bottom: 10px;">
                            <div class="col-xs-12">
                                <div class="register-divider-container">
                                    <div class="register-divider">
                                        <div class="register-divider-line"></div>
                                    </div>
                                    <div class="register-divider-label-container">
                                        <div class="register-divider-label">
                                            {{ t('login.message.or') }}
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="input-container">
                            <div class="overline-s no-line">{{ t('login.form.email') }}</div>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <input name="login" placeholder="" type="email" width="100%" class="login-inputs" v-model="eMail" @keydown.enter="primaryAction" @click="errorMessage = ''" />
                                </div>
                            </div>
                        </div>
                        <div class="input-container">
                            <div class="overline-s no-line">{{ t('login.form.password') }}</div>
                            <div class="form-group">
                                <div class="col-sm-12">
                                    <input name="password" placeholder="" :type="passwordInputType" width="100%" class="login-inputs" v-model="password" @keydown.enter="primaryAction" @click="errorMessage = ''" />
                                    <font-awesome-icon icon="fa-solid fa-eye" class="eyeIcon" v-if="passwordInputType === 'password'" @click="passwordInputType = 'text'" />
                                    <font-awesome-icon icon="fa-solid fa-eye-slash" class="eyeIcon" v-if="passwordInputType === 'text'" @click="passwordInputType = 'password'" />
                                </div>
                            </div>
                            <div class="infoContainer col-sm-12 noPadding">
                                <div class="col-sm-4 noPadding">
                                    <label class="cursor-hand">
                                        <input type="checkbox" class="cursor-hand" v-model="persistentLogin" />
                                        <span class="checkboxText">{{ t('login.form.stayLoggedIn') }}</span>
                                    </label>
                                </div>
                                <div class="col-sm-4 col-sm-offset-4 noPadding" style="text-align: right;">
                                    <div class="btn btn-link" @click="showPasswordReset = true">
                                        {{ t('login.button.forgotPassword') }}
                                    </div>
                                </div>
                            </div>
                            <div class="errorMessage" v-if="errorMessage.length > 0">{{ errorMessage }}</div>
                        </div>
                    </form>
                </template>


            </template>
            <template v-slot:footer-text>
                <div class="row" v-if="showLoginIsInProgress">
                    <p>
                        <button type="button" class="btn btn-default pull-right memo-button" style="margin-right:10px"
                            @click="showLoginIsInProgress = false">
                            {{ t('login.button.back') }}
                        </button>
                    </p>
                </div>
                <div v-else-if="showPasswordReset" class="footerText">
                    <p>
                        <strong>{{ t('login.message.rememberedPassword') }}</strong><br />
                        <button class="btn-link" @click="showPasswordReset = false">
                            {{ t('login.message.clickToLogin') }}
                        </button>
                    </p>
                </div>
                <div class="footerText" v-else-if="!showGooglePluginInfo && !showFacebookPluginInfo">
                    <p>
                        <strong style="font-weight: 700;">{{ t('login.message.notRegistered') }}</strong> <br />
                        <NuxtLink :to="`/${t('url.register')}`" @click="userStore.showLoginModal = false">
                            {{ t('login.button.registerNow') }}
                        </NuxtLink>
                    </p>
                </div>
                <div class="row" v-else-if="showGooglePluginInfo">
                    <p>
                        <button type="button" class="btn btn-primary pull-right memo-button"
                            @click="loadGooglePlugin()">
                            {{ t('login.button.agree') }}
                        </button>
                        <button type="button" class="btn btn-default pull-right memo-button" style="margin-right:10px"
                            @click="showGooglePluginInfo = false">
                            {{ t('login.button.cancel') }}
                        </button>
                    </p>
                </div>
                <div class="row" v-else-if="showFacebookPluginInfo">
                    <p>
                        <button type="button" class="btn btn-primary pull-right memo-button"
                            @click="loadFacebookPlugin(true)">
                            {{ t('login.button.agree') }}
                        </button>
                        <button type="button" class="btn btn-default pull-right memo-button" style="margin-right:10px"
                            @click="showFacebookPluginInfo = false">
                            {{ t('login.button.cancel') }}
                        </button>
                    </p>
                </div>
            </template>
        </LazyModal>
    </div>
    <UserFacebookLogin ref="facebookLoginComponent" />
    <UserGoogleLogin ref="googleLoginComponent" />
</template>

<style scoped lang="less">
@import '~~/assets/shared/register.less';

h2 {
    margin-bottom: 36px;
}
</style>