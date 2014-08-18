<%@ Page Title="Frage erstellen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="ViewPage<EditQuestionModel>" ValidateRequest="false" %>

<%@ Import Namespace="System.Web.Mvc.Html" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="TrueOrFalse" %>
<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">
    <link href="/Views/Questions/Edit/EditQuestion.css" rel="stylesheet" />
    <link type="text/css" href="/Content/blue.monday/jplayer.blue.monday.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/markdown") %>
    <%= Scripts.Render("~/bundles/questionEdit") %>
    <%= Scripts.Render("~/bundles/fileUploader") %>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">
    
<input type="hidden" id="questionId" value="<%= Model.Id %>"/>
<input type="hidden" id="urlSolutionEditBody" value="<%=Url.Action("SolutionEditBody", "EditQuestion") %>" />
    
<% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditQuestion", null, FormMethod.Post, new { id="EditQuestionForm", enctype = "multipart/form-data", style="margin:0px;" })){ %>
    <div class="row">
        <div class="PageHeader col-xs-12">
            <h2 class="pull-left"><span class="ColoredUnderline"><%=Model.FormTitle %></span></h2>
            
            <div class="headerControls pull-right">
                <div>
                    <a href="<%= Url.Action(Links.Questions, Links.QuestionsController) %>" class="SimpleTextLink" style="font-size: 12px;
                        margin: 0;"><i class="fa fa-list"></i> <span class="TextSpan">zur Übersicht</span></a>
                </div>
                <% if (!Model.ShowSaveAndNewButton){ %>
                    <div style="line-height: 12px">
                        <a href="<%= Links.CreateQuestion(Url) %>" style="font-size: 12px;
                            margin: 0px;"><i class="fa fa-plus-circle"></i> Frage erstellen</a>
                    </div>
                <%} %>
                    
                <% if(Model.IsEditing){ %>
                    <div style="line-height: 12px; padding-top: 3px;">
                        <a href="<%= Links.AnswerQuestion(Url, Model.Question, (int)Model.Id) %>" style="font-size: 12px;
                            margin: 0px;"><i class="fa fa-check-square"></i> Frage beantworten</a>
                    </div>                    
                <% } %>
            </div>
        </div>
    </div>
        

    <div class="row">
        <div class="aside col-md-3 col-md-push-9" style="margin-bottom: 11px;">
            <div class="form-horizontal" role="form">
                <div class="FormSection">
                    <div class="form-group">
                        <label for="Visibility" class="columnLabel labelVisibility control-label">Sichtbar</label>

                        <div class="columnControlsFull">
                            <div class="radio">
                                <label style="font-weight: normal">
                                    <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.All)%>
                                        für alle <span class="smaller">(öffentliche Frage)</span>
                                        <i class="fa fa-question-circle show-tooltip" title="" data-placement="right" 
                                           data-html="true"
                                           data-original-title="
                                            <ul class='show-tooltip-ul'>
                                                <li>Die Frage ist für alle auffindbar.</li>
                                                <li>Jeder kann die Frage in sein Wunschwissen aufnehmen.</li>
                                            </ul>">
                                        </i>
                                        <br/>

                                </label>
                            </div>
                            <div class="radio">
                                <label style="font-weight: normal">
                                    <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.Owner)  %>
                                        für mich <span class="smaller">(<i class="fa fa-lock"></i> private Frage)</span> &nbsp;&nbsp;
                                        (Die Frage ist nur von Dir nutzbar.)
                                        <i class="fa fa-question-circle show-tooltip tooltip-width-200" title="" data-placement="right" 
                                           data-html="true"
                                           data-original-title="
                                            <ul class='show-tooltip-ul'>
                                                <li>Die Frage kann nur von Dir genutzt werden.</li>
                                                <li>Niemand anders hat jemals Zugriff auf die Frage.</li>
                                            </ul>">
                                        </i>

                                </label>
                            </div>
                            <div class="radio">
                                <label style="font-weight: normal">
                                    <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.OwnerAndFriends, new { disabled = true} )  %>
                                    für mich und meine Freunde<br/> <span class="smaller">(private Frage <i class="fa fa-lock"></i>)</span>
                                </label>
                            </div>
                            <div style="background-color: lavender; padding: 0 10px;">
                                0 von 30 privaten Fragen verwendet.
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-9 col-md-pull-3">
            <div class="form-horizontal" role="form">
                
                <% if(!Model.IsLoggedIn){ %>
                    <div class="bs-callout bs-callout-info" style="margin-top: 0;">
                        <h4>Anmelden oder registrieren</h4>
                        <p>
                            Um Fragen zu erstellen, <br/>
                            musst du dich <a href="/Anmelden">anmelden</a> oder dich <a href="/Registrieren">registrieren</a>.
                        </p>
                    </div>
                <% }%>

                <div>
                    <% Html.Message(Model.Message); %>
                </div>
                <div class="FormSection">

                <div class="form-group">
                    <%= Html.LabelFor(m => m.Question, new { @class = "columnLabel control-label" })%>
                    <div class="columnControlsFull">
                        <%= Html.TextAreaFor(m => m.Question, new { @class="form-control", placeholder = "Bitte gib den Fragetext ein", rows = 3})%>
                        <div style="padding-top: 4px;">
                            <a href="#" id="openExtendedQuestion" class="SimpleTextLink"><i class="fa fa-plus-circle"></i> <span class="TextSpan">Erweiterte Beschreibung (z.B.: mit Bildern, Formeln oder Quelltext)</span></a> 
                        </div>    
                    </div>
                </div>
               
               

                <div class="form-group markdown" style="display: none" id="extendedQuestion">
                    <%= Html.LabelFor(m => m.QuestionExtended, new { @class = "columnLabel control-label" })%>
                    <div class="columnControlsFull">
                        <div class="wmd-panel">
                            <div id="wmd-button-bar-1"></div>
                            <%= Html.TextAreaFor(m => m.QuestionExtended, new 
                                { @class= "wmd-input form-control", id="wmd-input-1", placeholder = "Erweiterte Beschreibung", rows = 4 })%>
                        </div>                            
                        <div id="wmd-preview-1" class="wmd-panel wmd-preview"></div>
                    </div>
                </div>

                <%--
                <div class="form-group">
                    <% if (!String.IsNullOrEmpty(Model.SoundUrl)){
                            Html.RenderPartial("AudioPlayer", Model.SoundUrl); } %>
                    <label for="soundfile" class="control-label">Ton:</label>
                    &nbsp;&nbsp;<input type="file" name="soundfile" id="soundfile" />
                </div>--%>
                <div class="form-group">    
                    <label class="columnLabel control-label">
                        <span class="show-tooltip" data-toggle="tooltip" title = "Kategorien helfen bei der Einordnung der Frage u. ermöglichen Dir und anderen die Fragen wiederzufinden." data-placement = "top">Kategorien</span>
                    </label>

                    <div class="JS-RelatedCategories columnControlsFull">
                        <script type="text/javascript">
                            $(function () {
                                <%foreach (var category in Model.Categories) { %>
                                $("#txtNewRelatedCategory")
                                    .val('<%=category.Name %>')
                                    .data('category-id', '<%=category.Id %>')
                                    .trigger("initCategoryFromTxt");
                                <% } %>
                            });
                        </script>
                        <div class="JS-CatInputContainer"><input id="txtNewRelatedCategory" class="form-control" type="text" placeholder="Wähle eine Kategorie" /></div>
                    </div>
                </div>
                </div>
                <div class="FormSection">
                    <div class="form-group">
                        <%= Html.LabelFor(m => m.SolutionType, new { @class = "columnLabel control-label" }) %>
                        <div class="columnControlsSmall">
                            <%= Html.DropDownListFor(m => Model.SolutionType, Model.AnswerTypeData, new {@id = "ddlAnswerType", @class="form-control"})%>
                        </div>
                    </div>
                
                    <div id="answer-body"></div>
                </div>
                <div class="FormSection">

                    <div class="form-group markdown">
                        <label class="columnLabel control-label">
                            <span class="show-tooltip"  title = "Je ausführlicher die Erklärung, desto besser! Verwende Links u. Bilder aber achte auf die Urheberrechte." data-placement = "top">Erklärungen und Ergänzungen</span>
                        </label>
                        <div class="columnControlsFull">
                            <div class="wmd-panel">
                                <div id="wmd-button-bar-2"></div>
                                <%= Html.TextAreaFor(m => m.Description, new 
                                    { @class= "form-control wmd-input", id="wmd-input-2", placeholder = "Erklärung der Antwort und Quellen.", rows = 4 })%>
                            </div>
                            <div id="wmd-preview-2" class="wmd-panel wmd-preview"></div>
                        </div>
                    </div>
                    
                    <div class="form-group">
                        <%--<label class="columnLabel control-label">Frage</label>--%>

                        <label class="columnLabel control-label">Quellen <br/>
                            <div style="font-weight: normal">(Gute Quellen machen gute Fragen/Antworten noch besser!)</div>
                        </label>
                    
                        <div id="JS-References" class="columnControlsFull"></div>
                    </div>
                    <%--<div class="form-group">
                        <label class="columnLabel control-label" for="">Ergänzungen zur Quelle</label>
                        <div class="columnControlsFull">
                            <input type="text" placeholder="Seitenangaben etc."/>
                        </div>
                    </div>--%>
                
                    <div id="AddReferenceControls" class="form-group">
                        <div class="noLabel columnControlsFull">
                            <label class="control-label LabelInline">Quelle vom Typ</label> 
                                <div class="ControlInline">
                                    <select id="ReferenceType" class="form-control">
                                        <option value="Book"><%= CategoryType.Book.GetName() %></option>                
                                        <option value="Article">Artikel</option>                
                                        <option value="VolumeChapter"><%= CategoryType.VolumeChapter.GetName() %></option>                
                                        <option value="WebsiteArticle"><%= CategoryType.WebsiteArticle.GetName() %></option>
                                        <option>Freitext-Url</option> 
                                        <option>Freitext</option> 
                                    </select>
                                </div>
                            <button class="btn" id="AddReference">hinzufügen</button>
                        </div>
                    </div>
                </div>
                <div class="FormSection">
                    <div class="form-group">
                        <div class="noLabel columnControlsFull">
                            <div class="checkbox">
                                <label>
                                    <%= Html.CheckBoxFor(x => x.ConfirmContentRights) %>
                                    Ich stelle diesen Eintrag unter eine LGPL Lizenz. 
                                    Der Eintrag kann ohne Einschränkung weiter genutzt werden, 
                                    wie zum Beispiel bei Wikipedia-Einträgen. 
                                    <a href="" target="_blank">mehr erfahren</a> <br />
                                    Die Frage und Anwort sind meine eigene Arbeit und
                                    nicht aus urheberrechtlich geschützten Quellen kopiert. 
                                    <a href="" target="_blank">mehr erfahren</a>
                                </label>
                            </div>
                        </div>
                    </div>
                <% if(Model.IsLoggedIn){ %>
                    <div class="form-group">
                        <div class="noLabel columnControlsFull">
                            <button type="submit" class="btn btn-primary" name="btnSave" value="save">Speichern</button>&nbsp;&nbsp;&nbsp;
                            <% if (Model.ShowSaveAndNewButton){ %>
                                <button type="submit" class="btn btn-default" name="btnSave" value="saveAndNew" >Speichern & Neu</button>&nbsp;
                            <% } %>                        
                        </div>
                    </div>
                </div>
                <% } %>
            </div>
        </div>
    </div>
    <% } %>
    
    <% Html.RenderPartial("../Shared/ImageUpload/ImageUpload"); %>

</asp:Content>