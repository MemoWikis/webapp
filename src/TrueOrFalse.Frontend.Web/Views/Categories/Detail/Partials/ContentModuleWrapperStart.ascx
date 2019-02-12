<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<BaseContentModule>" %>

<content-module inline-template >
    <li class="module" v-if="!isDeleted" markdown="<%: Model.Markdown %>">
        <div class="ContentModule" @mouseenter="updateHoverState(true)" @mouseleave="updateHoverState(false)">
            <div class="ModuleBorder" :class="{ active : hoverState }">