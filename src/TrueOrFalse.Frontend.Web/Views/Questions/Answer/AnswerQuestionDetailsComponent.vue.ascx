<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div>
    <div class="separationBorderTop" style="min-height: 20px;"></div>

    <div id="questionDetailsContainer" class="row">
        
        <div id="categoryList" class="col-sm-5" v-html="categoryList"></div>

        <div id="questionStatistics" class="col-sm-7 row">
            <div id="probabilityContainer" class="col-sm-6" ref="probabilityContainer">
                <div style="text-transform: uppercase; font-family: 'Open Sans'; font-size: 10px; font-weight: 600; text-align: center;">Antwortwahrscheinlichkeit</div>
                <div id="semiPieChart">
                    <div ref="semiPie" style="display: flex;justify-content: center">
                    </div>
                </div>
                <div id="probabilityText" style="display: flex;justify-content: center">
                    <div v-if="isLoggedIn" style="text-align: center; font-size: 10px;max-width:300px">
                        <strong>{{personalProbability}}%</strong> beträgt die Wahrscheinlichkeit, dass du die Frage richtig beantwortest. Durchschnitt aller memucho-Nutzer: <strong>{{avgProbability}}%</strong>
                    </div>
                    <div v-else style="text-align: center;font-size:10px">
                        <strong>{{personalProbability}}%</strong> beträgt die Wahrscheinlichkeit, dass du die Frage richtig beantwortest. Melde dich an, damit wir deine individuelle Wahrscheinlichkeit berechnen können.
                    </div>
                </div>
            </div>
            <div id="counterContainer" class="col-sm-6" style="font-size:10px">
                <div style="text-transform: uppercase; font-family: 'Open Sans'; font-size: 10px; font-weight: 600; text-align: center;">Antworten</div>
                <div style="display: flex; justify-content: center;padding-top:30px">
                    <div ref="personalCounter"></div>
                    <div v-if="personalAnswerCount > 0" style="max-width: 160px; width: 100%;padding-left:20px">
                        Von Dir: <br/>
                        <strong>{{personalAnswerCount}}x</strong> beantwortet <br/>
                        <strong>{{personalAnsweredCorrectly}}x</strong> richtig / <strong>{{personalAnswerCount - personalAnsweredCorrectly}}x</strong> falsch
                    </div>
                    <div v-else style="max-width: 160px;width:100%;padding-left:20px">
                        Du hast diese Frage noch nie beantwortet.
                    </div>
                </div>
                <div style="display: flex; justify-content: center;padding-top:30px">
                    <div ref="overallCounter"></div>
                    <div v-if="overallAnswerCount > 0" style="max-width: 160px;width:100%;padding-left:20px">
                        Von allen Nutzern: <br/>
                        {{overallAnswerCount}}x beantwortet <br/>
                        {{overallAnsweredCorrectly}}x richtig / {{overallAnswerCount - overallAnsweredCorrectly}}x falsch
                    </div>
                    <div v-else style="max-width: 160px;width:100%;padding-left:20px">
                        Diese Frage wurde noch nie beantwortet.
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <div class="separationBorderTop" style="min-height: 20px;"></div>


</div>
