
    <div class="modal fade" id="cardsSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
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
                            <div class="radio">
                                <label class="clickable">
                                    <input type="radio" value="Landscape" v-model="selectedCardOrientation">
                                    Querformat
                                </label>
                            </div>
                            <div class="radio">
                                <label class="clickable">
                                    <input type="radio" value="Portrait" v-model="selectedCardOrientation">
                                    Hochformat
                                </label>
                            </div>
                        </div>
                    </form>

                    <div class="cardsSettings" v-sortable="cardOptions">
                        <div class="cardsSettings grid" v-for="(id, index) in sets" :setId="id" :key="index" :class="{ portrait : vertical }">
                            <div class="cardSettings card">
                                <div>
                                    <span>Set: {{id}}</span>
                                </div>
                                <div>
                                    <a class="clickable" @click.prevent="removeSet(index)"><i class="fa fa-trash"></i> Set entfernen</a>
                                </div>
                            </div>
                        </div>
                        <div id="addCardPlaceholder" class="cardsSettings grid placeholder" :class="{ portrait : vertical }">
                            <div class="addCard" v-if="showSetInput" :class="{ portrait : vertical }">
                                <div class="form-group">
                                    <input class="form-control" v-model="newSetId" placeholder="" type="number">
                                    <div class="applyAndCancel" :class="{ portrait : vertical }">
                                        <a class="clickable" @click="hideSetInput">abbrechen</a>
                                        <div class="btn btn-primary" @click="addCard(newSetId)">hinzufügen</div>
                                    </div>
                                </div>
                            </div>
                            <div v-else class="addCard btn btn-primary" @click="showSetInput = true" :class="{ portrait : vertical }">Set hinzufügen</div>
                        </div>
                    </div>
                    <div class="applyAndCancel modalFooter">
                        <a class="CancelEdit clickable" @click="closeModal()">abbrechen</a>
                        <div class="btn btn-primary" @click="applyNewMarkdown()">Konfiguration übernehmen</div>       
                    </div>   
                </div>
    
            </div>
        </div>
    </div>