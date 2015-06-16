$(function () {

    var table = createOverviewTable();
    var container = $("#overview-table")[0];
    container.appendChild(table);
});

function createOverviewTable() {

    // create table
    var table = document.createElement('table');

    // create table header
    var headerInfo = createTableHeader(table);

    // create table body
    createTableRows(table, headerInfo);

    return table;
};

function createTableHeader(table) {

    // create header row, append 1st empty cell
    var thead = document.createElement('thead');
    var tr = document.createElement('tr');
    tr.appendChild(document.createElement('th'));
    thead.appendChild(tr);

    var headerInfo = [];

    // create other header cells
    WAVE.arrayWalkable(data)
        .wSelect(function (e) { return getColumnHeaderData(e); })
        .wDistinct(function (a, b) { return a.serializerType == b.serializerType && a.serializerName == b.serializerName; })
        .wEach(function (h) {
            // insert column header cell
            var th = document.createElement("th");
            var htmlTemplate = "<th><b>@serializerType@<br>@serializerName@</b></th>";
            th.innerHTML = WAVE.strHTMLTemplate(htmlTemplate, { serializerType: h.serializerType, serializerName: h.serializerName });
            tr.appendChild(th);
            // populate header info
            headerInfo.push(h);
        });

    table.appendChild(thead);

    return headerInfo;
}

function createTableRows(table, headerInfo) {

    var tbody = document.createElement('tbody');

    WAVE.arrayWalkable(data)
        .wSelect(function (e) { return getRowHeaderData(e); })
        .wDistinct(function (a, b) { return a.testType == b.testType && a.testName == b.testName; })
        .wEach(function (r) {
            // create row, insert first cell (row header)
            var tr = document.createElement('tr');
            var th = document.createElement('th');
            var htmlTemplate = "<b>@testType@<br>@testName@</b>";
            th.innerHTML = WAVE.strHTMLTemplate(htmlTemplate, { testType: r.testType, testName: r.testName });
            tr.appendChild(th);
            // fill table cells
            WAVE.arrayWalkable(headerInfo)
                .wEach(function (h) { createTableCell(tr, r, h); });

            tbody.appendChild(tr);
        });

    table.appendChild(tbody);
}

function createTableCell(tr, rowInfo, headerInfo) {

    var benchTest = WAVE.arrayWalkable(data)
                        .wFirst(function (t) {
                            return t.TestType == rowInfo.testType &&
                                    t.TestName == rowInfo.testName &&
                                    t.SerializerType == headerInfo.serializerType &&
                                    t.SerializerName == headerInfo.serializerName;
                        });

    var serClass;
    if (JSON.parse(benchTest.SerSupported)) serClass = "supported";
    else serClass = "unsupported";
    var deserClass;
    if (JSON.parse(benchTest.DeserSupported)) deserClass = "supported";
    else deserClass = "unsupported";

    var td = document.createElement('td');
    var htmlTemplate =
        "<div class=\"overview-table-cell\">" +
            "<div>" +
                "Run <b>@runNumber@</b> time(s)<br>" +
                "Payload: <b>@payloadSize@</b> byte(s)<br>" +
                "GC: <b>@doGc@</b>" +
            "</div>" +
            "<div class=\"@serClass@\"><table>" +
                "<tr><td align=\"center\"><b>Serialization</b></td></tr>" +
                "<tr><td>iterations:</td><td><b>@serIterations@</b></td></tr>" +
                "<tr><td>exceptions:</td><td><b>@serExceptions@</b></td></tr>" +
                "<tr><td>aborts:</td><td><b>@serAborts@</b></td></tr>" +
                "<tr><td>duration (ms):</td><td><b>@serDurationMs@</b></td></tr>" +
                "<tr><td>duration (ticks):</td><td><b>@serDurationTick@</b></td></tr>" +
                "<tr><td>ops/sec:</td><td><b>@serOpsSec@</b></td></tr>" +
            "</table></div>" +
            "<div class=\"@deserClass@\"><table>" +
                "<tr><td align=\"center\"><b>Deserialization</b></td></tr>" +
                "<tr><td>iterations:</td><td><b>@deserIterations@</b></td></tr>" +
                "<tr><td>exceptions:</td><td><b>@deserExceptions@</b></td></tr>" +
                "<tr><td>aborts:</td><td><b>@deserAborts@</td></tr>" +
                "<tr><td>duration (ms):</td><td><b>@deserDurationMs@</b></td></tr>" +
                "<tr><td>duration (ticks):</td><td><b>@deserDurationTick@</b></td></tr>" +
                "<tr><td>ops/sec:</td><td><b>@deserOpsSec@</b></td></tr>" +
            "</table></div>" +
        "</div>";

    td.innerHTML = WAVE.strHTMLTemplate(htmlTemplate,
                                      {
                                          runNumber: benchTest.RunNumber,
                                          payloadSize: benchTest.PayloadSize,
                                          doGc: benchTest.DoGc,
                                          serClass: serClass,
                                          serIterations: benchTest.SerIterations,
                                          serExceptions: benchTest.SerExceptions,
                                          serAborts: benchTest.SerAborts,
                                          serDurationMs: benchTest.SerDurationMs,
                                          serDurationTick: benchTest.SerDurationTicks,
                                          serOpsSec: benchTest.SerOpsSec,
                                          deserClass: deserClass,
                                          deserIterations: benchTest.DeserIterations,
                                          deserExceptions: benchTest.DeserExceptions,
                                          deserAborts: benchTest.DeserAborts,
                                          deserDurationMs: benchTest.DeserDurationMs,
                                          deserDurationTick: benchTest.DeserDurationTicks,
                                          deserOpsSec: benchTest.DeserOpsSec,
                                      });
    tr.appendChild(td);
};

function getColumnHeaderData(benchTest) {
    return { serializerType: benchTest.SerializerType, serializerName: benchTest.SerializerName };
}

function getRowHeaderData(benchTest) {
    return { testType: benchTest.TestType, testName: benchTest.TestName };
};