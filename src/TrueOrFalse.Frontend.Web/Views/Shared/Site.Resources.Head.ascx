﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<% if(Request.IsLocal){ %>        
    <link href="/Style/smoothness/jquery-ui.css" rel="stylesheet" type="text/css" media="screen" />
    <script src="/Scripts/jquery-1.8.3.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery-ui-1.8.17.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.validate.min.js" type="text/javascript"></script>
    <script src="/Scripts/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
<% }else{ %>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8.17/themes/smoothness/jquery-ui.css" rel="stylesheet" type="text/css" media="screen" />
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js" type="text/javascript"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.17/jquery-ui.min.js" type="text/javascript"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/jquery.validate/1.9/jquery.validate.min.js" type="text/javascript"></script>
    <script src="http://ajax.aspnetcdn.com/ajax/mvc/3.0/jquery.validate.unobtrusive.min.js" type="text/javascript"></script>
<% } %>

<script src="/Scripts/lib.js" type="text/javascript"></script>
<script src="/Scripts/jquery.sparkline.min.js" type="text/javascript"></script>


<link rel="stylesheet" href="/style/bootstrap.css">
<link href="/Style/site.css" rel="stylesheet" type="text/css" />
<link href="/Style/menu.css" rel="stylesheet" type="text/css" />
<link href="/Style/form.css" rel="stylesheet" type="text/css" />
<link href="/Style/zocial/css/zocial.css" rel="stylesheet" type="text/css" /> <!--buttons for social login-->
<link href="/Style/font-awesome.min.css" rel="stylesheet" type="text/css" /> <%--http://fortawesome.github.com/Font-Awesome/--%>

<link href='http://fonts.googleapis.com/css?family=Pacifico' rel='stylesheet' type='text/css'>

<script src="/Scripts/highcharts.js" type="text/javascript"></script>

<script src="/Scripts/google-code-prettify/prettify.js" type="text/javascript"></script>
<script src="/Scripts/bootstrap.js" type="text/javascript"></script>

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



