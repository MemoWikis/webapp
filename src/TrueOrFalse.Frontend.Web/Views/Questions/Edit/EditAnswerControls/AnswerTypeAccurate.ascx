<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EditQuestionModel>" %>

<%--<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">

    <script language="javascript" type="text/javascript">
        $(function() {
            $('#Answer').defaultText("Antwort eingeben.");
        });
    </script>
</asp:Content>--%>


<%= Html.LabelFor(m => m.Solution ) %>
<%= Html.TextAreaFor(m => m.Solution, new { @id = "Answer" })%><br />