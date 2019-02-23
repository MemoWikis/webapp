
    <div class="modal fade" id="cardsSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div style="margin:20px">
                    <span>"Cards" bearbeiten</span>
                    <br/>
                    Format: 
                    <select v-model="selectedCardOrientation">
                        <option>Landscape</option>
                        <option>Portrait</option>
                    </select>
                    <br/>
                    <br/>
                    <ul class="cardsSettings" v-sortable>
                        <li class="cardsSettings" v-sortable v-for="(id, index) in sets" :setId="id" :key="index">
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