<%@ Import Namespace="System.Web.Optimization" %>

<publish-category-component inline-template>
    <div id="PublishCategoryComponent">
        <div class="btn btn-primary" @click="openPublishModal">
            <i class="fas fa-upload"></i> Thema veröffentlichen
        </div>  
        <div class="modal fade" id="PublishCategoryModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
            <div v-if="publishRequestConfirmation" class="modal-dialog modal-xs">
                <div class="modal-content after-request-modal">
                    <div class="modalBody">
                        <i v-if="publishSuccess" class="fas fa-check-circle"></i>
                        <i v-else class="far fa-times-circle"></i>
                        <div class="message">
                            <template v-if="publishSuccess">
                                Dein Thema wurde erfolgreich veröffentlicht.
                            </template>
                            <template v-else>
                                Veröffentlichung ist nicht möglich.
                                <br/>
                                Das übergeordnete Thema ist privat.
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
                        <h4 class="modal-title">Thema {{categoryName}} veröffentlichen</h4>
                    </div>
                    <div class="modalBody">
                        <div class="subHeader">
                            Öffentliche Inhalte sind für alle auffindbar und können frei weiterverwendet werden. <br/>
                            Du veröffentlichst unter Creative-Commons-Lizenz.
                        </div>
                        <div class="checkbox-container" @click="publishQuestions = !publishQuestions" v-if="questionCount > 0">
                            <div class="checkbox-icon">
                                <i class="fas fa-check-square" v-if="publishQuestions" ></i>
                                <i class="far fa-square" v-else></i>
                            </div>
                            <div class="checkbox-label">
                                Möchtest Du {{questionCount}} private Fragen veröffentlichen?
                            </div>

                        </div>
                        <div class="checkbox-container license-info" @click="confirmLicense = !confirmLicense">
                            <div class="checkbox-icon" :class="{ blink : blink }">
                                <i class="fas fa-check-square" v-if="confirmLicense"></i>
                                <i class="far fa-square" v-else></i>
                            </div>
                            <div class="checkbox-label" :class="{ blink : blink }">
                                Ich stelle diesen Eintrag unter die Lizenz "Creative Commons - Namensnennung 4.0 International" (CC BY 4.0, Lizenztext, deutsche Zusammenfassung). 
                                Der Eintrag kann bei angemessener Namensnennung ohne Einschränkung weiter genutzt werden. Die Texte und ggf. 
                                Bilder sind meine eigene Arbeit und nicht aus urheberrechtlich geschützten Quellen kopiert. 
                            </div>
                        </div>
                    </div>
                    <div class="modalFooter">
                        <div class="btn btn-link" data-dismiss="modal" aria-label="Close">abbrechen</div>
                        <div class="btn btn-primary" id="PublishCategoryBtn" @click="publishCategory" :class="{ 'disabled-btn': !confirmLicense }">veröffentlichen</div>       
                    </div>   
                </div>
            </div>
        </div>
    </div>

</publish-category-component>
