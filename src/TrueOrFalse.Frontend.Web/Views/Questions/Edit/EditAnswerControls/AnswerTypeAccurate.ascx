<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<EditQuestionModel>" %>

<%--<asp:Content runat="server" ID="head" ContentPlaceHolderID="Head">

    <script language="javascript" type="text/javascript">
        $(function() {
            $('#Answer').defaultText("Antwort eingeben.");
        });
    </script>
</asp:Content>--%>


<%= Html.LabelFor(m => m.Answer ) %>
<%= Html.TextAreaFor(m => m.Answer, new { @id = "txtAnswerValue" })%><br />