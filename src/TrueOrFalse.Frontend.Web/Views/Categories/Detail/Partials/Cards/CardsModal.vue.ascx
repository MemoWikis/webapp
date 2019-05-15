
    <div class="modal fade" id="cardsSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">Cards bearbeiten</h4>
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

                    <div class="setCards" v-sortable="cardOptions">
                        <div class="setCards grid cardsDialogData" v-for="(id, index) in sets" :setId="id" :key="index" :class="{ portrait : vertical }">
                            <div class="setCards card">
                                <div>
                                    <div>Set: {{id}}</div>
                                </div>
                                <div>
                                    <a class="clickable" @click.prevent="removeSet(index)"><i class="fa fa-trash">&nbsp;</i> Set entfernen</a>
                                </div>
                            </div>
                        </div>
                        <div id="addCardPlaceholder" class="setCards grid placeholder" :class="{ portrait : vertical }">
                            <div class="addCard" v-if="showSetInput" :class="{ portrait : vertical }">
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

                                    <div class="settingsConfirmation" :class="{ portrait : vertical }">
                                        <div class="btn btn-link" @click="hideSetInput">Abbrechen</div>
                                        <div class="btn btn-primary" @click="addSet()">Hinzufügen</div>
                                    </div>
                                </div>
                            </div>
                            <div v-else class="addCard btn btn-primary" @click="showSetInput = true" :class="{ portrait : vertical }">Set hinzufügen</div>
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