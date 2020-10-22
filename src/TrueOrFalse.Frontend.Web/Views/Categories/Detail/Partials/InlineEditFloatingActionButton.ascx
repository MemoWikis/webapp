<%--<div id="InlineEditToolbar" :class="{sticky : footerIsVisible}">
    <div class="floatingActionBtn" :class="{sticky : footerIsVisible}">
        <div class="trigger" @click.prevent="setEditMode()" :class="{ open : editMode }">
            <i class="fas fa-pen"></i>  
        </div>
    </div>

    <div class="toolbar" v-cloak :class="{ open : editMode , sticky : footerIsVisible }">
        
        <div class="inner" :class="{sticky : footerIsVisible}">
            <div class="btnLeft">
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

                <div class="button" @click.prevent="saveContent()" :class="{ expanded : editMode }">
                    <div class="icon">
                        <i class="fas fa-save"></i>
                    </div>
                    <div class="btnLabel">
                        Speichern
                    </div>
                </div>

            </div>
        </div>
        
        <div class="pseudo-circle" :class="{ open : editMode }">
            <div class="dialog" v-show="editMode" :class="{ open : editMode }"></div>
        </div>

    </div>

</div>--%>

<div id="FloatingActionButtonApp" class="">
    <floating-action-button inline-template>
        <div id="FABContainer">
            <div id="MainFABContainer">
                <div id="MainFAB" @click="toggleFAB()">
                    <i class="fas fa-pen"></i>
                </div>
                <div id="MiniFABContainer">
                    <div class="mini-fab">1
                        <i class="fas fa-pen"></i>
                    </div>
                    <div class="mini-fab">2
                        <i class="fas fa-pen"></i>
                    </div>
                    <div class="mini-fab">3
                        <i class="fas fa-pen"></i>
                    </div>
                </div>
            </div>

        </div>
    </floating-action-button>
    <div id="EditToolBar"></div>
</div>