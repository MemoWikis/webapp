<script lang="ts" setup>
import { useUserStore } from '../user/userStore'

const userStore = useUserStore()
const { t } = useI18n()

const cookieName = ref('loginReminderBanner')


</script>

<template>
    <Banner :cookieName="cookieName" id="LoginReminderBanner">
        <template #subText>
            {{ t('banner.login.subText', `'You've been invited to collab on 'name' `) }}
        </template>
        <template #mainText>
            {{ t('banner.login.mainText', 'Login is required to edit this page') }}
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
</style>