
<div id="SessionConfigApp">
    
    <div id="SessionConfigBtn" type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal">
        create new Session
    </div>
    
    <div class="modal fade" id="SessionConfigModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header" id="SessionConfigHeader">
                    <h4 class="modal-title" >{{Title}} konfigurieren</h4>
                </div>

                <div class="modal-body">

                    <div v-if="isLoggedIn">
                        <div class="form-check">
                            <input class="form-check-input" type="radio" name="exampleRadios" id="exampleRadios1" value="0" checked>
                            <label class="form-check-label" for="exampleRadios1">
                                Alle Fragen
                            </label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="radio" name="exampleRadios" id="exampleRadios2" value="1">
                            <label class="form-check-label" for="exampleRadios2">
                                Fragen im Wunschwissen
                            </label>
                        </div>
                    </div>

                    
                    <label>Antwortwahrscheinlichkeit</label>
                    <input id="minProbability" type="number" min="1" max="100"/>
                    <input id="maxProbability" type="number" min="1" max="100"/>
    
                    <div class="sliderContainer">
                        <div class="sliderLabel">
                            <label for="sessionConfigQuestionCount">Max. Knotenpunkte</label>
                            <input id="maxQuestionCount" class="col-sm-4" type="number" min="1" max="50" value="50">
                        </div>
                        <div class="col-sm-12">
                            <input id="maxQuestionCountSlider" type="range" min="1" max="50" class="slider sessionConfigQuestionCount">
                        </div>
                    </div>
    
                </div>
                <div class="modal-footer">
                    <div type="button" class="btn btn-link" data-dismiss="modal">Abbrechen</div>
                    <div type="button" class="btn btn-primary" @click="loadNewSession()"><i class="fas fa-play"></i> Starten</div>
                </div>
            </div>
        </div>
    </div>
</div>