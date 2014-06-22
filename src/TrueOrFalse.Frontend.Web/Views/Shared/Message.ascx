<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.Web.UIMessage>" %>

<div class="<%= Model.CssClass %> fade in">
    <a class="close" data-dismiss="alert" href="#">×</a>
    <%= Model.Text %>
</div>


<script type="text/javascript">
    $(function() {
        $(".alert")
            .animate({ opacity: 0.35 }, 400)
            .animate({ opacity: 1.00 }, 1200);
    });
</script>
