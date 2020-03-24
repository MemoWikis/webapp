﻿<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div>
    <div v-if="showTopBorder"></div>

    <div id="questionDetailsContainer">
        <div id="questionStatistics">
            <div id="probabilityContainer">
                <div id="semiPieChart">
                    <div ref="semiPie">
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
        <button @click="updateArc()">updateArc</button>
    </div>

</div>
