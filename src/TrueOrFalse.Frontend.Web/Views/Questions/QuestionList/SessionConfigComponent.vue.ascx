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
                    <div id="SessionConfigTemplate" style="height: 800px; width: 800px; padding: 10px;">
                        <div class="filter-button" @click="showFilterDropdown = !showFilterDropdown" style="position: relative;border: solid 1px silver; padding: 10px; z-index: 901; width: 200px; background: white" :class="showFilterDropdown ? 'open' : 'closed'">
                            Filter
                        </div>
                        <div v-show="showFilterDropdown" style="position: relative;display: flex; flex-direction: row; padding: 10px; padding-top: 11px; border: 1px solid silver; margin-top: -1px; z-index: 900">
                            <div class="dropdown-container">
                                <div style="width: 210px; height: 40px; border: solid 1px silver; padding: 6px; display: flex; flex-wrap: nowrap;" @click="showDropdown = !showDropdown">
                                    <template v-for="o in questionFilterOptions">
                                        <i v-if="o.isSelected" :class="o.icon"></i>
                                    </template>
                                    <span v-if="selectedQuestionFilterOptionsExtraCount >= 2">+ {{selectedQuestionFilterOptionsExtraCount}}</span>
                                </div>
                                <div v-if="showDropdown" style="border: solid 1px silver; padding: 6px;">
                                    <div v-for="q in questionFilterOptions" @click="selectQuestionFilter(q)" style="display: flex; flex-direction:row">
                                        <i class="fas fa-check-square" v-if="q.isSelected"></i>
                                        <i class="far fa-square" v-else></i>
                                        <i :class="q.icon"></i>
                                        <div>{{q.label}}</div>
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
                                        <div v-if="s.isSelected" :class="s.colorClass">
                                            <template v-if="knowledgeSummaryCount == 1"> {{s.label}}</template>
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
                            </div>
                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>


</session-config-component>