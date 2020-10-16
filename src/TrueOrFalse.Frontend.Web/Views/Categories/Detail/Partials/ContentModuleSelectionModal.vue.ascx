
    <div class="modal fade" id="ContentModuleSelectionModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">Modul auswählen</h4>
                    <form>
                        <div class="form-group">
                            <div class="selectModuleContainer">
                                <div class="moduleSection" v-if="showMainModules">
                                    <div class="mainModules" >
                                        <div v-for="module in mainModules" :key="module.id" style="width: 160px;">
                                            <div class="sampleContainer" @click="setActive(module.type)">
                                                <img class="show-tooltip" :title="module.tooltip" :src="'/Images/ContentModuleSamples/' + module.type + '.png'">
                                                <div class="sampleContainerText">
                                                    {{module.name}}
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </form>

                    <div class="settingsConfirmation modalFooter">
                        <div class="btn btn-link" @click="closeModal">Abbrechen</div>
                    </div>   
                </div>
    
            </div>
        </div>
    </div>