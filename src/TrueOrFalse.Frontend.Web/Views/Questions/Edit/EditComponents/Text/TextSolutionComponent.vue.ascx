<textsolution-component inline-template :solution="textSolution">

    <div class="input-container">
        <div class="overline-s no-line">Antwort</div>

        <form class="form-horizontal">
            <div class="form-group">
                <div class="col-sm-12">
                    
                    <textarea-autosize
                        placeholder="Gib deine Antwort ein"
                        ref="textSolutionInput"
                        v-model="text"
                        rows="1"
                        @keydown.enter.native.prevent
                        @keyup.enter.native.prevent
                        :min-height="43"
                        width="100%"
                        
                        :class="isEmpty"
                    />
                </div>
            </div>

<%--            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <div class="checkbox">
                        <label>
                            <input type="checkbox" v-model="matchCase" v-on:change="setSolution()"> Groß-/Kleinschreibung beachten
                        </label>
                    </div>
                    <div class="checkbox">
                        <label>
                            <input type="checkbox" v-model="exactSpelling" v-on:change="setSolution()"> Exakte Schreibweise
                        </label>
                    </div>
                </div>
            </div>--%>
        </form>

    </div>

</textsolution-component>
