<!-- <script lang="ts" setup>
import { Visibility } from '../shared/visibilityEnum'
import { useUserStore } from '../user/userStore'
import { useEditQuestionStore, SolutionType } from './editQuestionStore'
import { useEditor, EditorContent } from '@tiptap/vue-3'
import StarterKit from '@tiptap/starter-kit'
import Link from '@tiptap/extension-link'
import Placeholder from '@tiptap/extension-placeholder'
import Underline from '@tiptap/extension-underline'
import Image from '@tiptap/extension-image'
import CodeBlockLowlight from '@tiptap/extension-code-block-lowlight'
import Blockquote from '@tiptap/extension-blockquote'
import { lowlight } from 'lowlight/lib/core'
import { AlertType, useAlertStore, AlertMsg, messages } from '../alert/alertStore'
import _ from 'underscore'

const userStore = useUserStore()
const editQuestionStore = useEditQuestionStore()
const edit = ref(false)
const visibility = ref(Visibility.All)
const solutionType = ref(SolutionType.Text)
const addToWuwi = ref(true)
const alertStore = useAlertStore()

const highlightEmptyFields = ref(false)

const questionJson = ref({})
const questionHtml = ref('')

const questionEditor = useEditor({
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
            placeholder: 'Gib den Fragetext ein',
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
            let eventContent = event.content.content;
            if (eventContent.length >= 1 && !_.isEmpty(eventContent[0].attrs)) {
                let src = eventContent[0].attrs.src;
                if (src.length > 1048576 && src.startsWith('data:image')) {
                    alertStore.openAlert(AlertType.Error, { text: messages.error.image.tooBig })
                    return true;
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
    },
})

const showQuestionExtension = ref(false)

function save() {

}
</script>

<template>
    <div id="EditQuestionModal">
        <LazyModal :showCloseButton="true" :modalWidth="600" button1Text="Speichern" :isFullSizeButtons="true"
            @close="editQuestionStore.showModal = false" @mainBtn="save()" :show="editQuestionStore.showModal">
            <template slot:header></template>
            <template slot:body>
                <div class="edit-question-modal-header overline-m overline-title">

                    <div class="main-header">
                        <div class="add-inline-question-label main-label">
                            <template v-if="edit">Frage bearbeiten</template>
                            <template v-else>Frage erstellen</template>
                            <font-awesome-icon v-if="visibility == Visibility.All" icon="fa-solid fa-lock" />
                        </div>

                        <div class="solutionType-selector">
                            <select v-if="!edit" v-model="solutionType">
                                <option :value="SolutionType.Text">Text</option>
                                <option :value="SolutionType.MultipleChoice">MultipleChoice</option>
                                <option :value="SolutionType.MatchList">Zuordnung (Liste)</option>
                                <option :value="SolutionType.FlashCard">Karteikarte</option>
                            </select>
                        </div>
                    </div>

                    <div class="heart-container wuwi-red" @click="addToWuwi = !addToWuwi" v-if="!edit">
                        <div>
                            <font-awesome-icon v-if="addToWuwi" icon="fa-solid fa-heart" />
                            <font-awesome-icon v-else icon="fa-regular fa-heart" />
                        </div>
                    </div>
                </div>
                <div class="inline-question-editor">

                    <div class="input-container">
                        <div class="overline-s no-line">Frage</div>

                        <div v-if="questionEditor">
                            <EditorMenuBar :editor="questionEditor" />
                            <editor-content :editor="questionEditor"
                                :class="{ 'is-empty': highlightEmptyFields && questionEditor.state.doc.textContent.length <= 0 }" />
                            <div v-if="highlightEmptyFields && questionEditor.state.doc.textContent.length <= 0"
                                class="field-error">Bitte formuliere eine Frage.</div>
                        </div>
                    </div>

                    <div class="input-container" v-if="solutionType != 9">
                        <div class="overline-s no-line">Ergänzungen zur Frage</div>
                        <div v-if="showQuestionExtension && questionExtensionEditor">
                            <EditorMenuBar :editor="questionExtensionEditor" />
                            <editor-content :editor="questionExtensionEditor" />

                        </div>
                        <template v-else>
                            <div class="d-flex">
                                <div class="btn grey-bg form-control col-md-6" @click="showQuestionExtension = true">
                                    Ergänzungen hinzufügen</div>
                                <div class="col-sm-12 hidden-xs"></div>
                            </div>
                        </template>
                    </div>
                    <template v-if="solutionType == 1">
                        <textsolution-component :solution="textSolution"
                            :highlight-empty-fields="highlightEmptyFields" />
                    </template>
                    <template v-if="solutionType == 7">
                        <multiplechoice-component :solution="multipleChoiceJson"
                            :highlight-empty-fields="highlightEmptyFields" />
                    </template>
                    <template v-if="solutionType == 8">
                        <matchlist-component :solution="matchListJson" :highlight-empty-fields="highlightEmptyFields" />
                    </template>
                    <template v-if="solutionType == 9">
                        <flashcard-component :solution="flashCardJson" :highlight-empty-fields="highlightEmptyFields" />
                    </template>

                    <div class="input-container description-container">
                        <div class="overline-s no-line">Ergänzungen zur Antwort</div>
                        <div v-if="showDescription && descriptionEditor">
                            <template>
                                <editor-menu-bar-component :editor="descriptionEditor" />
                            </template>
                            <template>
                                <editor-content :editor="descriptionEditor" />
                            </template>
                        </div>
                        <template v-else>
                            <div class="d-flex">
                                <div class="btn grey-bg form-control col-md-6" @click="showDescription = true">
                                    Ergänzungen hinzufügen</div>
                                <div class="col-sm-12 hidden-xs"></div>
                            </div>
                        </template>
                    </div>
                    <div class="input-container">
                        <div class="overline-s no-line">Themenzuordnung</div>
                        <form class="" v-on:submit.prevent>
                            <div class="form-group dropdown categorySearchAutocomplete"
                                :class="{ 'open': showDropdown }">
                                <div class="related-categories-container">
                                    <categorychip-component v-for="(category, index) in selectedCategories" :key="index"
                                        :category="category" :index="index"
                                        v-on:remove-category-chip="removeCategory" />
                                </div>
                                <input ref="searchInput" class="form-control dropdown-toggle" type="text"
                                    v-model="searchTerm" id="questionCategoriesList" autocomplete="off"
                                    @click="lockDropdown = false" aria-haspopup="true"
                                    placeholder="Bitte gib den Namen des Themas ein" />
                                <ul class="dropdown-menu" aria-labelledby="questionCategoriesList">
                                    <li class="searchResultItem" v-for="c in categories" @click="selectCategory(c)"
                                        data-toggle="tooltip" data-placement="top" :title="c.Name">
                                        <img :src="c.ImageUrl" />
                                        <div>
                                            <div class="searchResultLabel body-m">{{ c.Name }}</div>
                                            <div class="searchResultQuestionCount body-s">{{ c.QuestionCount }}
                                                Frage<template v-if="c.QuestionCount != 1">n</template></div>
                                        </div>
                                    </li>
                                    <li class="dropdownFooter body-m">
                                        <b>{{ totalCount }}</b> Treffer. <br />
                                        Deins ist nicht dabei? <span class="dropdownLink"
                                            @click="createCategory = true">Erstelle hier dein Thema</span>
                                    </li>
                                </ul>
                            </div>

                        </form>
                    </div>
                    <div class="input-container">
                        <div class="overline-s no-line">
                            Sichtbarkeit
                        </div>
                        <div class="privacy-selector"
                            :class="{ 'not-selected': !licenseIsValid && highlightEmptyFields }">
                            <div class="checkbox-container">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" v-model="isPrivate" :value="1"> Private Frage <i
                                            class="fas fa-lock show-tooltip tooltip-min-200" title=""
                                            data-placement="top" data-html="true" data-original-title="
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
                        <div v-if="!licenseIsValid && highlightEmptyFields"></div>
                    </div>
                </div>
                <div v-if="userStore.isAdmin">
                    <select v-model="licenseId">
                        <option value="0">Keine Lizenz</option>
                        <option value="1">CC BY 4.0</option>
                        <option value="2">Amtliches Werk BAMF</option>
                        <option value="3">ELWIS</option>
                        <option value="4">BLAC</option>
                    </select>
                </div>
            </template>
            <template slot:footer-text></template>


        </LazyModal>
    </div>

</template> -->