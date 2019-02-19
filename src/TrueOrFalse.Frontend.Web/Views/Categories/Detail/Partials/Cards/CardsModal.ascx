    <modal-cards-settings inline-template>
        <div class="modal fade" id="modalContentModuleSettings" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div style="margin:20px">
                        <span>{{_cardSettings.Title}} bearbeiten</span>
                        <br/>
                        Format: 
                        <select v-model="selectedCardOrientation">
                            <option>Landscape</option>
                            <option>Portrait</option>
                        </select>
                        <br/>
                        <br/>
                        <ul class="cardSettings" v-sortable>
                            <li class="cardSettings" v-sortable v-for="(id, index) in sets" :setId="id" :key="index">
                                {{id}} 
                                <a @click.prevent="removeSet(index)"><i class="fa fa-trash"></i> Set entfernen</a>
                            </li>
                        </ul>
                        <input type="number" v-model="newSetId"/>
                        <button @click="addSet(newSetId)">Set hinzufügen</button>
                        <br/>
                        <button @click="applyNewMarkdown()">Konfiguration übernehmen</button>
                    </div>
                </div>
            </div>
        </div>            
    </modal-cards-settings>