<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<QuestionSolutionMatchList>" %>


<% var random = new Random();
   foreach (var choice in Model.Pairs.OrderBy(x => random.Next()))
   { %>
    <div id="pairs">
        
    </div>
<% } %>
<br/>

HERE THE DRAGABLE RIGHT ELEMENTS
<script type="text/javascript">
    $

</script>