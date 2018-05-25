<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<KnowledgeCardMiniCategoryModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="row">
    <div class="col-xs-3">
        <div class="ImageContainer">
            <%= Model.GetCategoryImage(Model.Category).RenderHtmlImageBasis(128, true, ImageType.Category, linkToItem: Links.CategoryDetail(Model.Category.Name, Model.Category.Id)) %>
        </div>
    </div>
    <div class="col-xs-9">
        <a class="topic-name" href="<%= Links.GetUrl(Model.Category) %>">
            <div class="topic-name">
                <%: Model.Category.Name %>
            </div>
        </a>
        <div class="set-question-count">
            <%: Model.GetTotalSetCount(Model.Category) %> Lernset<%= StringUtils.PluralSuffix(Model.GetTotalSetCount(Model.Category),"s") %>
            <%: Model.GetTotalQuestionCount(Model.Category) %> Frage<%= StringUtils.PluralSuffix(Model.GetTotalQuestionCount(Model.Category),"n") %>
        </div>
        <div class="KnowledgeBarWrapper">
            <% Html.RenderPartial("~/Views/Categories/Detail/CategoryKnowledgeBar.ascx", new CategoryKnowledgeBarModel(Model.Category)); %>
            <div class="KnowledgeBarLegend">Dein Wissensstand</div>
        </div>
    </div>

    
        <span class="fa fa-heart-o" style="float: left"></span>
        <div class="tooltip">Zu deinem Wunschwissen hinzufügen</div>
    
<div class="buttons" style="float: left">
            <a href="#" class="btn btn-primary" id="getLearningTab" data-allowed="logged-in" data-allowed-type="learning-session" rel="nofollow">
                <i class="fa fa-lg fa-line-chart">&nbsp;</i> Gleich richtig lernen
            </a>
       
      
        <div class="dropdown"style="border: 1px solid black">
            <% var buttonId = Guid.NewGuid(); %>
            <a href="#" id="<%=buttonId %>" class="dropdown-toggle btn btn-link ButtonOnHover ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                <i class="fa fa-ellipsis-v"></i>
            </a>
            <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%=buttonId %>">
                <li><a href="#"><i class="fa fa-search-plus">&nbsp;</i> Detailseite des Lernsets</a></li>
                <li><a href="#"><i class="fa fa-gamepad">&nbsp;</i> Spiel starten</a></li>
                <li><a href="#"><i class="fa fa-calendar">&nbsp;</i> Prüfungstermin anlegen</a></li>
            </ul>
        </div>
  </div> 
    
    <div id="test"></div>
</div>
<script type="text/javascript">
    $("#getLearningTab").click(function () {
        $.get('@Url.Action( "CategoryById","CategoryController", new{id = 682})', {}, function (response) {     
            $("#test").html(response);
        });
    });
</script>