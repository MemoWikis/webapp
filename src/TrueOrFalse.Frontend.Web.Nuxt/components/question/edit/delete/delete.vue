<script lang="ts" setup>
import { useAlertStore, messages, AlertType } from '~~/components/alert/alertStore'
import { useLearningSessionStore } from '~~/components/topic/learning/learningSessionStore'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { useDeleteQuestionStore } from './deleteQuestionStore'
import { useTopicStore } from '~~/components/topic/topicStore'

const showDeleteInfo = ref(true)

const errorMsg = ref('')
const showErrorMsg = ref(false)
const showDeleteBtn = ref(false)

const name = ref('')
const alertStore = useAlertStore()

interface DeleteDetails {
    questionTitle: string
    totalAnswers: number
    canNotBeDeleted: boolean
    wuwiCount: number
    hasRights: boolean
}
const { $logger } = useNuxtApp()

async function getDeleteDetails(id: number) {

    var result = await $fetch<DeleteDetails>(`/apiVue/QuestionEditDelete/DeleteDetails?questionId=${id}`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])

        }
    })

    if (result) {
        name.value = result.questionTitle
        if (result.canNotBeDeleted) {
            if (result.wuwiCount > 0)
                errorMsg.value = messages.error.question.isInWuwi(result.wuwiCount)
            else
                errorMsg.value = messages.error.question.rights

            showDeleteInfo.value = false
            showErrorMsg.value = false
        } else
            showDeleteBtn.value = true
    } else
        alertStore.openAlert(AlertType.Error, { text: errorMsg.value })

}

const deletionInProgress = ref(false)
const learningSessionStore = useLearningSessionStore()
const spinnerStore = useSpinnerStore()
const deleteQuestionStore = useDeleteQuestionStore()
const topicStore = useTopicStore()

async function deleteQuestion() {
    deletionInProgress.value = true
    showDeleteBtn.value = false
    spinnerStore.showSpinner()

    var data = {
        questionId: deleteQuestionStore.id,
        sessionIndex: learningSessionStore.currentIndex
    }

    var result = await $fetch<any>('/apVue/DeleteQuestion/Delete', {
        method: 'POST',
        body: data,
        credentials: 'include',
        mode: 'cors',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])

        }
    })

    if (result) {
        spinnerStore.hideSpinner()
        deletionInProgress.value = false
        deleteQuestionStore.questionDeleted(result.questionId, result.sessionIndex - 1)
        topicStore.questionCount--
        alertStore.openAlert(AlertType.Success, { text: messages.success.question.delete })
        deleteQuestionStore.showModal = false
    } else {
        spinnerStore.hideSpinner()
        deletionInProgress.value = false
        showDeleteBtn.value = false
        showErrorMsg.value = true
        errorMsg.value = messages.error.question.errorOnDelete
    }
}

watch(() => deleteQuestionStore.showModal, (val) => {
    if (val) {
        getDeleteDetails(deleteQuestionStore.id)
    }
})
</script>

<template>
    <LazyModal :show-close-button="true" :primary-btn-label="!deletionInProgress ? 'Frage löschen' : ''"
        :is-full-size-buttons="false" :show-cancel-btn="!deletionInProgress" @close="deleteQuestionStore.showModal = false"
        @primary-btn="deleteQuestion()" :show="deleteQuestionStore.showModal">
        <template v-slot:header>
            <h4 class="modal-title">Frage löschen</h4>
        </template>
        <template v-slot:body>

            <div class="cardModalContent">
                <div class="modalBody">
                    <div class="body-m" v-if="showDeleteInfo">Möchtest Du "{{ name }}" unwiederbringlich löschen?
                        Alle damit verknüpften Daten werden entfernt!</div>
                    <div class="alert alert-danger" v-if="showErrorMsg">{{ errorMsg }}</div>
                    <div class="alert alert-info" v-if="deletionInProgress">Die Frage wird gelöscht... Bitte
                        habe einen Moment Geduld.</div>
                </div>
            </div>
        </template>

        <template v-slot:footer> </template>
    </LazyModal>
</template>

<style lang="less" scoped></style>