<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<InlineTextModel>" %>

<div v-if="tiptapIsReady">
    <keep-alive>
        <text-component :content="decodedHtml"/>
    </keep-alive>
</div>
<div v-else ref="rawHtml" class="contentPlaceholder">
    <%: Html.Raw(Model.Content)  %>
</div>
