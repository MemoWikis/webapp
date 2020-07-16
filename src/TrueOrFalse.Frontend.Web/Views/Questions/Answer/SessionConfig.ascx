<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div id="SessionConfigApp">
    <div id="sessionSelectionContainer">
        <div class="row">
            <div class="col-md-3 col-xs-6">
                <div class="sessionSelectionButton" @click="loadNewSession('highProbability')">
                    <div class="sessionSelectionContent">
                        <div class="sessionSelectionButtonIcon highProbability"><i class="far fa-smile"></i></div>
                        <div class="sessionSelectionText">Fragen mit hoher Antwortwahrscheinlichkeit</div>
                    </div>
                <div class="sessionSelectionBottomBorder highProbability"></div>
            </div></div>
            <div class="col-md-3 col-xs-6">
                <div class="sessionSelectionButton" @click="loadNewSession('lowProbability')">
                    <div class="sessionSelectionContent">
                        <div class="sessionSelectionButtonIcon lowProbability"><i class="fas fa-fire"></i></div>
                        <div class="sessionSelectionText">Fragen mit geringer Antwortwahrscheinlichkeit </div>
                    </div>
                    <div class="sessionSelectionBottomBorder lowProbability"></div>
                </div>
            </div>
            <div class="col-md-3 col-xs-6">
                <div class="sessionSelectionButton" @click="loadNewSession('randomQuestions')">
                    <div class="sessionSelectionContent">
                        <div class="sessionSelectionButtonIcon randomQuestions"><i class="fas fa-dice"></i></div>
                        <div class="sessionSelectionText">Zufällige Fragen</div>
                    </div>
                    <div class="sessionSelectionBottomBorder randomQuestions"></div>
                </div>
            </div>
            <div class="col-md-3 col-xs-6">
                <div id="CustomSessionConfigBtnContainer">
                    <div id="CustomSessionConfigBtn" @click="openModal()">
                        <div class="sessionSelectionContent">
                            <div class="sessionSelectionButtonIcon customQuestions"><i class="fas fa-tools"></i></div>
                            <div class="sessionSelectionText">Benutzerdefiniertes Lernen</div>
                        </div>
                        <div class="sessionSelectionBottomBorder customQuestions"></div>
                    </div>
                    <div class="sessionConfigReminder">
                        <img src="/Images/Various/SessionConfigReminder.svg">
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="SessionConfigModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="SessionConfigHeader">
                    <h5>LERNOPTIONEN</h5>
                    <h4 class="modal-title" >{{title}} personalisieren</h4>
                </div>
                <div class="modal-body">
                    <transition name="fade">
                        <div class="restricted-options" v-show="!isLoggedIn && isHoveringOptions" @mouseover="isHoveringOptions = true" transition="fade">
                            <div class="info-content" style="">Diese Optionen sind nur für eingeloggte Nutzer verfügbar.</div>
                            <div class="restricted-options-buttons">                            
                                <div type="button" class="btn btn-link" @click="goToLogin()">Ich bin schon Nutzer!</div>
                                <a type="button" class="btn btn-primary" href="<%= Url.Action(Links.RegisterAction, Links.RegisterController) %>">Jetzt Registrieren!</a>
                            </div>
                        </div>
                    </transition>
                    <div ref="radioSection" class="must-logged-in" :class="{'disabled-radios' : !isLoggedIn}" @mouseover="isHoveringOptions = true" @mouseleave="isHoveringOptions = false">
                            <transition name="fade">
                                <div v-show="!isLoggedIn && isHoveringOptions" class="blur" :style="{maxWidth: radioWidth + 'px', maxHeight: radioHeight + 'px'}"></div>
                            </transition>
                            <div class="modal-section-label">Prüfungsmodus</div>
                            <div class="test-mode">
                            <%= Html.Partial("~/Views/Shared/Switch/Switch.ascx") %>
                            </div>
                            <div class="test-mode-info">
                                Du willst es Wissen? Im Prüfungsmodus kannst Du Dein Wissen realistisch testen: zufällige Fragen ohne Antworthilfe und Wiederholungen. Viel Erfolg!
                            </div>
                            <div class="modal-divider"></div>
                        <div id="CheckboxesLearnOptions" class="row">
                            <div class="col-sm-6">
                                <label class="checkbox-label">
                                    <input id="AllQuestions" type="checkbox" v-model="allQuestions" :disabled="!isLoggedIn" value="False"/>
                                    Alle Fragen
                                </label> <br />
                                <label class="checkbox-label">
                                    <input id="QuestionInWishknowledge" type="checkbox" v-model="questionsInWishknowledge" :disabled="!isLoggedIn" value="False"/>
                                    In meinem Wunschwissen
                                </label>
                            </div>
                            <div class="col-sm-6">
                                <label class="checkbox-label">
                                    <input id="UserIsAuthor" type="checkbox" v-model="userIsAuthor" :disabled="!isLoggedIn" value="False"/>
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
                                    <vue-slider :max="maxSelectableQuestionCount" v-model="selectedQuestionCount"></vue-slider>
                                </div>
                                <div class="rightLabel">{{maxSelectableQuestionCount}}</div>
                            </div>
                            <div v-else class="alert alert-warning" role="alert">Leider sind keine Fragen mit diesen Einstellungen verfügbar. Bitte ändere die Antwortwahrscheinlichkeit oder wähle "Alle Fragen" aus.</div>
                            <div class="alert alert-warning" v-if="(selectedQuestionCount == 0) && maxSelectableQuestionCount > 0">Du musst mindestens 1 Frage auswählen.</div>
                        </div>
                    </div>
                    <div class="row modal-more-options" @click="displayNone = !displayNone">
                        <div class="more-options class= col-sm-12">
                            <span>Mehr Optionen</span>
                            <span class="angle">
                                <i class="fas fa-angle-down"></i>
                            </span>
                        </div>
                    </div>
                    <div class="themes-info" v-bind:class="{displayNone: displayNone}">
                        <p> Du lernst <b>113 Fragen</b> aus dem Thema Allgmeinwissen(4.112)</p>
                    </div>
                    <div class="row" v-bind:class="{displayNone: displayNone}">
                        <div id="SafeLearnOptions">
                            <div class="col-sm-12 safe-settings">
                                <label>
                                    <input type="checkbox" id="safeOptions" v-model="safeLearningSessionOptions" :disabled="!isLoggedIn"/>
                                    Diese Einstellungen für zukünftiges Lernen speichern.
                                </label>
                            </div>
                            <div class="info-options col-sm-12">
                                Ein Neustart deiner Lernsitzung setzt deinen Lernfortschritt zurück. Die Antwortwahrscheinlichkeit der bisher beantworteten Fragen bleibt erhalten.
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div type="button" class="btn btn-link" data-dismiss="modal">Abbrechen</div>
                    <div type="button" class="btn btn-primary" :class="{ 'disabled' : maxQuestionCountIsZero }" @click="loadCustomSession()"><i class="fas fa-play"></i> Anwenden</div>
                </div>
            </div>
        </div>
    </div>
</div>

<%= Scripts.Render("~/bundles/js/SessionConfig") %>
