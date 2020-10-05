<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BaseContentModule>" %>

<content-module inline-template orig-markdown="<%: Model.Markdown %>" content-module-type="<%: Model.Type %>">
    <div class="ContentModule <%: Model.Type %>" v-if="!isDeleted" :id="id" :markdown="markdown" v-cloak @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)" :uid="uid">
        <div class="ModuleBorder" :class="{ hover : hoverState, inEditMode : canBeEdited }" v-on="canBeEdited == true ? {click: () => editModule()} : null">