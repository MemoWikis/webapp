
<div style="min-height:300px">
    <div class="separationBorderTop" style="min-height: 20px;"></div>

    <div id="questionDetailsContainer" class="row">
        <div id="categoryList" class="col-sm-5" >
            <div class="section-label">Verwandte Themen</div>
            <div v-html="categoryList">

            </div>
        </div>

        <div id="questionStatistics" class="col-sm-7 row">
            <div id="probabilityContainer" class="col-sm-6" ref="probabilityContainer">
                <div class="section-label">Antwortwahrscheinlichkeit</div>
                <div id="semiPieSection">
                    <div id="semiPieChart">
                        <div class="semiPieSvgContainer" ref="semiPie">
                        </div>
                    </div>
                    <div id="probabilityText">
                        <div v-if="isLoggedIn" style="text-align: center; font-size: 12px;max-width:300px">
                            <strong>{{personalProbability}}%</strong> beträgt die Wahrscheinlichkeit, dass du die Frage richtig beantwortest. Durchschnitt aller memucho-Nutzer: <strong>{{avgProbability}}%</strong>
                        </div>
                        <div v-else style="text-align: center;font-size:12px">
                            <strong>{{personalProbability}}%</strong> beträgt die Wahrscheinlichkeit, dass du die Frage richtig beantwortest. Melde dich an, damit wir deine individuelle Wahrscheinlichkeit berechnen können.
                        </div>
                    </div>
                </div>
                
            </div>
            <div id="counterContainer" class="col-sm-6" style="font-size:12px">
                <div class="section-label">Antworten</div>
                
                <div style="display: flex; flex-wrap: wrap;">
                    <div style="display: flex; justify-content: center;padding-top:30px">
                        <div ref="personalCounter"></div>
                        <div v-if="personalAnswerCount > 0" style="max-width: 160px; width: 100%;padding:5px 0 0 20px">
                            Von Dir: <br/>
                            <strong>{{personalAnswerCount}}x</strong> beantwortet <br/>
                            <strong>{{personalAnsweredCorrectly}}x</strong> richtig / <strong>{{personalAnswerCount - personalAnsweredCorrectly}}x</strong> falsch
                        </div>
                        <div v-else style="max-width: 160px;width:100%;padding:5px 0 0 20px">
                            Du hast diese Frage noch nie beantwortet.
                        </div>
                    </div>
                    <div style="display: flex; justify-content: center;padding-top:30px">
                        <div ref="overallCounter"></div>
                        <div v-if="overallAnswerCount > 0" style="max-width: 160px;width:100%;padding:5px 0 0 20px">
                            Von allen Nutzern: <br/>
                            <strong>{{overallAnswerCount}}x</strong> beantwortet <br/>
                            <strong>{{overallAnsweredCorrectly}}x</strong> richtig / <strong>{{overallAnswerCount - overallAnsweredCorrectly}}x</strong> falsch
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
