<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<HeaderModel>" %>

<div id="MobileSubHeader" class="MobileSubHeader DesktopHide">
    <div class=" container">
        <div id="mobilePageHeader" class="">
            <h3 class="">
                Nutzer
            </h3>
        </div>
    </div>
    <div class="MainFilterBarWrapper">
        <div id="MainFilterBarBackground" class="btn-group btn-group-justified">
            <div class="btn-group <%= Html.IfTrue(!Model.IsNetworkTab, "active") %>">
                <a class="btn btn-default disabled">.</a>
            </div>
        </div>
        <div class="container">
            <div id="MainFilterBar" class="btn-group btn-group-justified JS-Tabs">
                
                <div class="btn-group">
                    <a href="<%= Url.Action("Users", "Users") %>" type="button" class="btn btn-default">
                        Alle (<span class="JS-Amount"><%= Model.TotalUsers %></span>)
                    </a>
                </div>
                <div class="btn-group <%= Html.IfTrue(Model.IsNetworkTab, "active") %>">
                    <a href="<%= Url.Action("Network", "Users") %>" type="button" class="btn btn-default">
                        Mein Netzwerk<span class="hidden-xxs"></span> 
                        (<span class="JS-AmountFollowers"><%= Model.TotalIFollow %></span>/<%= Model.TotalFollowingMe %>)
                    </a>
                </div>
            </div>
        </div>
    </div>
</div>