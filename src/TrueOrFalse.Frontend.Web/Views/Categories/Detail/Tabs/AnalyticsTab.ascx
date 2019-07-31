<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>

<div id="KnowledgeGraphTools" class="" style="display: flex;flex-direction: row-reverse">
    
    <div id="graphToolbar" class="graphToolbar dropdown">

        <div id="graphDropdown" class="btn btn-link" type="button" aria-haspopup="true" aria-expanded="true" style="font-size: 18px;color: #999999">
            <i class="fa fa-ellipsis-v"></i>
        </div>

        <div id="graphDropdownMenu" class="dropdown-menu dropdown-menu-right" aria-labelledby="graphDropdown" style="">
            <div class="selectionContainer">
                <label>Darstellung</label>
                <div id="GraphSelectionContainer">
                    <div id="radNodeButton" class="btn" onclick="toggleRad()">
                        <img src="/Images/Various/RadNodeSelect.svg">
                    </div> 
                    <div id="rectNodeButton" class="btn" onclick="toggleRect()">
                        <img src="/Images/Various/RectNodeSelect.svg">
                    </div>
                </div>
            </div>

            <div class="sliderContainer">
                <div class="sliderLabel"> 
                    <label for="graphMaxLevel">Ebenentiefe</label>
                    <input id="nodeLevelValue" class="col-sm-4"  type="number" for="graphMaxLevel" min="1" max="11" value="3" onchange="setGraph()" oninput="setNodeLevelValue(slider)">
                </div>
                <div class="col-sm-12">
                    <input id="graphMaxLevel" type="range" min="1" max="11" value="3" class="slider" onchange="setGraph()" oninput="setNodeLevelValue('slider')">
                </div>
            </div>
            
            <div class="sliderContainer">
                <div class="sliderLabel">
                    <label for="graphMaxNodeCount">Knotenpunkte</label>
                    <input id="nodeCountValue" class="col-sm-4" type="number" for="graphMaxNodeCount" min="1" max="50" value="50" onchange="setGraph()" oninput="setNodeCountValue()" >
                </div>
                <div class="col-sm-12">
                    <input id="graphMaxNodeCount"  type="range" min="1" max="50" class="slider graphMaxNodeCount" onchange="setGraph()" oninput="setNodeCountValue('slider')">
                </div>

            </div>
            
            <div id="nodeCountWarning" class="alert alert-warning hidden">
                <strong>Achtung!</strong> Eine zu hohe Anzahl an Knotenpunkten kann die Performance beeinträchtigen.
            </div>
        </div>
    </div>
    
    <div id="knowledgeGraphFullscreen" style="font-size: 18px;">
        <div class="fullScreen-btn btn btn-link " type="button">
            <i id="toggleFullScreenGraph" class="fas fa-expand" style="font-size:18px" onclick="toggleFullscreen()"></i>
        </div>
    </div>

    <div id="knowledgeBarCheckBox" class="invisible">
        <span class="knowledgeGraphBarLabel">Wissenstandsanzeige</span>
        <label class="knowledgeGraphBarToggle">
            <input type="checkbox" id="graphShowKnowledgeGraph" onclick="setGraph()" checked>
            <span class="graphToggle round"></span>
        </label>
    </div>
</div>


<div id="KnowledgeGraph" style="height:600px">
    <svg id="graph-body" style="width: 100%; height: 100%;"></svg>
    <div class="knowledgeGraphData"></div>
</div>