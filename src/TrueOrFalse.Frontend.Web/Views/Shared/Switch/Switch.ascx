<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%= Styles.Render("~/bundles/switch") %>


<div class="center">
    <input type="checkbox" id="cbx" style="display:none" />
    <label for="cbx" class="toggle">
        <span></span>
    </label>
</div>