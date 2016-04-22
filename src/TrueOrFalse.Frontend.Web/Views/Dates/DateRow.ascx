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
                            <a href="<%= Links.DateEdit(Url, date.Id) %>"><i class="fa fa-pencil"></i></a>
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
                    <% if (Model.Date.Visibility == DateVisibility.InNetwork) { %>
                    <i class="fa fa-files-o"></i> 0x kopiert (keine Reputationspunkte)
                    <% }else { %>
                    <i class="fa fa-lock"></i> Privater Termin
                    <% } %>
                </div>
            </div>
    
            <div class="row">
                <% if (!Model.IsNetworkDate){ %>
                    <div class="col-xs-12" style="text-align: left">
                        <a href="<%= Links.GameCreateFromDate(date.Id) %>" class="show-tooltip" data-original-title="Spiel mit Fragen aus diesem Termin starten."
                            style="display: inline-block; margin-top: 29px; margin-right: 11px;">
                            <i class="fa fa-gamepad" style="font-size: 18px;"></i>
                            Spiel starten
                        </a>
                        
                        <a style="display: inline-block;"
                            data-btn="startLearningSession" 
                            href="/Termin/Lernen/<%=Model.Date.Id %>"><i class="fa fa-line-chart"></i> 
                            Jetzt üben
                        </a>
                    </div>
                <% }else{ %>
                    <div class="col-sm-12" style="text-align: right;">
                        <div style="margin-top: 29px;">
                            <a data-toggle="modal" data-dateId="<%= date.Id %>" class="btn btn-sm btn-info" href="#modalCopy" data-url="toSecurePost">
                                <i class="fa fa-files-o"></i>
                                 Termin übernehmen
                            </a>
                        </div>
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
            <% if(!Model.IsPast) { %>
                <div class="row">
                    <div class="col-md-1"><i class="fa fa-calendar"></i></div>
                    <div class="col-md-10">Übungsplan: <span style="font-size: 9px">(verbleibend)</span></div>
                </div>
                <div class="row">
                    <div class="col-md-12">ca. <%= Model.TrainingDateCount %> Übungssitzungen</div>
                </div>
                <div class="row">
                    <div class="col-md-12">ca. <%= Model.TrainingLength %> Übungszeit</div>
                </div>
                <div class="row">
                    <div class="col-md-1"><i class="fa fa-bell"></i></div>
                    <div class="col-md-10">
                        <% if(trainingPlan.HasOpenDates) { %>
                            nächste Übungssitzung <br/>
                            in <%= new TimeSpanLabel(trainingPlan.TimeToNextDate, showTimeUnit:true).Full %> 
                            (<%= trainingPlan.Questions.Count %> Fragen)
                        <% } %>
                    </div>
                </div>
                <div class="row" style="height: 100%;">
                    <% if(!Model.HideEditPlanButton) { %>
                    <div class="col-md-1"><i class="fa fa-pencil"></i></div>
                    <div class="col-md-10">
                        <a href="#modalTraining" style="margin-top: 29px;" data-dateId="<%= date.Id %>">bearbeiten</a>
                    </div>
                    <% } %>
                </div>
            <% }else{ /* Model.IsPast */ %>
                <div class="row">
                    <div class="col-md-1"><i class="fa fa-calendar"></i></div>
                    <div class="col-md-10">Übungshistorie:</div>
                </div>
                <div class="row">
                    <div class="col-md-12"><%= Model.NumberOfTrainingsDone %> Übungssitzung<%= StringUtils.Plural(Model.NumberOfTrainingsDone,"en","","en") %> absolviert</div>
                </div>
                <div class="row">
                    <div class="col-md-12">ca. <%= new TimeSpanLabel(trainingPlan.TimeSpent, showTimeUnit: true).Full %> </div>
                </div>
            <% } %>

        </div>
    </div>
</div>