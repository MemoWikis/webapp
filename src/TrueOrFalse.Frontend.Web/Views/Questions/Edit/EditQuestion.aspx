<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<EditQuestionModel>" ValidateRequest="false" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="ContentHeadSEO" ContentPlaceHolderID="HeadSEO" runat="server">
    <% Title = Model.PageTitle; %>
    <% if (Model.IsEditing) { %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.EditQuestion(Model.QuestionText, Model.Id) %>">
        <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Fragen", Url = "/Fragen", ImageUrl = "fa-search"});
           Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
    <% } else {  %>
        <link rel="canonical" href="<%= Settings.CanonicalHost %><%= Links.CreateQuestion() %>">
        <% Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem{Text = "Fragen", Url = "/Fragen/Erstelle", ImageUrl = "fa-search"});
           Model.TopNavMenu.IsCategoryBreadCrumb = false; %>
    <% } %>
</asp:Content>

<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">
    <link href="/Views/Questions/Edit/EditQuestion.css" rel="stylesheet" />
    <link type="text/css" href="/Content/blue.monday/jplayer.blue.monday.css" rel="stylesheet" />
    <script type="text/javascript" src="/Scripts/vendor/jquery-watch.js"></script>
    <%= Scripts.Render("~/bundles/markdown") %>
    <%= Scripts.Render("~/bundles/questionEdit") %>
    <%= Scripts.Render("~/bundles/fileUploader") %>
</asp:Content>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">

<% using (Html.BeginForm(Model.IsEditing ? "Edit" : "Create", "EditQuestion", null, FormMethod.Post, new { id="EditQuestionForm", enctype = "multipart/form-data", style="margin:0px;" })){ %>
    
    <input type="hidden" id="hddReferencesJson" name="hddReferencesJson"/>
    <input type="hidden" id="questionId" name="questionId" value="<%= Model.Id %>"/>
    <input type="hidden" id="isEditing" name="isEditing" value="<%= Model.IsEditing %>"/>
    <input type="hidden" id="urlSolutionEditBody" value="<%=Url.Action("SolutionEditBody", "EditQuestion") %>" />
    
    <% if(Model.Set != null){ %>
        <input type="hidden" id="hddSetId" name="hddSetId" value="<%= Model.Set.Id %>"/>
    <% } %>

    <div class="row">
        <div class="PageHeader col-xs-12">
            <h2 class="pull-left"><span class="ColoredUnderline"><%=Model.FormTitle %></span></h2>
            
            <div class="headerControls pull-right">
                <div>
                    <a href="<%= Url.Action(Links.Questions, Links.QuestionsController) %>" class="TextLinkWithIcon"
                        style="font-size: 12px; margin: 0;"><i class="fa fa-list"></i> <span class="TextSpan">zur Übersicht</span></a>
                </div>
                <% if (!Model.ShowSaveAndNewButton){ %>
                    <div style="line-height: 12px">
                        <a href="<%= Links.CreateQuestion() %>" style="font-size: 12px;
                            margin: 0;"><i class="fa fa-plus-circle"></i> Frage erstellen</a>
                    </div>
                <%} %>
                    
                <% if(Model.IsEditing){ %>
                    <div style="line-height: 12px; padding-top: 3px;">
                        <a href="<%= Links.AnswerQuestion(Model.Question) %>" style="font-size: 12px;
                            margin: 0;"><i class="fa fa-check-square"></i> Frage beantworten</a>
                    </div>                    
                <% } %>
            </div>
        </div>
        <div class="PageHeader col-xs-12">
            <% if(!Model.IsLoggedIn){ %>
                <div class="bs-callout bs-callout-danger" style="margin-top: 0;">
                    <h4>Einloggen oder registrieren</h4>
                    <p>
                        Um Fragen zu erstellen, <br/>
                        musst du dich <a href="#" data-btn-login="true">einloggen</a> oder <a href="/Registrieren">registrieren</a>.
                    </p>
                </div>
            <% } %>
            <div>
                <% Html.Message(Model.Message); %>
            </div>
        </div>
    </div>
        
    <% if(Model.IsLoggedIn){ %>
        <div class="row">
            <div class="aside col-md-3 col-md-push-9" style="margin-bottom: 11px;">
                <div class="form-horizontal rowBase" role="form">
                    <div class="FormSection">
                        <div class="form-group">
                            <label for="Visibility" class="columnLabel labelVisibility control-label">Sichtbarkeit</label>

                            <div class="columnControlsFull">
                                <div class="radio">
                                    <label style="font-weight: normal">
                                        <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.All)%>
                                            Öffentliche Frage
                                            <i class="fa fa-question-circle show-tooltip" title="" data-placement="<%= CssJs.TooltipPlacementLabel %>" 
                                               data-html="true"
                                               data-original-title="
                                                    <ul class='show-tooltip-ul'>
                                                        <li>Die Frage ist für alle auffindbar.</li>
                                                        <li>Jeder kann die Frage in sein Wunschwissen aufnehmen.</li>
                                                        <li>Die Frage steht unter einer Creative-Commons-Lizenz und kann frei weiterverwendet werden.</li>
                                                    </ul>">
                                            </i>
                                            <br/>
                                    </label>
                                </div>
                                <div class="radio">
                                    <label style="font-weight: normal">
                                        <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.Owner)  %>
                                            <i class="fa fa-lock"></i> Private Frage
                                            <i class="fa fa-question-circle show-tooltip tooltip-min-200" title="" data-placement="top" 
                                               data-html="true"
                                               data-original-title="
                                                    <ul class='show-tooltip-ul'>
                                                        <li>Die Frage kann nur von dir genutzt werden.</li>
                                                        <li>Niemand sonst kann die Frage sehen oder nutzen.</li>
                                                    </ul>">
                                            </i>

                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-9 col-md-pull-3">
                <div class="form-horizontal rowBase" role="form">
                    <div class="FormSection">
                        <div class="form-group" id="formGroupQuestionText">
                            <%= Html.LabelFor(m => m.QuestionText, new { @class = "RequiredField columnLabel control-label" })%>
                            <div class="columnControlsFull">
                            <%--<div class="columnControls3of4">--%>
                                <%= Html.TextAreaFor(m => m.QuestionText, new { @class="form-control", placeholder = "Bitte gib den Fragetext ein", rows = 3})%>
                                <%: Html.ValidationMessageFor(model => model.QuestionText) %>
                            </div>
    <%--                    </div>
                        <div class="form-group">--%>
                            <div id="OpenImageUpload" class="columnControlsFull" style="padding-top: 4px;">
                                <a href="#" class="TextLinkWithIcon"><i class="fa fa-file-image-o"></i> <span class="TextSpan">Bild hinzufügen</span></a>: 
                                Bitte füge nur ein Bild hinzu, wenn es für die Beantwortung der Frage notwendig ist (keine rein illustrierenden Bilder).
                            </div>
                        </div>
                        <div class="form-group markdown" id="openExtendedQuestion">
                            <label class="columnLabel control-label" for="QuestionExtended" style="max-width: 420px;">
                                <a id="openExtendedQuestion" href="#" class="TextLinkWithIcon">
                                    <i class="fa fa-caret-right"></i>
                                    <span class="show-tooltip" data-toggle="tooltip" title="Hier kannst du z.B. Erläuterungen, Formeln, Quelltext oder eine Bildunterschrift hinzufügen." data-placement = "<%= CssJs.TooltipPlacementLabel %>">Erweiterte Beschreibung&nbsp;</span>
                                </a>
                            </label>
                        </div>
                        <div class="form-group markdown" style="display: none" id="extendedQuestion">
                            <label class="columnLabel control-label" for="QuestionExtended" style="max-width: 420px;">
                                <i class="fa fa-caret-down"></i> Erweiterte Beschreibung
                                <a id="hideExtendedQuestion" href="#"> <span class="TextSpan">(ausblenden)</span></a>
                            </label>
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

                        <% if(Model.Set != null){ %>
                            <div class="row" id="RowAssignSet">
                                <div class="col-lg-12" style="padding-bottom: 10px">
                                    Im Lernsets: <a href="<%= Links.SetDetail(Url, Model.Set) %>"><span class="label label-set show-tooltip" data-placement="top" data-original-title="Zum Lernset"><%= Model.Set.Name %></span></a> 
                                    <a href="#" id="RemoveSet" style="margin-left: 2px;" class="show-tooltip" data-placement="top" data-original-title="Lernset nicht zuordnen"><img alt="" src="/Images/Buttons/cross.png"></a>
                                </div>
                            </div>
                        <% } %>

                        <div class="form-group">
                            <label class="columnLabel control-label">
                                <span class="show-tooltip" data-toggle="tooltip" title="Themen helfen bei der Einordnung der Frage und ermöglichen dir und anderen die Fragen wiederzufinden. Tipp: Falls du ein gesuchtes Thema nicht findest, kannst du es in einem neuen Tab anlegen und dann einfach hier weitermachen." data-placement = "<%= CssJs.TooltipPlacementLabel %>">Themen</span>
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
                                <div class="JS-CatInputContainer ControlInline"><input id="txtNewRelatedCategory" class="form-control" type="text" placeholder="Wähle ein Thema" /></div>
                            </div>
                        </div>
                    </div>
                    <div class="FormSection">
                        <div class="form-group">
                            <label class="columnLabel control-label" for="SolutionType">
                                <span <%= Model.IsEditing ? "class='show-tooltip' title='Der Antworttyp kann nach dem ersten Speichern der Frage leider nicht mehr verändert werden.' data-placement ='"+ CssJs.TooltipPlacementLabel + "'" : ""%>>Antworttyp</span>
                            </label>
                            <div id="SolutionTypeContainer" <%= Model.IsEditing ? "class='columnControlsFull show-tooltip' data-toggle='tooltip' title='Der Antworttyp kann nach dem ersten Speichern der Frage leider nicht mehr verändert werden.' data-placement ='"+ CssJs.TooltipPlacementLabel + "'" : "class='columnControlsFull'"%>>
                                <%= Html.DropDownListFor(m => 
                                        Model.SolutionType,
                                        Model.AnswerTypeData,
                                        Model.IsEditing ? 
                                            (object)new 
                                            {
                                                @id = "ddlAnswerType", @class="form-control",
                                                disabled="disabled"
                                            } :
                                            new
                                            {
                                                @id = "ddlAnswerType", @class="form-control",
                                            })%>
                                    <%--http://stackoverflow.com/questions/23159003/optionally-disable-element-rendered-via-mvc#answer-23159114--%>
                            </div>
                            <% if(Model.IsEditing){ %>
                                <input type="hidden" name="SolutionType" value="<%= Model.SolutionType %>"/>
                            <% } %>
                        </div>
                
                        <div id="answer-body"></div>
                    </div>
                    <div class="FormSection">

                        <div class="form-group markdown" id="formGroupDescription">
                            <label class="columnLabel control-label" for="Description">
                                <span class="show-tooltip"  title = "Erscheinen nach dem Beantworten der Frage zusammen mit der richtigen Lösung und sollen beim Einordnen und Merken der abgefragten Fakten helfen. Oft wird eine Frage erst durch informative Zusatzangaben so richtig gut." data-placement = "<%= CssJs.TooltipPlacementLabel %>">Ergänzungen</span>
                            </label>
                            <div class="columnControlsFull">
                                <div class="wmd-panel">
                                    <div id="wmd-button-bar-2"></div>
                                    <%= Html.TextAreaFor(m => m.Description, new 
                                        { @class= "form-control wmd-input", id="wmd-input-2", placeholder = "Erklärungen, Zusatzinfos, Merkhilfen, Abbildungen, weiterführende Literatur und Links etc.", rows = 6 })%>
                                </div>
                                <div id="wmd-preview-2" class="wmd-panel wmd-preview"></div>
                            </div>
                        </div>
                    
                        <div class="form-group" style="margin-bottom: 0;">
                            <label class="columnLabel control-label"> 
                                <span class="show-tooltip" data-toggle="tooltip" title = "Bitte belege die von dir angeführten Fakten mit Quellen und mache wörtliche und auch indirekte Zitate als solche erkennbar. Bitte gehe sparsam mit wörtlichen Zitaten um und formuliere wenn möglich mit eigenen Worten. Du kannst in Frage, Anwort und Ergänzungen auf die hier eingefügten Quellen verweisen." data-placement = "<%= CssJs.TooltipPlacementLabel %>">
                                    Quellen
                                </span>
                            </label>
                    
                            <div id="JS-References" class="columnControlsFull">
                                <script type="text/javascript">
                                    <%if(Model.References.Count != 0) {%>
                                        $(function () {
                                            $("#AddReference").trigger('click');
                                            var catIds = new Array();
                                            <% for (var i = 0; i < Model.References.Count; i++){ %>
                                                <% switch (Model.References[i].ReferenceType){

                                                    case ReferenceType.MediaCategoryReference: %>
                                                        $(window).bind('referenceAdded<%= Model.References[i].Id %>', function() {
                                                            $('.JS-ReferenceContainer[data-ref-id="' + '<%= Model.References[i].Id %>' + '"]')
                                                                .find('.InputRefAddition')
                                                                .val('<%= Model.References[i].AdditionalInfo %>');
                                                        });

                                                        catIds[<%= i%>] = <%= Model.References[i].Category.Id %>;
                                                        <% break;

                                                    case ReferenceType.FreeTextreference: %>
                                                        $(window).bind('referenceAdded' + '<%= Model.References[i].Id %>', function() {
                                                            $('.JS-ReferenceContainer[data-ref-id="' + '<%= Model.References[i].Id %>' + '"]').find('.ReferenceText').val('<%= Model.EscapedReferences[i].ReferenceText %>');
                                                        });
                                                        catIds[<%= i%>] = -1;
                                                        <% break;

                                                    case ReferenceType.UrlReference: %>
                                                        $(window).bind('referenceAdded' + '<%= Model.References[i].Id %>', function() {
                                                            $('.JS-ReferenceContainer[data-ref-id="' + '<%= Model.References[i].Id %>' + '"]').find('.ReferenceText').val('<%= Model.References[i].ReferenceText %>').trigger('blur');
                                                            $('.JS-ReferenceContainer[data-ref-id="' + '<%= Model.References[i].Id %>' + '"]').find('.AdditionalInfo').val('<%= Model.EscapedReferences[i].AdditionalInfo %>');
                                                        });
                                                        catIds[<%= i%>] = -1;
                                                        <% break;
                                                } %>
                                                <% if (i != 0){%>
                                                
                                                    $(window).bind('referenceAdded' + '<%= Model.References[i-1].Id%>', function() {
                                                        $("#ReferenceSearchInput")
                                                            .data('category-id', catIds[<%= i%>])
                                                            .data('referenceType', '<%= Model.References[i].ReferenceType.GetName() %>')
                                                            .trigger('initCategoryFromTxt', '<%= Model.References[i].Id %>');                                                
                                                    });
                                                <%}
                                            } %>
                                            $("#ReferenceSearchInput")//Init first reference
                                                .data('category-id', catIds[0])
                                                .data('referenceType', '<%= Model.References[0].ReferenceType.GetName() %>')
                                                .trigger('initCategoryFromTxt', '<%= Model.References[0].Id %>');
                                        });
                                    <%}%>
                                </script>
                                <div id="JS-ReferenceSearch" class='JS-ReferenceContainer well' style="display: none;">
                                    <a id='JS-HideReferenceSearch' class='close' href ='#'>×</a>
                                    <div class="form-group">
                                        <label class="columnLabel control-label">Quelle hinzufügen</label>
                                        <div class="columnControlsFull">
                                            <div class="ControlInline ReferenceSearchControl">
                                                <select id="ReferenceType" class="form-control">
                                                    <option value="Book"><%= CategoryType.Book.GetName() %></option>                
                                                    <option value="Article">Artikel</option>          
                                                    <option value="VolumeChapter"><%= CategoryType.VolumeChapter.GetName() %></option>                
                                                    <option value="WebsiteArticle"><%= CategoryType.WebsiteArticle.GetName() %></option>
                                                    <option value="Url">Url</option>
                                                    <option value="FreeText">Freitext</option>
                                                </select>
                                            </div>
                                            <div class='JS-CatInputContainer ControlInline ReferenceSearchControl'>
                                                <input id='ReferenceSearchInput' class='form-control' name ='txtReference' type ='text' value ='' placeholder=''/>
                                            </div>
                                            <div  id="AddFreeTextReference" class='ControlInline' style="display: none;">
                                                <button class="btn">Freitextquelle hinzufügen</button>
                                            </div>
                                            <div  id="AddUrlReference" class='ControlInline' style="display: none;">
                                                <button class="btn">Url als Quelle hinzufügen</button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div id="AddReferenceControls" class="form-group">
                            <div class="noLabel columnControlsFull">
                                <button class="btn" id="AddReference">Quelle hinzufügen</button>
                            </div>
                        </div>
                    </div>
                    <div class="FormSection">
                    
                    <% if(Model.IsInstallationAdmin) {%>
                        <div class="form-group">
                            <label class="columnLabel control-label" for="SolutionType">
                                <span>Lizenz (nur für Admins)</span>
                            </label>
                            <div class="columnControlsFull">
                                <%= Html.DropDownListFor(m => Model.LicenseId, Model.LicenseDropdownList, new { @class = "form-control" })%>
                            </div>
                        </div>
                        <%} else { %>
                      <div id="Agreement" class="form-group">
                            <div class="noLabel columnControlsFull">
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" name="ConfirmContentRights" value="confirmed"/> 
                                        Ich stelle diesen Eintrag unter die Lizenz "Creative Commons - Namensnennung 4.0 International" (CC&nbsp;BY&nbsp;4.0, <a href="https://creativecommons.org/licenses/by/4.0/legalcode" target="_blank">Lizenztext</a>, <a>deutsche Zusammenfassung</a>).
                                        Der Eintrag kann bei angemessener Namensnennung ohne Einschränkung weiter genutzt werden .
                                        <br/>
                                        Die Frage, die Anwort und ggf. Bilder sind meine eigene Arbeit und nicht aus urheberrechtlich geschützten Quellen kopiert.
                                    </label>
                                </div>
                            </div>
                        </div>
                        <% } %>
                        <div class="form-group">
                            <div class="noLabel columnControlsFull">
                                <button type="submit" class="btn btn-primary" id="btnSave" name="btnSave" value="save"
                                    <% if(!Model.IsLoggedIn){ %> disabled="disabled" <% } %>>Speichern</button>&nbsp;&nbsp;&nbsp;
                                <% if (Model.ShowSaveAndNewButton){ %>
                                    <button type="submit" class="btn btn-default" name="btnSave" value="saveAndNew" 
                                        <% if(!Model.IsLoggedIn){ %> disabled="disabled" <% } %> >Speichern &amp; neu</button>&nbsp;
                            </div>
                        </div>
                    </div>
                    <% } %>
                </div>
            </div>
        <% } %>
    </div>
    <% } %>
    
    <% Html.RenderPartial("~/Views/Images/ImageUpload/ImageUpload.ascx"); %>

</asp:Content>