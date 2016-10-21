﻿<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<WelcomeBoxTopCategoriesModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<% foreach (var category in Model.Categories) {%>
    <div class="LabelItem LabelItem-Category">
        <a href="<%= Links.CategoryDetail(category) %>"><span class=""><%: category.Name %></span></a> 
        <% if (category.CountQuestions == 0) { %> 
            (0 Fragen)
        <% } else if (category.CountQuestions == 1) { %> 
            <a href="<%: Links.QuestionWithCategoryFilter(Url, category) %>" class="NumberQuestions">(1 Frage)</a>
        <% } else {%>
            <a href="<%: Links.QuestionWithCategoryFilter(Url, category) %>">(<%: category.CountQuestions %> Fragen)</a>
        <%} %>

    </div>
<%} %>

