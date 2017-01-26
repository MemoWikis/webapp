<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>


<div id="ItemMainInfo" class="Box">
    <div class="">
        <div class="row">
            <div class="col-xs-12 col-sm-3">
                <div class="ImageContainer">
                    <%= Model.ImageFrontendData.RenderHtmlImageBasis(350, false, ImageType.Category, "ImageContainer") %>
                </div>
            </div>
            <div class="col-xs-12 col-sm-9">
                <header>
                    <div>
                        <% if (Model.Type != "Standard"){ %>
                            <%= Model.Type %>
                        <% }
                        else { %>
                            Kategorie
                        <% }  %>
                    </div>
                    <h1 style="margin-top: 5px; font-size: 26px;">
                        <%= Model.Name %>
                    </h1>
                </header>
                <div>
                        <% if (Model.Type != "Standard") {
                        Html.RenderPartial("Reference", Model.Category);
                    }
                    
                    if (!String.IsNullOrEmpty(Model.Description)) {%>
                        <div class="Description"><span><%= Model.Description %></span></div>
                    <% } %>
                    
                    <% if (!String.IsNullOrEmpty(Model.WikiUrl)){ %>
                        <div class="WikiLink" style="margin-top: 10px;">
                            <a href="<%= Model.WikiUrl %>" target="_blank" class="show-tooltip" title="<div style='white-space: normal; word-wrap: break-word; text-align:left; '>Link&nbsp;auf&nbsp;Wikipedia:&nbsp;<%= Model.WikiUrl %></div>" data-placement="left" data-html="true">
                                <img src="/Images/wiki-24.png" style="margin-top: -1px;" /><%= Model.WikiUrl %>
                            </a>
                        </div>
                    <% } %>
                </div>
                            
                <% if(Model.AnswersTotal > 0) { %>
                    <div class="Divider" style="margin-top: 10px; margin-bottom: 5px;"></div>
                    <div style="margin-top: 6px; font-size: 11px;">
                        In dieser Kategorie wurden
                        <%= Model.AnswersTotal + "x Frage" + StringUtils.PluralSuffix(Model.AnswersTotal, "n") %> beantwortet, 
                        davon <%= Model.CorrectnesProbability %>% richtig.
                    </div>
                <% } %>                

                <div class="Divider" style="margin-top: 10px; margin-bottom: 5px;"></div>
                <div class="BottomBar">
                    <div style="float: left; padding-top: 3px;">
                        <div class="fb-share-button" style="width: 100%" data-href="<%= Settings.CanonicalHost + Links.CategoryDetail(Model.Name, Model.Id) %>" data-layout="button" data-size="small" data-mobile-iframe="true"><a class="fb-xfbml-parse-ignore" target="_blank" href="https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fdevelopers.facebook.com%2Fdocs%2Fplugins%2F&amp;src=sdkpreparse">Teilen</a></div>                
                    </div>
                    <%-- <div class="dropdown">
                        <% var buttonId = Guid.NewGuid(); %>
                        <a href="#" id="<%=buttonId %>" <%= Model.QuestionCount == 0 ? "disabled " : "" %>class="dropdown-toggle  btn btn-link ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <i class="fa fa-ellipsis-v"></i>
                        </a>
                        <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                            <li><a href="<%= Links.StartSetLearningSession(Model.Id) %>" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">Jetzt üben</a></li>
                            <li><a href="<%= Links.GameCreateFromSet(Model.Id) %>"> Spiel starten</a></li>
                            <li><a href="<%= Links.DateCreate(Model.Id) %>"> Termin anlegen</a></li>
                        </ul>
                    </div>--%>
                    <a class="btn btn-primary show-tooltip" href="<%= Links.TestSessionStartForCategory(Model.Name,Model.Id) %>" title="Teste dein Wissen in dieser Kategorie" rel="nofollow">
                        <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>WISSEN TESTEN
                    </a>
                </div>
            </div>
        </div>
    </div>    
</div>
<%--<div class="row BoxButtonBar">
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-gamepad"></i></div>
            <div class="BoxButtonText">
                <span>Spiel starten</span>
            </div>
            <a></a>
        </div>
    </div>
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-calendar"></i></div>
            <div class="BoxButtonText">
                <span>Prüfungstermin anlegen</span> 
            </div>
            <a href="<%= Links.DateCreateForCategory(Model.Id) %>" rel="nofollow"></a>
        </div>
    </div>
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-line-chart"></i></div>
            <div class="BoxButtonText">
                <span>Üben</span>
            </div>
            <a href="<%= Links.StartCategoryLearningSession(Model.Id) %>" rel="nofollow"></a>
        </div>
    </div>
    <div class="BoxButtonColumn">
        <div class="BoxButton">
            <div class="BoxButtonIcon"><i class="fa fa-play-circle"></i></div>
            <div class="BoxButtonText">
                <span>Wissen testen</span>
            </div>
            <a href="<%= Links.TestSessionStartForCategory(Model.Name,Model.Id) %>" rel="nofollow"></a>
        </div>
    </div>
</div>--%>