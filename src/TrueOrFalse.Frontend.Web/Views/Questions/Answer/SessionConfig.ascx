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

                    <div ref="radioSection" class="radios" :class="{'disabled-radios' : !isLoggedIn}" @mouseover="isHoveringOptions = true" @mouseleave="isHoveringOptions = false">

                        <transition name="fade">
                            <div v-show="!isLoggedIn && isHoveringOptions" class="blur" :style="{maxWidth: radioWidth + 'px', maxHeight: radioHeight + 'px'}"></div>
                        </transition>

                        <div class="modal-section-label">Modus</div>
                        <label class="radio">
                            <input type="radio" name="r1" value="Test" v-model="questionFilter.mode" :disabled="!isLoggedIn">
                            <span></span>
                            <div class="radioLabelContainer">                            
                                <div class="radioLabel">Testen</div>
                                <div class="labelInfo">- keine Antworthilfe, keine Wiederholungen, zufällige Fragen</div>
                            </div>
                        </label>
                        <label class="radio">
                            <input type="radio" name="r1" value="Learning" v-model="questionFilter.mode" :disabled="!isLoggedIn">
                            <span></span>
                            <div class="radioLabelContainer">                            
                                <div class="radioLabel">Lernen</div>
                                <div class="labelInfo">- Fragen mit geringer Antwortwahrscheinlichkeit werden zuerst geübt, Falsch gelöste Fragen werden wiederholt</div>
                            </div>
                        </label>
                    
                        <div class="modal-divider"></div>
                    
                        <div class="modal-section-label">Fragen</div>
                        <label class="radio">
                            <input type="radio" name="r2" value="False" v-model="questionsInWishknowledge" :disabled="!isLoggedIn">
                            <span></span>
                            <div class="radioLabelContainer">                            
                                <div class="radioLabel fullSize">Alle Fragen</div>
                            </div>
                        </label>
                        <label class="radio">
                            <input type="radio" name="r2" value="True" v-model="questionsInWishknowledge" :disabled="!isLoggedIn">
                            <span></span>
                            <div class="radioLabelContainer">                            
                                <div class="radioLabel fullSize">Fragen im Wunschwissen</div>
                            </div>
                        </label>
                    </div>

                    <div class="sliders">

                        <label class="sliderLabel">Antwortwahrscheinlichkeit</label>
                        <div class="sliderContainer">
                            <div class="leftLabel">gering</div>
                            <div class="vueSlider">                            
                                <vue-slider direction="ltr" :lazy="true" v-model="probabilityRange" :tooltip-formatter="percentages"></vue-slider>
                            </div>
                            <div class="rightLabel">hoch</div>
                        </div>
                    
                        <label class="sliderLabel">Anzahl an Fragen</label>
                        <div v-if="maxSelectableQuestionCount > 0" class="sliderContainer">
                            <div class="leftLabel">0</div>
                            <div class="vueSlider">                            
                                <vue-slider :max="maxSelectableQuestionCount" v-model="selectedQuestionCount"></vue-slider>
                            </div>
                            <div class="rightLabel">{{maxSelectableQuestionCount}}</div>
                        </div>
                        <div v-else class="alert alert-warning" role="alert">Leider sind keine Fragen mit diesen Einstellungen verfügbar. Bitte ändere die Antwortwahrscheinlichkeit oder wähle "Alle Fragen" aus.</div>
                        <div class="alert alert-warning" v-if="(selectedQuestionCount == 0 || maxQuestionCountIsZero ) && maxSelectableQuestionCount > 0">Du musst mindestens 1 Frage auswählen.</div>

                    </div>
                    


                </div>
                <div class="modal-footer">
                    <div type="button" class="btn btn-link" data-dismiss="modal">Abbrechen</div>
                    <div type="button" class="btn btn-primary" :class="{ 'disabled' : maxQuestionCountIsZero }" @click="loadCustomSession()"><i class="fas fa-play"></i> Starten</div>
                </div>
            </div>
        </div>
    </div>
</div>

<%= Scripts.Render("~/bundles/js/SessionConfig") %>
