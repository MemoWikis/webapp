<div id="defaultModal">
    <div class="modal-default-mask" @click="closeModal()">
        <div class="modal-default-wrapper">
            <div class="modal-default-container" v-on:click.stop>
                <i v-if="showCloseButton" class="fa fa-times pull-right pointer modal-close-button" @click="closeModal()"></i>
                <div class="modal-default-header">
                    <slot name="header">
                    </slot>
                </div>
                <div class="modal-default-body">
                    <slot name="body">
                    </slot>
                </div>

                <div class="modal-default-footer">
                    <slot name="footer">
                    </slot>
                </div>
            </div>
        </div>
    </div>
</div>