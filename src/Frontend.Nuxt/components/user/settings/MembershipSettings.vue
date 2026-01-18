<script lang="ts" setup>
import { useUserStore } from '../userStore'
import * as Subscription from '~~/components/user/membership/subscription'
import { AlertType, useAlertStore } from '~/components/alert/alertStore'

const config = useRuntimeConfig()
const headers = useRequestHeaders(['cookie']) as HeadersInit
const { t } = useI18n()
const userStore = useUserStore()
const alertStore = useAlertStore()
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
        await navigateTo(data.value, { external: true })
    } else {
        alertStore.openAlert(AlertType.Error, { text: t('settings.error.redirectFailed') })
    }
}
</script>

<template>
    <div class="content">
        <div v-if="userStore.subscriptionType != Subscription.Type.Basic" class="settings-section">
            <button v-if="userStore.isSubscriptionCanceled === false" class="memo-button btn btn-primary"
                @click="cancelPlan()">
                <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                {{ t('settings.membership.manageOrCancel') }}
            </button>
            <button
                v-else-if="userStore.isSubscriptionCanceled === true && userStore.subscriptionType === Subscription.Type.Plus"
                class="memo-button btn btn-primary" @click="cancelPlan()">
                <font-awesome-icon icon="fa-solid fa-floppy-disk" />
                {{ t('settings.membership.resume') }}
            </button>
        </div>

        <div class="settings-section plans">
            <UserMembershipPlans />
        </div>
    </div>
</template>
