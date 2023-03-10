<script lang="ts" setup>
import { useTopicToPrivateStore } from './topicToPrivateStore'
import { useUserStore } from '~~/components/user/userStore'
const topicToPrivateStore = useTopicToPrivateStore()
const userStore = useUserStore()
</script>

<template>
    <!-- <LazyModal>
        <template slot:header>
            <h4 class="modal-title">Thema {{ topicToPrivateStore.name }} auf privat setzen</h4>
        </template>

        <template slot:body>
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
                    <font-awesome-icon icon="fa-solid fa-square-check"
                        v-if="topicToPrivateStore.allQuestionsToPrivate" />
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

        <template slot:footer>
            <div class="btn btn-link" data-dismiss="modal" aria-label="Close">abbrechen</div>
            <div class="btn btn-primary" id="SetCategoryToPrivateBtn" @click="topicToPrivateStore.setToPrivate()">
                Thema auf Privat
                setzen</div>
        </template>
    </LazyModal> -->
</template>