function initChart() {
    WAVE.GUI.rulerSetScope("", {});

    var chartElement = $("#chart-element")[0];
    var chart = new WAVE.Chart.SVG.Chart(chartElement);

    chart.xAxis().dataType(WAVE.Chart.SVG.DataType.NUMBER);
    chart.xAxis().isBottom(false);
    chart.yAxis().isLeft(false);
    chart.yAxis().minWidth(500);
    chart.lZone().corner(WAVE.Chart.SVG.RectCorner.LEFTTOP);
    chart.sZone().showPointTitle(true);
    chart.sZone().chartType(WAVE.Chart.SVG.ChartType.LINE);
    chart.sZone().attachToRulerScope("");

    return chart;
};

function addTestData(chart) {
    var series = chart.addSeries({ title: "Simple series test", pointClass: "series-point-0", showPointTitleLeg: false });
    for (var i = 0; i < data.length; i++) {
        var x = i;
        var y = data[i].SerDurationMs;
        series.dataSet().addPoint(x, y, "");
    };
};

$(function () {
    var chart = initChart();
    chart.beginUpdate();
    addTestData(chart);
    chart.endUpdate();
});