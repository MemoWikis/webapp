<%@ Control Language="C#"  Inherits="System.Web.Mvc.ViewUserControl<TopNavMenu>" %>

<div class="container" style="display: flex; flex-wrap: nowrap;">                      
    <div id="BreadcrumbLogoSmall" style="display:none;">
        <img src="/Images/Logo/LogoSmall.png">
    </div>
    <div style="height: auto;" class="show-tooltip" data-placement="bottom"  title="Zur Startseite">
     <%if(!(Model.IsWelcomePage)){ %> 
        <a href="/" class="category-icon">
            <span style="margin-left: 10px">Home</span>
        </a>
        <span><i class="fa fa-chevron-right"></i></span>
     <%}else{ %>
        <a href="/" class="category-icon">
            <span style="margin-left: 10px; color:#000000; opacity:0.50;">Home</span>
        </a>
     <%}%>
    </div>

<%if(!(Model.IsWelcomePage)){ %>  
    <%if(Model.IsCategoryBreadCrumb){ %>
        <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model) %>
    <% }else{
            if (Model.IsAnswerQuestionOrSetBreadCrumb) { %>
             <%= Html.Partial("/Views/Categories/Detail/Partials/BreadCrumbCategories.ascx", Model) %>
          <%}
              
        var last = Model.BreadCrumb.Last();
       foreach (var breadCrumbItem in Model.BreadCrumb) { %>
        <div style="display: flex; height: auto; margin-bottom: 5px" class="show-tooltip" data-placement="bottom" <% if (Model.IsAnswerQuestionOrSetBreadCrumb){%>title="Zum Lernset" <% }else{ %> title="<%= breadCrumbItem.ToolTipText%>" <%}%> >                                                                                          
           <%if (breadCrumbItem.Equals(last)){%>
              <span style="display: inline-table; margin-left: 10px; color:#000000; opacity:0.50;"><a href="<%= breadCrumbItem.Url %>"><%= breadCrumbItem.Text %></a></span>
            <%} else {%>
               <span style="display: inline-table; margin-left: 10px;"><a href="<%= breadCrumbItem.Url %>"><%= breadCrumbItem.Text %></a>
                <i style="display: inline;" class="fa fa-chevron-right"></i>
               </span>  
            <%} %>
        </div>
    <% } %>        
    <%}%>
<%} %>
    <div id="StickyHeaderContainer">        
        <div class="input-group" id="StickyHeaderSearchBoxDiv">
            <input type="text" class="form-control" placeholder="Suche" id="StickyHeaderSearchBox">
            <div class="input-group-btn">
                <button class="btn btn-default" id="SearchButton" onclick="SearchButtonClick()" style="border: 1px #979797 solid; height:34px;" type="submit"><i class="fa fa-search" style="font-size:25px; padding:0px;margin:0px; margin-top:-3px" aria-hidden="true"></i></button>
            </div>
        </div>
        <i class="fa fa-dot-circle"></i>
        <a id="StickyMenuButton" style="margin-top:0px;"><i class="fa fa-bars"></i></a>
    </div>
</div> 