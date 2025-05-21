<script lang="ts" setup>
import { useUserStore } from '../user/userStore'

const userStore = useUserStore()
const { t } = useI18n()

const cookieName = ref('missionControlLoginReminderBanner')

</script>

<template>
    <Banner :cookieName="cookieName" id="MissionControlLoginReminderBanner">
        <template #mainText>
            {{ t('banner.login.loginToSeeMissionControl') }}
        </template>
        <template #action>
            <button class="memo-button btn btn-primary" @click="userStore.openLoginModal()">
                {{ t('label.login') }}
            </button>
        </template>
    </Banner>
</template>
