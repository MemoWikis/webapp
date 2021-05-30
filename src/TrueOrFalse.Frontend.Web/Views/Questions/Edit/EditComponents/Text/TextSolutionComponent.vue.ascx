<textsolution-component inline-template :solution="textSolution">
    <div>
        <form class="form-horizontal">
            <div class="form-group">
                <label for="TextAnswer" class="col-sm-2 control-label">Antwort</label>
                <div class="col-sm-10">
                    <input type="text" class="form-control" id="TextSolutution" placeholder="" v-model="text" v-on:change="setSolution()">
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <div class="checkbox">
                        <label>
                            <input type="checkbox" v-model="matchCase" v-on:change="setSolution()"> Groß-/Kleinschreibung beachten
                        </label>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <div class="checkbox">
                        <label>
                            <input type="checkbox" v-model="exactSpelling" v-on:change="setSolution()"> Exakte Schreibweise
                        </label>
                    </div>
                </div>
            </div>
        </form>

    </div>

</textsolution-component>
