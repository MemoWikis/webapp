<script lang="ts" setup>


</script>

<template>
    <div class="cardModal">
        <div class="modal fade" id="modalDeleteQuestion" tabindex="-1" role="dialog"
            aria-labelledby="modal-content-module-settings" aria-hidden="true">
            <div class="modal-dialog modal-m" role="document">
                <button type="button" class="close dismissModal" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>

                <div class="modal-content">
                    <div class="cardModalContent">
                        <div class="modalHeader">
                            <h4 class="modal-title">Frage löschen</h4>
                        </div>
                        <div class="modalBody">
                            <div class="body-m" v-if="showDeleteInfo">Möchtest Du "{{name}}" unwiederbringlich löschen?
                                Alle damit verknüpften Daten werden entfernt!</div>
                            <div class="alert alert-danger" v-if="showErrorMsg">{{errorMsg}}</div>
                            <div class="alert alert-info" v-if="deletionInProgress">Die Frage wird gelöscht... Bitte
                                habe einen Moment Geduld.</div>
                        </div>
                        <div class="modalFooter" v-if="!deletionInProgress">
                            <div @click="deleteQuestion()" class="btn btn-danger memo-button" v-if="showDeleteBtn">Frage
                                Löschen</div>
                            <div class="btn btn-link memo-button" data-dismiss="modal">Abbrechen</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>