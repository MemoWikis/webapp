<script lang="ts" setup>
import { AlertType, messages, useAlertStore } from '~/components/alert/alertStore'
import { Page } from '~/components/shared/pageEnum'
import { Topic } from '~/components/topic/topicStore'
import { CurrentUser, useUserStore } from '~/components/user/userStore'
const userStore = useUserStore()
const alertStore = useAlertStore()
interface Props {
    documentation: Topic
}
const props = defineProps<Props>()
const emit = defineEmits(['setPage'])

onBeforeMount(() => {
    emit('setPage', Page.ResetPassword)

    if (userStore.isLoggedIn)
        navigateTo('/')
})


const { $logger } = useNuxtApp()
const route = useRoute()
const { data: tokenValidationResult } = await useFetch<FetchResult<any>>(`/apiVue/ResetPassword/Validate/${route.params.token}`, {
    method: 'GET',
    credentials: 'include',
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    },
})
const errorMessage = ref<string>('')

if (tokenValidationResult.value?.success == false) {
    errorMessage.value = messages.getByCompositeKey(tokenValidationResult.value?.messageKey)
}

const newPassword = ref<string>('')
const newPasswordInputType = ref<string>('password')

const repeatedPassword = ref<string>('')
const repeatedPasswordInputType = ref<string>('password')

async function saveNewPassword() {
    errorMessage.value = ''

    if (newPassword.value != repeatedPassword.value) {
        errorMessage.value = 'Die Passwörter stimmen nicht überein'
        return
    }
    if (newPassword.value.length < 5) {
        errorMessage.value = 'Bitte gib mindestens 5 Zeichen'
        return
    }

    const data = {
        token: route.params.token,
        password: newPassword.value
    }

    const result = await $fetch<FetchResult<CurrentUser>>('/apiVue/ResetPassword/SetNewPassword', {
        method: 'POST',
        body: data,
        credentials: 'include'
    })

    if (result.success) {
        userStore.initUser(result.data)
        await nextTick()
        if (userStore.isLoggedIn)
            navigateTo('/')

    } else {
        alertStore.openAlert(AlertType.Error, { text: messages.getByCompositeKey(result.messageKey) })
    }
}
</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="col-lg-9 col-md-12 container main-content">

                <div class="row content">
                    <div class="form-horizontal col-md-12">

                        <div class="row" style="margin-bottom: 23px; margin-top: -13px;">
                            <h1 class="col-sm-offset-2 col-sm-8 reset-title">
                                Neues Passwort setzen
                            </h1>
                        </div>
                        <fieldset v-if="tokenValidationResult?.success">
                            <div class="col-sm-offset-2 alert alert-danger col-sm-8" v-if="errorMessage.length > 0">
                                {{ errorMessage }}
                            </div>

                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-8">
                                    <div class="overline-s no-line">Passwort</div>
                                </div>

                                <div class="col-sm-offset-2 col-sm-8">
                                    <input name="password" placeholder="min. 5 Zeichen" :type="newPasswordInputType"
                                        width="100%" class="password-inputs" v-model="newPassword"
                                        @keydown.enter="saveNewPassword()" />
                                    <font-awesome-icon icon="fa-solid fa-eye" class="eyeIcon"
                                        v-if="newPasswordInputType == 'password'" @click="newPasswordInputType = 'text'" />
                                    <font-awesome-icon icon="fa-solid fa-eye-slash" class="eyeIcon"
                                        v-if="newPasswordInputType == 'text'" @click="newPasswordInputType = 'password'" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-8">
                                    <div class="overline-s no-line">Passwort wiederholen</div>
                                </div>

                                <div class="col-sm-offset-2 col-sm-8">
                                    <input name="password" placeholder="" :type="repeatedPasswordInputType" width="100%"
                                        class="password-inputs" v-model="repeatedPassword"
                                        @keydown.enter="saveNewPassword()" />
                                    <font-awesome-icon icon="fa-solid fa-eye" class="eyeIcon"
                                        v-if="repeatedPasswordInputType == 'password'"
                                        @click="repeatedPasswordInputType = 'text'" />
                                    <font-awesome-icon icon="fa-solid fa-eye-slash" class="eyeIcon"
                                        v-if="repeatedPasswordInputType == 'text'"
                                        @click="repeatedPasswordInputType = 'password'" />
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                                    <button @click="saveNewPassword()" class="btn btn-primary memo-button col-sm-12">
                                        Neues Passwort speichern
                                    </button>
                                </div>
                            </div>

                            <div class="form-group">
                                <div class="col-sm-offset-2 col-sm-8" style="border-top: 0px; margin-top: 10px;">
                                    <p href="#" style="text-align: center;">
                                        <button style="text-align: center;" class="btn btn-link"
                                            @click="userStore.openLoginModal()">
                                            Mein Passwort ist mir wieder eingefallen.
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
            <Sidebar :documentation="props.documentation" />

        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~/assets/includes/imports.less';

.main-page {
    padding-bottom: 45px;
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
</style>