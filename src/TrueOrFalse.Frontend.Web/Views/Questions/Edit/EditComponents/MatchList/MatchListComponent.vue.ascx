<matchlist-component inline-template :solution="matchListJson">
    <div>
        <form class="form-inline" v-for="(pair, index) in pairs" :key="'pair' + index">
            <div class="form-group">
                <label :for="'left-'+index">Linkes Element</label>
                <input type="text" class="form-control" :id="'left-'+index" v-model="pair.ElementLeft.Text" placeholder="">
            </div>
            <div class="form-group">
                <label :for="'right-'+index">Rechtes Element</label>
                <select v-model="pair.ElementRight" id="'right-'+index">
                    <template v-for="el in rightElements">
                        <option v-if="el.Text != null && el.Text.length > 0" value="el">{{el.Text}}</option>
                    </template>
                </select>
            </div>
            <div @click="deletePair(index)" class="btn btn-default">Paar loeschen</div>
        </form>
        <div @click="addPair()" class="btn btn-default">Paar hinzufuegen</div>
        <label>Antwortoptionen</label>
        <form v-for="(element, i) in rightElements" :key="i">
            <div class="form-group">
                <input type="text" class="form-control" :id="i" v-model="element.Text" placeholder="">
            </div>
        </form>
        <div @click="addRightElement()" class="btn btn-default">Rechtes Element hinzufuegen</div>

    </div>

</matchlist-component>
