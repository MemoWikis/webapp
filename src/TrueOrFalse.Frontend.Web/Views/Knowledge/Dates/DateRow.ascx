<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DateRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% 
    var date = Model.Date;
    var trainingPlan = Model.TrainingPlan;
%>

<div class="rowBase date-row" style="position: relative; padding: 5px; "
    data-date-id="<%= date.Id %>"
    data-notLearned="<%= Model.KnowledgeNotLearned %>"
    data-needsLearning="<%= Model.KnowledgeNeedsLearning %>"
    data-needsConsolidation="<%= Model.KnowledgeNeedsConsolidation %>"
    data-solid="<%= Model.KnowledgeSolid %>">
    
    <div class="row">

        <div class="col-md-2" style="">
            <div class="row">
                <div class="col-md-12" style="font-size: 13px;">
                    <%
                        if(Model.IsPast){
                            Response.Write("Vorbei seit ");
                        }else { 
                            Response.Write("Noch ");
                        }
                    %>                
                </div>
            </div>
            <div class="row">
                <div class="col-md-12" style="font-size: 19px;">
                    <%= Model.RemainingLabel.Value %>
                    <% Response.Write(Model.RemainingLabel.Label); %>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12" style="color: darkgrey;  position: relative; top: 3px;">
                    <% if(Model.IsPast){ %>
                        <span style="font-size: 11px">Termin war am <br/></span> 
                    <% }else{ %>
                        <span style="font-size: 11px">bis Termin am <br/></span> 
                    <% } %>
                    <span style="font-size: 11px;">
                        <%= date.DateTime.ToString("dd.MM.yyy HH:mm") %>
                    </span>
                </div>                
            </div>
        </div>

        <div class="col-sm-5">
            <div class="row">
                <div class="col-md-9" style="font-size: 16px">
                    <%= Model.Date.GetTitle() %>
                </div>
                
                <div class="col-md-3 hidden-xs hidden-sm" style="text-align: right; vertical-align: bottom">
                    <%if(!Model.IsNetworkDate){ %> 
                        <div style="font-size: 13px; margin-top: 7px;">
                            <a href="<%= Links.DateEdit(date.Id) %>"><i class="fa fa-pencil"></i></a>
                            &nbsp;
                            <a data-toggle="modal" data-dateId="<%= date.Id %>" href="#modalDelete">
                                <i class="fa fa-trash-o"></i>
                            </a>
                        </div>
                    <% } %>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12">
                    <%= Model.AmountQuestions %> Fragen aus
                    <div style="display: inline; position: relative; top: -2px;" >
                        <%  foreach(var set in date.Sets){ %>
                            <a href="<%= Links.SetDetail(Url, set) %>">
                                <span class="label label-set"><%= set.Name %></span>
                            </a>
                        <% } %>
                    </div>
                </div>        
            </div>
            
            <div class="row">
                <div class="col-md-12">
                    <% if (!Model.IsNetworkDate){ %>
                        <% if (Model.Date.Visibility == DateVisibility.InNetwork) { %>
                            <i class="fa fa-unlock"></i> Im Netzwerk sichtbar (<i class="fa fa-files-o"></i> <span class="numberOfTimesCopied"><%= Model.CopiedCount %></span>x übernommen)
                        <% }else { %>
                            <i class="fa fa-lock"></i> Privater Termin
                        <% } %>
                        <% if (Model.CopiedFromUserName != "") { %>
                            <br/><span style="color: darkgrey;">Du hast diesen Termin von <a href="<%= Links.UserDetail(Model.CopiedFromUser) %>"><%= Model.CopiedFromUserName %></a> übernommen.</span>
                        <% } %>
                    <% }else{ %>
                        <i class="fa fa-unlock"></i> Termin von <a href="<%= Links.UserDetail(Model.Date.User) %>"><%= Model.Date.User.Name %></a> (<i class="fa fa-files-o"></i> <span class="numberOfTimesCopied"><%= Model.CopiedCount %></span>x übernommen)
                        <%--<% if (Model.CopiedFromUserName != "") { %>
                            <br/><span style="color: darkgrey;">Übernommen von <a href="<%= Links.UserDetail(Model.CopiedFromUser) %>"><%= Model.CopiedFromUserName %></a></span>
                        <% } %>--%>
                    <% } %>
                </div>
            </div>

            <div class="row">
                <% if (!Model.IsNetworkDate){ %>
                    <div class="col-xs-12" style="text-align: left; margin-top: 15px; margin-bottom: 10px;">
                        <a style="display: inline-block;" class="btn btn-primary btn-sm"
                            data-btn="startLearningSession" 
                            href="<%= Links.StartDateLearningSession(Model.Date.Id) %>"><i class="fa fa-line-chart"></i> 
<%--                            href="/Termin/Lernen/<%=Model.Date.Id %>"><i class="fa fa-line-chart"></i> --%>
                            Jetzt lernen
                        </a>
                        <a href="<%= Links.GameCreateFromDate(date.Id) %>" class="btn btn-link btn-sm show-tooltip" data-original-title="Spiel mit Fragen aus diesem Termin starten."
                            style="display: inline-block;">
                            <i class="fa fa-gamepad" style="font-size: 18px;"></i>
                            Spiel starten
                        </a>
                    </div>
                <% } %>
            </div>
        </div>

        <div class="col-sm-2">
            <% if (!Model.IsNetworkDate){ %>
                <div id="chartKnowledgeDate<% =date.Id %>"></div>
            <% }else{ %>
                <div style="text-align: center">
                    <i class="fa fa-question-circle show-tooltip" data-original-title="Dein Wissensstand wird nur für deine Termine angezeigt." style="font-size: 97px;"></i><br />
                    <p style="font-size: 11px; line-height: 11px;">
                        Dein Wissensstand wird nur für deine Termine angezeigt.
                    </p>
                    
                </div>
            <% } %>
        </div>
        
        <div class="col-sm-3">
            <% if (!Model.IsNetworkDate && !Model.IsPast) { %>
                <div class="row">
                    <div class="col-md-1"><i class="fa fa-calendar"></i></div>
                    <div class="col-md-10">
                        <b>Lernplan:</b> <span style="font-size: 9px">(verbleibend)</span>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">ca. <span class="TPTrainingDateCount"><%= Model.TrainingDateCount %></span> Lernsitzungen</div>
                </div>
                <div class="row">
                    <div class="col-md-12">ca. <span class="TPRemainingTrainingTime"><%= Model.RemainingTrainingTime %></span> Lernzeit</div>
                </div>
                <div class="row">
                    <div class="col-md-1"><i class="fa fa-bell"></i></div>
                    <div class="col-md-10">
                        <% if(trainingPlan.HasOpenDates) {
                            var timeSpanLabel = new TimeSpanLabel(trainingPlan.TimeToNextDate, showTimeUnit: true);
                            if (timeSpanLabel.TimeSpanIsNegative) { %>
                                <a style="display: inline-block;" data-btn="startLearningSession" href="/Termin/Lernen/<%=Model.Date.Id %>">Jetzt lernen!</a>
                            <% } else { %>
                                nächste Lernsitzung <br/>
                                in <span class="TPTimeToNextTrainingDate"><%= timeSpanLabel.Full %></span> 
                            <% } %>
                            (<span class="TPQuestionsInNextTrainingDate"><%= trainingPlan.QuestionCountInNextDate %></span> Fragen)
                        <% } %>
                    </div>
                </div>
                <div class="row" style="height: 100%;">
                    <% if(!Model.IsNetworkDate && !Model.HideEditPlanButton) { %>
                    <div class="col-md-1">&nbsp;</div>
                    <div class="col-md-10">
                        <a href="#modalTraining" class="btn btn-default btn-sm" style="margin: 5px 0;" data-dateId="<%= date.Id %>">
                            <i class="fa fa-pencil"></i> Details &amp; bearbeiten
                        </a>
                    </div>
                    <% } %>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <div style="border-top-style: dotted; border-top-width: 1px; margin-top: 10px;">
                            <i class="fa fa-clock-o"></i>
                            <% if (Model.LearningSessionQuestionsLearned == 0) { %>
                                Bisher nicht gelernt
                            <% }else { %>
                                Bisher gelernt: ca. <%= new TimeSpanLabel(Model.TimeSpentLearning, showTimeUnit: true).Full %>
                            <% } %>
                        </div>
                    </div>
                </div>
            <% }else if (Model.IsPast){ %>
                <div class="row">
                    <div class="col-md-1"><i class="fa fa-calendar"></i></div>
                    <div class="col-md-10"><b>Lernhistorie:</b></div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <%= Model.LearningSessionCount %> Lernsitzung<%= StringUtils.PluralSuffix(Model.LearningSessionCount,"en") %> mit <br/>
                        <%= Model.LearningSessionQuestionsLearned %> Frage<%= StringUtils.PluralSuffix(Model.LearningSessionQuestionsLearned,"n") %>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">ca. <%= new TimeSpanLabel(Model.TimeSpentLearning, showTimeUnit: true).Full %> (Schätzung)</div>
                </div>
            <% }else if (Model.IsNetworkDate){ %>
                <div class="row">
                    <div class="col-md-11" style="padding-top: 35px;">
                        <p style="font-size: 11px; line-height: 11px; padding: 5px 13px;">Übernehme diesen Termin, damit memucho einen persönlichen Lernplan für dich erstellen kann.</p>
                        <p style="text-align: center;">
                            <a data-toggle="modal" data-dateId="<%= date.Id %>" class="btn btn-sm btn-info" href="#modalCopyDate" data-url="toSecurePost">
                                <i class="fa fa-files-o"></i> Termin übernehmen
                            </a>
                        </p>
                    </div>
                </div>
            <% } %>

        </div>
    </div>
</div>