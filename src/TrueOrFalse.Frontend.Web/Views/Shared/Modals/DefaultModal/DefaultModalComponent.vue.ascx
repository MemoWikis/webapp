<div id="defaultModal" class="modal-default">
    <div class="modal-default-mask" @click="closeModal()">
        <div class="modal-default-wrapper">
            <div class="modal-default-container" :style="{width: modalWidthData}" v-on:click.stop>
                <div><img v-if="showCloseButton" src="/img/close_black.svg" class="pull-right pointer modal-close-button" @click="closeModal()"/>
                <div class="header-default-modal" v-bind:class="{ errorHeaderModal: isError, successHeaderModal: isSuccess }">
                    <i v-if="isError" class="fas fa-times-circle iconHeaderModal"></i>
                    <i v-if="isSuccess" class="fas fa-check-circle iconHeaderModal"></i>
                    <i v-if="iconClasses != null && iconClasses.length > 0" v-bind:class="iconClasses" class="iconHeaderModal"></i>

                    <slot name="header">
                    </slot>
                    
                    <i v-if="isAdminContent" class="fas fa-users-cog adminIconHeaderModal"></i>
                </div>
                <div class="modal-default-body">
                    <slot name="body">
                    </slot>
                </div>

                <div class="modal-default-footer">
                    <slot name="footer"></slot>
                    <div class="row">
                        <div class="col-xs-12">
                            <a v-if="button1Text != null" class="btn btn-primary memo-button pull-right modal-button" v-bind:class="{ errorButton1Modal: isError, successButton1Modal: isSuccess, fullSizeButtons: isFullSizeButtons }" @click="action1()">{{button1Text}}</a>
                            <a v-if="button2Text != null" class="btn btn-lg btn-link memo-button pull-right modalSecondActionButton modal-button" v-bind:class="{ errorButton2Modal: isError, successButton2Modal: isSuccess, fullSizeButtons: isFullSizeButtons }" @click="action2()">{{button2Text}}</a>
                        </div>
                    </div>
                    <div class="modal-default-footer-text">
                        <slot name="footer-text"></slot>
                    </div>
                </div>

                </div>
            </div>
        </div>
    </div>
</div>