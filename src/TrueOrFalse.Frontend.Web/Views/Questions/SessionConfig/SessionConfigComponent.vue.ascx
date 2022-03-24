<div class="session-configurator col-xs-12 " v-click-outside="closeDropdowns">
    <div class="session-config-header">
        <div class="filter-button selectable-item session-title" @click="showFilterDropdown = !showFilterDropdown" :class="showFilterDropdown ? 'open' : 'closed'">
            Filter
        </div>
        <slot></slot>
    </div>

    <div v-show="showFilterDropdown" class="session-config-dropdown row">
        <div class="dropdown-container col-xs-12 col-sm-6">
            <div class="sub-header">Fragen</div>
            <div class="question-filter-options-button selectable-item" @click="showQuestionFilterOptionsDropdown = !showQuestionFilterOptionsDropdown" :class="{  'is-open': showQuestionFilterOptionsDropdown }">
                <div v-if="allQuestionFilterOptionsAreSelected">Alle Fragen</div>
                <div v-else class="question-filter-options-icon-container">
                    <template v-for="o in selectedQuestionFilterOptionsDisplay">
                        <i v-if="o.isSelected" :class="o.icon"></i>
                    </template>
                    <div class="icon-counter" v-if="selectedQuestionFilterOptionsExtraCount >= 2">+{{selectedQuestionFilterOptionsExtraCount}}</div>
                </div>

                <i v-if="showQuestionFilterOptionsDropdown" class="fa fa-chevron-up" aria-hidden="true"></i>
                <i v-else class="fa fa-chevron-down" aria-hidden="true"></i>

            </div>
            <div v-if="showQuestionFilterOptionsDropdown" class="question-filter-options-dropdown">
                <div @click="selectAllQuestionFilter()" class="selectable-item dropdown-item" :class="{'item-disabled' : !isLoggedIn }">
                    <i class="fas fa-check-square session-select active" v-if="allQuestionFilterOptionsAreSelected"></i>
                    <i class="far fa-square session-select" v-else></i>
                    <div class="selectable-item">Alles auswählen</div>
                </div>
                <div class="dropdown-divider"></div>

                <div v-for="q in questionFilterOptions" @click="selectQuestionFilter(q)" class="dropdown-item selectable-item" :class="{'item-disabled' : !isLoggedIn }">
                    <i class="fas fa-check-square session-select active" v-if="q.isSelected"></i>
                    <i class="far fa-square session-select" v-else></i>
                    <i class="dropdown-filter-icon" :class="q.icon"></i>

                    <div class="selectable-item dropdown-item-label">                    
                        {{q.label}} ({{q.count}})
                    </div>
                </div>

            </div>
        </div>

        <div class="col-xs-12 col-sm-6 question-counter-container">
            <div class="sub-header">Max. Fragen</div>
            <div class="question-counter" :class="{ 'input-is-focused': inputFocused }">
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

        <div class="dropdown-container col-xs-12 col-sm-6">
            <div class="sub-header">Wissenstand</div>

            <div class="knowledge-summary-button selectable-item" @click="showKnowledgeSummaryDropdown = !showKnowledgeSummaryDropdown" :class="{ 'is-open': showKnowledgeSummaryDropdown }">
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
                <div class="selectable-item dropdown-item" @click="selectAllKnowledgeSummary()" :class="{'item-disabled' : !isLoggedIn }">
                    <i class="fas fa-check-square session-select active" v-if="allKnowledgeSummaryOptionsAreSelected"></i>
                    <i class="far fa-square session-select" v-else></i>
                    <div class="selectable-item">Alles auswählen</div>
                </div>
                <div class="dropdown-divider"></div>
                <div v-for="k in knowledgeSummary" class="dropdown-item" :class="{'item-disabled' : !isLoggedIn }" @click="selectKnowledgeSummary(k)">
                    <i class="fas fa-check-square session-select active" v-if="k.isSelected"></i>
                    <i class="far fa-square session-select" v-else></i>
                    <div :class="k.colorClass" class="knowledge-summary-chip">
                        {{k.label}} ({{k.count}})
                    </div>
                </div>

            </div>
        </div>

        <div class="dropdown-container col-xs-12 col-sm-6">
            <div class="sub-header">Modus <i class="fas fa-info-circle show-tooltip mode-info-icon" data-original-title="Mehr über Lernmodi erfährst Du in der Dokumentation."></i></div>

            <div class="mode-change-button selectable-item" @click="showModeSelectionDropdown = !showModeSelectionDropdown" :class="{ 'is-open': showModeSelectionDropdown }">
                <div v-if="isTestMode"><i class="fas fa-graduation-cap dropdown-filter-icon"></i> Testen</div>
                <div v-if="isPracticeMode"><i class="fas fa-lightbulb dropdown-filter-icon"></i> Lernen</div>
                
                <i v-if="showModeSelectionDropdown" class="fa fa-chevron-up" aria-hidden="true"></i>
                <i v-else class="fa fa-chevron-down" aria-hidden="true"></i>
            </div>
            <div v-if="showModeSelectionDropdown" class="mode-change-dropdown">
                <div>
                    <div class="selectable-item dropdown-item mode-change-header" @click="selectPracticeMode">
                        <div class="dropdown-item">
                            <i class="fas fa-dot-circle session-select active" v-if="isPracticeMode"></i>
                            <i class="far fa fa-circle-o session-select" v-else></i>
                            <div class="selectable-item"><i class="fas fa-lightbulb dropdown-filter-icon"></i> Lernen</div>
                        </div>
                        <i v-if="isPracticeMode" class="fa fa-chevron-up" aria-hidden="true"></i>
                        <i v-else class="fa fa-chevron-down" aria-hidden="true"></i>
                    </div>
                    <div v-if="isPracticeMode" class="mode-group-container">
                        <div class="mode-item-container" @click="selectPracticeOption('questionOrder', 0)">
                            <div class="mode-sub-label">
                                Einfache Fragen zuerst
                            </div>
                            <i class="fas fa-dot-circle session-mini-select active" v-if="practiceOptions.questionOrder == 0"></i>
                            <i class="far fa fa-circle-o session-mini-select" v-else></i>
                        </div>
                        <div class="mode-item-container" @click="selectPracticeOption('questionOrder', 1)">
                            <div class="mode-sub-label">
                                Schwere Fragen zuerst
                            </div>
                            <i class="fas fa-dot-circle session-mini-select active" v-if="practiceOptions.questionOrder == 1"></i>
                            <i class="far fa fa-circle-o session-mini-select" v-else></i>
                        </div>
                        <div class="mode-item-container" @click="selectPracticeOption('questionOrder', 2)" :class="{'item-disabled' : !isLoggedIn }">
                            <div class="mode-sub-label">
                                Nicht gewusste Fragen zuerst
                            </div>
                            <i class="fas fa-dot-circle session-mini-select active" v-if="practiceOptions.questionOrder == 2"></i>
                            <i class="far fa fa-circle-o session-mini-select" v-else></i>
                        </div>
                        <div class="mode-item-container" @click="selectPracticeOption('questionOrder', 3)">
                            <div class="mode-sub-label">
                                Zufällige Fragen
                            </div>
                            <i class="fas fa-dot-circle session-mini-select active" v-if="practiceOptions.questionOrder == 3"></i>
                            <i class="far fa fa-circle-o session-mini-select" v-else></i>
                        </div>

                        <div class="dropdown-spacer"></div>

                        <div class="mode-item-container" @click="selectPracticeOption('repetition', 0)">
                            <div class="mode-sub-label">
                                Keine Wiederholung
                            </div>
                            <i class="fas fa-dot-circle session-mini-select active" v-if="practiceOptions.repetition == 0"></i>
                            <i class="far fa fa-circle-o session-mini-select" v-else></i>
                        </div>
                        <div class="mode-item-container" @click="selectPracticeOption('repetition', 1)">
                            <div class="mode-sub-label">
                                Falsch beantwortete Fragen wiederholen
                            </div>
                            <i class="fas fa-dot-circle session-mini-select active" v-if="practiceOptions.repetition == 1"></i>
                            <i class="far fa fa-circle-o session-mini-select" v-else></i>
                        </div>
                        <div class="mode-item-container item-disabled">
                            <div class="mode-sub-label">
                                Wiederholung nach Leitner (kommt bald)
                            </div>
                            <i class="fas fa-dot-circle session-mini-select active" v-if="practiceOptions.repetition == 2"></i>
                            <i class="far fa fa-circle-o session-mini-select" v-else></i>
                        </div>
                        
                        <div class="dropdown-spacer"></div>
                        
                        <div class="mode-item-container" @click="selectPracticeOption('answerHelp', !practiceOptions.answerHelp)">
                            <div class="mode-sub-label">
                                Antworthilfe
                            </div>
                            <i class="fas fa-check-square session-mini-select active" v-if="practiceOptions.answerHelp"></i>
                            <i class="far fa-square session-mini-select" v-else></i>
                        </div>
                    </div>

                </div>
                <div class="dropdown-divider"></div>
                
                <div>
                    <div class="selectable-item dropdown-item mode-change-header" @click="selectTestMode">
                        <div class="dropdown-item">
                            <i class="fas fa-dot-circle session-select active" v-if="isTestMode"></i>
                            <i class="far fa fa-circle-o session-select" v-else></i>
                            <div class="selectable-item"><i class="fas fa-graduation-cap dropdown-filter-icon"></i> Testen</div>
                        </div>
                        <i v-if="isTestMode" class="fa fa-chevron-up" aria-hidden="true"></i>
                        <i v-else class="fa fa-chevron-down" aria-hidden="true"></i>
                    </div>
                    <div v-if="isTestMode" class="mode-group-container">
                        <div class="mode-item-container" @click="selectTestOption('questionOrder', 0)">
                            <div class="mode-sub-label">
                                Einfache Fragen zuerst
                            </div>
                            <i class="fas fa-dot-circle session-mini-select active" v-if="testOptions.questionOrder == 0"></i>
                            <i class="far fa fa-circle-o session-mini-select" v-else></i>
                        </div>
                        <div class="mode-item-container" @click="selectTestOption('questionOrder', 1)">
                            <div class="mode-sub-label">
                                Schwere Fragen zuerst
                            </div>
                            <i class="fas fa-dot-circle session-mini-select active" v-if="testOptions.questionOrder == 1"></i>
                            <i class="far fa fa-circle-o session-mini-select" v-else></i>
                        </div>
                        <div class="mode-item-container" @click="selectTestOption('questionOrder', 2)">
                            <div class="mode-sub-label">
                                Nicht gewusste Fragen zuerst
                            </div>
                            <i class="fas fa-dot-circle session-mini-select active" v-if="testOptions.questionOrder == 2"></i>
                            <i class="far fa fa-circle-o session-mini-select" v-else></i>
                        </div>
                        <div class="mode-item-container" @click="selectTestOption('questionOrder', 3)">
                            <div class="mode-sub-label">
                                Zufällige Fragen
                            </div>
                            <i class="fas fa-dot-circle session-mini-select active" v-if="testOptions.questionOrder == 3"></i>
                            <i class="far fa fa-circle-o session-mini-select" v-else></i>
                        </div>

                        <div class="dropdown-spacer"></div>
                        
                        <div class="mode-item-container item-disabled">
                            <div class="mode-sub-label">
                                Zeitlimit in Minuten (kommt bald)
                            </div>
                            <input type="number" v-model="testOptions.timeLimit" disabled>
                        </div>
                    </div>

                </div>

            </div>
        </div>
        
            <div class="col-xs-12 reset-session-button-container" >
                <div class="selectable-item reset-session-button" @click="reset">
                    <i class="fa fa-times" aria-hidden="true"></i>
                    <div>
                        Alle Filter zurücksetzen
                    </div>
                </div>

            </div>


    </div>

</div>