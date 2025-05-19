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

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#MissionControlLoginReminderBanner {
    @media (min-width: 900px) and (max-width: 1650px) {
        padding-left: clamp(100px, 10vw, 320px);
    }

    @media (min-width: 1651px) {
        padding-left: clamp(100px, 20vw, 320px);
    }
}
</style>
