<script lang="ts" setup>
import { useAlertStore, AlertType } from '~~/components/alert/alertStore'
import { useLearningSessionStore } from '~/components/page/learning/learningSessionStore'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { useDeleteQuestionStore } from './deleteQuestionStore'
import { usePageStore } from '~/components/page/pageStore'

const { t } = useI18n()

const showDeleteInfo = ref(false)

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

    showErrorMsg.value = false

    var result = await $api<DeleteDetails>(`/apiVue/QuestionEditDelete/DeleteDetails/${id}`, {
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
                errorMsg.value = t('error.question.isInWishknowledge', result.wuwiCount)
            else
                errorMsg.value = t('error.question.rights')
            showErrorMsg.value = true
        } else {
            showDeleteInfo.value = true
            showDeleteBtn.value = true

        }
    } else
        alertStore.openAlert(AlertType.Error, { text: errorMsg.value })

}

const deletionInProgress = ref(false)
const learningSessionStore = useLearningSessionStore()
const loadingStore = useLoadingStore()
const deleteQuestionStore = useDeleteQuestionStore()
const pageStore = usePageStore()

async function deleteQuestion() {
    deletionInProgress.value = true
    showDeleteBtn.value = false
    loadingStore.startLoading()
    showDeleteInfo.value = false

    const result = await $api<{ id: number, sessionIndex: number, reloadAnswerBody: boolean }>(`/apiVue/QuestionEditDelete/Delete/${deleteQuestionStore.id}`, {
        method: 'POST',
        credentials: 'include',
        mode: 'cors',
        onResponseError(context) {
            loadingStore.stopLoading()
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        }
    })
    loadingStore.stopLoading()

    if (result) {
        deletionInProgress.value = false
        deleteQuestionStore.questionDeleted(result.id)
        if (result.reloadAnswerBody)
            learningSessionStore.changeActiveQuestion(result.sessionIndex)
        pageStore.questionCount--
        alertStore.openAlert(AlertType.Success, { text: t('success.question.delete') })
        deleteQuestionStore.showModal = false
    } else {
        deletionInProgress.value = false
        showDeleteBtn.value = false
        showErrorMsg.value = true
        errorMsg.value = t('error.question.errorOnDelete')
    }
}

watch(() => deleteQuestionStore.showModal, (val) => {
    if (val) {
        getDeleteDetails(deleteQuestionStore.id)
    }
})

</script>

<template>
    <LazyModal :show-close-button="true"
        :primary-btn-label="!deletionInProgress && showDeleteBtn ? t('question.deleteModal.button.delete') : ''" :is-full-size-buttons="false"
        :show-cancel-btn="!deletionInProgress" @close="deleteQuestionStore.showModal = false"
        @primary-btn="deleteQuestion()" :show="deleteQuestionStore.showModal">
        <template v-slot:header>
            <h2 class="modal-title">{{ t('question.deleteModal.title') }}</h2>
        </template>
        <template v-slot:body>
            <div class="cardModalContent">
                <div class="modalBody">
                    <div class="body-m" v-if="showDeleteInfo">
                        <i18n-t keypath="question.deleteModal.confirmation" tag="p">
                            <template #name>
                                <b>{{ name }}</b>
                            </template>
                        </i18n-t>
                    </div>
                    <div class="alert alert-danger" v-if="showErrorMsg">
                        {{ errorMsg }}
                    </div>
                    <div class="alert alert-info" v-if="deletionInProgress">
                        {{ t('question.deleteModal.progress') }}
                    </div>
                </div>
            </div>
        </template>

        <template v-slot:footer></template>
    </LazyModal>
</template>

<style lang="less" scoped></style>