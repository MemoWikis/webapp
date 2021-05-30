<date-component inline-template :solution="textSolution">
    <div>
        <form class="form-horizontal">
            <div class="form-group">
                <label for="TextAnswer" class="col-sm-2 control-label">Antwort</label>
                <div class="col-sm-10">
                    <input type="text" class="form-control" id="TextSolutution" placeholder="" v-model="date" v-on:change="setSolution()">
                </div>
            </div>
            <div class="form-group">
                <select class="form-control" v-model="seletedPrecision" v-on:change="setSolution()">
                    <option v-for="(el, index) in precision" :value="index + 1">{{el}}</option>
                </select>
            </div>
            <div class="form-group">
            </div>
        </form>

    </div>

</date-component>
