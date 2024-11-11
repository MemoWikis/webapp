<script lang="ts" setup>
import { usePublishPageStore } from './publishPageStore'
const publishPageStore = usePublishPageStore()

const blinkTimer = ref<ReturnType<typeof setTimeout>>()
const blink = ref(false)

async function publish() {
    if (!publishPageStore.confirmLicense) {
        blinkTimer.value = undefined
        blink.value = true
        blinkTimer.value = setTimeout(() => {
            blink.value = false
        }, 2000)
        return
    }
    publishPageStore.publish()
}

</script>

<template>
    <LazyModal :show="publishPageStore.showModal" primary-btn-label="Veröffentlichen" v-if="publishPageStore.showModal"
        @close="publishPageStore.showModal = false" @primary-btn="publish()"
        @keydown.esc="publishPageStore.showModal = false" :disabled="!publishPageStore.confirmLicense"
        :show-cancel-btn="true">

        <template v-slot:header>
            <h4>{{ publishPageStore.name }} veröffentlichen</h4>
        </template>

        <template v-slot:body>
            <div class="subHeader">
                Öffentliche Inhalte sind für alle auffindbar und können frei weiterverwendet werden. <br />
                Du veröffentlichst unter Creative-Commons-Lizenz.
            </div>
            <div class="checkbox-container"
                @click="publishPageStore.includeQuestionsToPublish = !publishPageStore.includeQuestionsToPublish"
                v-if="publishPageStore.questionCount > 0">
                <div class="checkbox-icon">
                    <font-awesome-icon icon="fa-solid fa-square-check" v-if="publishPageStore.includeQuestionsToPublish" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else />
                </div>
                <div class="checkbox-label">
                    Möchtest Du {{ publishPageStore.questionCount }} private Fragen veröffentlichen?
                </div>

            </div>
            <div class="checkbox-container license-info"
                @click="publishPageStore.confirmLicense = !publishPageStore.confirmLicense">
                <div class="checkbox-icon" :class="{ blink: blink }">
                    <font-awesome-icon icon="fa-solid fa-square-check" v-if="publishPageStore.confirmLicense" />
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

    </LazyModal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.subHeader {
    line-height: 18px;
    margin-bottom: 16px;
}

.checkbox-container {
    cursor: pointer;
    padding: 16px;
    display: flex;
    justify-content: flex-start;

    .checkbox-icon {
        font-size: 24px;

        .fa-check-square {
            color: @memo-blue-link;
        }
    }

    .checkbox-label {
        padding-top: 4px;
        line-height: 20px;
        padding-left: 10px;
    }

    &.license-info {
        background-color: @background-grey;
        user-select: none;

        .checkbox-label {
            line-height: 18px;
            font-size: 12px;
        }

        margin-bottom: 40px;

        .blink {
            animation: blinker 1s linear infinite;
        }

        @keyframes blinker {
            50% {
                opacity: 0;
            }
        }
    }
}
</style>