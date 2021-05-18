<multiplechoice-singlesolution-component :answer="singleSolutionAnswer">
    <div>
        <form class="form-inline" v-for="(choice, index) in choices" :key="index">
            <div class="form-group">
                <label class="sr-only" for=""></label>
                <div class="input-group">
                    <input type="text" class="form-control" id="" placeholder="">
                </div>
            </div>
            <button @click="toggle()" class="btn btn-primary">Transfer cash</button>
            <button @click="deleteChoice(index)" class="btn btn-primary">Antwort loeschen</button>
        </form> 
        
        <button @click="addChoice()" class="btn btn-primary">Antwort loeschen</button>

    </div>

</multiplechoice-singlesolution-component>
