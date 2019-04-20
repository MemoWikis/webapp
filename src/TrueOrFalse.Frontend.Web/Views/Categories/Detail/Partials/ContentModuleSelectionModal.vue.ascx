
    <div class="modal fade" id="ContentModuleSelectionModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">Modul auswählen</h4>
                    <form>
                        <div class="form-group">
                            <div class="selectModuleContainer">
                                <div v-for="module in contentModules" :key="module.id" style="width: 160px; height: 160px;">
                                    <div class="sampleContainer"  :class="{active : selectedModule === module.type }" @click="setActive(module.type)">
                                            <img class="show-tooltip" :title="module.tooltip" :src="'/Images/ContentModuleSamples/' + module.type + '.png'" style="max-width: 120px; max-height: 80px; margin:0 auto">
                                        <div style="text-align: center; width: 100%; line-height: 1.3;margin-top: 5px;">
                                            {{module.name}}
                                        </div>
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