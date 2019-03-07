
    <div class="modal fade" id="topicnavigationSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">TopicNavigation bearbeiten</h4>
                    <form>
                        <div class="form-group">
                            <label for="title">Titel</label>
                            <input class="form-control" v-model="title" placeholder="">
                            <small class="form-text text-muted">Der Titel ist optional.</small>
                        </div>
                        
                        <div class="form-group">
                            <label for="text">Text</label>
                            <textarea-autosize class="form-control" rows="2" v-model="text" :min-height="40" />
                            <small class="form-text text-muted">Beschreibe die Navigation</small>
                        </div>
                        
                        <div class="form-group">
                            <label for="load">Lade</label>
                            <div class="radio">
                                <label class="clickable">
                                    <input type="radio" value="All" v-model="load">
                                    Alle
                                </label>
                            </div>
                            <small class="form-text">oder</small>
                            <input class="form-control" v-model="load" placeholder="12,58,100">
                            <small class="form-text text-muted">Gebe eine SetId ein und trenne dies mit einem Komma.</small>
                        </div>

                        <div class="form-group">
                            <div class="radio">
                                <label class="clickable">
                                    <input type="radio" value="Name" v-model="order">
                                    Alphabetisch
                                </label>
                            </div>
                            <div class="radio">
                                <label class="clickable">
                                    <input type="radio" value="QuestionAmount" v-model="order">
                                    Anzahl Fragen
                                </label>
                            </div>
                            <div class="radio">
                                <label class="clickable">
                                    <input type="radio" value="Free" v-model="order">
                                    Frei
                                </label>
                            </div>
                        </div>
                    </form>
                    
                    
                    <div class="applyAndCancel modalFooter">
                        <a class="CancelEdit clickable" @click="closeModal()">abbrechen</a>
                        <div class="btn btn-primary" @click="applyNewMarkdown()">Konfiguration übernehmen</div>       
                    </div>   
                </div>
            </div>
        </div>
    </div>