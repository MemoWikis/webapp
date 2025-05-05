<script lang="ts" setup>

import { GeneratedFlashcard, usePageStore } from '../../pageStore'
import { useLearningSessionStore } from '~/components/page/learning/learningSessionStore'
import { useLearningSessionConfigurationStore } from '~/components/page/learning/learningSessionConfigurationStore'
import { SnackbarData, useSnackbarStore } from '~/components/snackBar/snackBarStore'

const pageStore = usePageStore()
const learningSessionStore = useLearningSessionStore()
const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const snackbarStore = useSnackbarStore()
const { t } = useI18n()

const show = ref(false)
const acceptFlashcards = async () => {
    interface Result {
        success: boolean
        ids?: number[]
        messageKey?: string
        lastIndex?: number
    }

    var sessionConfig = learningSessionConfigurationStore.buildSessionConfigJson(pageStore.id)

    const result = await $api<Result>(`/apiVue/AiCreateFlashcard/Create/`, {
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
            if (learningSessionStore.steps == null || learningSessionStore.steps.length === 0) {
                handleLearningSession(0, result.lastIndex)
            } else {
                const startIndex = learningSessionStore.lastIndexInQuestionList + 1
                handleLearningSession(startIndex, result.lastIndex)
            }
        }
    } else if (result.messageKey) {
        const data: SnackbarData = {
            type: 'error',
            text: { message: t(result.messageKey) },
            dismissible: true
        }
        snackbarStore.showSnackbar(data)
    }

    show.value = false
}

const handleLearningSession = (startIndex: number, lastIndex: number) => {
    learningSessionStore.lastIndexInQuestionList = lastIndex
    learningSessionStore.getLastStepInQuestionList()
    learningSessionStore.addNewQuestionsToList(startIndex, learningSessionStore.lastIndexInQuestionList)

    const data: SnackbarData = {
        type: 'success',
        text: { message: t('success.question.flashcardsAdded', lastIndex - startIndex + 1) },
        dismissible: true
    }
    snackbarStore.showSnackbar(data)
}

const flashcards = ref<GeneratedFlashcard[]>([])
const message = ref('')

pageStore.$onAction(({ name, after }) => {
    if (name === 'generateFlashcard') {
        after((result) => {
            if (result.flashcards.length > 0) {
                show.value = true
                flashcards.value = result.flashcards
                if (result.messageKey) {
                    message.value = t(result.messageKey)
                }
            } else if (result.messageKey) {
                const data: SnackbarData = {
                    type: 'error',
                    text: { message: t(result.messageKey) },
                    dismissible: true
                }
                snackbarStore.showSnackbar(data)
            }
        })
    }
    if (name === 'reGenerateFlashcard') {
        after((result) => {
            if (result.flashcards.length > 0) {
                flashcards.value = result.flashcards
                if (result.messageKey) {
                    message.value = t(result.messageKey)
                }
            } else if (result.messageKey) {
                const data: SnackbarData = {
                    type: 'error',
                    text: { message: t(result.messageKey) },
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
    <Modal :show="show" @close="show = false" @primary-btn="acceptFlashcards" :show-cancel-btn="true"
        :primary-btn-label="t('page.ai.flashcard.button.create')" content-class="wide-modal"
        :fullscreen="false" container-class="wide-modal"
        :show-close-button="true" :disabled="flashcards.length === 0">
        <template #body>
            <div v-if="message" class="alert alert-info">{{ message }}</div>
            <div id="AiFlashcard">
                <PageLearningAiFlashcard v-for="(flashcard, i) in flashcards" :flash-card="flashcard" :index="i"
                    @delete-flashcard="deleteFlashcard" />
                <div v-if="flashcards.length === 0" class="no-flashcards">
                    <p>{{ t('page.ai.flashcard.message.noFlashcards') }}</p>
                </div>
            </div>
        </template>
    </Modal>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#AiFlashcard {}
</style>
<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

#AiFlashcard {
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