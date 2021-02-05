<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<add-category-component inline-template>
        <div class="categoryCardModal">
            <div class="modal fade" id="AddCategoryModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
                <div class="modal-dialog modal-s" role="document">
                    <button type="button" class="close dismissModal" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>

                    <div class="modal-content">
                        <div class="addCategoryCardModal">
                            <div class="modalHeader">
                                <h4 class="modal-title">Neues Thema erstellen</h4>
                            </div>
                            <div class="modalBody">
                                <form>
                                    <div class="form-group">
                                        <input class="form-control" v-model="name" placeholder="Bitte gib den Namen des Themas ein" />
                                        <small class="form-text text-muted"></small>
                                    </div>
                                </form>
                                <div class="alert alert-warning" role="alert" v-if="showErrorMsg">
                                    <a :href="existingCategoryUrl" target="_blank" class="alert-link">{{existingCategoryName}}</a>
                                    {{errorMsg}}
                                </div>
                                <div class="checkBox" @click="private = !private">
                                    <i class="fas fa-check-square" v-if="private" ></i>
                                    <i class="far fa-square" v-else></i>
                                    Privates Thema
                                </div>
                            </div>
                            <div class="modalFooter">
                                <div class="btn btn-primary" @click="addCategory">Thema Erstellen</div>       
                            </div>   
                        </div>
                    </div>
                </div>
            </div>
        </div>
</add-category-component>
