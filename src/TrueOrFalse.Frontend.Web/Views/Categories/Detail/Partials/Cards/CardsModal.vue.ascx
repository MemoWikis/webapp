
    <div class="modal fade" id="cardsSettingsDialog" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
    
                <div style="margin:20px">
                    <span>"Cards" bearbeiten</span>
                    <br/>
                    <br/>
                    <input v-model="title" placeholder="optionaler Titel"></input>
                    <br/>
                    Format: 
                    <select v-model="selectedCardOrientation">
                        <option>Landscape</option>
                        <option>Portrait</option>
                    </select>
                    <br/>
                    <br/>
                    <div class="cardsSettings" v-sortable>
                        <div class="cardsSettings grid" v-sortable v-for="(id, index) in sets" :setId="id" :key="index" :class="{ portrait : vertical }">
                            <div class="cardSettings card">
                                Set: {{id}}
                                <br/>
                                <a @click.prevent="removeSet(index)" style="align-self:flex-end"><i class="fa fa-trash"></i> Set entfernen</a>
                            </div>
                        </div>
                    </div>
                    <input type="number" v-model="newSetId"/>
                    <button @click="addSet(newSetId)">Set hinzufügen</button>
                    <br/>
                    <div style="display: flex; justify-content: flex-end; margin-top:0.5rem">
                        <a class="CancelEdit" @click="closeModal()" style="margin-right:0.5rem">abbrechen</a>
                        <div class="btn btn-primary" @click="applyNewMarkdown()">Konfiguration übernehmen</div>       
                    </div>   
                </div>
    
            </div>
        </div>
    </div>