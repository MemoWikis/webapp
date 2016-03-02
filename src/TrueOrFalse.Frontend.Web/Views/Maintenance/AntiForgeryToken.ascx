<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<script language="javascript">
        $(function() {
            $("a[data-url=toSecurePost]").click(function (e) {
                e.preventDefault();

                $("#form").attr("action", $(this).attr("href"));
                $("#form").submit();
            });

        });
</script>

<form id="form" action="destination.html" method="post">
    <%= Html.AntiForgeryToken() %>
</form>
