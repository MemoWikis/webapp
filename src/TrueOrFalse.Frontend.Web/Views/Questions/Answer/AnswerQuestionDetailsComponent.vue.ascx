<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

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
                    <div v-if="isLoggedIn"><strong>{{personalProbability}}%</strong> beträgt die Wahrscheinlichkeit, dass du die Frage richtig beantwortest. Durchschnitt aller memucho-Nutzer: <strong>{{avgProbability}}%</strong>
                    </div>
                    <div v-else>
                        <strong>{{personalProbability}}%</strong> beträgt die Wahrscheinlichkeit, dass du die Frage richtig beantwortest. Melde dich an, damit wir deine individuelle Wahrscheinlichkeit berechnen können.
                    </div>
                </div>
                <div id="probabilityState" :class=""></div>
            </div>
            <div id="counterContainer"></div>
        </div>
    
        <div id="categoryList" v-if="showCategoryList"></div>
        <button @click="calculateLabelWidth()">calc Pos</button>
    </div>

</div>
