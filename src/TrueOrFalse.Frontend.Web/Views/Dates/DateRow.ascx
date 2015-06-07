<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DateRowModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% var date = Model.Date; %>

<div class="rowBase date-row" style="position: relative; padding: 5px; "
    data-date-id="<%= date.Id %>"
    data-weak="<%= Model.KnowledgeWeak %>"
    data-unknown="<%= Model.KnowledgeUnknown %>"
    data-secure="<%= Model.KnowledgeSecure %>">
    
    <div class="row">
        <div class="col-md-9">
            <div class="row">
                <div class="col-md-9 header" style="font-size: 19px">
                    Noch 4 Tage <span style="font-size: 13px;">bis zum 4.7.2015 18:21</span>
                </div>
                
                <div class="col-md-3 hidden-xs hidden-sm" style="text-align: right; vertical-align: bottom">
                    <div style="font-size: 13px; margin-top: 7px;">0x kopiert</div>
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
                    <%= date.Details %>
                </div>
            </div>
    
            <div class="row">
                <div class="col-md-12" style="text-align: right; margint-to: 7px;">
                    <a href="#">
                        <i class="fa fa-gamepad"></i>
                        Spiel starten
                    </a>

                    <a class="btn btn-info" href="#">
                        Jetzt üben
                    </a>
                    <a class="btn btn-primary" href="#"><i class="fa fa-lightbulb-o"></i> 
                        Jetzt testen
                    </a>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div id="chartKnowledgeDate<% =date.Id %>"></div>            
        </div>
    </div>
</div>