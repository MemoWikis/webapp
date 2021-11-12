<div id="defaultModal" class="modal-default">
    <div class="modal-default-mask" @click="closeModal()">
        <div class="modal-default-wrapper">
            <div class="modal-default-container" v-on:click.stop>
                <div><img v-if="showCloseButton" src="/img/close_black.svg" class="pull-right pointer modal-close-button" @click="closeModal()"/>
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
</div>