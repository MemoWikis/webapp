<div id="defaultModal">
    <div class="modal-default-mask" @click="closeModal()">
        <div class="modal-default-wrapper">
            <div class="modal-default-container" v-on:click.stop>
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