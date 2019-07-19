<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>

<div id="KnowledgeTab"></div>

<div id="KnowledgeGraph" style="height:600px">
    <svg id="graph-body" style="width: 100%; height: 100%;"></svg>
    <div class="knowledgeGraphData"></div>
</div>