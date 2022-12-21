<script lang="ts" setup>
import { useTopicStore } from './topicStore'
import { useAlertStore, AlertType, AlertMsg, messages } from '../alert/alertStore'
const topicStore = useTopicStore()
const alertStore = useAlertStore()

const publishRequestConfirmation = ref(false)
const publishQuestions = ref(true)

const confirmLicense = ref(false)

const blinkTimer = ref(null as ReturnType<typeof setTimeout> | null)
const blink = ref(false)

async function publishTopic() {
    if (!confirmLicense.value) {
        blinkTimer.value = null
        blink.value = true
        blinkTimer.value = setTimeout(() => {
            blink.value = false
        }, 2000)
        return
    }

    const data = {
        topicId: topicStore.id
    }

    const result = await $fetch<any>('/apiVue/PublishTopic/Publish', { method: 'POST', body: data, mode: 'cors', credentials: 'include' })
    if (result.success) {
        alertStore.openAlert(AlertType.Success, { text: messages.category.publish } as AlertMsg)
    }
}
</script>

<template>
    <LazyModal :show="topicStore.showPublishModal">

        <template v-slot:header>
            <h4>{{ topicStore.name }} veröffentlichen</h4>
        </template>

        <template v-slot:body>
            <div class="subHeader">
                Öffentliche Inhalte sind für alle auffindbar und können frei weiterverwendet werden. <br />
                Du veröffentlichst unter Creative-Commons-Lizenz.
            </div>
            <div class="checkbox-container" @click="publishQuestions = !publishQuestions"
                v-if="topicStore.questionCount > 0">
                <div class="checkbox-icon">
                    <font-awesome-icon icon="fa-solid fa-square-check" v-if="publishQuestions" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else />
                </div>
                <div class="checkbox-label">
                    Möchtest Du {{ topicStore.questionCount }} private Fragen veröffentlichen?
                </div>

            </div>
            <div class="checkbox-container license-info" @click="confirmLicense = !confirmLicense">
                <div class="checkbox-icon" :class="{ blink: blink }">
                    <font-awesome-icon icon="fa-solid fa-square-check" v-if="confirmLicense" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else />
                </div>
                <div class="checkbox-label" :class="{ blink: blink }">
                    Ich stelle diesen Eintrag unter die Lizenz "Creative Commons - Namensnennung 4.0 International" (CC
                    BY 4.0, <NuxtLink :external="true" to="https://creativecommons.org/licenses/by/4.0/legalcode.de"
                        target="_blank">Lizenztext, deutsche Zusammenfassung</NuxtLink>).
                    Der Eintrag kann bei angemessener Namensnennung ohne Einschränkung weiter genutzt werden. Die Texte
                    und ggf.
                    Bilder sind meine eigene Arbeit und nicht aus urheberrechtlich geschützten Quellen kopiert.
                </div>
            </div>
        </template>

        <template v-slot:footer>
            <div class="btn btn-link memo-button" @click="topicStore.showPublishModal = false">abbrechen</div>
            <div class="btn btn-primary memo-button" id="PublishCategoryBtn" @click="publishTopic"
                :class="{ 'disabled-btn': !confirmLicense }">veröffentlichen</div>
        </template>

    </LazyModal>
</template>