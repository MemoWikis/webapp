
    <div class="modal fade" id="singlecategoryfullwidthSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">SingleCategoryFullWidth bearbeiten</h4>
                    <form>
                        <div class="form-group">
                            <label for="title" placeholder="">Titel</label>
                            <input class="form-control" v-model="title" placeholder="" />
                            <small class="form-text text-muted">Der Titel ist optional.</small>
                        </div>
                        
                        <div class="form-group">
                            <label for="text">Beschreibung</label>
                            <input class="form-control" v-model="description" placeholder="optional" />
                            <small class="form-text text-muted">Beschreibe dieses Thema</small>
                        </div>  
                        
                        <div class="form-group">
                            <label for="topic">Thema</label>
                            <v-select label="Item.Name" :filterable="false" :options="options" @search="onSearch" v-model="selected">
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
                        <a class="CancelEdit clickable" @click="closeModal()">abbrechen</a>
                        <div class="btn btn-primary" @click="applyNewMarkdown()">Konfiguration übernehmen</div>       
                    </div>   
                </div>
            </div>
        </div>
    </div>