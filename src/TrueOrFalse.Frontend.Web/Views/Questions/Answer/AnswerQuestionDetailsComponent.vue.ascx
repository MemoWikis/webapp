
<div style="min-height:300px">
    <div class="separationBorderTop" style="min-height: 20px;"></div>

    <div id="questionDetailsContainer" class="row">
        <div id="categoryList" class="col-sm-5" >
            <div class="sectionLabel">Verwandte Themen</div>
            <div v-html="categoryList">

            </div>
        </div>

        <div id="questionStatistics" class="col-sm-7 row">
            <div id="probabilityContainer" class="col-sm-6" ref="probabilityContainer">
                <div class="sectionLabel">Antwortwahrscheinlichkeit</div>
                <div id="semiPieSection">
                    <div id="semiPieChart">
                        <div class="semiPieSvgContainer" ref="semiPie">
                        </div>
                    </div>
                    <div id="probabilityText">
                        <div v-if="isLoggedIn" style="">
                            <strong>{{personalProbability}}%</strong> beträgt die Wahrscheinlichkeit, dass du die Frage richtig beantwortest. Durchschnitt aller memucho-Nutzer: <strong>{{avgProbability}}%</strong>
                        </div>
                        <div v-else style="">
                            <strong>{{personalProbability}}%</strong> beträgt die Wahrscheinlichkeit, dass du die Frage richtig beantwortest. Melde dich an, damit wir deine individuelle Wahrscheinlichkeit berechnen können.
                        </div>
                    </div>
                </div>
                
            </div>
            <div id="counterContainer" class="col-sm-6" style="font-size:12px">
                <div class="sectionLabel">Antworten</div>
                
                <div style="display: flex; flex-wrap: wrap;">
                    <div style="display: flex; justify-content: center;padding-top:30px">
                        <div ref="personalCounter"></div>
                        <div v-if="personalAnswerCount > 0" style="max-width: 160px; width: 100%;padding:5px 0 0 20px">
                            Von Dir: <br/>
                            <strong>{{personalAnswerCount}}</strong> mal beantwortet <br/>
                            <strong>{{personalAnsweredCorrectly}}</strong> richtig / <strong>{{personalAnswerCount - personalAnsweredCorrectly}}</strong> falsch
                        </div>
                        <div v-else style="max-width: 160px;width:100%;padding:5px 0 0 20px">
                            Du hast diese Frage noch nie beantwortet.
                        </div>
                    </div>
                    <div style="display: flex; justify-content: center;padding-top:30px">
                        <div ref="overallCounter"></div>
                        <div v-if="overallAnswerCount > 0" style="max-width: 160px;width:100%;padding:5px 0 0 20px">
                            Von allen Nutzern: <br/>
                            <strong>{{overallAnswerCount}}</strong> mal beantwortet <br/>
                            <strong>{{overallAnsweredCorrectly}}</strong> richtig / <strong>{{overallAnswerCount - overallAnsweredCorrectly}}</strong> falsch
                        </div>
                        <div v-else style="max-width: 160px;width:100%;padding:5px 0 0 20px">
                            Diese Frage wurde noch nie beantwortet.
                        </div>
                    </div>
                </div>
                
            </div>
        </div>
    </div>
    
    <div class="separationBorderTop" style="min-height: 20px;"></div>
</div>
