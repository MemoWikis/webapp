
    <div class="modal fade" id="topicnavigationSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">TopicNavigation bearbeiten</h4>
                    <form>
                        <div class="form-group">
                            <label for="title">Titel</label>
                            <input class="form-control" v-model="title" placeholder="" />
                            <small class="form-text text-muted">Der Titel ist optional.</small>
                        </div>
                        
                        <div class="form-group">
                            <label for="text">Text</label>
                            <input class="form-control" v-model="text" />
                            <small class="form-text text-muted">Beschreibe die Navigation</small>
                        </div>

                        
                        <div class="form-group" style="margin-top: 30px;">
                            <label for="load">Themenauswahl</label>
                            <div class="radio" style="margin-top:0">
                                <label class="clickable">
                                    <input type="radio" value="All" v-model="load">
                                    Alle Unterthemen
                                </label>
                            </div>
                            <div class="radio">
                                <label class="clickable">
                                    <input type="radio" value="Custom" v-model="load">
                                    Benutzerdefinierte Themen
                                </label>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label for="order">Sortierung</label>
                            <div class="radio" style="margin-top:0">
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
                            <div v-if="load == 'All'" class="radio">
                                <label class="clickable">
                                    <input type="radio" value="ManualSort" v-model="order">
                                    Manuelle Sortierung
                                </label>
                            </div>
                        </div>     
                    </form>
                    
                    <div v-if="showTopicList">
                        <div class="topicCards" v-sortable="topicOptions">
                            <div class="topicCards smallCard grid topicNavigationDialogData" v-for="(id, index) in topics" :topicId="id" :key="index">
                                <div class="topicCards card">
                                    <div>Thema: {{id}}</div>
                                     <a class="clickable" @click.prevent="removeTopic(index)"><i class="fa fa-trash"></i> Thema entfernen</a>
                                </div>
                            </div>
                            <div id="addCardPlaceholder" class="topicCards smallCard grid placeholder">
                                <div class="addCard" v-if="showTopicInput">
                                    <div class="form-group">
                                        <v-select label="Item.Name" :filterable="false" :options="options" @search="onSearch" v-model="newTopic">
                                            <template slot="no-options">
                                                Tippen um zu suchen.
                                            </template>
                                            <template slot="option" slot-scope="option">
                                                <div class="d-center">
                                                    {{ option.Item.Name }}
                                                </div>
                                            </template>
                                            <template slot="selected-option" slot-scope="option">
                                                <div id="selectedSetId" class="selected d-center">
                                                    {{ option.Item.Name }}
                                                </div>
                                            </template>
                                        </v-select>
                                        <div class="settingsConfirmation">
                                            <div class="btn btn-link" @click="hideTopicInput">abbrechen</div>
                                            <div class="btn btn-primary" @click="addTopic()">Hinzufügen</div>
                                        </div>
                                    </div>
                                </div>
                                <div v-else class="addCard btn btn-primary" @click="showTopicInput = true">Thema hinzufügen</div>
                            </div>
                        </div>
                    </div>
                    
                    
                    <div class="settingsConfirmation modalFooter">
                        <div class="btn btn-link" @click="closeModal">Abbrechen</div>
                        <div class="btn btn-primary" @click="applyNewMarkdown()">Konfiguration übernehmen</div>       
                    </div>   
                </div>
            </div>
        </div>
    </div>