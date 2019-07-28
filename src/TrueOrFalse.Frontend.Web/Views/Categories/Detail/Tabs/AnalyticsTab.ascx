<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>

<div id="KnowledgeGraphTools">
    <div class="Buttons" style="display: flex;flex-direction: row-reverse">
        
        <div class="Button dropdown">
            <div id="graphDropdown" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true" style="font-size: 18px;color: #999999;">
                <i class="fa fa-ellipsis-v"></i>
            </div>
            <div class="dropdown-menu dropdown-menu-right" aria-labelledby="graphDropdown">
                <span>Ansichtsauswahl:</span>
                <div id="GraphSelectionContainer" style="display: flex">
                    <div class="btn btn-primary" onclick="toggleRad()">Ansicht 1</div>       
                    <div class="btn btn-primary" onclick="toggleRect()">Ansicht 2 (experimentell)</div>
                </div>
                
                <span>Ebenentiefe</span>
                <div class="slidecontainer">
                    <input type="range" min="1" max="11" value="3" class="slider" onchange="setGraph()" id="graphMaxLevel">
                </div>
                
                <span>Knotenzahl</span>
                <div class="slidecontainer">
                    <input type="range" min="1" max="50" value="50" class="slider" onchange="setGraph()" id="graphMaxNodeCount">
                </div>
            </div>
        </div>
        
        <div class="Button knowledgeGraphFullscreen" style="font-size: 18px;color: #999999;">
            <div class="noTextdecoration" style="font-size: 22px; height: 10px;">
                <i id="toggleFullScreenGraph" class="fas fa-expand" style="font-size:18px" onclick="toggleFullscreen()"></i>
            </div>
        </div>

        <div class="Button knowledgeBarCheckBox" style="font-size: 12px;color: #999999;">
            <span>Wissenstandsanzeige</span>
            <input type="checkbox" id="graphShowKnowledgeGraph" onclick="setGraph()">
        </div>
    </div>
</div>


<div id="KnowledgeGraph" style="height:600px">
    <svg id="graph-body" style="width: 100%; height: 100%;"></svg>
    <div class="knowledgeGraphData"></div>
</div>