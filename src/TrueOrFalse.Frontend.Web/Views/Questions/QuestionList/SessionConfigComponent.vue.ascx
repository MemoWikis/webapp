<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<session-config-component inline-template :questions-count="allQuestionCount" :all-question-count="allQuestionCount" is-my-world="<%= UserCache.GetItem(Model.UserId).IsFiltered %>">
    <div class="rootElement">
                <% if(Model.IsSessionNoteFadeIn){%>
                <div id="LearningSessionReminderQuestionList">
                    <img id="SessionConfigReminderLeft" src="/Images/Various/SessionConfigReminderLeft.svg" >
                    <img id="SessionConfigReminderRight" src="/Images/Various/SessionConfigReminder.svg" >
                    <span class="far fa-times-circle"></span>
                </div>
                    <% } %>
        <div id="CustomSessionConfigBtn" class="btn btn-link" @click="openModal()" data-toggle="tooltip" data-html="true" title="<p><b>Persönliche Filter helfen Dir</b>. Nutze die Lernoptionen und entscheide welche Fragen Du lernen möchtest.</p>">
            <i class="fa fa-cog" aria-hidden="true"></i>
        </div>
        <div class="modal fade" id="SessionConfigModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div id="SessionConfigTemplate" style="height: 300px; width: 600px">
                        <div id="questionCounter" style="width: 210px; height: 40px; border: solid 1px silver; padding: 6px; display: flex;">
                            <input type="number" min="1" v-model="selectedQuestionCount"/>
                            <div style="border: solid 1px silver; height: 100%; width: 20px; padding: 4px;" @click="++selectedQuestionCount"> + </div>
                            <div style="border: solid 1px silver; height: 100%; width: 20px; padding: 4px;" @click="--selectedQuestionCount"> - </div>
                        </div>
                        <div id="dropdowncontainer">
                            <div style="width: 210px; height: 40px; border: solid 1px silver; padding: 6px;" @click="showDropdown = !showDropdown"></div>
                            <div v-if="showDropdown" style="width: 210px; border: solid 1px silver; padding: 6px;">
                                <div class="dropdown-item" style="height: 40px; width: 100%; display:flex; align-items: center"><div style="border-radius: 10px; height: 20px;background-color:silver">Ungelernt</div></div>
                                <div class="dropdown-item" style="height: 40px; width: 100%; display:flex; align-items: center"><div style="border-radius: 10px; height: 20px;background-color:salmon">zu lernen</div></div>
                                <div class="dropdown-item" style="height: 40px; width: 100%; display:flex; align-items: center"><div style="border-radius: 10px; height: 20px;background-color:yellow">zu festigen</div></div>
                                <div class="dropdown-item" style="height: 40px; width: 100%; display:flex; align-items: center"><div style="border-radius: 10px; height: 20px;background-color:green">sicheres wissen</div></div>
                                <div v-for="summary in knowledgeSummary"></div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
</session-config-component>


