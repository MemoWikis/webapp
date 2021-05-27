<matchlist-component inline-template :solution="matchListJson">
    <div>
        <form class="form-inline" v-for="(pair, index) in pairs" :key="index">
            <div class="form-group">
                <label :for="'left-'+index">Linkes Element</label>
                <input type="text" class="form-control" :id="'left-'+index" v-model="pair.ElementLeft.Text" placeholder="">
            </div>
            <div class="form-group">
                <label :for="'right-'+index">Rechtes Element</label>
                <select v-model="pair.ElementRight" id="'right-'+index">
                    <option v-for="el in rightElements" value="el">{{el.Text}}</option>
                </select>
            </div>
            <div @click="deletePair(index)" class="btn btn-default">Paar loeschen</div>
        </form>
        <div @click="addPair()" class="btn btn-default">Paar hinzufuegen</div>
        
        <form v-for="(element, index) in rightElements" :key="index">
            <div class="form-group">
                <label :for="index">Antwortoptionen</label>
                <input type="text" class="form-control" :id="index" v-model="element.Text" placeholder="">
            </div>
        </form>
        <div @click="addRightElement()" class="btn btn-default">Rechtes Element hinzufuegen</div>

    </div>

</matchlist-component>
