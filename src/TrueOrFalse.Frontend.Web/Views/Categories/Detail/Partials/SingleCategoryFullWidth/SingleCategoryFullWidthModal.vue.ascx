
    <div class="modal fade" id="singlecategoryfullwidthSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">SingleCategoryFullWidth bearbeiten</h4>
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
                            <label for="topicId">Thema</label>
                            <input class="form-control" type="number" v-model="topicId" placeholder="123" />
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