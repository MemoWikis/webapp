<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div>
    <div class="separationBorderTop" style="min-height: 20px;"></div>

    <div id="questionDetailsContainer" class="row">
        
        <div id="categoryList" class="col-sm-5" v-html="categoryList"></div>

        <div id="questionStatistics" class="col-sm-7 row">
            <div id="probabilityContainer" class="col-m-6" ref="probabilityContainer">
                <div style="text-transform: uppercase; font-family: 'Open Sans'; font-size: 10px; font-weight: 600; text-align: center;">Antwortwahrscheinlichkeit</div>
                <div id="semiPieChart">
                    <div ref="semiPie" style="display: flex;justify-content: center">
                    </div>
                </div>
                <div id="probabilityText" style="display: flex;justify-content: center">
                    <div v-if="isLoggedIn" style="text-align: center;font-size:10px">
                        <strong>{{personalProbability}}%</strong> beträgt die Wahrscheinlichkeit, dass du die Frage richtig beantwortest. Durchschnitt aller memucho-Nutzer: <strong>{{avgProbability}}%</strong>
                    </div>
                    <div v-else style="text-align: center;font-size:10px">
                        <strong>{{personalProbability}}%</strong> beträgt die Wahrscheinlichkeit, dass du die Frage richtig beantwortest. Melde dich an, damit wir deine individuelle Wahrscheinlichkeit berechnen können.
                    </div>
                </div>
            </div>
            <div id="counterContainer" class="col-m-6">
                <div style="text-transform: uppercase; font-family: 'Open Sans'; font-size: 10px; font-weight: 600; text-align: center;">Antworten</div>
                <div>
                    <div ref="personalCounter"></div>
                    <div v-if="personalAnswerCount > 0">
                        Von Dir: <br/>
                        {{personalAnswerCount}}x beantwortet <br/>
                        {{personalAnsweredCorrectly}}x richtig / {{personalAnswerCount - personalAnsweredCorrectly}}x falsch
                    </div>
                    <div v-else>
                        Du hast diese Frage noch nie beantwortet.
                    </div>
                </div>
                <div>
                    <div ref="overallCounter"></div>
                    <div v-if="overallAnswerCount > 0">
                        Von allen Nutzern: <br/>
                        {{overallAnswerCount}}x beantwortet <br/>
                        {{overallAnsweredCorrectly}}x richtig / {{overallAnswerCount - overallAnsweredCorrectly}}x falsch
                    </div>
                    <div v-else>
                        Diese Frage wurde noch nie beantwortet.
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="separationBorderTop" style="min-height: 20px;"></div>


</div>
