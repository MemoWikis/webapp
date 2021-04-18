
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div>
    <div class="separationBorderTop" style="min-height: 20px;"></div>

    <div id="questionDetailsContainer" class="row" style="min-height:265px">
        <div id="categoryList" class="col-sm-5" :class="{ isLandingPage:'isLandingPage'}">
            <div class="sectionLabel">Themen</div>
            <div class="categoryListChips" v-html="categoryList">
            </div>
            <div class="categoryListLinks">
                <a v-for="(c, i) in categories" :href="c.linkToCategory">{{c.name}}{{i != categories.length - 1 ? ', ' : ''}}</a>
            </div>
        </div>
        <div id="questionStatistics" class="col-sm-7 row">
            <div id="probabilityContainer" class="col-sm-6" ref="probabilityContainer">
                <div class="sectionLabel">Antwortwahrscheinlichkeit</div>
                <div id="semiPieSection">
                    <div id="semiPieChart" style="min-height:130px">
                        <div class="semiPieSvgContainer" ref="semiPie" :class="{'isInWishknowledge' : isInWishknowledge}">
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
                
                <div class="counterBody">
                    <div class="counterHalf">
                        <div ref="personalCounter" style="min-width:50px"></div>
                        <div v-if="personalAnswerCount > 0" class="counterLabel">
                            Von Dir: <br/>
                            <strong>{{answerCount}}</strong> mal beantwortet <br/>
                            <strong>{{correctAnswers}}</strong> richtig / <strong>{{wrongAnswers}}</strong> falsch
                        </div>
                        <div v-else-if="isLoggedIn" class="counterLabel">
                            Du hast diese Frage noch nie beantwortet.
                        </div>
                        <div v-else class="counterLabel">
                            Du bist nicht angemeldet. Wir haben keine Daten. <a role="button" @click="openLogin()">Anmelden</a>
                        </div>
                    </div>
                    <div class="counterHalf">
                        <div ref="overallCounter" style="min-width:50px"></div>
                        <div v-if="overallAnswerCount > 0" class="counterLabel">
                            Von allen Nutzern: <br/>
                            <strong>{{allAnswerCount}}</strong> mal beantwortet <br/>
                            <strong>{{allCorrectAnswers}}</strong> richtig / <strong>{{allWrongAnswers}}</strong> falsch
                        </div>
                        <div v-else class="counterLabel">
                            <template v-if="visibility == 1">
                                Diese Frage ist <br/>
                                privat und nur für <br/>
                                dich sichtbar
                            </template>
                            <template v-else>
                                Diese Frage wurde noch nie beantwortet.
                            </template>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="separationBorderTop" style="min-height: 10px;"></div>
</div>
