
    <div class="modal fade" id="videowidgetSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">TopicNavigation bearbeiten</h4>
                    <form>
                        <div class="form-group">
                            <label for="title">Video zum Lernset</label>
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
                        </div>
                    </form>   
                    
                    <div class="settingsConfirmation modalFooter">
                        <div class="btn btn-link" @click="closeModal">Abbrechen</div>
                        <div class="btn btn-primary" @click="applyNewMarkdown()">Konfiguration übernehmen</div>       
                    </div>   
                </div>
            </div>
        </div>
    </div>