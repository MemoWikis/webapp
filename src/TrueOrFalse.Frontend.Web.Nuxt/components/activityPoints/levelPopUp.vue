<script setup lang="ts">
import { useUserStore } from '../user/userStore'
import { useActivityPointsStore } from './activityPointsStore'

const userStore = useUserStore()
const activityPointsStore = useActivityPointsStore()
function close() {
    activityPointsStore.showLevelPopUp = false
}
</script>

<template>
    <Modal :show="activityPointsStore.showLevelPopUp" :show-close-button="true"
        :primary-btn-label="userStore.isLoggedIn ? undefined : 'Registrieren'" :secondary-btn="'Weiterlernen'"
        @secondary-btn="close()" @primary-btn="navigateTo('/Registrieren')" @close="close()" @keydown.esc="close()">
        <template v-slot:header>
            <img class="happy-memo-svg" width="120" src="/Images/memucho_MEMO_happy_blau.svg">
            <span class="title-text"><b>Fortschritt:</b> Du bist jetzt Level </span>
            <span class="level-display">
                <svg>
                    <circle cx="50%" cy="50%" r="15" />
                    <text class="level-count" x="50%" y="50%" dy=".34em">{{
                        activityPointsStore.level
                    }}</text>
                </svg>
            </span>
            <div class="title-text" v-if="userStore.isLoggedIn">
                Super! Du wirst immer schlauer.
            </div>
        </template>
        <template v-slot:body>
            <template v-if="userStore.isLoggedIn">
                Das nächste Level erreichst du bei <b>{{ activityPointsStore.activityPointsTillNextLevel }}</b> Punkten.
            </template>

            <template v-else>
                Wenn du dich jetzt registrierst, behältst du deine Punkte und
                dein erreichtes Level. <br /> <br />
                <b>
                    <NuxtLink to="/Registrieren">Registriere</NuxtLink> dich jetzt und werde immer schlauer mit memucho!
                </b>
            </template>
        </template>
    </Modal>
</template>