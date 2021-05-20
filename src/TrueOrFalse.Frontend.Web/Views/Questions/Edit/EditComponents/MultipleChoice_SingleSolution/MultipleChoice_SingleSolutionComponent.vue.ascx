<multiplechoice-singlesolution-component :solution="singleSolutionJson">
    <div>
        <form class="form-inline" v-for="(choice, index) in choices" :key="index">
            <div class="form-group">
                <label class="sr-only" for=""></label>
                <div class="input-group">
                    <input type="text" class="form-control" id="" placeholder="" v-model="choice.value">
                </div>
                <button @click="deleteChoice(index)" class="btn btn-primary">Antwort loeschen</button>
            </div>
            
            <button @click="addChoice()" class="btn btn-primary">Antwort loeschen</button>


        </form> 
        

    </div>

</multiplechoice-singlesolution-component>
