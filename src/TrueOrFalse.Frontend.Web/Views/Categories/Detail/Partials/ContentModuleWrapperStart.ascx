<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BaseContentModule>" %>

<content-module inline-template orig-markdown="<%: Model.Markdown %>" content-module-type="<%: Model.Type %>">
    <li class="module" v-if="!isDeleted" :markdown="markdown" :id="id" :role="button" :data-toggle="modal" :data-target="modalType" :data-component-id="id" :data-markdown="origMarkdown" @click="isListening = true">
        <div class="ContentModule" @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)">
            <div class="ModuleBorder" :class="{ active : hoverState }">