<%@ Page Title="Frage erstellen" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuLeft.Master"
    Inherits="ViewPage<EditQuestionModel>" ValidateRequest="false" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
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
    <input type="hidden" id="urlSolutionEditBody" value="<%=Url.Action("SolutionEditBody", "EditQuestion") %>" />

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
                        <a href="<%= Links.CreateQuestion(Url) %>" style="font-size: 12px;
                            margin: 0;"><i class="fa fa-plus-circle"></i> Frage erstellen</a>
                    </div>
                <%} %>
                    
                <% if(Model.IsEditing){ %>
                    <div style="line-height: 12px; padding-top: 3px;">
                        <a href="<%= Links.AnswerQuestion(Url, Model.Question, (int)Model.Id) %>" style="font-size: 12px;
                            margin: 0;"><i class="fa fa-check-square"></i> Frage beantworten</a>
                    </div>                    
                <% } %>
            </div>
        </div>
        <div class="PageHeader col-xs-12">
            <% if(!Model.IsLoggedIn){ %>
                <div class="bs-callout bs-callout-danger" style="margin-top: 0;">
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
                                        <i class="fa fa-question-circle show-tooltip" title="" data-placement="<%= CssJs.TooltipPlacementLabel %>" 
                                           data-html="true"
                                           data-original-title="
                                                <ul class='show-tooltip-ul'>
                                                    <li>Die Frage ist für alle auffindbar.</li>
                                                    <li>Jeder kann die Frage in sein Wunschwissen aufnehmen.</li>
                                                    <li>Die Frage steht unter einer LGPL-Lizenz und kann frei weiterverwendet werden.</li>
                                                </ul>">
                                        </i>
                                        <br/>
                                </label>
                            </div>
                            <div class="radio">
                                <label style="font-weight: normal">
                                    <%= Html.RadioButtonFor(m => m.Visibility, QuestionVisibility.Owner)  %>
                                        für mich <span class="smaller">(<i class="fa fa-lock"></i> private Frage)</span>
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
                <div class="FormSection">
                <div class="form-group">
                    <%= Html.LabelFor(m => m.Question, new { @class = "RequiredField columnLabel control-label" })%>
                    <div class="columnControlsFull">
                        <%= Html.TextAreaFor(m => m.Question, new { @class="form-control", placeholder = "Bitte gib den Fragetext ein", rows = 3})%>
                    </div>
                </div>
                <div class="form-group">
                    <div id="OpenImageUpload" class="columnControlsFull" style="padding-top: 4px;">
                        <a href="#" class="TextLinkWithIcon"><i class="fa fa-file-image-o"></i> <span class="TextSpan">Bild hinzufügen</span></a> 
                    </div>
                </div>
                <div class="form-group">
                    <div id="openExtendedQuestion" class="columnControlsFull" style="padding-top: 4px;">
                        <a href="#" class="TextLinkWithIcon"><i class="fa fa-plus-circle"></i> <span class="TextSpan">Erweiterte Beschreibung (z.B.: mit Bildern, Formeln oder Quelltext)</span></a> 
                    </div>
                </div>
                <div class="form-group markdown" style="display: none" id="extendedQuestion">
                    <label class="columnLabel control-label" for="QuestionExtended" style="max-width: 420px;">
                        Frage erweitert
                        <a id="hideExtendedQuestion" href="#" class="TextLinkWithIcon" style="font-size: 90%; float: right;"><i class="fa fa-minus-circle"></i> <span class="TextSpan">ausblenden</span></a>
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
                <div class="form-group">    
                    <label class="columnLabel control-label">
                        <span class="show-tooltip" data-toggle="tooltip" title = "Kategorien helfen bei der Einordnung der Frage und ermöglichen dir und anderen die Fragen wiederzufinden. Tipp: Falls du eine gesuchte Kategorie nicht findest, kannst du sie in einem neuen Tab anlegen und dann einfach hier weitermachen." data-placement = "<%= CssJs.TooltipPlacementLabel %>">Kategorien</span>
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
                        <div class="JS-CatInputContainer ControlInline"><input id="txtNewRelatedCategory" class="form-control" type="text" placeholder="Wähle eine Kategorie" /></div>
                    </div>
                </div>
                </div>
                <div class="FormSection">
                    <div class="form-group">
                        <label class="columnLabel control-label" for="SolutionType">
                            <span <%= Model.IsEditing ? "class='show-tooltip' title='Der Abfragetyp kann nach dem ersten Speichern der Frage leider nicht mehr verändert werden.' data-placement ='"+ CssJs.TooltipPlacementLabel + "'" : ""%>>Abfragetyp</span>
                        </label>
                        <div id="SolutionTypeContainer" <%= Model.IsEditing ? "class='columnControlsFull show-tooltip' data-toggle='tooltip' title='Der Abfragetyp kann nach dem ersten Speichern der Frage leider nicht mehr verändert werden.' data-placement ='"+ CssJs.TooltipPlacementLabel + "'" : "class='columnControlsFull'"%>>
                            <%= Html.DropDownListFor(m => Model.SolutionType,
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

                    <div class="form-group markdown">
                        <label class="columnLabel control-label" for="Description">
                            <span class="show-tooltip"  title = "Erscheinen nach dem Beantworten der Frage zusammen mit der richtigen Lösung und sollen beim Einordnen und Merken der abgefragten Fakten helfen. Oft wird eine Frage erst durch informative Zusatzangaben so richtig gut." data-placement = "<%= CssJs.TooltipPlacementLabel %>">Ergänzungen</span>
                        </label>
                        <div class="columnControlsFull">
                            <div class="wmd-panel">
                                <div id="wmd-button-bar-2"></div>
                                <%= Html.TextAreaFor(m => m.Description, new 
                                    { @class= "form-control wmd-input", id="wmd-input-2", placeholder = "Erklärungen, Zusatzinfos, Merkhilfen, Abbildungen, weiterführende Literatur und Links etc.", rows = 4 })%>
                            </div>
                            <div id="wmd-preview-2" class="wmd-panel wmd-preview"></div>
                        </div>
                    </div>
                    
                    <div class="form-group" style="margin-bottom: 0;">
                        <label class="columnLabel control-label"> 
                            <%--<br/>
                            <div style="font-weight: normal">(Gute Quellen machen gute Fragen/Antworten nochbesser!)</div>--%>
                            <span class="show-tooltip" data-toggle="tooltip" title = "Bitte belege die von dir angeführten Fakten mit Quellen und mache wörtliche und auch indirekte Zitate als solche erkennbar. Bitte gehe sparsam mit wörtlichen Zitaten um und formuliere wenn möglich mit eigenen Worten. Du kannst in Frage, Anwort und Ergänzungen auf die hier eingefügten Quellen verweisen." data-placement = "<%= CssJs.TooltipPlacementLabel %>">
                                Quellen
                            </span>
                        </label>
                    
                        <div id="JS-References" class="columnControlsFull">
                            <script type="text/javascript">
                                <%if(Model.References.Count != 0){%>
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
                                                        $('.JS-ReferenceContainer[data-ref-id="' + '<%= Model.References[i].Id %>' + '"]').find('.ReferenceText').val('<%= Model.References[i].ReferenceText %>');
                                                    });
                                                    catIds[<%= i%>] = -1;
                                                    <% break;

                                                case ReferenceType.UrlReference: %>
                                                    $(window).bind('referenceAdded' + '<%= Model.References[i].Id %>', function() {
                                                        $('.JS-ReferenceContainer[data-ref-id="' + '<%= Model.References[i].Id %>' + '"]').find('.ReferenceText').val('<%= Model.References[i].ReferenceText %>').trigger('blur');
                                                        $('.JS-ReferenceContainer[data-ref-id="' + '<%= Model.References[i].Id %>' + '"]').find('.AdditionalInfo').val('<%= Model.References[i].AdditionalInfo %>');
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
                    <div id="Agreement" class="form-group">
                        <div class="noLabel columnControlsFull">
                            <div class="checkbox">
                                <label>
                                    <%= Html.CheckBoxFor(x => x.ConfirmContentRights) %>
                                    Ich stelle diesen Eintrag unter eine LGPL-Lizenz. 
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
                            <button type="submit" class="btn btn-primary" id="btnSave" name="btnSave" value="save">Speichern</button>&nbsp;&nbsp;&nbsp;
                            <% if (Model.ShowSaveAndNewButton){ %>
                                <button type="submit" class="btn btn-default" name="btnSave" value="saveAndNew" >Speichern &amp; neu</button>&nbsp;
                            <% } %>                        
                        </div>
                    </div>
                </div>
                <% } %>
            </div>
        </div>
    </div>
    <% } %>
    
    <% Html.RenderPartial("~/Views/Images/ImageUpload/ImageUpload.ascx"); %>

</asp:Content>