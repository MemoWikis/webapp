<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<session-config-component inline-template :questions-count="allQuestionCount" :all-question-count="allQuestionCount" is-my-world="<%= UserCache.GetItem(Model.UserId).IsFiltered %>">
    <div class="rootElement">
        <% if (Model.IsSessionNoteFadeIn)
           { %>
            <div id="LearningSessionReminderQuestionList">
                <img id="SessionConfigReminderLeft" src="/Images/Various/SessionConfigReminderLeft.svg">
                <img id="SessionConfigReminderRight" src="/Images/Various/SessionConfigReminder.svg">
                <span class="far fa-times-circle"></span>
            </div>
        <% } %>
        <div id="CustomSessionConfigBtn" class="btn btn-link" @click="openModal()" data-toggle="tooltip" data-html="true" title="<p><b>Persönliche Filter helfen Dir</b>. Nutze die Lernoptionen und entscheide welche Fragen Du lernen möchtest.</p>">
            <i class="fa fa-cog" aria-hidden="true"></i>
        </div>
        <div class="modal fade" id="SessionConfigModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document" style="width: 100%">
                <div class="modal-content">
                    <div class="session-configurator">
                        <div class="filter-button" @click="showFilterDropdown = !showFilterDropdown"  :class="showFilterDropdown ? 'open' : 'closed'">
                            Filter
                        </div>
                        <div v-show="showFilterDropdown" class="session-config-dropdown">
                            <div class="dropdown-container">
                                <div class="question-filter-options-button" @click="showDropdown = !showDropdown">
                                    <template v-for="o in selectedQuestionFilterOptionsDisplay">
                                        <i v-if="o.isSelected" :class="o.icon"></i>
                                    </template>
                                    <span v-if="selectedQuestionFilterOptionsExtraCount >= 2">+ {{selectedQuestionFilterOptionsExtraCount}}</span>
                                </div>
                                <div v-if="showDropdown" class="question-filter-options-dropdown">
                                    <div @click="selectAllQuestionFilter()">
                                        <i class="fas fa-check-square" v-if="allQuestionFilterOptionsAreSelected"></i>
                                        <i class="far fa-square" v-else></i>
                                        <div>Alles auswaehlen</div>
                                    </div>
                                    <div v-for="q in questionFilterOptions" @click="selectQuestionFilter(q)" class="dropdown-item">
                                        <i class="fas fa-check-square" v-if="q.isSelected"></i>
                                        <i class="far fa-square" v-else></i>
                                        <i :class="q.icon"></i>
                                        <div>{{q.label}} ({{q.count}})</div>
                                    </div>

                                </div>
                            </div>

                            <div id="questionCounter" style="height: 40px; border: solid 1px silver; padding: 6px; display: flex;">
                                <input type="number" min="1" v-model="selectedQuestionCount"/>
                                <div style="border: solid 1px silver; height: 100%; width: 20px; padding: 4px;" @click="++selectedQuestionCount"> + </div>
                                <div style="border: solid 1px silver; height: 100%; width: 20px; padding: 4px;" @click="--selectedQuestionCount"> - </div>
                            </div>

                            <div class="dropdown-container">
                                <div style="width: 210px; height: 40px; border: solid 1px silver; padding: 6px; display: flex; flex-wrap: nowrap;" @click="showDropdown = !showDropdown">
                                    <template v-for="s in knowledgeSummary" >
                                        <div v-if="s.isSelected" class="knowledge-summary-dot" :class="s.colorClass">
                                            <template v-if="knowledgeSummaryCount == 1">{{s.label}}</template>
                                        </div>
                                    </template>
                                </div>
                                <div v-if="showDropdown" style="width: 210px; border: solid 1px silver; padding: 6px;">
                                    <div @click="selectAllKnowledgeSummary()">Alles auswaehlen</div>
                                    <div v-for="k in knowledgeSummary" style="display: flex; flex-direction:row">
                                        <i class="fas fa-check-square" v-if="k.isSelected"></i>
                                        <i class="far fa-square" v-else></i>
                                        <div :class="k.colorClass" @click="selectKnowledgeSummary(k)">{{k.label}} ({{k.count}})</div>
                                    </div>

                                </div>
                            </div>
                            
<%--                            <div class="dropdown-container">
                                <div style="width: 210px; height: 40px; border: solid 1px silver; padding: 6px; display: flex; flex-wrap: nowrap;" @click="showDropdown = !showDropdown">
                                    <template v-for="s in knowledgeSummary" >
                                        <div v-if="s.isSelected" class="knowledge-summary-dot" :class="s.colorClass">
                                            <template v-if="knowledgeSummaryCount == 1">{{s.label}}</template>
                                        </div>
                                    </template>
                                </div>
                                <div v-if="showDropdown" style="width: 210px; border: solid 1px silver; padding: 6px;">
                                    <div v-for="k in knowledgeSummary" style="display: flex; flex-direction:row">
                                        <i class="fas fa-check-square" v-if="k.isSelected"></i>
                                        <i class="far fa-square" v-else></i>
                                        <div :class="k.colorClass" @click="selectKnowledgeSummary(k)">{{k.label}}</div>
                                    </div>

                                </div>
                            </div>--%>
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>


</session-config-component>