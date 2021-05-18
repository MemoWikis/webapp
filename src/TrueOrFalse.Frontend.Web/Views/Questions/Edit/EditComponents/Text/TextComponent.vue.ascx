<text-component :answer="textAnswer">
    <div>
        <form class="form-horizontal">
            <div class="form-group">
                <label for="TextAnswer" class="col-sm-2 control-label">Antwort</label>
                <div class="col-sm-10">
                    <input type="text" class="form-control" id="TextAnswer" placeholder="">
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <div class="checkbox">
                        <label>
                            <input type="checkbox"> Groß-/Kleinschreibung beachten
                        </label>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <div class="col-sm-offset-2 col-sm-10">
                    <div class="checkbox">
                        <label>
                            <input type="checkbox"> Exakte Schreibweise
                        </label>
                    </div>
                </div>
            </div>
        </form>

    </div>

</text-component>
