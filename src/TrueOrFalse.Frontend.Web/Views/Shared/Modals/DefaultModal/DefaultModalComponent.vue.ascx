<%@ Import Namespace="System.Web.Optimization" %>
<default-modal-component inline-template @close="showModal = false" show-Close-Button="true" >
    <div id="defaultModal" v-if="showModal">
    <transition name="modal">
        <div class="modal-default-mask">
            <div class="modal-default-wrapper">
                <div class="modal-default-container">
                    <div class="modal-default-header">
                        <a v-if="showCloseButton" @click="$emit('close')"></a>
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
    </transition>
    </div>
</default-modal-component>

<%= Scripts.Render("~/bundles/js/defaultModal") %>