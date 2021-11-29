<%@ Import Namespace="System.Web.Optimization" %>

<set-category-to-private-component inline-template>
    <div class="modal fade" id="SetToPrivateModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
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
                <button type="button" class="close dismissModal dismiss-modal-button" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>

                <div class="modal-content">
                    <div class="modalHeader">
                        <h4 class="modal-title">Thema {{categoryName}} Thema auf privat setzen</h4>
                    </div>
                    <div class="modalBody">
                        <div class="subHeader">
                            Der Inhalt kann nur von Dir genutzt werden. Niemand sonst kann ihn sehen oder nutzen.
                        </div>
                        <div class="checkbox-container" @click="publishQuestions = !publishQuestions" v-if="questionCount > 0">
                            <div class="checkbox-icon">
                                <i class="fas fa-check-square" v-if="publishQuestions" ></i>
                                <i class="far fa-square" v-else></i>
                            </div>
                            <div class="checkbox-label">
                                Möchtest Du {{questionCount}} öffentliche Frage ebenfalls auf privat stellen?
                            </div>

                        </div>
                    </div>
                    <div class="modalFooter">
                        <div class="btn btn-link" data-dismiss="modal" aria-label="Close">abbrechen</div>
                        <div class="btn btn-primary" id="SetCategoryToPrivateBtn" @click="setCategoryToPrivate" :class="{ 'disabled-btn': !confirmLicense }">Thema auf Privat setzen</div>       
                    </div>   
                </div>
            </div>
        </div>

</set-category-to-private-component>
