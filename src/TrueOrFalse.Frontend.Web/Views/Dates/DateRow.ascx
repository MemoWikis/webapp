<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DateRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% var date = Model.Date; %>

<div class="rowBase date-row" style="position: relative; padding: 5px; "
    data-date-id="<%= date.Id %>"
    data-weak="<%= Model.KnowledgeWeak %>"
    data-unknown="<%= Model.KnowledgeUnknown %>"
    data-secure="<%= Model.KnowledgeSecure %>">
    
    <div class="row">

        <div class="col-sm-2" style="">
            <div class="row">
                <div class="col-xs-4 col-md-12" style="color: silver; font-size: 16px; font-weight: 400; padding: 2px 0 0 12px;">
                    <%
                        if(Model.IsPast){
                            Response.Write("Vorbei seit <br>");
                            Response.Write(Model.ShowMinutesLeft ? "Minuten:" : "Tagen:");
                        }else { 
                            Response.Write(Model.ShowMinutesLeft ? "Minuten" : "Noch Tage");
                        }
                    %>
                </div>
                <div class="col-xs-4 col-md-12" style="margin-bottom: -11px;">
                    <span style="font-size: 48px; position: relative; top:-7px;">
                        <%= Model.ShowMinutesLeft ?
                            String.Format("{0:00}", Model.RemainingMinutes) : 
                            String.Format("{0:00}", Model.RemainingDays)
                        %>
                    </span>
                </div>                
                <div class="col-md-12 " style="color: darkgrey; font-weight: bolder;  position: relative; left: 0px;">
                    <% if(!Model.IsPast){ %>
                        <span style="font-size: 11px">bis Termin am <br/></span> 
                    <% } %>
                    <span style="font-size: 11px;">
                        <%= date.DateTime.ToString("dd.MM.yyy HH:mm") %>
                    </span>
                </div>
            </div>
        </div>

        <div class="col-sm-7">
            <div class="row">
                <div class="col-md-9 header" style="font-size: 19px">
                    <%= Model.Date.Details %>
                </div>
                
                <div class="col-md-3 hidden-xs hidden-sm" style="text-align: right; vertical-align: bottom">
                    <div style="font-size: 13px; margin-top: 7px;">
                        <a href="<%= Links.DateEdit(Url, date.Id) %>"><i class="fa fa-pencil"></i></a>
                        &nbsp;
                        <a data-toggle="modal" data-dateId="<%= date.Id %>" href="#modalDelete">
                            <i class="fa fa-trash-o"></i>
                        </a>
                    </div>
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
                    <i class="fa fa-files-o"></i> 0x kopiert (keine Reputationspunkte)
                </div>
            </div>
    
            <div class="row">
                <div class="col-sm-4">
                    <a href="#" class="show-tooltip" data-original-title="Spiel mit Fragen aus diesem Termin starten."
                        style="display: block; margin-top: 29px;">
                        <i class="fa fa-gamepad" style="font-size: 18px;"></i>
                        Spiel starten
                    </a>                    
                </div>
                <div class="col-sm-8" style="text-align: right;">
                    <div style="margin-top: 20px;">
                        <a class="btn btn-sm btn-info" href="#">
                            Jetzt üben
                        </a>
                        <a class="btn btn-sm btn-primary" href="#"><i class="fa fa-lightbulb-o"></i> 
                            Jetzt testen
                        </a>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-3">
            <div id="chartKnowledgeDate<% =date.Id %>"></div>            
        </div>
    </div>
</div>