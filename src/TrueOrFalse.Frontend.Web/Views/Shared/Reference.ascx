<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.Category>" %>
<%@ Import Namespace="TrueOrFalse" %>

<%
    object type = Model.GetTypeModel();
    switch (Model.Type)
    {
       case CategoryType.Book:
           var book = (CategoryTypeBook)type;
            
%>
           <div class="ReferenceBook"> </div>
<%
           break; 
   
       case CategoryType.Daily:
           var daily = (CategoryTypeDaily)type;
            
%>
           <html></html>
<%
           break; 
%> 




<% } %>
