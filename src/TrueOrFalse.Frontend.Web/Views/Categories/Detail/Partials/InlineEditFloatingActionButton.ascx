    <div class="trigger" @click.prevent="setEditMode()" data-allowed="logged-in" :class="{ open : editMode }">
        <i class="fas fa-pen"></i>  
    </div>
    <div class="toolbar" v-cloak>
        <div class="buttons" v-show="editMode">
            <div class="cancelBtn">
                <button @click.prevent="cancelEditMode()"><i class="fas fa-times"></i> Änderungen verwerfen</button>
            </div>
            <div class="saveBtn">
                <button @click.prevent="saveMarkdown('bottom')" style="right: 20px;"><i class="fas fa-save"></i> Änderungen speichern</button>
            </div>
        </div>
        <div class="pseudo-circle" :class="{ open : editMode }">
            <div class="dialog" v-if="editMode"></div>
        </div>
    </div>
