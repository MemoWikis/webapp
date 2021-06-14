    <div class="col-sm-12">
        <div class="checkbox">
            <label> 
                <input type="checkbox" v-model="visibility" value="1"> Private Frage <i class="fas fa-lock show-tooltip tooltip-min-200" title="" data-placement="top" data-html="true" data-original-title="
                            <ul class='show-tooltip-ul'>
                                <li>Die Frage kann nur von dir genutzt werden.</li>
                                <li>Niemand sonst kann die Frage sehen oder nutzen.</li>
                            </ul>">
                </i>
            </label>
        </div>
    </div>
    <div class="col-sm-12 license-confirmation-box" v-if="visibility == 0">
        <div class="checkbox">
            <label>
                <input type="checkbox" v-model="licenseConfirmation">
                Dieser Eintrag wird veröffentlicht unter CC BY 4.0. <a @click="showMore = !showMore">mehr</a>
                <template v-if="showMore">
                    <br/>
                    <br/>
                    Ich stelle diesen Eintrag unter die Lizenz "Creative Commons - 
                    Namensnennung 4.0 International" (CC BY 4.0, Lizenztext, deutsche Zusammenfassung). 
                    Der Eintrag kann bei angemessener Namensnennung ohne Einschränkung weiter genutzt werden. 
                    Die Texte und ggf. Bilder sind meine eigene Arbeit und nicht aus urheberrechtlich geschützten Quellen kopiert.
                </template>

            </label>
        </div>
    </div>
