<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BaseContentModule>" %>

<content-module inline-template orig-markdown="<%: Model.Markdown %>" content-module-type="<%: Model.Type %>">
    <li class="module" v-if="!isDeleted" :id="id" :markdown="markdown" @click="editModule()" v-cloak>
        <div class="ContentModule" @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)">
            <div class="ModuleBorder" :class="{ active : hoverState }">