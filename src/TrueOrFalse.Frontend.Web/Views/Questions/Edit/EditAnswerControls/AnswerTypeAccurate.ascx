<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EditQuestionModel>" %>

<%--<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">

    <script language="javascript" type="text/javascript">
        $(function() {
            $('#Answer').defaultText("Antwort eingeben.");
        });
    </script>
</asp:Content>--%>


<div class="control-group">
    <%= Html.LabelFor(m => m.Solution ) %>
    <%= Html.TextAreaFor(m => m.Solution, new { @id = "Answer", @style = "height:18px; width:435px;" })%><br />
</div>