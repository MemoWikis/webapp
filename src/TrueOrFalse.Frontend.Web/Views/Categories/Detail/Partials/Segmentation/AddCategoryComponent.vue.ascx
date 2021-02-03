<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<add-category-component inline-template>
        <div class="categoryCardModal">
            <div class="modal fade" id="AddCategoryModal" tabindex="-1" role="dialog" aria-labelledby="modal-content-module-settings" aria-hidden="true">
                <div class="modal-dialog" role="document">
                    <div class="modal-content">
            
                        <div class="addCategoryCardModal">
                            <h4 class="modalHeader">Neues Thema erstellen</h4>
                            <form>
                                <div class="form-group">
                                    <input class="form-control" v-model="name" placeholder="Bitte gib den Namen des Themas ein" />
                                    <small class="form-text text-muted"></small>
                                </div>
                                <div class="checkbox">
                                    <label>
                                        <input type="checkbox" v-model="private" value="true"> Privates Thema
                                    </label>
                                </div>
                            </form>

                            <div class="modalFooter">
                                <div class="btn btn-primary" @click="addCategory">Thema Erstellen</div>       
                            </div>   
                        </div>
                    </div>
                </div>
            </div>
        </div>
</add-category-component>
