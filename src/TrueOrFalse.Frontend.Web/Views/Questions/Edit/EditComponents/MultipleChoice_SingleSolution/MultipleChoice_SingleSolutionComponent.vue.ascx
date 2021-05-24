<multiplechoice-singlesolution-component inline-template :solution="singleSolutionJson">
    <div>
        <form class="form-inline" v-for="(choice, index) in choices" :key="index">
            <div class="form-group">
                <label class="sr-only" :for="'SolutionInput-'+index"></label>
                <div class="input-group">
                    <input type="text" class="form-control" :id="'SolutionInput-'+index" placeholder="" v-model="choice.value">
                </div>
                <div @click="deleteChoice(index)" class="btn btn-primary">Antwort loeschen</div>
            </div>
            
        </form>
        <div @click="addChoice()" class="btn btn-primary">Option hinzufuegen</div>

    </div>

</multiplechoice-singlesolution-component>
