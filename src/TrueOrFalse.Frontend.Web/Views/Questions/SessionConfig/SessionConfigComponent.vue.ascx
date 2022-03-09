<div class="session-configurator col-xs-12 ">
    <div class="session-config-header">
        <div class="filter-button" @click="showFilterDropdown = !showFilterDropdown" :class="showFilterDropdown ? 'open' : 'closed'">
            Filter
        </div>
        <slot></slot>
    </div>

    <div v-show="showFilterDropdown" class="session-config-dropdown">
        <div class="dropdown-container">
            <div class="question-filter-options-button" @click="showQuestionFilterOptionsDropdown = !showQuestionFilterOptionsDropdown">
                <template v-for="o in selectedQuestionFilterOptionsDisplay">
                    <i v-if="o.isSelected" :class="o.icon"></i>
                </template>
                <span v-if="selectedQuestionFilterOptionsExtraCount >= 2">+ {{selectedQuestionFilterOptionsExtraCount}}</span>
            </div>
            <div v-if="showQuestionFilterOptionsDropdown" class="question-filter-options-dropdown">
                <div @click="selectAllQuestionFilter()">
                    <i class="fas fa-check-square" v-if="allQuestionFilterOptionsAreSelected"></i>
                    <i class="far fa-square" v-else></i>
                    <div>Alles auswaehlen</div>
                </div>
                <div v-for="q in questionFilterOptions" @click="selectQuestionFilter(q)" class="dropdown-item">
                    <i class="fas fa-check-square" v-if="q.isSelected"></i>
                    <i class="far fa-square" v-else></i>
                    <i :class="q.icon"></i>
                    <div style="user-select: none;">{{q.label}} ({{q.count}})</div>
                </div>

            </div>
        </div>

        <div id="questionCounter" style="height: 40px; border: solid 1px silver; padding: 6px; display: flex;">
            <input type="number" min="1" v-model="selectedQuestionCount" :max="maxSelectableQuestionCount" v-on:input="setSelectedQuestionCount($event)"/>
            <div style="border: solid 1px silver; height: 100%; width: 20px; padding: 4px;" @click="selectQuestionCount(1)">
                <i class="fa fa-plus" aria-hidden="true"></i>
            </div>
            <div style="border: solid 1px silver; height: 100%; width: 20px; padding: 4px;" @click="selectQuestionCount(-1)">
                <i class="fa fa-minus" aria-hidden="true"></i>
            </div>
        </div>

        <div class="dropdown-container">
            <div style="width: 210px; height: 40px; border: solid 1px silver; padding: 6px; display: flex; flex-wrap: nowrap;" @click="showKnowledgeSummaryDropdown = !showKnowledgeSummaryDropdown">
                <template v-for="s in knowledgeSummary">
                    <div v-if="s.isSelected" class="knowledge-summary-dot" :class="s.colorClass">
                        <template v-if="knowledgeSummaryCount == 1">{{s.label}}</template>
                    </div>
                </template>
            </div>
            <div v-if="showKnowledgeSummaryDropdown" style="width: 210px; border: solid 1px silver; padding: 6px;">
                <div @click="selectAllKnowledgeSummary()">Alles auswaehlen</div>
                <div v-for="k in knowledgeSummary" style="display: flex; flex-direction: row">
                    <i class="fas fa-check-square" v-if="k.isSelected"></i>
                    <i class="far fa-square" v-else></i>
                    <div :class="k.colorClass" @click="selectKnowledgeSummary(k)">{{k.label}} ({{k.count}})</div>
                </div>

            </div>
        </div>

        <div class="dropdown-container">
            <div style="width: 210px; height: 40px; border: solid 1px silver; padding: 6px; display: flex; flex-wrap: nowrap;" @click="showModeSelectionDropdown = !showModeSelectionDropdown">
                <div v-if="isTestMode">TestModus</div>
                <div v-if="isPracticeMode">UebungsModus</div>
            </div>
            <div v-if="showModeSelectionDropdown" style="width: 210px; border: solid 1px silver; padding: 6px;">
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