<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BaseContentModule>" %>

<content-module inline-template orig-content="<%: Model.TemplateJson.OriginalJson %>" content-module-type="<%: Model.Type %>" :editMode="editMode">
    <div class="ContentModule <%: Model.Type %>" v-if="!isDeleted" :id="id" v-cloak @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)" :uid="uid">
        <div class="ModuleBorder" :class="{ hover : hoverState, inEditMode : canBeEdited }">