<script lang="ts" setup>
import { useSetTopicToPrivateStore } from './setTopicToPrivateStore'
import { useUserStore } from '~~/components/user/userStore'
const setTopicToPrivateStore = useSetTopicToPrivateStore()
const userStore = useUserStore()
</script>

<template>
    <LazyModal>
        <template slot:header>
            <h4 class="modal-title">Thema {{ setTopicToPrivateStore.name }} auf privat setzen</h4>
        </template>

        <template slot:body>
            <div class="subHeader">
                Der Inhalt kann nur von Dir genutzt werden. Niemand sonst kann ihn sehen oder nutzen.
            </div>
            <div class="checkbox-container"
                @click="setTopicToPrivateStore.questionsToPrivate = !setTopicToPrivateStore.questionsToPrivate"
                v-if="setTopicToPrivateStore.personalQuestionCount > 0">
                <div class="checkbox-icon">
                    <font-awesome-icon icon="fa-solid fa-square-check"
                        v-if="setTopicToPrivateStore.questionsToPrivate" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else />
                </div>
                <div class="checkbox-label">
                    Möchtest Du {{ setTopicToPrivateStore.personalQuestionCount }} von {{
                            setTopicToPrivateStore.allQuestionCount
                    }} öffentliche
                    Frage{{ setTopicToPrivateStore.personalQuestionCount > 0 ? 'n' : '' }} ebenfalls auf privat stellen?
                    (Du kannst nur deine
                    eigenen Fragen auf privat stellen.)
                </div>

            </div>

            <div class="checkbox-container"
                @click="setTopicToPrivateStore.allQuestionsToPrivate = !setTopicToPrivateStore.allQuestionsToPrivate"
                v-if="setTopicToPrivateStore.allQuestionCount > 0 && userStore.isAdmin">
                <div class="checkbox-icon">
                    <font-awesome-icon icon="fa-solid fa-square-check"
                        v-if="setTopicToPrivateStore.allQuestionsToPrivate" />
                    <font-awesome-icon icon="fa-regular fa-square" v-else />
                </div>
                <div class="checkbox-label">
                    Möchtest Du {{ setTopicToPrivateStore.allQuestionCount }} öffentliche Frage{{
                            setTopicToPrivateStore.allQuestionCount > 0 ? 'n' : ''
                    }} ebenfalls
                    auf
                    privat stellen? (Admin)
                </div>

            </div>
        </template>

        <template slot:footer>
            <div class="btn btn-link" data-dismiss="modal" aria-label="Close">abbrechen</div>
            <div class="btn btn-primary" id="SetCategoryToPrivateBtn" @click="setTopicToPrivateStore.setToPrivate()">
                Thema auf Privat
                setzen</div>
        </template>
    </LazyModal>
</template>