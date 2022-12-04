<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import { useTopicStore } from '../topicStore'
import { useEditQuestionStore } from '~~/components/question/edit/editQuestionStore'
import { useEditor, EditorContent, JSONContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import Image from '@tiptap/extension-image'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import Blockquote from '@tiptap/extension-blockquote'
import { lowlight } from 'lowlight/lib/core'
import _ from 'underscore'
import { AlertType, useAlertStore, AlertMsg, messages } from '../../alert/alertStore'
import { useLearningSessionStore } from './learningSessionStore'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'

const highlightEmptyFields = ref(false)

const userStore = useUserStore()

const addToWishknowledge = ref(true)
const questionJson = ref(null as null | JSONContent)
const questionHtml = ref('')
const flashCardAnswer = ref('')
const flashCardJson = ref(null)
const licenseConfirmation = ref(false)
const showMore = ref(false)
const licenseIsValid = ref(false)
const solutionIsValid = ref(false)
const disabled = ref(true)
const isPrivate = ref(true)
const sessionConfigJson = ref(null as null | { [key: string]: any; })

const alertStore = useAlertStore()

const editor = useEditor({
    extensions: [
        StarterKit,
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
            placeholder: 'Ergänzungen zur Frage zB. Bilder, Code usw.',
            showOnlyCurrent: true,
        }),
        Image.configure({
            inline: true,
            allowBase64: true,
        })
    ],
    editorProps: {
        handleClick: (view, pos, event) => {
        },
        handlePaste: (view, pos, event) => {
            let eventContent = event.content as any
            let content = eventContent.content
            if (content.length >= 1 && !_.isEmpty(content[0].attrs)) {
                let src = content[0].attrs.src
                if (src.length > 1048576 && src.startsWith('data:image')) {
                    alertStore.openAlert(AlertType.Error, { text: messages.error.image.tooBig })
                    return true
                }
            }
        },
        attributes: {
            id: 'QuestionInputField',
        }
    },
    onUpdate: ({ editor }) => {
        questionJson.value = editor.getJSON()
        questionHtml.value = editor.getHTML()
        validateForm()
    },
})
function validateForm() {
    licenseIsValid.value = licenseConfirmation.value || isPrivate.value

    var questionIsValid = editor.value ? editor.value.state.doc.textContent.length > 0 : false
    disabled.value = !questionIsValid || !solutionIsValid.value || !licenseIsValid.value
}

const topicStore = useTopicStore()
const editQuestionStore = useEditQuestionStore()
const flashCardEditor = ref()
const learningSessionStore = useLearningSessionStore()
const learningSessionConfigStore = useLearningSessionConfigurationStore()

function createQuestion() {
    var question = {
        topicId: topicStore.id,
        edit: false,
        questionHtml: questionHtml.value,
        solution: flashCardAnswer.value,
    }
    editQuestionStore.createQuestion(question)
    editor.value?.commands.setContent('')
    flashCardEditor.value.clearFlashCard()
}

const emit = defineEmits(['changeActiveQuestion'])

async function addFlashcard() {
    if (!userStore.isLoggedIn) {
        // NotLoggedIn.ShowErrorMsg("CreateFlashCard")
        return
    }
    if (disabled.value) {
        highlightEmptyFields.value = true
        return
    }

    sessionConfigJson.value = learningSessionConfigStore.buildSessionConfigJson(topicStore.id)

    var json = {
        CategoryId: sessionConfigJson.value.id,
        TextHtml: questionHtml.value,
        Answer: flashCardAnswer.value,
        Visibility: isPrivate.value ? 1 : 0,
        AddToWishknowledge: addToWishknowledge.value,
        LastIndex: learningSessionStore.lastIndex,
        SessionConfig: sessionConfigJson.value
    }

    var data = await $fetch<any>('/apiVue/Question/CreateFlashcard', {
        method: 'POST', body: json, mode: 'cors', credentials: 'include'
    })

    if (data != null) {
        if (data.SessionIndex < 0)
            alertStore.openAlert(AlertType.Success, {
                text: messages.success.question.created,
                customHtml: '<div class="session-config-error fade in col-xs-12"><span><b>Der Fragenfilter ist aktiv.</b> Die Frage wird dir nicht angezeigt. Setze den Filter zurück, um alle Fragen anzuzeigen.</span></div>',
                customBtn: '<div class="btn memo-button col-xs-4 btn-link" data-dismiss="modal" onclick="eventBus.$emit(\'reset-session-config\')">Filter zurücksetzen</div>',
            })

        learningSessionStore.loadQuestion(-5, learningSessionStore.lastIndex)

        emit('changeActiveQuestion', learningSessionStore.lastIndex)
        highlightEmptyFields.value = false
        editor.value?.commands.setContent('')
        questionHtml.value = ''
        flashCardAnswer.value = ''
        flashCardEditor.value.clearFlashCard()
        topicStore.questionCount++
    }
}
</script>

<template>
    <div id="AddInlineQuestionContainer">

        <div id="AddQuestionHeader" class="">
            <div class="add-inline-question-label main-label">
                Frage hinzufügen
                <span>(Karteikarte)</span>
            </div>
            <div class="heart-container wuwi-red" @click="addToWishknowledge = !addToWishknowledge">
                <div>
                    <i class="fa fa-heart" v-if="addToWishknowledge"></i>
                    <i class="fa fa-heart-o" v-else></i>
                </div>
                <div class="Text">
                    <span v-if="addToWishknowledge">Hinzugefügt</span>
                    <span v-else class="wuwi-grey">Hinzufügen</span>
                </div>
            </div>
        </div>

        <div id="AddQuestionBody">
            <div id="AddQuestionFormContainer" class="inline-question-editor">
                <div v-if="editor">
                    <div class="overline-s no-line">Frage</div>
                    <EditorMenuBar :editor="editor" />
                    <editor-content :editor="editor"
                        :class="{ 'is-empty': highlightEmptyFields && editor.state.doc.textContent.length <= 0 }" />
                    <div v-if="highlightEmptyFields && editor.state.doc.textContent.length <= 0" class="field-error">
                        Bitte formuliere eine Frage.
                    </div>
                </div>
            </div>
            <div>
                <QuestionEditFlashCard :solution="flashCardJson" :highlight-empty-fields="highlightEmptyFields"
                    :ref="flashCardEditor" />
            </div>
            <div class="input-container">
                <div class="overline-s no-line">
                    Sichtbarkeit
                </div>
                <div class="privacy-selector" :class="{ 'not-selected': !licenseIsValid && highlightEmptyFields }">
                    <div class="checkbox-container">
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" v-model="isPrivate" :value="1"> Private Frage <i
                                    class="fas fa-lock show-tooltip tooltip-min-200" title="" data-placement="top"
                                    data-html="true" data-original-title="
                            <ul class='show-tooltip-ul'>
                                <li>Die Frage kann nur von dir genutzt werden.</li>
                                <li>Niemand sonst kann die Frage sehen oder nutzen.</li>
                            </ul>">
                                </i>
                            </label>
                        </div>
                    </div>
                    <div class="checkbox-container license-confirmation-box" v-if="!isPrivate">
                        <div class="checkbox">
                            <label>
                                <input type="checkbox" v-model="licenseConfirmation" value="false">
                                Dieser Eintrag wird veröffentlicht unter CC BY 4.0. <span class="btn-link"
                                    @click.prevent="showMore = !showMore">mehr</span>
                                <template v-if="showMore">
                                    <br />
                                    <br />
                                    Ich stelle diesen Eintrag unter die Lizenz "Creative Commons -
                                    Namensnennung 4.0 International" (CC BY 4.0, Lizenztext, deutsche
                                    Zusammenfassung).
                                    Der Eintrag kann bei angemessener Namensnennung ohne Einschränkung weiter
                                    genutzt werden.
                                    Die Texte und ggf. Bilder sind meine eigene Arbeit und nicht aus
                                    urheberrechtlich geschützten Quellen kopiert.
                                </template>

                            </label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="btn-container">
                <div class="btn btn-lg btn-link memo-button" @click="createQuestion()">erweiterte Optionen</div>
                <div class="btn btn-lg btn-primary memo-button" @click="addFlashcard()">Hinzufügen</div>
            </div>
        </div>
    </div>

</template>


<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#AddInlineQuestionContainer {
    background: white;
    margin-top: 20px;
    padding: 22px 21px 30px;
    font-size: 16px;
    margin-right: 20px;

    @media(max-width: @screen-xxs-max) {
        margin-right: 0;
    }

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
            width: 100px;
            margin-right: -22px;
            cursor: pointer;

            .Text {
                padding: 0 3px !important;
                font-size: 8px !important;
                line-height: 14px !important;
                font-weight: 600 !important;
                text-transform: uppercase !important;
                letter-spacing: 0.1em;
                color: #FF001F !important;
            }
        }
    }

    #AddQuestionBody {
        display: flex;

        #AddQuestionFormContainer {
            padding-right: 24px;
            flex: 1;

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
</style>