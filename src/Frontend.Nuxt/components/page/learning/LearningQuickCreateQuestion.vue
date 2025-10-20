<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import { usePageStore } from '../pageStore'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'
import { useEditor, EditorContent, JSONContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import { all, createLowlight } from 'lowlight'
import { isEmpty } from 'underscore'
import { AlertType, useAlertStore, } from '../../alert/alertStore'
import { useLearningSessionStore } from './learningSessionStore'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'
import { ReplaceStep, ReplaceAroundStep } from 'prosemirror-transform'
import UploadImage from '~/components/shared/imageUploadExtension'
import FigureExtension from '~~/components/shared/figureExtension'
import { useLoadingStore } from '~/components/loading/loadingStore'

const { t } = useI18n()
const highlightEmptyFields = ref(false)

const userStore = useUserStore()
const pageStore = usePageStore()
const editQuestionStore = useEditQuestionStore()
const learningSessionStore = useLearningSessionStore()
const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const loadingStore = useLoadingStore()

const flashCardEditor = ref()
const addToWishKnowledge = ref(true)
const questionJson = ref(null as null | JSONContent)
const questionHtml = ref('')
const flashCardAnswer = ref('')
const flashCardJson = ref('')
const licenseConfirmation = ref(false)
const showMore = ref(false)
const licenseIsValid = ref(false)
const solutionIsValid = ref(false)
const disabled = ref(true)
const isPrivate = ref(true)
const sessionConfigJson = ref(null as null | { [key: string]: any })

const alertStore = useAlertStore()

const lowlight = createLowlight(all)
const deleteImageSrc = ref<string | null>(null)

const editor = useEditor({
    extensions: [
        StarterKit.configure({
            codeBlock: false,
        }),
        Link.configure({
            HTMLAttributes: {
                target: '_self',
                rel: 'noopener noreferrer nofollow'
            }
        }),
        CodeBlockLowlight.configure({
            lowlight,
        }),
        Underline,
        Placeholder.configure({
            emptyEditorClass: 'is-editor-empty',
            emptyNodeClass: 'is-empty',
            placeholder: t('page.questionsSection.quickCreateQuestion.placeholders.question'),
            showOnlyCurrent: true,
        }),
        FigureExtension.configure({
            inline: true,
            allowBase64: true,
        }),
        UploadImage.configure({
            uploadFn: editQuestionStore.uploadContentImage
        })
    ],
    editorProps: {
        handleClick: (view, pos, event) => {
        },
        handlePaste: (view, pos, event) => {
            const eventContent = event.content as any
            const content = eventContent.content
            if (content.length >= 1 && !isEmpty(content[0].attrs)) {
                const src = content[0].attrs.src
                if (src.startsWith('data:image')) {
                    editor.value?.commands.addBase64Image(src)
                    return true
                }
            }
        },
        attributes: {
            id: 'QuickCreateEditor',
        }
    },
    onUpdate: ({ editor }) => {
        questionJson.value = editor.getJSON()
        questionHtml.value = editor.getHTML()
        validateForm()
        checkContentImages()
    },
    onTransaction({ transaction }) {
        let clearDeleteImageSrcRef = true
        const { selection, doc } = transaction

        const node = doc.nodeAt(selection.from)
        if (node && node.type.name === 'uploadImage') {
            deleteImageSrc.value = node.attrs.src
            clearDeleteImageSrcRef = false
        }

        const hasDeleted = transaction.steps.some(step =>
            step instanceof ReplaceStep || step instanceof ReplaceAroundStep
        )

        if (hasDeleted && deleteImageSrc.value)
            editQuestionStore.addImageUrlToDeleteList(deleteImageSrc.value)

        if (clearDeleteImageSrcRef)
            deleteImageSrc.value = null
    },
})

function validateForm() {
    licenseIsValid.value = licenseConfirmation.value || isPrivate.value

    var questionIsValid = editor.value
        ? editor.value.state.doc.textContent.length > 0
        : false

    disabled.value = !questionIsValid || !solutionIsValid.value || !licenseIsValid.value
}

watch([isPrivate, licenseConfirmation, flashCardAnswer], () => {
    validateForm()
})

function createQuestion() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    const question = {
        pageId: pageStore.id,
        questionHtml: questionHtml.value,
        flashCardAnswerHtml: flashCardAnswer.value,
    }
    editQuestionStore.createQuestion(question)
    editor.value?.commands.setContent('')
    flashCardEditor.value?.clearFlashcard()
}

const { $logger } = useNuxtApp()
async function addFlashcard() {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    if (disabled.value) {
        highlightEmptyFields.value = true
        return
    }

    loadingStore.startLoading()
    await editQuestionStore.waitUntilAllUploadsComplete()

    sessionConfigJson.value = learningSessionConfigurationStore.buildSessionConfigJson(pageStore.id)

    const json = {
        PageId: pageStore.id,
        TextHtml: questionHtml.value,
        Answer: flashCardAnswer.value,
        Visibility: isPrivate.value ? 1 : 0,
        AddToWishKnowledge: addToWishKnowledge.value,
        LastIndex: learningSessionStore.lastIndexInQuestionList,
        SessionConfig: sessionConfigJson.value,
        uploadedImagesMarkedForDeletion: editQuestionStore.uploadedImagesMarkedForDeletion,
        uploadedImagesInContent: editQuestionStore.uploadedImagesInContent
    }

    const result = await $api<FetchResult<number>>('/apiVue/QuickCreateQuestion/CreateFlashcard', {
        method: 'POST',
        body: json,
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })
    if (result.success === true) {
        pageStore.questionCount++
        if (result.data < 0) {
            alertStore.openAlert(AlertType.Success, {
                text: t('success.question.created'),
                customHtml: `<div class="session-config-error fade in col-xs-12"><span><b>${t('page.questionsSection.quickCreateQuestion.filterAlert.active')}</b> ${t('page.questionsSection.quickCreateQuestion.filterAlert.notShown')} ${t('page.questionsSection.quickCreateQuestion.filterAlert.resetToShow')}</span></div>`,
                customBtn: `<div class="btn memo-button col-xs-4 btn-link">${t('page.questionsSection.quickCreateQuestion.filterAlert.resetButton')}</div>`,
                customBtnKey: 'resetLearningSessionConfiguration'
            })

            alertStore.$onAction(({ name, after }) => {
                if (name === 'closeAlert') {
                    after((result) => {
                        if (result.cancelled === false && result.customKey === 'resetLearningSessionConfiguration')
                            learningSessionConfigurationStore.reset()
                    })
                }
            })
        }
        else {
            learningSessionStore.lastIndexInQuestionList = result.data
            learningSessionStore.getLastStepInQuestionList()
            learningSessionStore.addNewQuestionToList(learningSessionStore.lastIndexInQuestionList)
        }
    }
    highlightEmptyFields.value = false
    editor.value?.commands.setContent('')
    questionHtml.value = ''
    flashCardAnswer.value = ''
    flashCardEditor.value?.clearFlashcard()
    pageStore.reloadKnowledgeSummary()
    editQuestionStore.uploadedImagesMarkedForDeletion = []
    editQuestionStore.uploadedImagesInContent = []
}

function setFlashcardContent(e: { solution: string, solutionIsValid: boolean }) {
    flashCardAnswer.value = e.solution
    solutionIsValid.value = e.solutionIsValid
}

const checkContentImages = () => {
    if (editor.value == null)
        return

    const { state } = editor.value
    state.doc.descendants((node: any, pos: number) => {
        if (node.type.name === 'uploadImage') {
            const src = node.attrs.src
            if (src.startsWith('/Images/'))
                editQuestionStore.uploadedImagesInContent.push(src)
        }
    })

    editQuestionStore.refreshDeleteImageList()
}
const ariaId = useId()
</script>

<template>
    <div id="AddInlineQuestionContainer">
        <div id="AddQuestionHeader" class="">
            <div class="add-inline-question-label main-label">
                {{ t('page.questionsSection.quickCreateQuestion.title') }}
                <span>({{ t('page.questionsSection.quickCreateQuestion.flashcard') }})</span>
            </div>
            <div class="heart-container wuwi-red" @click="addToWishKnowledge = !addToWishKnowledge">
                <div>
                    <font-awesome-icon icon="fa-solid fa-heart" v-if="addToWishKnowledge" />
                    <font-awesome-icon icon="fa-regular fa-heart" v-else />
                </div>
                <div class="Text">
                    <span v-if="addToWishKnowledge">{{
                        t('page.questionsSection.quickCreateQuestion.wishKnowledge.added') }}</span>
                    <span v-else class="wuwi-grey">{{ t('page.questionsSection.quickCreateQuestion.wishKnowledge.add')
                        }}</span>
                </div>
            </div>
        </div>

        <div id="AddQuestionBody">
            <div id="AddQuestionFormContainer" class="inline-question-editor">
                <div v-if="editor">
                    <div class="overline-s no-line">{{ t('page.questionsSection.quickCreateQuestion.form.question') }}
                    </div>
                    <EditorMenuBar :editor="editor" @handle-undo-redo="checkContentImages" />
                    <editor-content :editor="editor"
                        :class="{ 'is-empty': highlightEmptyFields && editor.state.doc.textContent.length <= 0 }" />
                    <div v-if="highlightEmptyFields && editor.state.doc.textContent.length <= 0" class="field-error">
                        {{ t('page.questionsSection.quickCreateQuestion.validation.questionRequired') }}
                    </div>
                </div>
                <div>
                    <QuestionEditFlashcard :solution="flashCardJson" :highlight-empty-fields="highlightEmptyFields"
                        ref="flashCardEditor" @set-flashcard-content="setFlashcardContent" :is-init="false" />
                </div>
                <div class="input-container">
                    <div class="overline-s no-line">
                        {{ t('page.questionsSection.quickCreateQuestion.form.visibility') }}
                    </div>
                    <div class="privacy-selector" :class="{ 'not-selected': !licenseIsValid && highlightEmptyFields }">
                        <div class="checkbox-container">
                            <div class="checkbox">
                                <label>
                                    <VTooltip :aria-id="ariaId">
                                        <input type="checkbox" v-model="isPrivate" :value="1">
                                        {{ t('page.questionsSection.quickCreateQuestion.visibility.private') }}
                                        <font-awesome-icon icon="fa-solid fa-lock" />
                                        <template #popper>
                                            <ul>
                                                <li>{{
                                                    t('page.questionsSection.quickCreateQuestion.visibility.privateTooltip.onlyYou')
                                                    }}</li>
                                                <li>{{
                                                    t('page.questionsSection.quickCreateQuestion.visibility.privateTooltip.noOneElse')
                                                    }}</li>
                                            </ul>
                                        </template>
                                    </VTooltip>
                                </label>
                            </div>
                        </div>
                        <div class="checkbox-container license-confirmation-box" v-if="!isPrivate">
                            <div class="checkbox">
                                <label>
                                    <input type="checkbox" v-model="licenseConfirmation" value="false">
                                    {{ t('page.questionsSection.quickCreateQuestion.license.short') }}
                                    <span class="btn-link" @click.prevent="showMore = !showMore">
                                        {{ t('page.questionsSection.quickCreateQuestion.license.more') }}
                                    </span>
                                    <template v-if="showMore">
                                        <br />
                                        <br />
                                        {{ t('page.questionsSection.quickCreateQuestion.license.full') }}
                                    </template>
                                </label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="btn-container">
                    <div class="btn btn-lg btn-link memo-button" @click="createQuestion()">
                        {{ t('page.questionsSection.quickCreateQuestion.buttons.advancedOptions') }}
                    </div>
                    <div class="btn btn-lg btn-primary memo-button" @click="addFlashcard()">
                        {{ t('page.questionsSection.quickCreateQuestion.buttons.add') }}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#AddInlineQuestionContainer {
    background: white;
    margin-top: 20px;
    padding: 36px 24px;
    font-size: 16px;
    border-radius: 8px;

    .add-inline-question-label {
        font-weight: 700;

        &.s-label {
            font-size: 14px;
            margin-bottom: 5px;
        }

        span,
        a {
            font-weight: 400;
        }

        a {
            cursor: pointer;
        }
    }

    #AddQuestionHeader {
        display: flex;

        .main-label {
            padding-bottom: 6px;
            flex: 1;
        }

        .heart-container {
            display: flex;
            align-content: center;
            align-items: center;
            flex-direction: column;
            width: 65px;
            margin-right: 8px;
            cursor: pointer;
            font-size: 18px;

            .Text {
                padding: 0 3px !important;
                font-size: 8px !important;
                line-height: 14px !important;
                font-weight: 600 !important;
                text-transform: uppercase !important;
                letter-spacing: 0.1em;
                color: @memo-wuwi-red !important;
            }
        }
    }

    #AddQuestionBody {
        display: flex;

        #AddQuestionFormContainer {
            padding-right: 24px;
            flex: 1;
            width: 100%;

            @media(max-width: @screen-xxs-max) {
                padding-right: 0;
            }

            .ProseMirror {
                padding: 11px 15px 0;
            }

            .overline-s {
                margin-top: 26px;
            }
        }

        #AddQuestionPrivacyContainer {
            padding: 20px;
            border: solid 1px @memo-grey-light;
            border-radius: 4px;
            min-width: 216px;
            max-height: 143px;
            margin-top: 24px;

            .form-check-label,
            form-check-radio {
                cursor: pointer;
            }

            .form-check-label {
                i.fa-lock {
                    font-size: 14px;
                }
            }
        }

        .btn-container {
            display: flex;
            justify-content: flex-end;

            @media(max-width: @screen-xxs-max) {
                justify-content: center;
                flex-wrap: wrap-reverse;
            }
        }
    }

    .wuwi-red {
        color: rgb(255, 0, 31);
    }

    .wuwi-grey {
        color: @memo-grey-dark;
    }
}

:deep(.ProseMirror) {
    border: solid 1px @memo-grey-light;
    border-radius: 0;
    padding: 11px 15px 0;

    &.is-empty {
        border: solid 1px @memo-salmon;
    }
}

:deep(.is-empty) {

    .ProseMirror {
        border: solid 1px @memo-salmon;
    }
}


:deep(.ProseMirror-focused) {

    &:focus,
    &:focus-visible {
        outline: none !important;
        border: solid 1px @memo-green;
    }
}

:deep(.overline-s) {
    margin-top: 26px;
}
</style>