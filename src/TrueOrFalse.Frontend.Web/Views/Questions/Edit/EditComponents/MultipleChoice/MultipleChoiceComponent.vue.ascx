<multiplechoice-component :answer="multipleChoiceAnswer">
    <div>
        <form class="form-inline" v-for="(choice, index) in choices" :key="index">
            <div class="form-group">
                <label class="sr-only" for=""></label>
                <div class="input-group">
                    <input type="text" class="form-control" id="" placeholder="">
                </div>
                <button @click="choice.IsCorrect = !choice.IsCorrect" class="btn btn-primary"></button>
                <button @click="deleteChoice(index)" class="btn btn-primary">Antwort loeschen</button>
            </div>

        </form> 
        
        <button @click="addChoice()" class="btn btn-primary">Antwort loeschen</button>

    </div>

</multiplechoice-component>
