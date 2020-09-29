<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<InlineTextModel>" %>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperStart.ascx") %>

<text-component content="<%: HttpUtility.HtmlDecode(Model.Content)  %>" inline-template>
    <div>
     <editor-menu-bar :editor="editor" v-slot="{ commands }">
      <div class="menubar">
          <button
          @click="commands.undo"
        >
          Undo
        </button>

        <button
          @click="commands.redo"
        >
          Redo
        </button>

      </div>
    </editor-menu-bar>
    <editor-content :editor="editor">
        <%: Html.Raw(Model.Content)  %>
    </editor-content>
    </div>

</text-component>
     

<%--    <div v-if="textCanBeEdited">
        <inline-text-component/>
    </div>
    <div v-else @click="editInlineText()">
            <%: Html.Raw(HttpUtility.HtmlDecode(Model.Content))  %>
        <div v-if="missingText" class="missingTextInModule"> Hier klicken um Text zu bearbeiten</div>
    </div>--%>

<%: Html.Partial("~/Views/Categories/Detail/Partials/ContentModuleWrapperEnd.ascx") %>