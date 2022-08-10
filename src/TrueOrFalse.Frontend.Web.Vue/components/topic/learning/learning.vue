<script lang="ts" setup>
import { ref } from 'vue'
import { useSessionConfigurationStore } from './sessionConfigurationStore'

const sessionConfigurationStore = useSessionConfigurationStore()
const expandAllQuestions = ref(false)
const showFilter = ref(false)
</script>

<template>

    <TopicLearningSessionConfiguration v-show="showFilter">
        <slot>
            <input id="hdnIsTestMode" hidden :value="sessionConfigurationStore.isTestMode" />
            <div class="col-xs-12 drop-down-question-sort">
                <div class="session-config-header">
                    <span class="hidden-xs">Du lernst</span>
                    <template
                        v-if="sessionConfigurationStore.currentQuestionCount == sessionConfigurationStore.allQuestionCount">
                        <b>&nbsp;alle&nbsp;</b>
                    </template>
                    <template v-else>
                        <b>&nbsp;{{ sessionConfigurationStore.currentQuestionCount }}&nbsp;</b>
                    </template>
                    Fragen&nbsp;
                    <span class="hidden-xs">aus diesem Thema</span>
                    &nbsp;({{ sessionConfigurationStore.allQuestionCount }})
                </div>
                <div class="session-config-header"
                    v-if="sessionConfigurationStore.categoryHasNoQuestions && sessionConfigurationStore.showError">
                    Leider
                    hat dieses Thema noch keine
                    Fragen, erstelle oder füge eine Frage hinzu.</div>

                <div id="ButtonAndDropdown">
                    <div id="QuestionListHeaderDropDown" class="Button dropdown">
                        <a href="#" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button"
                            data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <i class="fa fa-ellipsis-v"></i>
                        </a>
                        <ul class="dropdown-menu dropdown-menu-right standard-question-drop-down">
                            <li onclick="eventBus.$emit('open-edit-question-modal', { categoryId: <%= Model.CategoryId %>, edit: false })"
                                data-allowed="logged-in">
                                <a>
                                    <div class="dropdown-icon">
                                        <i class="fa fa-plus-circle"></i>
                                    </div><span>Frage hinzufügen</span>
                                </a>
                            </li>
                            <li @click="expandAllQuestions = !expandAllQuestions" style="cursor: pointer">
                                <a>
                                    <div class="dropdown-icon">
                                        <font-awesome-icon v-if="expandAllQuestions" icon="fa-solid fa-angles-up" />
                                        <font-awesome-icon v-else icon="fa-solid fa-angles-down" />
                                    </div>
                                    <span>{{ expandAllQuestions ? 'Alle Fragen zuklappen' : 'Alle Fragen erweitern'
                                    }}</span>
                                </a>
                            </li>
                            <li style="cursor: pointer">
                                <a data-allowed="logged-in" @click="sessionConfigurationStore.startNewSession()">
                                    <div class="dropdown-icon">
                                        <i class="fa fa-play"></i>
                                    </div><span>Fragen jetzt lernen</span>
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>
            </div>
        </slot>

    </TopicLearningSessionConfiguration>
    <div class="session-configurator col-xs-12" v-if="!showFilter">
        <div class="session-config-header">
            <div class="col-xs-12 drop-down-question-sort">
                <div class="session-config-header">
                    Leider hat dieses Thema noch keine Fragen, erstelle oder füge eine Frage hinzu.
                </div>
            </div>
        </div>
    </div>

</template>