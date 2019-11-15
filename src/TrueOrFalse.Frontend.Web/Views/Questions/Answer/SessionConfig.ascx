<%@ Import Namespace="System.Web.Optimization" %>


<div id="SessionConfigApp">
    
    <div id="sessionSelectionContainer">

        <div class="sessionSelectionButton" @click="loadNewSession('highProbability')">
            <div class="sessionSelectionContent">
                <div class="sessionSelectionButtonIcon highProbability"><i class="far fa-smile"></i></div>
                <div class="sessionSelectionText">Fragen mit hoher Antwortwahrscheinlichkeit</div>
            </div>
            <div class="sessionSelectionBottomBorder highProbability"></div>
        </div>
        
        <div class="sessionSelectionButton" @click="loadNewSession('lowProbability')">
            <div class="sessionSelectionContent">
                <div class="sessionSelectionButtonIcon lowProbability"><i class="fas fa-fire"></i></div>
                <div class="sessionSelectionText">Fragen mit geringer Antwortwahrscheinlichkeit</div>
            </div>
            <div class="sessionSelectionBottomBorder lowProbability"></div>
        </div>
        
        <div class="sessionSelectionButton" @click="loadNewSession('randomQuestions')">
            <div class="sessionSelectionContent">
                <div class="sessionSelectionButtonIcon randomQuestions"><i class="fas fa-dice"></i></div>
                <div class="sessionSelectionText">Zufällige Fragen</div>
            </div>
            <div class="sessionSelectionBottomBorder randomQuestions"></div>
        </div>
        
        <div id="CustomSessionConfigBtnContainer" >
            <div id="CustomSessionConfigBtn" data-toggle="modal" data-target="#SessionConfigModal">
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
    


    <div class="modal fade" id="SessionConfigModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="SessionConfigHeader">
                    <h4 class="modal-title" >{{title}} konfigurieren</h4>
                </div>

                <div class="modal-body">

                    <div v-if="isLoggedIn">
                        <label class="radio">
                            <input type="radio" name="r" value="False" v-model="questionsInWishknowledge" checked>
                            <span class="radioLabel">Alle Fragen</span>
                        </label>
                        <label class="radio">
                            <input type="radio" name="r" value="True" v-model="questionsInWishknowledge">
                            <span class="radioLabel">Fragen im Wunschwissen</span>
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
