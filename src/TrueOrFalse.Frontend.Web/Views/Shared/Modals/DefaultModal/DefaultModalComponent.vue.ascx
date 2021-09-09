<div id="defaultModal">
    <div class="modal-default-mask">
        <div class="modal-default-wrapper">
            <div class="modal-default-container">
                <div class="modal-default-header">
                    <a v-if="showCloseButton" @click="$emit('closeModal')">
                        x
                    </a>
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