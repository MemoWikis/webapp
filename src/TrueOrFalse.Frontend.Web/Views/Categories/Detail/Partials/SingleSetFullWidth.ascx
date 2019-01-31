<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<SingleSetFullWidthModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<content-module inline-template >
    <li class="module" markdown="<%: Model.Markdown %>" v-if="!isDeleted">
        <div class="ContentModule" @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)">
            <div class="ModuleBorder" :class="{ active : hoverState }">
                
                <div class="singleSetFullWidth">
                    <div class="row">
                        <div class="col-xs-3">
                            <div class="ImageContainer">
                                <%= Model.ImageFrontendData.RenderHtmlImageBasis(128, true, ImageType.QuestionSet) %>
                            </div>
                        </div>
                        <div class="col-xs-9">
                            <div>
                                <div class="setQuestionCount">
                                    <span class="Pin" data-set-id="<%= Model.SetId %>" style="">
                                        <a href="#" class="noTextdecoration">
                                            <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(Model.IsInWishknowledge)) %>
                                        </a>
                                    </span>&nbsp;
                                    Lernset von <a href="<%= Links.UserDetail(Model.Creator,Model.CreatorId) %>"><%=Model.Creator %></a> mit <%: Model.QuestionCount %> Frage<%= StringUtils.PluralSuffix(Model.QuestionCount, "n") %>
                                </div>
                                <div class="KnowledgeBarWrapper">
                                    <% Html.RenderPartial("~/Views/Sets/Detail/SetKnowledgeBar.ascx", new SetKnowledgeBarModel(Model.Set)); %>
                                    <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                                </div>
                            </div>
                            <div class="setTitle">
                                <h3><%: Model.Title %></h3>
                            </div>
                            <div class="setDescription">
                                <%= Model.Text %>
                            </div>
                            <div class="buttons">
                                <a href="<%= Links.TestSessionStartForSet(Model.Title, Model.SetId) %>" class="btn btn-primary">
                                    <i class="fa fa-lg fa-play-circle">&nbsp;</i> Wissen testen
                                </a>
                                <br />
                                <a href="<%= Links.StartLearningSessionForSet(Model.SetId) %>" class="btn btn-link" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">
                                    <i class="fa fa-lg fa-line-chart">&nbsp;</i> Gleich richtig lernen
                                </a>
                                <div class="dropdown">
                                    <% var buttonId = Guid.NewGuid(); %>
                                    <a href="#" id="<%=buttonId %>" class="dropdown-toggle btn btn-link ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                        <i class="fa fa-ellipsis-v"></i>
                                    </a>
                                    <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%=buttonId %>">
                                        <li><a href="<%= Links.SetDetail(Model.Title, Model.SetId) %>"><i class="fa fa-search-plus">&nbsp;</i> Detailseite des Lernsets</a></li>
                                        <%--<li><a href="<%= Links.GameCreateFromSet(Model.SetId) %>"><i class="fa fa-gamepad">&nbsp;</i> Spiel starten</a></li>--%>
                                        <li><a href="<%= Links.DateCreateForSet(Model.SetId) %>"><i class="fa fa-calendar">&nbsp;</i> Prüfungstermin anlegen</a></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>    
        </div>     
    </li>        
</content-module> 




