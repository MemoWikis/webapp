<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div>
    <div v-if="showTopBorder"></div>

    <div id="questionDetailsContainer">
        <div id="questionStatistics">
            <div id="probabilityContainer">
                <div id="semiPieChart">
                    <div ref="semiPie">
                        <svg width="400" height="200">
                            <g transform="translate(200,100)">
                                <path ref="baseArc" :d="baseArcPath" ></path>
                                <path ref="personalArc" :d="personalArcPath"></path>
                                <path ref="avgArc" :d="avgArcPath"></path>
                            </g>
                        </svg>
                    </div>
                </div>
                <div id="probabilityText">
                    <div v-if="isLoggedIn"></div>
                    <div v-else></div>
                </div>
                <div id="probabilityState" :class=""></div>
            </div>
            <div id="counterContainer"></div>
        </div>
    
        <div id="categoryList" v-if="showCategoryList"></div>
        <button @click="updateData(50)"></button>
    </div>

</div>
