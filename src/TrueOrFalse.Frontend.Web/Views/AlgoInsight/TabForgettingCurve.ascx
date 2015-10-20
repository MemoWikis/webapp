<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TabForgettingCurveModel>" %>

    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
      google.load("visualization", "1", {packages:["corechart"]});
      google.setOnLoadCallback(drawChart);
      function drawChart() {
        var data = google.visualization.arrayToDataTable([
          ['Age', 'Leichte', 'Schwer', 'Mittel', 'Nobrainer'],
          [8, 3, 2, 4, 2],
          [4, 12, 2, 1, 3],
          [11, 12, 1, 2, 2],
          [4, 12, 2, 4, 5],
          [3, 12, 3, 1, 2],
          [6.5, 3, 5, 9, 3]
        ]);

        var options = {
          hAxis: {minValue: 0, maxValue: 15},
          vAxis: {minValue: 0, maxValue: 15},
          chartArea: { width: '90%' },
          legend: { position: 'top' },
          trendlines: {
            0: {
              type: 'linear',
              showR2: true,
              visibleInLegend: true
            },
            1: {
                type: 'linear',
                showR2: true,
                visibleInLegend: true
            },
            2: {
              type: 'linear',
              showR2: true,
              visibleInLegend: true
            },
            3: {
                type: 'linear',
                showR2: true,
                visibleInLegend: true
            },
          }
        
        };

        var chartLinear = new google.visualization.ScatterChart(document.getElementById('chartLinear'));
        chartLinear.draw(data, options);

          
        options.trendlines[0].type = 'exponential';
        options.trendlines[1].type = 'exponential';
        options.trendlines[2].type = 'exponential';
        options.trendlines[3].type = 'exponential';

        var chartExponential = new google.visualization.ScatterChart(document.getElementById('chartExponential'));
        chartExponential.draw(data, options);

        options.legend.position = 'right';

        chartExponential2 = new google.visualization.ScatterChart(document.getElementById('chartExponential2'));
        chartExponential2.draw(data, options);
      }
    </script>

<div class="row" >
    <div class="col-md-12" style="margin-top:3px; margin-bottom:7px;">
        <h3>Vergleich Vergessenskurven</h3>
    </div>   
</div>

<div class="row">
    <div class="col-md-3">
        <div class="row" style="margin-bottom: 12px">
            <div class="col-md-4">            
                Intervall:
            </div>
            <div class="col-md-8" style="padding-left: 0px;">
                <select>
                    <option>Minuten</option>
                    <option>Stunden</option>
                    <option>Tage</option>
                    <option>Wochen</option>
                    <option>Logarithmisch</option>
                </select>
            </div>
        </div>
        
        <% for(var i = 0; i < 4; i++)
            {
                var colors = new[]
                {
                   "rgb(51, 102, 204)" /* blue */,
                   "rgb(220, 57, 18)" /* red */,
                   "rgb(255, 153, 0)" /* yellow */,
                   "rgb(16, 150, 24)" /* green */
               };

        %>
            <div class="row">
                <div class="col-md-4" style="text-align: right; background-color: <%= colors[i] %>;">
                    Achse <%= i %>
                </div>
            </div>
            <div class="row" style="padding: 3px;">
                <div class="col-md-4" style="text-align: right">Feature:</div>
                <div class="col-md-8" style="padding-left: 0px;">
                    <select style="width: 100%">
                        <option>Wiederholungen 1</option>
                        <option>Wiederholungen 2</option>
                        <option>Wiederholungen 3</option>
                    </select>
                </div>
            </div>
            <div class="row" style="padding: 3px;">
                <div class="col-md-4" style="text-align: right">Typ:</div>
                <div class="col-md-8" style="padding-left: 0px;">
                    <select style="width: 100%;">
                        <option>Mittelschwer</option>
                        <option>Schwer</option>
                        <option>Leicht</option>
                        <option>Nobrainer</option>
                    </select>
                </div>
            </div>
        <% } %>
    </div>
    <div class="col-md-9" style="vertical-align: top; text-align: left;">
        <div id="chartExponential2" style="width: 100%; height: 350px; vertical-align: top"></div> 
    </div>
    
    
    <div class="row" >
        <div class="col-md-12" style="margin-top:3px; margin-bottom:7px;">
            <h3>Ausgewählte Vergessenskurven</h3>
        </div>   
    </div>

    <div class="row">
        <div class="col-md-6">
            <h4>Schwere vs. leichte Fragen (Klassifzierung)</h4>
            <div id="chartLinear" style="width: 100%; height: 250px"></div>
        </div>
        <div class="col-md-6">
            <h4>Nach Tageszeit gelernt</h4>
            <div id="chartExponential" style="width: 100%; height: 250px"></div>        
        </div>
    </div>    

</div>

