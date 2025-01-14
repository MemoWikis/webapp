<script lang="ts" setup>
import { messages } from '~/components/alert/messages'
import { GeneratedFlashCard, usePageStore } from '../../pageStore'
import { useLearningSessionStore } from '~/components/page/learning/learningSessionStore'
import { useLearningSessionConfigurationStore } from '~/components/page/learning/learningSessionConfigurationStore'
import { SnackbarData, useSnackbarStore } from '~/components/snackBar/snackBarStore'

const pageStore = usePageStore()
const learningSessionStore = useLearningSessionStore()
const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const snackbarStore = useSnackbarStore()

const show = ref(false)
const acceptFlashCards = async () => {
    interface Result {
        success: boolean
        ids?: number[]
        messageKey?: string
        lastIndex?: number
    }

    var sessionConfig = learningSessionConfigurationStore.buildSessionConfigJson(pageStore.id)

    const result = await $api<Result>(`/apiVue/AiCreateFlashCard/Create/`, {
        method: 'POST',
        body: {
            pageId: pageStore.id,
            flashCards: flashcards.value,
            lastIndex: learningSessionStore.lastIndexInQuestionList,
            sessionConfig: sessionConfig
        },
        mode: 'cors',
        credentials: 'include',
    })

    if (result.success && result.ids) {
        pageStore.updateQuestionCount()
        if (result.lastIndex) {

            const startIndex = learningSessionStore.lastIndexInQuestionList + 1
            learningSessionStore.lastIndexInQuestionList = result.lastIndex
            learningSessionStore.getLastStepInQuestionList()
            learningSessionStore.addNewQuestionsToList(startIndex, learningSessionStore.lastIndexInQuestionList)

            const data: SnackbarData = {
                type: 'success',
                text: messages.success.question.flashcardsAdded(result.lastIndex - startIndex + 1),
                dismissible: true
            }
            snackbarStore.showSnackbar(data)
        }
    } else if (result.messageKey) {
        const data: SnackbarData = {
            type: 'error',
            text: messages.getByCompositeKey(result.messageKey),
            dismissible: true
        }
        snackbarStore.showSnackbar(data)
    }

    show.value = false
}

const flashcards = ref<GeneratedFlashCard[]>([])
const message = ref('')

pageStore.$onAction(({ name, after }) => {
    if (name === 'generateFlashCard') {
        after((result) => {
            if (result.flashcards.length > 0) {
                show.value = true
                flashcards.value = result.flashcards
                if (result.messageKey) {
                    message.value = messages.getByCompositeKey(result.messageKey)
                }
            } else if (result.messageKey) {
                const data: SnackbarData = {
                    type: 'error',
                    text: messages.getByCompositeKey(result.messageKey),
                    dismissible: true
                }
                snackbarStore.showSnackbar(data)
            }
        })
    }
    if (name === 'reGenerateFlashCard') {
        after((result) => {
            if (result.flashcards.length > 0) {
                flashcards.value = result.flashcards
                if (result.messageKey) {
                    message.value = messages.getByCompositeKey(result.messageKey)
                }
            } else if (result.messageKey) {
                const data: SnackbarData = {
                    type: 'error',
                    text: messages.getByCompositeKey(result.messageKey),
                    dismissible: true
                }
                snackbarStore.showSnackbar(data)
            }
        })
    }
})

const deleteFlashcard = (index: number) => {
    flashcards.value.splice(index, 1)
}

</script>


<template>
    <Modal :show="show" @close="show = false" @primary-btn="acceptFlashCards" :show-cancel-btn="true" :primary-btn-label="'Karteikarte erstellen'" content-class="wide-modal" :fullscreen="false" container-class="wide-modal"
        :show-close-button="true" :disabled="flashcards.length === 0">
        <template #body>
            <div v-if="message" class="alert alert-info">{{ message }}</div>
            <div id="AiFlashCard">
                <PageLearningAiFlashCard v-for="(flashcard, i) in flashcards" :flash-card="flashcard" :index="i" @delete-flashcard="deleteFlashcard" />
                <div v-if="flashcards.length === 0" class="no-flashcards">
                    <p>Oops! Du hast keine Karteikarten.</p>
                </div>
            </div>
        </template>
    </Modal>
</template>


<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#AiFlashCard {}
</style>
<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

#AiFlashCard {
    margin-top: 36px;
    padding: 0 24px;

    .ProseMirror {
        text-align: center;
        border: 1px solid @memo-grey-light;
        padding: 24px;
        min-height: 240px;
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
        box-shadow: 0 2px 6px rgba(0, 0, 0, 0.16);

        &.ProseMirror-focused {
            border: 1px @memo-grey-light solid;
        }

        ul {
            width: 100%;
        }
    }

    .no-flashcards {
        text-align: center;
        padding: 24px;
        color: @memo-grey;
    }

}
</style>
