<script lang="ts" setup>
import { usePageStore } from '../page/pageStore'
import { useUserStore } from '../user/userStore'

const userStore = useUserStore()
const pageStore = usePageStore()
const { t } = useI18n()

const cookieName = ref('loginToEditReminderBanner')
const shortPageName = ref(pageStore.name.length > 20 ? pageStore.name.substring(0, 20) + '...' : pageStore.name)

</script>

<template>
    <Banner :cookieName="cookieName" id="LoginToEditReminderBanner">
        <template #subText>
            {{ t('banner.login.invitedTo', { pageName: shortPageName }) }}
        </template>
        <template #mainText>
            {{ t('banner.login.loginToEdit') }}
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

#LoginToEditReminderBanner {
    @media (min-width: 900px) and (max-width: 1650px) {
        padding-left: clamp(100px, 10vw, 320px);
    }

    @media (min-width: 1651px) {
        padding-left: clamp(100px, 20vw, 320px);
    }
}
</style>