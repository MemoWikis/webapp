<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<CategoryGraphModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%= Scripts.Render("~bundles/CategoryGraph") %>

<div id="category-graph"></div>
<script src="/Views/Categories/Edit/GraphDisplay/CategoryGraph.ts" type="text/javascript"></script>