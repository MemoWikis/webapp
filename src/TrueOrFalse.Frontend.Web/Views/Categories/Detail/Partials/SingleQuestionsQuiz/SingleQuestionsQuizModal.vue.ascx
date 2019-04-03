
    <div class="modal fade" id="singlequestionsquizSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">SinglQuestionQuiz bearbeiten</h4>
                    <form>
                        <div class="form-group">
                            <label for="title">Titel</label>
                            <input class="form-control" v-model="title" placeholder="" />
                            <small class="form-text text-muted">Der Titel ist optional.</small>
                        </div>
                        
                        <div class="form-group">
                            <label for="text">Beschreibung</label>
                            <input class="form-control" v-model="description" placeholder="optional" />
                            <small class="form-text text-muted">Beschreibe dieses Thema</small>
                        </div>
                        
                        <div class="form-group">
                            <label for="order">Sortierung</label>
                            <div class="radio" style="margin-top:0">
                                <label class="clickable">
                                    <input type="radio" value="Random" v-model="order">
                                    Zufall
                                </label>
                            </div>
                            <div class="radio">
                                <label class="clickable">
                                    <input type="radio" value="ViewsDescending" v-model="order">
                                    Anzahl Views
                                </label>
                            </div>
                            <div class="radio">
                                <label class="clickable">
                                    <input type="radio" value="CorrectnessProbabilityDescending" v-model="order">
                                    Leichste Zuerst
                                </label>
                            </div>
                            <div class="radio">
                                <label class="clickable">
                                    <input type="radio" value="CorrectnessProbabilityAscending" v-model="order">
                                    Schwierigste Zuerst
                                </label>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label for="title">Maximaler Limit an Fragen</label>
                            <input class="form-control" type="number" v-model="maxQuestions" placeholder="5" />
                        </div>
                    </form>
                    
                    <label for="questionCards">Benutzerdefinierte Fragen:</label>
                    <small class="form-text text-muted">Dies ist optional.</small>
                    <div class="questionCards" v-sortable="cardOptions">
                        <div class="questionCards fullwidth grid singleQuestionsQuizDialogData" v-for="(id, index) in questions" :questionId="id" :key="index">
                            <div class="questionCards card">
                                <div>Frage: {{id}}</div>
                                <a class="clickable" @click.prevent="removeQuestion(index)"><i class="fa fa-trash"></i>Frage entfernen</a>
                            </div>
                        </div>
                        <div id="addCardPlaceholder" class="questionCards fullwidth grid placeholder">
                            <div class="addCard" v-if="showQuestionInput">
                                <div class="form-group">
                                    <v-select label="Item.Name" :filterable="false" :options="options" @search="onSearch" v-model="newQuestion">
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
                                        <a class="clickable" @click="hideQuestionInput">abbrechen</a>
                                        <div class="btn btn-primary" @click="addQuestion()">Hinzufügen</div>
                                    </div>
                                </div>
                            </div>
                            <div v-else class="addCard btn btn-primary" @click="showQuestionInput = true">Frage hinzufügen</div>
                        </div>
                    </div>
                    
                    <div class="settingsConfirmation modalFooter">
                        <a class="CancelEdit clickable" @click="closeModal()">abbrechen</a>
                        <div class="btn btn-primary" @click="applyNewMarkdown()">Konfiguration übernehmen</div>       
                    </div>   
                </div>
            </div>
        </div>
    </div>