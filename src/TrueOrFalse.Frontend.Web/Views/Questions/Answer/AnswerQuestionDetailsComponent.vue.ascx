
<div style="min-height:240px">
    <div class="separationBorderTop" style="min-height: 20px;"></div>

    <div id="questionDetailsContainer" class="row">
        <div id="categoryList" class="col-sm-5" >
            <div class="sectionLabel">Verwandte Themen</div>
            <div class="categoryListChips" v-html="categoryList">
            </div>
            <div class="categoryListLinks">
                <a v-for="(c, i) in categories" :href="c.linkToCategory">{{c.name}}{{i != categories.length - 1 ? ', ' : ''}}</a>
            </div>
        </div>
    </div>
    
    <div class="separationBorderTop" style="min-height: 10px;"></div>
</div>
