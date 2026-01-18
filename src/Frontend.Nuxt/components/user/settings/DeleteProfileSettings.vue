<script lang="ts" setup>
import { useUserStore } from '../userStore'
import { AlertType, useAlertStore } from '~/components/alert/alertStore'

const config = useRuntimeConfig()
const { t } = useI18n()
const userStore = useUserStore()
const alertStore = useAlertStore()

const showAlert = ref(false)
const msg = ref('')
const success = ref(false)

const canDeleteProfile = ref(false)

onMounted(() => {
    checkIfProfileCanBeDeleted()
})

const checkIfProfileCanBeDeleted = async () => {
    const result = await $api<boolean>('/apiVue/VueUserSettings/CanDeleteUser', {
        mode: 'cors',
        method: 'GET',
        credentials: 'include'
    })

    canDeleteProfile.value = result
}

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
            <div class="">
                <div class="alert alert-info">
                    <p>
                        <b>{{ t('settings.deleteProfile.warning') }}</b> {{
                            t('settings.deleteProfile.onlyIf') }}
                    </p>
                    <ul>
                        <li>{{ t('settings.deleteProfile.condition1') }}</li>
                        <li>{{ t('settings.deleteProfile.condition2') }}</li>
                    </ul>
                </div>

                <button v-if="canDeleteProfile" class="memo-button btn btn-danger" @click.prevent="deleteProfile()">
                    {{ t('settings.deleteProfile.deleteButton') }}
                </button>
                <div v-else class="alert alert-warning">
                    <p>
                        {{ t('settings.deleteProfile.notPossible') }}
                        <NuxtLink :to="`mailto:${config.public.teamEmail}`" :external="true">{{
                            config.public.teamEmail }}</NuxtLink>,
                        {{ t('settings.deleteProfile.contactReason') }}
                    </p>
                </div>
            </div>
        </div>
    </div>
</template>
