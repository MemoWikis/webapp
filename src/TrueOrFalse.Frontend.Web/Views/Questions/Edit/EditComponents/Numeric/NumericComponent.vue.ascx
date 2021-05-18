<numeric-component :answer="numericAnswer">
    <div>
        <form class="form-horizontal">
            <div class="form-group">
                <label for="NumericAnswer" class="col-sm-2 control-label">Antwort</label>
                <div class="col-sm-10">
                    <input type="number" class="form-control" id="NumericAnswer" placeholder="1234">
                </div>
            </div>
            <div class="form-group">
                <label for="NumberAccuracy" class="col-sm-2 control-label">Password</label>
                <div class="col-sm-10">
                    <input type="password" class="form-control" id="NumberAccuracy" placeholder="20">
                    <div class="input-group-addon">%</div>
                </div>
            </div>
            <div class="form-group">
                <label for="NumberUnit" class="col-sm-2 control-label">Einheit</label>
                <div class="col-sm-10">
                    <input type="password" class="form-control" id="NumberUnit" placeholder="">
                    <div class="input-group-addon">%</div>
                </div>
            </div>
        </form>

    </div>

</numeric-component>
