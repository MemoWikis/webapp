<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<InlineTextModel>" %>

<div v-if="tiptapIsLoaded">
    <keep-alive>
        <text-component :content="decodedHtml"/>
    </keep-alive>
</div>
<div v-else>
    <%: Html.Raw(Model.Content)  %>
</div>
