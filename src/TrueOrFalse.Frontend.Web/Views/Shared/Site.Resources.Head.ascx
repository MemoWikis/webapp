<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% if(Request.IsLocal){ %>        
    <script src="/Scripts/jquery-1.9.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.10.0.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
<% }else{ %>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.10.0/jquery-ui.min.js" type="text/javascript"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.10.0/jquery.validate.min.js" type="text/javascript"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/mvc/3.0/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
<% } %>

<script src="/Scripts/underscore-1.4.3.min.js" type="text/javascript"></script>
<script src="/Scripts/lib.js" type="text/javascript"></script>
<script src="/Scripts/jquery.sparkline.min.js" type="text/javascript"></script>
<script src="/Scripts/modernizr-2.6.2.js" type="text/javascript"></script>


<script src="/Scripts/google-code-prettify/prettify.js" type="text/javascript"></script>
<script src="/Scripts/bootstrap.js" type="text/javascript"></script>


<link rel="stylesheet" href="/style/bootstrap.css">
<link href="/Style/site.css" rel="stylesheet" type="text/css" />
<link href="/Style/menu.css" rel="stylesheet" type="text/css" />
<link href="/Style/form.css" rel="stylesheet" type="text/css" />
<link href="/Style/zocial/css/zocial.css" rel="stylesheet" type="text/css" /> <!--buttons for social login-->
<link href="/Style/font-awesome.min.css" rel="stylesheet" type="text/css" /> <%--http://fortawesome.github.com/Font-Awesome/--%>

<link href='http://fonts.googleapis.com/css?family=Pacifico' rel='stylesheet' type='text/css'>
<link href='http://fonts.googleapis.com/css?family=Lato' rel='stylesheet' type='text/css'>



<script type="text/javascript">
    $(function () {
        $('a.button').hover(
		    function () { $(this).addClass('ui-state-hover'); },
		    function () { $(this).removeClass('ui-state-hover'); }
		);

        /* links with class submit should submit a forms */
        $("a.submit").click(function (event) {
            $(this).closest('form').submit();
            event.preventDefault();
        });

        $(".alert-message").alert();
        $('.show-tooltip').tooltip();
        $('.show-popover').popover();
    });
</script>



