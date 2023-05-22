<script lang="ts" setup>
import { useTopicToPrivateStore } from './topicToPrivateStore'
import { useUserStore } from '~~/components/user/userStore'
const topicToPrivateStore = useTopicToPrivateStore()
const userStore = useUserStore()
</script>

<template>
    <LazyModal :show="topicToPrivateStore.showModal" :show-cancel-btn="true" v-if="topicToPrivateStore.showModal"
        @primary-btn="topicToPrivateStore.setToPrivate()" primary-btn-label="Thema auf Privat setzen"
        @close="topicToPrivateStore.showModal = false" @keydown.esc="topicToPrivateStore.showModal = false">
        <template v-slot:header>
            <h4>Thema {{ topicToPrivateStore.name }} auf privat setzen</h4>
        </template>

        <template v-slot:body>
            <div class="subHeader">
                Der Inhalt kann nur von Dir genutzt werden. Niemand sonst kann ihn sehen oder nutzen.
            </div>
            <div class="checkbox-container"
                @click="topicToPrivateStore.questionsToPrivate = !topicToPrivateStore.questionsToPrivate"
                v-if="topicToPrivateStore.personalQuestionCount > 0">
                <div class="checkbox-icon">
                    <font-awesome-icon icon="fa-solid fa-square-check" v-if="topicToPrivateStore.questionsToPrivate" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else />
                </div>
                <div class="checkbox-label">
                    Möchtest Du {{ topicToPrivateStore.personalQuestionCount }} von {{
                        topicToPrivateStore.allQuestionCount
                    }} öffentliche
                    Frage{{ topicToPrivateStore.personalQuestionCount > 0 ? 'n' : '' }} ebenfalls auf privat stellen?
                    (Du kannst nur deine
                    eigenen Fragen auf privat stellen.)
                </div>

            </div>

            <div class="checkbox-container"
                @click="topicToPrivateStore.allQuestionsToPrivate = !topicToPrivateStore.allQuestionsToPrivate"
                v-if="topicToPrivateStore.allQuestionCount > 0 && userStore.isAdmin">
                <div class="checkbox-icon">
                    <font-awesome-icon icon="fa-solid fa-square-check" v-if="topicToPrivateStore.allQuestionsToPrivate" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else />
                </div>
                <div class="checkbox-label">
                    Möchtest Du {{ topicToPrivateStore.allQuestionCount }} öffentliche Frage{{
                        topicToPrivateStore.allQuestionCount > 0 ? 'n' : ''
                    }} ebenfalls
                    auf
                    privat stellen? (Admin)
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