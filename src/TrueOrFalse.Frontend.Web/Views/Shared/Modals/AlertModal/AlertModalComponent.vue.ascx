
<alert-modal-component inline-template>
    
    <div>
        <div id="ErrorModal" class="modal fade">
            <div v-if="error" class="modal-dialog" role="document">
                <div class="modal-content">

                    <div class="modal-body">
                        <h3><i class="far fa-times-circle"></i></h3>

                        <div class="">{{message}}</div>
                    </div>

                    <div class="modal-footer">
                        <div class="btn memo-button col-xs-4 btn-error" data-dismiss="modal">Ok</div>       
                    </div>

                </div>
            </div>
        </div>
    
        <div id="SuccessModal" class="modal fade">
            <div v-if="!error" class="modal-dialog" role="document">
                <div class="modal-content">

                    <div class="modal-body">
                        <h3><i class="fas fa-check-circle"></i></h3>

                        <div class="">{{message}}</div>
                    </div>

                    <div class="modal-footer">
                        <div class="btn memo-button col-xs-4 btn-success" data-dismiss="modal">Ok</div>       
                    </div>

                </div>
            </div>
        </div>
    </div>


</alert-modal-component>
