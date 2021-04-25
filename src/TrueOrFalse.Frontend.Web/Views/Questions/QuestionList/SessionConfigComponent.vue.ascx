<%@ Control Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewUserControl<QuestionListModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<session-config-component inline-template @update="updateQuestionsCount" :questions-count="questionsCount" :all-questions-count-from-category="allQuestionsCountFromCategory">
    <div class="rootElement">
                <% if(Model.IsSessionNoteFadeIn){%>
                <div id="LearningSessionReminderQuestionList">
                    <img id="SessionConfigReminderLeft" src="/Images/Various/SessionConfigReminderLeft.svg" >
                    <img id="SessionConfigReminderRight" src="/Images/Various/SessionConfigReminder.svg" >
                    <span class="far fa-times-circle"></span>
                </div>
                    <% } %>
        <div id="CustomSessionConfigBtn" class="btn btn-link" @click="openModal()" data-toggle="tooltip" data-html="true" title="<p><b>Persönliche Filter helfen Dir</b>. Nutze die Lernoptionen und entscheide welche Fragen Du lernen möchtest.</p>">
            <i class="fa fa-cog" aria-hidden="true"></i>
        </div>
        <div class="modal fade" id="SessionConfigModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="notLoggedInModal" v-show="!isLoggedIn">
                        <span class="notLoggedInHeader">Die Lernoptionen sind nur für eingeloggte Nutzer verfügbar.</span>
                        <span class="notLoggedInButton"><button class="btn btn-primary"> <%: Html.ActionLink("Kostenlos registrieren", Links.RegisterAction, Links.RegisterController) %></button></span>
                        <span class="login">Ich bin schon Nutzer!&nbsp;<a href="#" data-btn-login="true">Anmelden!</a></span>
                    </div>
                    <div class="modal-header" id="SessionConfigHeader">
                        <h5>LERNOPTIONEN</h5>
                        <h4 class="modal-title">Personalisiere dein Lernen </h4>
                    </div>
                    <div class="modal-body">
                        <div ref="radioSection" class="must-logged-in" :class="{'disabled-radios' : !isLoggedIn}" @mouseover="isHoveringOptions = true" @mouseleave="isHoveringOptions = false">
                            <div class="modal-section-label" :class="{inactive: !isLoggedIn}">Prüfungsmodus&nbsp;<i class="fa fa-info-circle" aria-hidden="true"></i></div>
                                <div class="test-mode" :class="{inactive: !isLoggedIn}">
                                    <div class="center">
                                        <input type="checkbox" id="cbx" style="display:none" v-model="isTestMode" :disabled="!isLoggedIn"  />
                                        <label for="cbx" class="toggle" :class="{forbidden: !isLoggedIn}">
                                            <span></span>
                                        </label>
                                    </div>
                                </div>
                                <div class="test-mode-info" :class="{inactive: !isLoggedIn}" >
                                    Du willst es Wissen? Im Prüfungsmodus kannst Du Dein Wissen realistisch testen: zufällige Fragen ohne Antworthilfe und Wiederholungen. Viel Erfolg!
                                </div>
                                <div class="modal-divider"></div>
                                <div id="CheckboxesLearnOptions" class="row" :class="{inactive: !isLoggedIn}">
                                    <div class="col-sm-6">
                                        <label class="checkbox-label">
                                            <input id="AllQuestions" type="checkbox" v-model="allQuestions" :disabled="!isLoggedIn" value="False" v-if="allQuestions || !displayMinus" v-on:click="allQuestions = !allQuestions"/>
                                            <i id="AllQuestionMinus" class="fas fa-minus-square" v-if="displayMinus" v-on:click="allQuestions = !allQuestions"></i>
                                            Alle Fragen
                                        </label> 
                                        <br />
                                        <label class="checkbox-label">
                                            <input id="QuestionInWishknowledge" type="checkbox" v-model="inWishknowledge" :disabled="!isLoggedIn" value="False"/>
                                            In meinem Wunschwissen
                                        </label>
                                    </div>
                                    <div class="col-sm-6">
                                        <label class="checkbox-label">
                                            <input id="UserIsAuthor" type="checkbox" v-model="createdByCurrentUser" :disabled="!isLoggedIn" value="False"/>
                                            Von mir erstellt
                                        </label> <br />
                                        <label class="checkbox-label">
                                            <input id="IsNotQuestionInWishKnowledge" type="checkbox" v-model="isNotQuestionInWishKnowledge" :disabled="!isLoggedIn" value="False"/>
                                            Nicht in meinem Wunschwissen
                                        </label>
                                    </div>
                            </div>
                        </div>
                        <div class="sliders row">
                            <div class="col-sm-6">
                                <label class="sliderLabel">Deine Antwortwahrscheinlichkeit</label>
                                <div class="sliderContainer">
                                    <div class="leftLabel">gering</div>
                                    <div class="vueSlider">                            
                                        <vue-slider direction="ltr" :lazy="true" v-model="probabilityRange" :tooltip-formatter="percentages"></vue-slider>
                                    </div>
                                    <div class="rightLabel">hoch</div>
                                </div>
                            </div>
                            <div class="col-sm-6">
                                <label class="sliderLabel"> Maximale Anzahl an Fragen</label>
                                <div v-if="maxSelectableQuestionCount > 0" class="sliderContainer">
                                    <div class="leftLabel">0</div>
                                    <div class="vueSlider">                            
                                        <vue-slider v-model="selectedQuestionCount" :max="maxSelectableQuestionCount" :tooltip="'always'"></vue-slider>
                                    </div>
                                    <div class="rightLabel">{{maxSelectableQuestionCount}}</div>
                                </div>
                                <div v-else class="alert alert-warning noQuestions" role="alert">Leider sind keine Fragen mit diesen Einstellungen verfügbar. Bitte ändere die Antwortwahrscheinlichkeit oder wähle "Alle Fragen" aus.</div>
                                <div class="alert alert-warning" v-if="(selectedQuestionCount == 0) && maxSelectableQuestionCount > 0">Du musst mindestens 1 Frage auswählen.</div>
                                
                            </div>
                        </div>
                        <div class="row modal-more-options">
                            <div class="more-options col-sm-12" @click="displayNone = !displayNone">
                                <span >Erweiterte Optionen</span>
                                <span class="angle">
                                    <i v-if="displayNone" class="fas fa-angle-down"></i>
                                    <i v-if="!displayNone" class="fas fa-angle-up"></i>
                                </span>
                            </div>
                            <div id="QuestionSortSessionConfig" class=" col-sm-12" v-bind:class="{displayNone: displayNone}">
                                <div class="randomQuestions" :class="{inactive: !isLoggedIn || isTestMode}">
                                    <input type="checkbox" id="randomQuestions" style="display:none" :disabled="isTestModeOrNotLoginIn" v-model="randomQuestions"  />
                                    <label for="randomQuestions" class="toggle" :class="{forbidden: !isLoggedIn || isTestMode}">
                                        <span></span>
                                    </label>
                                    <span>&nbsp;Zufällige Fragen<i> Erhöhe die Schwierigkeit mit zufällig vorgelegten Fragen.</i></span> 
                                </div>
                                
                                <div class="answerHelp" :class="{inactive: !isLoggedIn || isTestMode}">
                                    <input type="checkbox" id="answerHelp" style="display:none" :disabled="isTestModeOrNotLoginIn" v-model="answerHelp" />
                                    <label for="answerHelp" class="toggle" :class="{forbidden: !isLoggedIn || isTestMode}">
                                        <span></span>
                                    </label>
                                    <span>&nbsp;Antworthilfe<i> Die Antworthilfe zeigt dir auf Wunsch die richtige Antwort</i></span>
                                </div>
                                <div class="repititions" :class="{inactive: !isLoggedIn || isTestMode}">
                                    <input type="checkbox" id="repititions" style="display:none" :disabled="isTestModeOrNotLoginIn" v-model="repititions" />
                                    <label for="repititions" class="toggle" :class="{forbidden: !isLoggedIn || isTestMode}">
                                        <span></span>
                                    </label>
                                    <span>&nbsp;Wiederholungen<i> Falsch gelöste Fragen werden dir zur Beantwortung erneut vorgelegt.</i></span>
                                </div>
                            </div>
                        </div>
                        <div class="themes-info">
                            <p> Du lernst <b>{{selectedQuestionCount}}</b> Fragen aus dem Thema {{categoryName}} ({{allQuestionsCountFromCategory}})</p>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div type="button" class="btn btn-link" data-dismiss="modal">Abbrechen</div>
                        <div type="button" class="btn btn-primary" :class="{ 'disabled' : maxQuestionCountIsZero }" @click="loadCustomSession(false)"><i class="fas fa-play"></i> Anwenden</div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</session-config-component>


