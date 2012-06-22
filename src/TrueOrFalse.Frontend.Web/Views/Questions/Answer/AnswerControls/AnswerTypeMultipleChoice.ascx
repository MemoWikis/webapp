<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<AnswerTypeMulitpleChoiceModel>" %>

<select>
<% var random = new Random();
   foreach (var choice in Model.Choices.OrderBy(x => random.Next()))
   { %>
  <option><%: choice %></option>
<% } %>
</select>