<div class="input-container">
    <div class="overline-s no-line">Antwort</div>

    <form class="form-horizontal">
        <div class="form-group">
            <div class="col-sm-12">
                <template>
                    <textarea-autosize
                        placeholder="Gib deine Antwort ein"
                        ref="textSolutionInput"
                        v-model.trim="text"
                        rows="1"
                        @keydown.enter.native.prevent
                        @keyup.enter.native.prevent
                        :min-height="43"
                        width="100%"

                        :class="{'is-empty':text.length === 0 && highlightEmptyFields}"/>
                </template>

                <div v-if="text.length === 0 && highlightEmptyFields" class="field-error" style="margin-top: -5px;">Bitte gib eine Antwort ein.</div>
            </div>
        </div>
    </form>

</div>