<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<SetListModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<h4>
    <%= Model.BoxTitle %> (<%= Model.Sets.Count %>)
</h4>
<div id="Content" class="Box">
    <% if(Model.Sets.Count > 0){ %>    
        <%--<h5 class="ContentSubheading Set"><%= Model.CountSets %> Frage<%= StringUtils.PluralSuffix(Model.CountSets,"sätze","satz") %> in dieser Kategorie</h5>--%>
        <div class="LabelList">
        <% foreach(var set in Model.Sets){ %>
            <div class="LabelItem LabelItem-Set">
                <div class="EllipsWrapper">
                    <a href="<%= Links.SetDetail(Url, set) %>"><%= set.Name %></a>
                    <span style="font-size: 90%;">
                        (<%= set.QuestionsInSet.Count %> Frage<%= StringUtils.PluralSuffix(set.QuestionsInSet.Count, "n") %>,    
                        <a href="<%= Links.TestSessionStartForSet(set.Name, set.Id) %>"><i class="fa fa-play-circle">&nbsp;</i>Jetzt Wissen testen</a>)
                    </span>
                </div>
            </div>
        <% } %>
        </div>
        <a class="btn btn-sm btn-primary show-tooltip" href="<%= Links.TestSessionStartForCategory(Model.Name,Model.Id) %>" style="float: right;" title="Teste dein Wissen in dieser Kategorie" rel="nofollow">
            &nbsp;JETZT TESTEN
        </a>
        <div class="clearfix"></div>
    <% } %>
</div>