<%@ Page Title="Drafts: Forms" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<WelcomeModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Questions/Questions.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/questions") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div style="margin-top: 20px;">
        <h4>Autocomplete</h4>
        <form>
            <div class="form-horizontal">
                <div class="form-group" style="margin-bottom: 15px;">
                    <label class="RequiredField columnLabel control-label" for="">
                        Ausgabe
                    </label>
                    <div class="JS-RelatedCategories columnControlsFull">
                        <div class="JS-CatInputContainer">
                            <input id="TxtDailyIssue" class="form-control" name="" type="text" value="" placeholder="Suche nach Datum">    
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="noLabel columnControlsFull">
                        <input class="form-control" name="" type="text" value="" placeholder="Suche nach Titel oder ISSN">
                        <ul class="ui-autocomplete ui-front ui-menu ui-widget ui-widget-content ui-corner-all" id="ui-id-1" tabindex="0" style="display: block; position: static; width: 330px;">
                            <li class="ui-menu-item" role="presentation">
                                <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                                    <img src="/Images/no-category-picture-50.png">
                                    <div class="CatDescription">
                                        <span class="" style="font-weight: bold;">Der Spiegel</span>
                                        <span class="NumberQuestions">ISSN: 0038-7452</span>
                                    </div>
                                </a>
                            </li>
                            <li class="ui-menu-item" role="presentation">
                                <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                                    <div class="CatDescription">
                                        <span class="" style="font-weight: bold;">Kein Treffer:</span>
                                        <span class="NumberQuestions">Zeitung in neuem Tab anlegen</span>
                                    </div>
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="form-group">
                    <label class="columnLabel control-label" for="xxx">
                        Ausgabe
                    </label>
                    <div class="columnControlsFull">
                        <input class="form-control" name="" type="text" value="" placeholder="Suche nach Titel oder ISSN">
                        <ul class="ui-autocomplete ui-front ui-menu ui-widget ui-widget-content ui-corner-all" id="ui-id-1" tabindex="0" style="display: block; position: static; width: 330px;">
                            <li class="ui-menu-item" role="presentation">
                                <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                                    <img src="/Images/no-category-picture-50.png">
                                    <div class="CatDescription">
                                        <span class="" style="font-weight: bold;">Nr. 52/2013</span>
                                        <span class="NumberQuestions">30.12.2013</span>
                                    </div>
                                </a>
                            </li>
                            <li class="ui-menu-item" role="presentation">
                                <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                                    <div class="CatDescription">
                                        <span class="" style="font-weight: bold;">Kein Treffer:</span>
                                        <span class="NumberQuestions">Ausgabe in neuem Tab anlegen</span>
                                    </div>
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class="form-group">
                    <label class="RequiredField columnLabel control-label" style="font-weight: bold;" for="xxx">
                        Zeitschrift
                    </label>
                    <div class="columnControlsFull">
                        <input class="form-control" name="" type="text" value="" placeholder="Suche nach Titel oder ISSN">
                        <ul class="ui-autocomplete ui-front ui-menu ui-widget ui-widget-content ui-corner-all" id="ui-id-1" tabindex="0" style="display: block; position: static; width: 330px;">
                            <li class="ui-menu-item" role="presentation">
                                <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                                    <img src="/Images/no-category-picture-50.png">
                                    <div class="CatDescription">
                                        <span class="" style="font-weight: bold;">Der Spiegel</span>
                                        <span class="NumberQuestions">ISSN: 0038-7452</span>
                                    </div>
                                </a>
                            </li>
                            <li class="ui-menu-item" role="presentation">
                                <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                                    <div class="CatDescription">
                                        <span class="" style="font-weight: bold;">Kein Treffer:</span>
                                        <span class="NumberQuestions">Zeitung in neuem Tab anlegen</span>
                                    </div>
                                </a>
                            </li>
                        </ul>
                    </div>
                </div>
                <div>
                    <ul class="ui-autocomplete ui-front ui-menu ui-widget ui-widget-content ui-corner-all" id="ui-id-1" tabindex="0" style="display: block; position: static; width: 330px;">
                        <li class="ui-menu-item" role="presentation">
                            <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                                <img src="/Images/no-category-picture-50.png">
                                <div class="CatDescription">
                                    <span class="" style="font-weight: bold;">Der Spiegel</span>
                                    <span class="NumberQuestions">ISSN: 0038-7452</span>
                                </div>
                            </a>
                        </li>
                        <li class="ui-menu-item" role="presentation">
                            <a class="CatListItem ui-corner-all" id="ui-id-39" tabindex="-1">
                                <div class="CatDescription">
                                    <span class="" style="font-weight: bold;">Kein Treffer:</span>
                                    <span class="NumberQuestions">Zeitung in neuem Tab anlegen</span>
                                </div>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </form>
    </div>
          
</asp:Content>