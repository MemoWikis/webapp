<matchlist-component inline-template :solution="matchListJson">
    <div class="input-container matchlist-container">
        <div class="overline-s no-line">Antworten</div>

        <form class="form-inline matchlist-pairs" v-for="(pair, index) in pairs" :key="'pair' + index">
            <div class="matchlist-left form-group">
                <input type="text" class="form-control col-sm-10" :id="'left-'+index" v-model="pair.ElementLeft.Text" placeholder="Linkes Element" v-on:change="solutionBuilder()">
                <i class="fas fa-arrow-right col-sm-2"></i>
            </div>
            <div class="matchlist-right form-group">                
                <select v-model="pair.ElementRight.Text" :id="'right-'+index" class="col-sm-10" v-on:change="solutionBuilder()">
                    <option disabled selected value="" hidden>Rechtes Element</option>

                    <option v-for="el in rightElements" v-if="el.Text != null && el.Text.length > 0" :value="el.Text">{{el.Text}}</option>
                </select>
                <div @click="deletePair(index)" class="btn grey-bg col-sm-2" v-if="pairs.length > 1"><i class="fas fa-trash"></i></div>
                <div class="col-sm-2" v-else></div>
            </div>
        </form>
        <div class="matchlist-options">
            <div class="matchlist-left d-flex">
                    <div @click="addPair()" class="form-control btn col-sm-10 grey-bg">Paar hinzufügen</div>
                    <div class="col-sm-2"></div>
            </div>
            <div class="matchlist-right">
                <div v-for="(element, i) in rightElements" :key="i" class="form-group">
                        <div class="d-flex">
                            <input type="text" class="form-control col-sm-10" :id="i" v-model="element.Text" placeholder="" v-on:change="solutionBuilder()">
                            <div @click="deleteRightElement(i)" class="btn grey-bg col-sm-2"><i class="fas fa-trash"></i></div>
                        </div>
                </div>
                <div class="d-flex">
                    <div @click="addRightElement()" class="btn col-sm-10 form-control grey-bg">Rechtes Element erstellen</div>
                    <div class="col-sm-2"></div>
                </div>
            </div>

        </div>
        <div class="col-sm-12 is-solutionordered-checkbox">
            <div class="checkbox">
                <label> 
                    <input type="checkbox" v-model="isSolutionOrdered" :true-value="false" :false-value="true">Paare zufällig anordnen
                </label>
            </div>
        </div>



    </div>

</matchlist-component>
