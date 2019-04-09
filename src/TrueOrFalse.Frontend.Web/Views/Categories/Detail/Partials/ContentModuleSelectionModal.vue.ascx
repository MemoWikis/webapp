
    <div class="modal fade" id="ContentModuleSelectionModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">Modul auswählen</h4>
                    <form>
                        <div class="form-group">
                            <div class="selectModuleContainer" style="display: flex; flex-direction: row; justify-content: space-between; align-content: flex-start; flex-wrap: wrap;">
                                <div class="radio" v-for="name in contentModules" :key="name.id" style="width: 160px;">
                                    <div class="sampleModuleContainer" style="display: flex; flex-direction: column; align-content: center; width: 100%;">
                                        <img :src="'/Images/ContentModuleSamples/' + name + '.png'" style="max-width: 120px; max-height: 80px;">
                                        <label class="clickable">
                                            <input type="radio" :value="name" v-model="selectedModule">
                                            {{name}}
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>

                    <div class="settingsConfirmation modalFooter">
                        <a class="CancelEdit clickable" @click="closeModal()">abbrechen</a>
                        <div class="btn btn-primary" @click="selectModule()">Content Module auswählen</div>       
                    </div>   
                </div>
    
            </div>
        </div>
    </div>