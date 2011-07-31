<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="ViewPage<EditCategoryModel>" %>

<asp:Content ID="head" ContentPlaceHolderID="Head" runat="server">
    <script src="EditCategory.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

//        $(function () {

//            $("#addClassificationRow").click(function () {
//                $.ajax({
//                    url: this.href,
//                    cache: false,
//                    error: function (jqXHR, textStatus, errorThrown) {
//                        alert("hello");
//                        /*console.info('in error');
//                        console.log(jqXHR, textStatus, errorThrown); */
//                    },
//                    success: function (html) {
//                        $("#classifications").append(html);
//                    }
//                });
//                return false;
//            });
//        
//            
//        });


//        $("a.deleteRow").live("click", function () {
//            $(this).parents("div.editorRow:first").remove();
//            return false;
//        });
        
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2 class="form-title">Kategorie erstellen</h2>

<% using (Html.BeginForm()){ %>

    <%= Html.LabelFor(m => m.Name ) %>
    <%= Html.TextBoxFor(m => m.Name ) %>

    <div id="classifications">
        <% foreach (var classification in Model.Classifications){ %>
            <h3 class="form-sub-title">Unterkategorie</h3> 
        <%      Html.RenderPartial("~/Views/Categories/Edit/ClassificationRow.ascx", classification);
           } %>
    </div>

    <br />
    <label>&nbsp;</label>
    <a href="Create/AddClassificationRow" id="addClassificationRow">
        <img src='/Images/Buttons/add.png'> <span>Unterkategorie hinzufügen</span>
    </a>

    <% Html.ActionLink("demo", "AddClassification"); %>

    <br/><br/><br/>
    <label>&nbsp;</label>
    <%= Buttons.Submit("Speichern", inline:true)%>
    <%= Buttons.Submit("Speichern & Neu", inline: true)%>

<% } %>

</asp:Content>

