<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<SetListCardModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="CardColumn">
    <div class="Card ItemList Set">
        <h4 class="ItemTitle"><%: Model.Title %></h4>
        <div class="ItemText"><%: Model.Description %></div>
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
                                    <i class="fa fa-heart show-tooltip iAdded <%= singleSetModel.IsInWishknowledge ? "" : "hide2" %>" style="color: #b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                                    <i class="fa fa-heart-o show-tooltip iAddedNot <%= singleSetModel.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                                    <i class="fa fa-spinner fa-spin hide2 iAddSpinner" style="color:#b13a48;"></i>
                                </a>
                            </span>&nbsp;
                            Fragesatz mit <a href="<%= Links.SetDetail(Url,singleSetModel.SetName,singleSetModel.SetId) %>"><%= singleSetModel.QCount %> Fragen</a>
                        </h6>
                        <div class="SetTitle">
                            <%= set.Name %>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                </div>
          <% }
            if (Model.Sets.Count < Model.RowCount)
            {
                for (var i = 0; i < Model.RowCount - Model.Sets.Count; i++)
                { %>
                    <div class="ItemRow"></div> 
                <% }
            }%>
        

        <div class="Divider" style="margin-bottom: 5px;"></div>
        <div class="BottomBar">
            
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
            <a class="btn btn-sm btn-primary show-tooltip" href="<%= Links.TestSessionStartForSetsInCategory(Model.Sets.Select(s => s.Id).ToList(), Model.Title, Model.CategoryId) %>" title="Teste dein Wissen in dieser Kategorie" rel="nofollow">
                &nbsp;JETZT TESTEN
            </a>
        </div>
    </div>
</div>
