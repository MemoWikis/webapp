<script lang="ts" setup>
const { t } = useI18n()

const currentPassword = ref<string>('')
const newPassword = ref<string>('')
const repeatedPassword = ref<string>('')

const showAlert = ref(false)
const msg = ref('')
const success = ref(false)

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
            <div class="input-container">
                <div class="overline-s no-line">{{ t('settings.password.currentPassword') }}</div>
                <form class="form-horizontal">
                    <div class="form-group">
                        <div class="col-sm-12 col-lg-6">
                            <input v-model="currentPassword" placeholder="" type="password" width="0"
                                class="settings-input" />
                        </div>
                    </div>
                </form>
            </div>

            <div class="input-container">
                <div class="overline-s no-line">{{ t('settings.password.newPassword') }}</div>
                <form class="form-horizontal">
                    <div class="form-group">
                        <div class="col-sm-12 col-lg-6">
                            <input v-model="newPassword" placeholder="" type="password" width="0"
                                class="settings-input" />
                        </div>
                    </div>
                </form>
            </div>

            <div class="input-container">
                <div class="overline-s no-line">{{ t('settings.password.repeatNewPassword') }}</div>
                <form class="form-horizontal">
                    <div class="form-group">
                        <div class="col-sm-12 col-lg-6">
                            <input v-model="repeatedPassword" placeholder="" type="password" width="0"
                                class="settings-input" />
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
</template>
