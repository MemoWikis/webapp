
    <div class="modal fade" id="setcardminilistSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">SetCardMiniList bearbeiten</h4>

                    <div class="setCards" v-sortable="cardOptions">
                        <div class="setCards smallCard grid setCardMiniListDialogData" v-for="(id, index) in sets" :setId="id" :key="index">
                            <div class="setCards card">
                                <div>Set: {{id}}</div>
                                <a class="clickable" @click.prevent="removeSet(index)"><i class="fa fa-trash"></i>Set entfernen</a>
                            </div>
                        </div>
                        <div id="addCardPlaceholder" class="setCards smallCard grid placeholder">
                            <div class="addCard" v-if="showSetInput">
                                <div class="form-group">
                                    <v-select label="Item.Name" :filterable="false" :options="options" @search="onSearch" v-model="newSet">
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
                                        <a class="clickable" @click="hideSetInput">abbrechen</a>
                                        <div class="btn btn-primary" @click="addSet()">Hinzufügen</div>
                                    </div>
                                </div>
                            </div>
                            <div v-else class="addCard btn btn-primary" @click="showSetInput = true">Set hinzufügen</div>
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