<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>

<div id="KnowledgeTab">
    <input type="checkbox" id="graphShowKnowledgeGraph" onclick="setGraph()">

    <i id="toggleFullScreenGraph" class="fas fa-expand" style="font-size:18px" onclick="toggleFullscreen()"></i>
</div>

<div class="btn btn-primary" onclick="toggleRad()">Ansicht 1</div>       
<div class="btn btn-primary" onclick="toggleRect()">Ansicht 2 (experimentell)</div>

<div class="slidecontainer">
    <input type="range" min="1" max="11" value="3" class="slider" onchange="setGraph()" id="graphMaxLevel">
</div>

<div class="slidecontainer">
    <input type="range" min="1" max="50" value="50" class="slider" onchange="setGraph()" id="graphMaxNodeCount">
</div>

<div id="KnowledgeGraph" style="height:600px">
    <svg id="graph-body" style="width: 100%; height: 100%;"></svg>
    <div class="knowledgeGraphData"></div>
</div>