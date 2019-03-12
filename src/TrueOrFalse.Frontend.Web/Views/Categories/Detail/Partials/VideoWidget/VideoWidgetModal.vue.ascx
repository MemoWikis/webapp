
    <div class="modal fade" id="videowidgetSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">TopicNavigation bearbeiten</h4>
                    <form>
                        <div class="form-group">
                            <label for="title">Video zum Lernset</label>
                            <input class="form-control" type="number" v-model="setId" placeholder="123" />
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