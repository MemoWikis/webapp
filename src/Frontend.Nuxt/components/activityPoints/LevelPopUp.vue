<script setup lang="ts">
import { useUserStore } from '../user/userStore'
import { useActivityPointsStore } from './activityPointsStore'
const { t } = useI18n()

const userStore = useUserStore()
const activityPointsStore = useActivityPointsStore()
function close() {
    activityPointsStore.showLevelPopUp = false
}
</script>

<template>
    <Modal :show="activityPointsStore.showLevelPopUp" :show-close-button="true"
        :primary-btn-label="userStore.isLoggedIn ? undefined : t('levelPopUp.buttons.register')"
        :secondary-btn="t('levelPopUp.buttons.continueLearning')"
        @secondary-btn="close()" @primary-btn="navigateTo(t('url.register'))" @close="close()" @keydown.esc="close()">
        <template v-slot:header>
            <div class="levelpopup-header">
                <img class="happy-memo-svg" width="120" src="/Images/memoWikis_MEMO_happy_blau.svg">
                <span class="title-text"><b>{{ t('levelPopUp.header.progress') }}</b> {{ t('levelPopUp.header.currentLevel') }} </span>
                <span class="level-display">
                    <svg>
                        <circle cx="50%" cy="50%" r="15" />
                        <text class="level-count" x="50%" y="50%" dy=".34em">
                            {{ activityPointsStore.level }}
                        </text>
                    </svg>
                </span>
            </div>

            <div class="title-text" v-if="userStore.isLoggedIn">
                {{ t('levelPopUp.header.congratulation') }}
            </div>
        </template>
        <template v-slot:body>
            <template v-if="userStore.isLoggedIn">
                {{ t('levelPopUp.body.nextLevel', { points: activityPointsStore.activityPointsTillNextLevel }) }}
            </template>

            <template v-else>
                {{ t('levelPopUp.body.registerKeepProgress') }} <br /> <br />
                <b>
                    <i18n-t keypath="levelPopUp.body.registerCta">
                        <template #register>
                            <NuxtLink :to="`/${t('url.register')}`">{{ t('levelPopUp.buttons.register') }}</NuxtLink>
                        </template>
                    </i18n-t>
                </b>
            </template>
        </template>
    </Modal>
</template>

<style lang="less" scoped>
.levelpopup-header {
    margin-top: 20px;
    margin-bottom: 20px;
}
</style>