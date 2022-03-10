<div class="session-configurator col-xs-12 ">
    <div class="session-config-header">
        <div class="filter-button selectable-item session-title" @click="showFilterDropdown = !showFilterDropdown" :class="showFilterDropdown ? 'open' : 'closed'">
            Filter
        </div>
        <slot></slot>
    </div>

    <div v-show="showFilterDropdown" class="session-config-dropdown row">
        <div class="dropdown-container col-xs-12 col-sm-6 col-md-3">
            <div class="sub-header">Fragen</div>
            <div class="question-filter-options-button selectable-item" @click="showQuestionFilterOptionsDropdown = !showQuestionFilterOptionsDropdown">
                <div v-if="allQuestionFilterOptionsAreSelected">Alle Fragen</div>
                <div v-else>
                    <template v-for="o in selectedQuestionFilterOptionsDisplay">
                        <i v-if="o.isSelected" :class="o.icon"></i>
                    </template>
                    <span v-if="selectedQuestionFilterOptionsExtraCount >= 2">+ {{selectedQuestionFilterOptionsExtraCount}}</span>
                </div>

                <i v-if="showQuestionFilterOptionsDropdown" class="fa fa-chevron-up" aria-hidden="true"></i>
                <i v-else class="fa fa-chevron-down" aria-hidden="true"></i>

            </div>
            <div v-if="showQuestionFilterOptionsDropdown" class="question-filter-options-dropdown">
                <div @click="selectAllQuestionFilter()" class="selectable-item">
                    <i class="fas fa-check-square" v-if="allQuestionFilterOptionsAreSelected"></i>
                    <i class="far fa-square" v-else></i>
                    <div class="selectable-item">Alles auswaehlen</div>
                </div>
                <div v-for="q in questionFilterOptions" @click="selectQuestionFilter(q)" class="dropdown-item selectable-item">
                    <i class="fas fa-check-square" v-if="q.isSelected"></i>
                    <i class="far fa-square" v-else></i>
                    <i :class="q.icon"></i>
                    <div class="selectable-item">{{q.label}} ({{q.count}})</div>
                </div>

            </div>
        </div>

        <div class="col-xs-12 col-sm-6 col-md-3 question-counter-container">
            <div class="sub-header">Max. Fragen</div>
            <div class="question-counter" :class="{  'input-is-focused': inputFocused}">
                <input type="number" min="1" v-model="selectedQuestionCount" :max="maxSelectableQuestionCount" v-on:input="setSelectedQuestionCount($event)" @focus="inputFocused = true" @blur="inputFocused = false"/>
                <div class="question-counter-selector-container">
                    <div class="question-counter-selector selectable-item" @click="selectQuestionCount(1)">
                        <i class="fa fa-chevron-up" aria-hidden="true"></i>
                    </div>
                    <div class="question-counter-selector  selectable-item" @click="selectQuestionCount(-1)">
                        <i class="fa fa-chevron-down" aria-hidden="true"></i>
                    </div>
                </div>

            </div>
        </div>

        <div class="dropdown-container col-xs-12 col-sm-6 col-md-3">
            <div class="sub-header">Wissenstand</div>

            <div class="knowledge-summary-button selectable-item" @click="showKnowledgeSummaryDropdown = !showKnowledgeSummaryDropdown">
                <div class="knowledge-summary-chip-container">
                    <template v-for="s in knowledgeSummary">
                        <div v-if="s.isSelected" class="knowledge-summary-chip" :class="s.colorClass">
                            <template v-if="knowledgeSummaryCount == 1">{{s.label}}</template>
                        </div>
                    </template>
                </div>
                <i v-if="showKnowledgeSummaryDropdown" class="fa fa-chevron-up" aria-hidden="true"></i>
                <i v-else class="fa fa-chevron-down" aria-hidden="true"></i>
            </div>
            <div v-if="showKnowledgeSummaryDropdown" class="knowledge-summary-dropdown">
                <div @click="selectAllKnowledgeSummary()">Alles auswaehlen</div>
                <div v-for="k in knowledgeSummary" class="dropdown-item">
                    <i class="fas fa-check-square" v-if="k.isSelected"></i>
                    <i class="far fa-square" v-else></i>
                    <div :class="k.colorClass" class="knowledge-summary-chip" @click="selectKnowledgeSummary(k)">{{k.label}}</div> ({{k.count}})
                </div>

            </div>
        </div>

        <div class="dropdown-container col-xs-12 col-sm-6 col-md-3">
            <div class="sub-header">Modus</div>

            <div class="mode-change-button selectable-item" @click="showModeSelectionDropdown = !showModeSelectionDropdown">
                <div v-if="isTestMode">TestModus</div>
                <div v-if="isPracticeMode">UebungsModus</div>
            </div>
            <div v-if="showModeSelectionDropdown" class="mode-change-dropdown">
                <div style="display: flex; flex-direction: column">
                    <div @click="isPracticeMode = true">
                        UebungsModus
                    </div>
                    <div style="display: flex; flex-direction: column" v-if="isPracticeMode">
                        <div>
                            <input type="radio" v-model="practiceOptions.questionOrder" :value="0"> Einfachste Zuerst
                        </div>
                        <div>
                            <input type="radio" v-model="practiceOptions.questionOrder" :value="1"> Schwerste Zuerst
                        </div>
                        <div>
                            <input type="radio" v-model="practiceOptions.questionOrder" :value="2"> Ungewusste Zuerst
                        </div>
                        <div>
                            <input type="radio" v-model="practiceOptions.questionOrder" :value="3"> Zufaellig
                        </div>
                        <div>
                            <input type="radio" v-model="practiceOptions.repetition" :value="0"> Keine Wiederholung
                        </div>
                        <div>
                            <input type="radio" v-model="practiceOptions.repetition" :value="1"> Wiederholung: Normal
                        </div>
                        <div>
                            <input type="radio" v-model="practiceOptions.repetition" :value="2" disabled> Wiederholung: Leitner
                        </div>
                        <div>
                            <input type="checkbox" v-model="practiceOptions.answerHelp"> AntwortHilfe
                        </div>
                    </div>

                </div>
                <div style="display: flex; flex-direction: column">
                    <div @click="isTestMode = true">
                        TestModus
                    </div>
                    <div style="display: flex; flex-direction: column" v-if="isTestMode">
                        <div>
                            <input type="radio" v-model="testOptions.questionOrder" :value="0"> Einfachste Zuerst
                        </div>
                        <div>
                            <input type="radio" v-model="testOptions.questionOrder" :value="1"> Schwerste Zuerst
                        </div>
                        <div>
                            <input type="radio" v-model="testOptions.questionOrder" :value="2"> Ungewusste Zuerst
                        </div>
                        <div>
                            <input type="radio" v-model="testOptions.questionOrder" :value="3"> Zufaellig
                        </div>
                        <div>
                            <input type="number" v-model="testOptions.timeLimit"> Zeitlimit
                        </div>
                    </div>


                </div>
            </div>
        </div>
    </div>


</div>