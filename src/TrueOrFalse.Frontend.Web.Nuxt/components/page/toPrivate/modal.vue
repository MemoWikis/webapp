<script lang="ts" setup>
import { usePageToPrivateStore } from './pageToPrivateStore'
import { useUserStore } from '~~/components/user/userStore'
const pageToPrivateStore = usePageToPrivateStore()
const userStore = useUserStore()
</script>

<template>
    <LazyModal :show="pageToPrivateStore.showModal" :show-cancel-btn="true" v-if="pageToPrivateStore.showModal"
        @primary-btn="pageToPrivateStore.setToPrivate()" primary-btn-label="Seite auf Privat setzen"
        @close="pageToPrivateStore.showModal = false" @keydown.esc="pageToPrivateStore.showModal = false">
        <template v-slot:header>
            <h4>{{ pageToPrivateStore.name }} auf privat setzen</h4>
        </template>

        <template v-slot:body>
            <div class="subHeader">
                Der Inhalt kann nur von Dir genutzt werden. Niemand sonst kann ihn sehen oder nutzen.
            </div>
            <div class="checkbox-container"
                @click="pageToPrivateStore.questionsToPrivate = !pageToPrivateStore.questionsToPrivate"
                v-if="pageToPrivateStore.personalQuestionCount > 0">
                <div class="checkbox-icon">
                    <font-awesome-icon icon="fa-solid fa-square-check" v-if="pageToPrivateStore.questionsToPrivate" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else />
                </div>
                <div class="checkbox-label">
                    Möchtest Du {{ pageToPrivateStore.personalQuestionCount }} von {{
                        pageToPrivateStore.allQuestionCount
                    }} öffentliche
                    Frage{{ pageToPrivateStore.personalQuestionCount > 0 ? 'n' : '' }} ebenfalls auf privat stellen?
                    (Du kannst nur deine
                    eigenen Fragen auf privat stellen.)
                </div>

            </div>

            <div class="checkbox-container"
                @click="pageToPrivateStore.allQuestionsToPrivate = !pageToPrivateStore.allQuestionsToPrivate"
                v-if="pageToPrivateStore.allQuestionCount > 0 && userStore.isAdmin">
                <div class="checkbox-icon">
                    <font-awesome-icon icon="fa-solid fa-square-check" v-if="pageToPrivateStore.allQuestionsToPrivate" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else />
                </div>
                <div class="checkbox-label">
                    Möchtest Du {{ pageToPrivateStore.allQuestionCount }} öffentliche Frage{{
                        pageToPrivateStore.allQuestionCount > 0 ? 'n' : ''
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