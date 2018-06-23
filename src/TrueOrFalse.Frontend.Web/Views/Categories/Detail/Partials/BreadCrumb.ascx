<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<TopNavMenu>" %>

<div class="container" style="display: flex; flex-wrap: wrap;">                      
    <div id="BreadcrumbLogoSmall" style="display:none;">
        <img src="/Images/Logo/LogoSmall.png">
    </div>
    <div style="height: auto;" class="show-tooltip" data-placement="bottom"  title="Zur Startseite">
        <a href="/" class="category-icon">
            <span style="margin-left: 10px">Home</span>
        </a>
        <span><i class="fa fa-chevron-right"></i></span>
    </div>            
    <%if(Model.IsCategoryBreadCrumb){ %>
        <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model) %>
    <% }else{
           if (Model.IsAnswerQuestionBreadCrumb) { %>
             <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model) %>

            <%}

        var last = Model.BreadCrumb.Last();
       foreach (var breadCrumbItem in Model.BreadCrumb) { %>
        <div style="display: flex; height: auto; margin-bottom: 5px" class="show-tooltip" data-placement="bottom" <% if (Model.IsAnswerQuestionBreadCrumb){%>title="Zum Lernset" <% }else{ %> title="<%= breadCrumbItem.ToolTipText%>" <%}%> >                                                                                          
           <%if (breadCrumbItem.Equals(last)){%>
              <span style="display: inline-table; margin-left: 10px; color:#000000; opacity:0.50;"><a style="<%= breadCrumbItem.TextStyles%>" href="<%= breadCrumbItem.Url %>"><%= breadCrumbItem.Text %></a></span>
            <%} else {%>
               <span style="display: inline-table; margin-left: 10px;"><a style="<%= breadCrumbItem.TextStyles%>" href="<%= breadCrumbItem.Url %>"><%= breadCrumbItem.Text %></a>
                <i style="display: inline;" class="fa fa-chevron-right"></i>
               </span>  
            <%} %>
        </div>
    <% } %>        
    <%}%>
    <div id="StickyHeaderContainer">
        <i class="fa fa-search" style="font-size:29px;"></i>
        <i class="fa fa-dot-circle"></i>
        <a id="StickyMenuButton" style="margin-top:0px;"><i class="fa fa-bars"></i></a>
    </div>
</div> 