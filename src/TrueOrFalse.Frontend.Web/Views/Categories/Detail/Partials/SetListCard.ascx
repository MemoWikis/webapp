<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<SetListCardModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="CardColumn">
    <div class="Card ItemList Set">
        <h4 class="ItemTitle"><%: Model.Title %></h4>
        <div class="ItemText"><%: Model.Description %></div>
        <% foreach (var set in Model.Sets)
           {
        var imageMetaData = Sl.R<ImageMetaDataRepo>().GetBy(set.Id, ImageType.QuestionSet);
        var imageFrontendData = new ImageFrontendData(imageMetaData);
               %>
        <div style="clear: left; margin-bottom: 10px;">
            <div class="ImageColumn" style="width: 50px; float: left; margin-right: 10px;">
                <div class="ImageContainer ShortLicenseLinkText">
                    <%= imageFrontendData.RenderHtmlImageBasis(50, true, ImageType.QuestionSet) %>
                </div>
            </div>
            <%= set.Name %>
            <div class="clearfix"></div>
        </div>
          <% } %>
        <div class="clearfix"></div>
        <div class="Divider" style="margin-top: 10px; margin-bottom: 5px;"></div>
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
            <a class="btn btn-sm btn-primary show-tooltip" href="#" title="Teste dein Wissen in dieser Kategorie" rel="nofollow">
                &nbsp;JETZT TESTEN
            </a>
        </div>
        <%--<div class="ImageContainer">
            <%= Model.ImageFrontendData.RenderHtmlImageBasis(300, true, ImageType.QuestionSet, linkToItem: Links.TestSessionStartForSet(Model.SetName, Model.SetId), noFollow: true) %>
        </div>--%>

     <%--   
        <div class="ContentContainer">
            <div class="CardContent">
                <h6 class="ItemInfo">
                    <span class="Pin" data-set-id="<%= Model.SetId %>" style="">
                        <a href="#" class="noTextdecoration">
                            <i class="fa fa-heart show-tooltip iAdded <%= Model.IsInWishknowledge ? "" : "hide2" %>" style="color: #b13a48;" title="Aus deinem Wunschwissen entfernen"></i>
                            <i class="fa fa-heart-o show-tooltip iAddedNot <%= Model.IsInWishknowledge ? "hide2" : "" %>" style="color:#b13a48;" title="Zu deinem Wunschwissen hinzuzufügen"></i>
                            <i class="fa fa-spinner fa-spin hide2 iAddSpinner" style="color:#b13a48;"></i>
                        </a>
                    </span>&nbsp;
                    Fragesatz mit <a href="<%= Links.SetDetail(Url,Model.SetName,Model.SetId) %>"><%= Model.QCount %> Fragen</a>
                </h6>
                
            </div>
            <div class="BottomBar">
                <div class="dropdown">
                    <% var buttonId = Guid.NewGuid(); %>
                    <a href="#" id="<%=buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu" aria-labelledby="<%=buttonId %>">
                        <li><a href="<%= Links.StartLearningSesssionForSet(Model.SetId) %>" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">Jetzt üben</a></li>
                        <li><a href="<%= Links.GameCreateFromSet(Model.SetId) %>"> Spiel starten</a></li>
                        <li><a href="<%= Links.DateCreate(Model.SetId) %>"> Termin anlegen</a></li>
                        <li><a href="<%= Links.SetDetail(Model.SetName, Model.SetId) %>"> Fragesatz-Detailseite</a></li>
                    </ul>
                </div>
                <a href="<%= Links.TestSessionStartForSet(Model.SetName, Model.SetId) %>" class="btn btn-link btn-sm ButtonOnHover" role="button" rel="nofollow">
                    &nbsp;JETZT TESTEN
                </a>
            </div>
        </div>--%>
    </div>
</div>
