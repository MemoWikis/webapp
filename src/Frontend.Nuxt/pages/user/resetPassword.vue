<script lang="ts" setup>
import { AlertType, useAlertStore } from '~/components/alert/alertStore'
import { SiteType } from '~/components/shared/siteEnum'
import { CurrentUser, useUserStore } from '~/components/user/userStore'
const userStore = useUserStore()
const alertStore = useAlertStore()
interface Props {
    site: SiteType
}
const props = defineProps<Props>()
const { t } = useI18n()

const emit = defineEmits(['setPage'])

onBeforeMount(() => {
    emit('setPage', SiteType.ResetPassword)

    if (userStore.isLoggedIn)
        return navigateTo('/')
})


const { $logger } = useNuxtApp()
const route = useRoute()
const { data: tokenValidationResult } = await useFetch<FetchResult<any>>(`/apiVue/ResetPassword/Validate/${route.params.token}`, {
    method: 'GET',
    credentials: 'include',
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    }
})
const errorMessage = ref<string>('')

if (tokenValidationResult.value?.success === false && tokenValidationResult.value?.messageKey) {
    errorMessage.value = t(tokenValidationResult.value.messageKey)
}

const newPassword = ref<string>('')
const newPasswordInputType = ref<string>('password')

const repeatedPassword = ref<string>('')
const repeatedPasswordInputType = ref<string>('password')

async function saveNewPassword() {
    errorMessage.value = ''
    if (newPassword.value != repeatedPassword.value) {
        errorMessage.value = t('resetPassword.passwordMismatch')
        return
    }
    if (newPassword.value.length < 5) {
        errorMessage.value = t('error.user.passwordTooShort')
        return
    }

    const data = {
        token: route.params.token,
        password: newPassword.value
    }

    const result = await $api<FetchResult<CurrentUser>>('/apiVue/ResetPassword/SetNewPassword', {
        method: 'POST',
        body: data,
        credentials: 'include'
    })

    if (result.success) {
        userStore.initUser(result.data)
        await nextTick()
        if (userStore.isLoggedIn)
            return navigateTo('/')
    } else {
        alertStore.openAlert(AlertType.Error, { text: t(result.messageKey) })
    }
}
</script>

<template>
    <div class="main-content">
        <div class="row reset-password-container">
            <div class="form-horizontal col-md-9 col-sm-12">
                <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                    <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                        {{ t('resetPassword.title') }}
                    </h1>
                </div>
                <fieldset v-if="tokenValidationResult?.success">
                    <div class="col-sm-offset-2 alert alert-danger col-sm-8" v-if="errorMessage.length > 0">
                        {{ errorMessage }}
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-8">
                            <div class="overline-s no-line">{{ t('resetPassword.password') }}</div>
                        </div>

                        <div class="col-sm-offset-2 col-sm-8">
                            <input name="password" :placeholder="t('resetPassword.passwordPlaceholder')" :type="newPasswordInputType"
                                width="100%" class="password-inputs" v-model="newPassword"
                                @keydown.enter="saveNewPassword()" />
                            <font-awesome-icon icon="fa-solid fa-eye" class="eyeIcon"
                                v-if="newPasswordInputType === 'password'"
                                @click="newPasswordInputType = 'text'" />
                            <font-awesome-icon icon="fa-solid fa-eye-slash" class="eyeIcon"
                                v-if="newPasswordInputType === 'text'"
                                @click="newPasswordInputType = 'password'" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-8">
                            <div class="overline-s no-line">{{ t('resetPassword.repeatPassword') }}</div>
                        </div>

                        <div class="col-sm-offset-2 col-sm-8">
                            <input name="password" placeholder="" :type="repeatedPasswordInputType" width="100%"
                                class="password-inputs" v-model="repeatedPassword"
                                @keydown.enter="saveNewPassword()" />
                            <font-awesome-icon icon="fa-solid fa-eye" class="eyeIcon"
                                v-if="repeatedPasswordInputType === 'password'"
                                @click="repeatedPasswordInputType = 'text'" />
                            <font-awesome-icon icon="fa-solid fa-eye-slash" class="eyeIcon"
                                v-if="repeatedPasswordInputType === 'text'"
                                @click="repeatedPasswordInputType = 'password'" />
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                            <button @click="saveNewPassword()" class="btn btn-primary memo-button col-sm-12">
                                {{ t('resetPassword.saveButton') }}
                            </button>
                        </div>
                    </div>
                    <div class="form-group">
                        <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                            <p href="#" style="text-align: center;">
                                <button style="text-align: center;" class="btn btn-link"
                                    @click="userStore.openLoginModal()">
                                    {{ t('resetPassword.rememberedPassword') }}
                                </button>
                            </p>
                        </div>
                    </div>

                </fieldset>
                <div v-else-if="!tokenValidationResult?.success"
                    class="alert alert-danger col-sm-offset-2 col-sm-8 ">
                    {{ errorMessage }}
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~/assets/includes/imports.less';

.reset-password-container {
    display: flex;
    justify-content: center;
}

.reset-title {
    padding-left: 7px;
}

.password-inputs {
    resize: none;
    height: 44px;
    overflow: hidden;
    width: 100%;
    padding: 0 15px 0;
    border: solid @memo-grey-light 1px;
    box-shadow: none;
    color: @memo-grey-dark !important;
}

.eyeIcon {
    position: absolute;
    top: 16px;
    font-size: 16px;
    right: 30px;
    cursor: pointer;
    width: 20px;
}

.sidesheet-open {

    .reset-password-container {
        @media (max-width: 1500px) {
            .col-sm-offset-2 {
                margin-left: 0;
            }

            .col-sm-8 {
                width: 100%;
            }
        }
    }
}
</style>