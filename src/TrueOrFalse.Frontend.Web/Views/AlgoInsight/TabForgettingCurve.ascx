<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TabForgettingCurveModel>" %>

<script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script type="text/javascript" src="/Views/AlgoInsight/TabForgettingCurve_.js"></script>

<div class="row">
    <div class="col-md-12">
        <div class="alert alert-danger" style="margin-top: 10px;">Achtung: Aus Performance gründen deaktiviert</div>
    </div>
</div>

<div class="row" >
    <div class="col-md-12" style="margin-top:3px; margin-bottom:7px;">
        <h3>Vergleich Vergessenskurven</h3>
    </div>   
</div>

<div class="row">
    <div class="col-md-3">
                
        <% 
            var allAnswerFeatures = Sl.R<AnswerFeatureRepo>().GetAll();
            var allQuestionFeatures = Sl.Resolve<QuestionFeatureRepo>().GetAll();

            for(var i = 0; i < 4; i++)
            {
                var colors = new[]
                {
                   "rgb(51, 102, 204)" /* blue */,
                   "rgb(220, 57, 18)" /* red */,
                   "rgb(255, 153, 0)" /* yellow */,
                   "rgb(16, 150, 24)" /* green */
               };

        %>
            <div class="row" style="border-bottom: 2px solid <%= colors[i] %>;">
                <div class="col-md-12" style="">
                    <b>Kurve <%= i %></b>
                    <span id="pairCount<%=i %>" class="show-tooltip" data-original-title="Anzahl Antwort-Paare"></span>
                    <%--<span id="regressionValue<%=i %>" class="show-tooltip" data-original-title="Regressionswert"></span>--%>

                    <%--<input id="ckbShowCurve<%= i %>" type="checkbox" class="pull-right" checked="checked" style="padding-right: 3px; position: relative; top: 0px;"  />--%>
                </div>
            </div>
            <div class="row" style="padding: 3px; margin-top: 5px;">
                <div class="col-md-4" style="text-align: right">Feature:</div>
                <div class="col-md-8" style="padding-left: 0px;">
                    <select style="width: 100%" id="ddlAnswerFeature<%= i %>">
                        <option value="all">Alle</option>
                        <% foreach(var feature in allAnswerFeatures) { %>
                            <option value="<%= feature.Id %>"><%= feature.Name %></option>
                        <% } %>
                    </select>
                </div>
            </div>
            <div class="row" style="padding: 3px; margin-bottom: 10px;">
                <div class="col-md-4" style="text-align: right">Typ:</div>
                <div class="col-md-8" style="padding-left: 0px;">
                    <select style="width: 100%;" id="ddlQuestionFeature<%= i %>">
                        <option value="all">Alle</option>
                        <% foreach(var feature in allQuestionFeatures) { %>
                            <option value="<%= feature.Id %>"><%= feature.Name %></option>
                        <% } %>
                    </select>
                </div>
            </div>
        <% } %>
    </div>

    <div class="col-md-9" style="vertical-align: top; text-align: left;">
        <div id="chartExplore" style="width: 100%; height: 350px; vertical-align: top"></div>
        
        <div class="row" style="margin-bottom: 12px; padding-left: 20px;">
            <div class="col-md-12">            
                Intervall: &nbsp;
                
                <select id="ddlInterval">
                    <option value="Minutes">Minuten</option>
                    <option value="Hours">Stunden</option>
                    <option value="Days">Tage</option>
                    <option value="Week">Wochen</option>
                    <option value="Month">Monate</option>
                    <option value="Logarithmic" disabled="disabled">Logarithmisch</option>
                </select>
                &nbsp;
                max: 
                <input id="txtIntervalCount" type="text" value="50" style="width: 90px;" />
            </div>
        </div>

    </div>
    
    <!-- 
    <div class="row" >
        <div class="col-md-12" style="margin-top:3px; margin-bottom:7px;">
            <h3>Ausgewählte Vergessenskurven</h3>
            
            <p>
                Eine Auswahl besonders aussagekräftiger 
            </p>

        </div>
    </div>

    <div class="row">
        <div class="col-md-6">
            <h4>Schwere vs. leichte Fragen (Klassifzierung)</h4>
            <div id="chartSuggested1" style="width: 100%; height: 250px"></div>
        </div>
        <div class="col-md-6">
            <h4>Nach Tageszeit gelernt</h4>
            <div id="chartSuggested2" style="width: 100%; height: 250px"></div>        
        </div>
    </div>
    -->

</div>

