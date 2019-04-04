    <div class="trigger" @click.prevent="setEditMode()" data-allowed="logged-in" :class="{ open : editMode }">
        <i class="fas fa-pen"></i>  
    </div>
    <div class="toolbar" v-cloak :class="{ open : editMode }">
        <div class="pseudo-circle" :class="{ open : editMode }">
            <div class="dialog" v-show="editMode"></div>
        </div>
    </div>

    <div class="toolbar inner" v-cloak>

        <div class="btnLeft" >
            <div class="button" :class="{ expanded : editMode }">
                <div class="icon">
                    <i class="fas fa-question"></i>
                </div>
                <div class="btnLabel">
                    Hilfe
                </div>
            </div>
        </div>

        <div class="btnRight">
            <div class="button" @click.prevent="cancelEditMode()" :class="{ expanded : editMode }">
                <div class="icon">
                    <i class="fas fa-times"></i>
                </div>
                <div class="btnLabel">
                    Verwerfen
                </div>
            </div>

            <div class="button" @click.prevent="saveMarkdown('bottom')" :class="{ expanded : editMode }">
                <div class="icon">
                    <i class="fas fa-save"></i>
                </div>
                <div class="btnLabel">
                    Speichern
                </div>
            </div>

        </div>
    </div>
