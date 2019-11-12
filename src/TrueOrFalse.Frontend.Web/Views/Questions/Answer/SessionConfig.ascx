<%@ Import Namespace="System.Web.Optimization" %>


<div id="SessionConfigApp">
    
    <div id="SessionConfigBtn" type="button" class="btn btn-primary" data-toggle="modal" data-target="#SessionConfigModal">
        create new Session
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
                            <input type="radio" name="r" value="false" v-model="questionInWishknowledge" checked>
                            <span>Alle Fragen</span>
                        </label>
                        <label class="radio">
                            <input type="radio" name="r" value="true" v-model="questionInWishknowledge">
                            <span>Fragen im Wunschwissen</span>
                        </label>
                    </div>
                    <div class="sliders">

                        <label>Antwortwahrscheinlichkeit</label>
                        <div class="sliderContainer">
                            <div class="leftLabel">leicht</div>
                            <div class="vueSlider">                            
                                <vue-slider direction="rtl" :lazy="true" v-model="probabilityRange"></vue-slider>
                            </div>
                            <div class="rightLabel">schwierig</div>
                        </div>
                    
                        <label>Maximale Anzahl an Fragen</label>
                        <div class="sliderContainer">
                            <div class="leftLabel">0</div>
                            <div class="vueSlider">                            
                                <vue-slider :max="maxSelectableQuestionCount" v-model="questionFilter.maxQuestionCount"></vue-slider>
                            </div>
                            <div class="rightLabel">{{maxSelectableQuestionCount}}</div>
                        </div>

                    </div>
                    


                </div>
                <div class="modal-footer">
                    <div type="button" class="btn btn-link" data-dismiss="modal">Abbrechen</div>
                    <div type="button" class="btn btn-primary" @click="loadNewSession()" data-dismiss="modal"><i class="fas fa-play"></i> Starten</div>
                </div>
            </div>
        </div>
    </div>
</div>

<%= Scripts.Render("~/bundles/js/SessionConfig") %>
<%--<%= Styles.Render("~/bundles/SessionConfig") %>--%>

