
    <div class="modal fade" id="ContentModuleSelectionModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div class="contentModuleSettings">
                    <h4 class="modalHeader">Modul auswählen</h4>
                    <form>
                        <div class="form-group">
                            <div class="selectModuleContainer">
                                <div class="moduleSection bottomBorder">
                                    <p>Hauptmodule</p>
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

                                <div class="moduleSection bottomBorder">
                                    <p>Nebenmodule</p>
                                    <div class="subModules" >
                                        <div v-for="module in subModules" :key="module.id">
                                            <div class="sampleList" @click="setActive(module.type)">
                                                <p class="sampleContainerText show-tooltip noImage" :title="module.tooltip" style="display: inline-block">
                                                    {{module.name}}
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="moduleSection">
                                    <p>
                                        Andere
                                        <a href="#miscModules" data-toggle="collapse" class="greyed noTextdecoration" style="font-weight: normal;"><i class="fa fa-caret-down">&nbsp;</i> Module ein-/ausblenden</a>
                                    </p>
                                    <div id="miscModules" class="collapse" >
                                        <div class="miscModules">
                                            <div v-for="module in miscModules" :key="module.id">
                                                <div class="sampleList" @click="setActive(module.type)">
                                                    <p class="sampleContainerText show-tooltip noImage" :title="module.tooltip" style="display: inline-block">
                                                        {{module.name}}
                                                    </p>
                                                </div>
                                            </div>
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