<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% var admin = SessionUserLegacy.IsInstallationAdmin; %>

<div class="modal fade" id="CategoryToPrivateModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
    <div v-if="setToPrivateConfirmation" class="modal-dialog modal-xs">
        <div class="modal-content after-request-modal">
            <div class="modalBody">
                <i v-if="publishSuccess" class="fas fa-check-circle"></i>
                <i v-else class="far fa-times-circle"></i>
                <div class="message">
                    <template v-if="publishSuccess">
                        Dein Thema wurde erfolgreich auf privat gesetzt.
                    </template>
                    <template v-else>
                        Privatisierung ist nicht möglich.
                        <br/>
                        Du hast untergeordnete öffentliche Themen.
                    </template>

                </div>
            </div>
            <div class="modalFooter">
                <div class="btn btn-primary" :class="{ success : publishSuccess }" onClick="window.location.reload()">OK</div>
            </div>
        </div>
    </div>
    <div v-else class="modal-dialog modal-m" role="document">
        <button type="button" class="close dismissModal dismiss-modal-button" data-dismiss="modal" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>

        <div class="modal-content">
            <div class="modalHeader">
                <h4 class="modal-title">Thema {{categoryName}} auf privat setzen</h4>
            </div>
            <div class="modalBody">
                <div class="subHeader">
                    Der Inhalt kann nur von Dir genutzt werden. Niemand sonst kann ihn sehen oder nutzen.
                </div>
                <div class="checkbox-container" @click="questionsToPrivate = !questionsToPrivate" v-if="personalQuestionCount > 0">
                    <div class="checkbox-icon">
                        <i class="fas fa-check-square" v-if="questionsToPrivate"></i>
                        <i class="far fa-square" v-else></i>
                    </div>
                    <div class="checkbox-label">
                        Möchtest Du {{personalQuestionCount}} von {{allQuestionCount}} öffentliche Frage{{personalQuestionCount > 0 ? 'n' : ''}} ebenfalls auf privat stellen? (Du kannst nur deine eigenen Fragen auf privat stellen.)
                    </div>

                </div>
                
                <%if (admin) {%>
                    <div class="checkbox-container" @click="allQuestionsToPrivate = !allQuestionsToPrivate" v-if="allQuestionCount > 0">
                        <div class="checkbox-icon">
                            <i class="fas fa-check-square" v-if="allQuestionsToPrivate"></i>
                            <i class="far fa-square" v-else></i>
                        </div>
                        <div class="checkbox-label">
                            Möchtest Du {{allQuestionCount}} öffentliche Frage{{allQuestionCount > 0 ? 'n' : ''}} ebenfalls auf privat stellen? (Admin)
                        </div>

                    </div>
                <%} %>
            </div>
            <div class="modalFooter">
                <div class="btn btn-link" data-dismiss="modal" aria-label="Close">abbrechen</div>
                <div class="btn btn-primary" id="SetCategoryToPrivateBtn" @click="setCategoryToPrivate">Thema auf Privat setzen</div>
            </div>
        </div>
    </div>
</div>