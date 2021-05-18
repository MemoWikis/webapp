﻿<matchlist-component :answer="matchlistAnswer">
    <div>
        <form class="form-inline" v-for="(pair, index) in pairs" :key="index">
            <div class="form-group">
                <label :for="'left-'+index">Name</label>
                <input type="text" class="form-control" :id="'left-'+index" placeholder="">
            </div>
            <div class="form-group">
                <label :for="'right-'+index">Email</label>
                <input type="text" class="form-control" :id="'right-'+index" placeholder="">
            </div>
            <button @click="deletePair(index)" class="btn btn-default">Paar loeschen</button>
        </form>
        <button @click="addPair()" class="btn btn-default">Paar hinzufuegen</button>

    </div>

</matchlist-component>
