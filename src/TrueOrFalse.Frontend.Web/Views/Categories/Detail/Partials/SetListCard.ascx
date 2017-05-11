<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<SetListCardModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="CardColumn">
    <div class="Card ItemList Set">
        <div class="ContentContainer">
            <div class="CardContent">
                <h4 class="ItemTitle<%= Model.TitleRowCount > 0 ? " rowCount" + Model.TitleRowCount : "" %>"><%: Model.Title %></h4>
                <div class="ItemText<%= Model.DescriptionRowCount > 0 ? " rowCount" + Model.DescriptionRowCount : "" %>"><%: Model.Description %></div>
                <% foreach (var set in Model.Sets)
                    {
                        var singleSetModel = new SingleSetModel(set);%>

                        <div class="ItemRow">
                            <div class="ImageColumn" style="width: 50px; float: left; margin-right: 10px;">
                                <div class="ImageContainer ShortLicenseLinkText">
                                    <%= singleSetModel.ImageFrontendData.RenderHtmlImageBasis(50, true, ImageType.QuestionSet) %>
                                </div>
                            </div>
                            <div class="ContentColumn">
                                <h6 class="ItemInfo">
                                    <span class="Pin" data-set-id="<%= singleSetModel.SetId %>" style="">
                                        <a href="#" class="noTextdecoration">
                                            <%= Html.Partial("AddToWishknowledge", new AddToWishknowledge(singleSetModel.IsInWishknowledge)) %>
                                        </a>
                                    </span>&nbsp;
                                    <a href="<%= Links.SetDetail(Url,singleSetModel.SetName,singleSetModel.SetId) %>">Fragesatz mit <%= singleSetModel.QCount %> Fragen</a>
                                </h6>
                                <div class="SetTitle">
                                    <a class="PlainTextLook" href="<%= Links.SetDetail(Url,singleSetModel.SetName,singleSetModel.SetId) %>"><%= set.Name %></a>
                                </div>
                            </div>
                            <div class="clearfix"></div>
                        </div>
                  <% } %>
            </div>
            <div class="BottomBar">
                <div class="dropdown">
                    <% var buttonId = Guid.NewGuid(); %>
                    <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                        <li><a href="<%= Links.StartLearningSessionForSets(Model.Sets.Select(s => s.Id).ToList(), Model.Title) %>" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">Jetzt lernen</a></li>
                        <li><a href="<%= Links.GameCreateFromSets(Model.Sets.Select(s => s.Id).ToList()) %>"> Spiel starten</a></li>
                        <li><a href="<%= Links.DateCreateForSets(Model.Sets.Select(s => s.Id).ToList(), Model.Title) %>"> Termin anlegen</a></li>
                    </ul>
                </div>

                <a class="btn btn-sm ButtonOnHover show-tooltip" href="<%= Links.TestSessionStartForSets(Model.Sets.Select(s => s.Id).ToList(), Model.Title) %>" title="Teste dein Wissen für diese Fragesätze" rel="nofollow">
                    <i class="fa fa-play-circle AnswerResultIcon">&nbsp;&nbsp;</i>WISSEN TESTEN
                </a>
            </div>
        </div>
    </div>
</div>
