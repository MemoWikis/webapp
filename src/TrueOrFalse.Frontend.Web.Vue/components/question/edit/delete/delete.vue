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

const showDeleteModal = ref(false)
const alertStore = useAlertStore()

async function getDeleteDetails(id) {

    var result = await $fetch<any>(`/api/Question/DeleteDetails/${id}`, {
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
        headers: useRequestHeaders(['cookie'])
    })

    if (result != null) {
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

    var result = await $fetch<any>('/api/Question/Delete', {
        method: 'POST',
        body: data,
        credentials: 'include'
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
</script>

<template>
    <LazyModal showCloseButton="true" :modalWidth="600" button1Text="Löschen" :isFullSizeButtons="true"
        @close="deleteQuestionStore.showModal = false" @mainBtn="deleteQuestion()"
        :show="deleteQuestionStore.showModal">
        <template slot:header>
            <h4 class="modal-title">Frage löschen</h4>
        </template>
        <template slot:body>

            <div class="cardModalContent">
                <div class="modalHeader">
                    <h4 class="modal-title">Frage löschen</h4>
                </div>
                <div class="modalBody">
                    <div class="body-m" v-if="showDeleteInfo">Möchtest Du "{{name}}" unwiederbringlich löschen?
                        Alle damit verknüpften Daten werden entfernt!</div>
                    <div class="alert alert-danger" v-if="showErrorMsg">{{errorMsg}}</div>
                    <div class="alert alert-info" v-if="deletionInProgress">Die Frage wird gelöscht... Bitte
                        habe einen Moment Geduld.</div>
                </div>
                <div class="modalFooter" v-if="!deletionInProgress">
                    <div @click="deleteQuestion()" class="btn btn-danger memo-button" v-if="showDeleteBtn">Frage
                        Löschen</div>
                    <div class="btn btn-link memo-button" data-dismiss="modal">Abbrechen</div>
                </div>
            </div>
        </template>

        <template slot:footer> </template>
    </LazyModal>
</template>

<style lang="less" scoped>

</style>