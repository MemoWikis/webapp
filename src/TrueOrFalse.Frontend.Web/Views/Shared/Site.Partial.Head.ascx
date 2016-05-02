<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%@ Import Namespace="System.Web.Optimization" %>
<meta http-equiv="X-UA-Compatible" content="IE=edge" />
<%= Styles.Render("~/bundles/css") %>
<%= Scripts.Render("~/bundles/shared") %>

<% if (!Settings.DevelopOffline()){ %>
    <link href='//fonts.googleapis.com/css?family=Emilys+Candy' rel='stylesheet' type='text/css'>
    <link href='//fonts.googleapis.com/css?family=Lato' rel='stylesheet' type='text/css'>
    <link href='//fonts.googleapis.com/css?family=Roboto:500' rel='stylesheet' type='text/css'>
    <link href='//fonts.googleapis.com/css?family=Roboto+Slab' rel='stylesheet' type='text/css'>
    <link href="//fonts.googleapis.com/css?family=Open+Sans:400italic,700italic,400,600,700,800&subset=latin,cyrillic-ext,greek-ext,greek,vietnamese,latin-ext,cyrillic" rel="stylesheet" type="text/css" />
<% } %>

<!-- links to different favicons, created with website http://realfavicongenerator.net -->
<link rel="apple-touch-icon" sizes="57x57" href="/apple-touch-icon-57x57.png">
<link rel="apple-touch-icon" sizes="60x60" href="/apple-touch-icon-60x60.png">
<link rel="apple-touch-icon" sizes="72x72" href="/apple-touch-icon-72x72.png">
<link rel="apple-touch-icon" sizes="76x76" href="/apple-touch-icon-76x76.png">
<link rel="apple-touch-icon" sizes="114x114" href="/apple-touch-icon-114x114.png">
<link rel="apple-touch-icon" sizes="120x120" href="/apple-touch-icon-120x120.png">
<link rel="apple-touch-icon" sizes="144x144" href="/apple-touch-icon-144x144.png">
<link rel="apple-touch-icon" sizes="152x152" href="/apple-touch-icon-152x152.png">
<link rel="apple-touch-icon" sizes="180x180" href="/apple-touch-icon-180x180.png">
<link rel="icon" type="image/png" href="/favicon-32x32.png" sizes="32x32">
<link rel="icon" type="image/png" href="/favicon-194x194.png" sizes="194x194">
<link rel="icon" type="image/png" href="/favicon-96x96.png" sizes="96x96">
<link rel="icon" type="image/png" href="/android-chrome-192x192.png" sizes="192x192">
<link rel="icon" type="image/png" href="/favicon-16x16.png" sizes="16x16">
<link rel="manifest" href="/manifest.json">
<link rel="mask-icon" href="/safari-pinned-tab.svg" color="#afd534">
<meta name="msapplication-TileColor" content="#da532c">
<meta name="msapplication-TileImage" content="/mstile-144x144.png">
<meta name="theme-color" content="#ffffff">


<meta name="viewport" content="width=device-width, initial-scale=1">

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

        $('#showUserOptions').click(function () {
            alert("hello");
            $('#userOptions').dropdown();
        });
        
    });
</script>