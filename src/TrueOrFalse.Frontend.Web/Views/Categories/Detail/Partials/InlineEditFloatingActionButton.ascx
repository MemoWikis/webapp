    <div class="trigger" @click.prevent="setEditMode()" data-allowed="logged-in" :class="{ open : editMode }">
        <i class="fas fa-pen"></i>  
    </div>
    <div class="toolbar" :class="{ open : editMode }" v-if="editMode">
        <div class="buttons">
            <div @click.prevent="cancelEditMode()" class="closeBtn"><i class="fas fa-times"></i> Änderungen verwerfen</div>
            <div @click.prevent="saveMarkdown('bottom')" class="saveBtn"><i class="fas fa-save"></i> Änderungen speichern</div>
        </div>
        <div class="pseudo-circle" :class="{ open : editMode }">
            <div class="dialog"></div>
        </div>
    </div>
