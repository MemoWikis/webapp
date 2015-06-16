<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HeaderModel>" %>

<div class="boxtainer-header MobileHide">
    <ul class="nav nav-tabs">
        <li class="<%= Html.IfTrue(!Model.IsNetworkTab, "active") %>">
            <a href="<%= Url.Action("Users", "Users") %>" >Alle Nutzer (<%= Model.TotalUsers %>)</a>
        </li>
        <li class="<%= Html.IfTrue(Model.IsNetworkTab, "active") %>">
            <a href="<%= Url.Action("Network", "Users") %>">
                Mein Netzwerk 
                (<span class="JS-AmountFollowers"><%= Model.TotalIFollow %></span>/<%= Model.TotalFollowingMe %>)
                <i class="fa fa-question-circle" id="tabInfoMyKnowledge"></i>
            </a>
        </li>
    </ul>
</div>