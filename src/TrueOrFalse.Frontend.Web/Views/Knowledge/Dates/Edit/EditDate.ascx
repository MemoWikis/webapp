﻿<%@ Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EditDateModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

    <% if (Model.IsEditing) { %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.DateEdit(Model.DateId) %>">
    <% } else {  %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.DateCreate() %>">
    <% } %>
          
<%= Styles.Render("~/bundles/EditDate") %>
    
<% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditDate", null, 
    FormMethod.Post, new { enctype = "multipart/form-data", id="EditDateForm"})){%>
    
    <input type="hidden" name="DateId" value="<%= Model.DateId %>"/>

    <div class="row">
        <div class="PageHeader col-xs-12">
            <h2 class="pull-left">
                <span class="ColoredUnderline Play">
                    <% if(Model.IsEditing){ %>
                        Termin bearbeiten
                    <% } else { %>
                        Termin erstellen
                    <% } %>
                </span>
            </h2>
            <div class="headerControls pull-right">
                <div>
                    <a href="<%= Links.Dates() %>" style="font-size: 12px; margin: 0;">
                        <i class="fa fa-list"></i>&nbsp;zur Übersicht
                    </a>
                </div>
            </div>
        </div>
    </div>
        
        <div class="row">
            <div class="col-md-9">
                <% if(!Model.IsLoggedIn){ %>
                    <div class="bs-callout bs-callout-danger" style="margin-top: 0;">
                        <h4>Einloggen oder registrieren</h4>
                        <p>
                            Um Termine zu erstellen,
                            musst du dich <a href="#" data-btn-login="true">einloggen</a> oder <a href="/Registrieren">registrieren</a>.
                        </p>
                    </div>
                <% }%>
            </div>            
        </div>
        
        <div class="row">
            <div class="col-md-9 xxs-stack">
                <% Html.Message(Model.Message); %>
            </div>
            
            <div class="col-md-9">
                <div class="form-horizontal">
                    <div class="FormSection">
                        
                        <div class="form-group">
                            <%--<%= Html.LabelFor(m => m.Details, new { @class = "columnLabel control-label" })%>--%>
                            <label class="columnLabel control-label">
                                Name des Termins:
                            </label>
                            <div class="columnControlsFull">
                                <%= Html.TextBoxFor(m => m.Details, new {@class="form-control", placeholder = "(z.B. Test Geografie)"}) %>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-xs-5 col-sm-4 xxs-stack">
                                <div class="form-group">
                                    <label class="columnLabel control-label">
                                        Datum
                                    </label>
                                    <div class="col-md-11">
                                        <div class="input-group date">
                                            <input class="form-control" name="Date" value="<%= Model.Date.ToString("dd.MM.yyyy") %>" style="height: 30px;" />
                                            <span class="input-group-addon" style="height: 30px;">
                                                <i class="fa fa-calendar"></i>
                                            </span>
                                        </div>
                                    </div>
                                </div>                                
                            </div>
                            <div class="col-xs-6 xxs-stack">
                                <div class="form-group" >
                                    <label class="columnLabel control-label">
                                        Uhrzeit
                                    </label>
                                    <div class="col-md-10">
                                        <div class="input-group clockpicker" data-autoclose="true">
                                            <input class="form-control" name="Time" value="<%= Model.Time %>" style="height: 30px;" />
                                            <span class="input-group-addon" style="height: 30px;">
                                                <i class="fa fa-clock-o"></i>
                                            </span>
                                        </div>
                                    </div>
                                </div>                                
                            </div>
                        </div>

                        <div class="form-group" style="margin-bottom: 5px;" >
                            <label class="columnLabel control-label">
                                Lernsets, die du zu diesem Termin lernen möchtest:
                            </label>
                            <div class="JS-Sets columnControlsFull">
                                <script type="text/javascript">
                                    $(function () {
                                        <%foreach (var set in Model.Sets) { %>
                                        $("#txtSet")
                                            .val('<%=set.Name %>')
                                            .data('set-id', '<%=set.Id %>')
                                            .trigger("initSetFromTxt");
                                        <% } %>
                                        InitLabelTooltips();
                                    });
                                </script>
                                <div class="JS-SetInputContainer ControlInline ">
                                    <input id="txtSet" class="form-control .JS-ValidationIgnore" type="text" placeholder="Beginne zu tippen"  />
                                </div>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="columnLabel control-label">
                                <span class="show-tooltip" data-toggle="tooltip" title="Termine, die in deinem Netzwerk sichtbar sind, können von deinen Freunden einfach übernommen werden. Dafür erhälst du Reputationspunkte.">Sichtbarkeit des Termins&nbsp;<i class='fa fa-info-circle'></i></span>
                            </label>
                            <div class="columnControlsFull">
                                <select class="form-control" name="Visibility">
                                    <option value="inNetwork" <%= Model.Selected("inNetwork") %>>Sichtbar in deinem Netzwerk</option>
                                    <option value="private" <%= Model.Selected("private") %>>Privat (nur für dich sichtbar)</option>
                                </select>
                            </div>
                        </div>
                        
                        <div class="form-group" id="safeDates">
                            <div class="noLabel columnControlsFull">
                                <button href="#" id="btnSave" class="btn btn-primary" v-on:click="setDate($event)">
                                    <%= Model.IsEditing ? "Termin bearbeiten" :  "Termin erstellen " %>
                                </button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>


    <%= Scripts.Render("~/bundles/js/EditDate") %>
    
    <script type="text/javascript">
        $(function() {
            $('.clockpicker').clockpicker();

            $('.input-group.date').datepicker({
                language: "de",
                calendarWeeks: true,
                autoclose: true,
                todayHighlight: true,
                startDate: new Date().toString()
            });
        });
    </script>

<% } %>
