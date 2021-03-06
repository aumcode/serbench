// **************************************** Globals ****************************************

var performancePalette = [
              '#ff7060',
              '#ff9070',
              '#ffa060',
              '#ffb050',
              '#ffc040',
              '#ffd030',
              '#e0e030',
              '#d0f030',
              '#c0ff30',
              '#c0ff70',
              '#c0ffA0',
              '#c0ffC0',
              '#b0ffE0',
              '#a0ffF0',
              '#80ffFF'];


// ******************************** Number format functions ********************************

// formats number with comma as thousand delimeter and rounds it with given accuracy 
function numberWithCommas(x, precision, removeTrailingZeros) {

    if (typeof precision != 'undefined')
        x = x.toFixed(precision);
    if (typeof removeTrailingZeros == 'undefined')
        removeTrailingZeros = true;

    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    if (removeTrailingZeros && parts.length > 1) {
        parts[1] = parts[1].replace(/0+$/g, "");
    }

    if (parts[1] !== "")
        return parts.join(".");

    return parts[0];
};

// shortens number with well-known suffixes (K, M, ...) and rounds it with given accuracy
function shortenQuantity(x, precision) {
    if (typeof precision == 'undefined')
        precision = 1;
    var shortens = ['', 'K', 'M', 'B', 'T'];

    var n = 0;
    while (x >= 1000 && n < 4) {
        x = x / 1000;
        n++;
    }

    x = x.toFixed(precision);
    x = numberWithCommas(x);

    return x + shortens[n];
}

// shortens byte size with well-known suffixes (KB, MB, GB, ...) and rounds it with given accuracy
function shortenByteSize(x, precision) {
    if (typeof precision == 'undefined')
        precision = 2;
    var shortens = ['B', 'KB', 'MB', 'GB', 'TB', 'PB'];

    var n = 0;
    while (x >= 1024 && n < 5) {
        x = x / 1024;
        n++;
    }

    x = x.toFixed(precision);
    x = numberWithCommas(x);

    return x + shortens[n];
}

// leverages given integer to next closest step (25, 50, 250, 500, 2500, 5000, ...)
function leverageNumber(x) {
    if (x < 0)
        return x;

    if (x < 25)
        return Math.floor(x) + 1;

    var t = 1;
    var nx = x / 25;
    while (nx >= 10) {
        t *= 10;
        nx /= 10;
    }

    var h = nx < 2 ? 5 * t : 10 * t;
    return h * Math.floor(x / h) + h;
}



// ******************************** Data aggregate functions *******************************

function isTestClear(testData) {
    return testData == null || 
          (testData.speedMin != 0 &&
           testData.PayloadSize != 0 &&
           JSON.parse(testData.SerSupported) &&
           JSON.parse(testData.DeserSupported));
}

// returns aggregate data for all tests with given test's and serializer's name and type
function getCellData(data, rowHeaderInfo, columnHeaderInfo) {

    // seek for all tests with the same column's and row's header data
    var benchTests = data.wWhere(function (t) {
        return t.TestType == rowHeaderInfo.testType &&
            t.TestName == rowHeaderInfo.testName &&
            t.SerializerType == columnHeaderInfo.serializerType &&
            t.SerializerName == columnHeaderInfo.serializerName;
    });

    // if there is no test - fill cell with empty (gray) space
    if (!benchTests.wAny()) {
        return null;
    }

    var benchTest = benchTests.wFirst();
    var result = {
        TestType : benchTest.TestType,
        TestName : benchTest.TestName,
        SerializerType : benchTest.SerializerType,
        SerializerName : benchTest.SerializerName,
        PayloadSize: benchTest.PayloadSize,
        RunException : benchTest.RunException,
        DoGc : benchTest.DoGc,
        FirstSerAbortMsg : benchTest.FirstSerAbortMsg, 
        SerSupported : benchTest.SerSupported,
        SerIterations : benchTest.SerIterations,
        SerExceptions: benchTest.SerExceptions,
        SerAborts: benchTest.SerAborts,
        SerDurationMs: benchTest.SerDurationMs,
        SerDurationTicks: benchTest.SerDurationTicks,
        SerOpsSec: benchTest.SerOpsSec,
        FirstDeserAbortMsg : benchTest.FirstDeserAbortMsg, 
        DeserSupported : benchTest.DeserSupported,
        DeserIterations : benchTest.DeserIterations, 
        DeserExceptions: benchTest.DeserExceptions,
        DeserAborts: benchTest.DeserAborts,
        DeserDurationMs: benchTest.DeserDurationMs,
        DeserDurationTicks: benchTest.DeserDurationTicks,
        DeserOpsSec: benchTest.DeserOpsSec
    };

    // if there are many tests, calculate avarage data
    var count = benchTests.wCount();
    if (count > 1) {
        benchTests.wSkip(1).wEach(function (t) {
            result.PayloadSize += t.PayloadSize;
            result.SerExceptions += t.SerExceptions;
            result.SerAborts += t.SerAborts;
            result.SerDurationMs += t.SerDurationMs;
            result.SerDurationTicks += t.SerDurationTicks;
            result.SerOpsSec += t.SerOpsSec;
            result.DeserExceptions += t.DeserExceptions;
            result.DeserAborts += t.DeserAborts;
            result.DeserDurationMs += t.DeserDurationMs; 
            result.DeserDurationTicks += t.DeserDurationTicks;
            result.DeserOpsSec += t.DeserOpsSec;
        });
        result.SerDurationMs = result.SerDurationTicks / 1000;
        result.SerOpsSec /= count;
        result.DeserDurationMs = result.DeserDurationTicks / 1000;
        result.DeserOpsSec /= count;
    }
    result.runCount = count;
    result.speedMin = Math.min(result.SerOpsSec, result.DeserOpsSec);
    result.speedMax = Math.max(result.SerOpsSec, result.DeserOpsSec);

    return result;
};
                    
// returns information about best serializers for a given test (by speed and payload)
function createTestSummary(testName, testType, serializersData, onlyClearTests) {

    // payload filtering and sorting

    var payloadFilterPredicate = onlyClearTests ?
                           function (d) { return d.data !== null && isTestClear(d.data); } :
                           function (d) { return d.data !== null && d.data.PayloadSize != 0; };
    var payloadOrdered = serializersData.wWhere(payloadFilterPredicate)
                                        .wOrder(function (a, b) { return sortByPayloadPredicate(a, b, onlyClearTests); })
                                        .wToArray();

    var goldPayloadTest = payloadOrdered[0];
    var silverPayloadTest = payloadOrdered[1];
    var bronzePayloadTest = payloadOrdered[2];

    var goldPayload = goldPayloadTest === undefined || goldPayloadTest == null ? '' : '(' + numberWithCommas(goldPayloadTest.data.PayloadSize, 0) + ' byte(s))';
    var silverPayload = silverPayloadTest === undefined || silverPayloadTest == null ? '' : '(' + numberWithCommas(silverPayloadTest.data.PayloadSize, 0) + ' byte(s))';
    var bronzePayload = bronzePayloadTest === undefined || bronzePayloadTest == null ? '' : '(' + numberWithCommas(bronzePayloadTest.data.PayloadSize, 0) + ' byte(s))';

    // speed filtering and sorting

    var speedFilterPredicate = onlyClearTests ?
                         function (d) { return d.data !== null && isTestClear(d.data); } :
                         function (d) { return d.data !== null && d.data.speedMin != 0; };
    var speedOrdered = serializersData.wWhere(speedFilterPredicate)
                                      .wOrder(function (a, b) { return sortBySpeedPredicate(a, b, onlyClearTests); })
                                      .wToArray();

    var goldSpeedTest = speedOrdered[0];
    var silverSpeedTest = speedOrdered[1];
    var bronzeSpeedTest = speedOrdered[2];

    var goldSpeed = goldSpeedTest === undefined || goldSpeedTest == null ? '' : '(' + numberWithCommas(goldSpeedTest.data.speedMin, 0) + ' ops/sec)';
    var silverSpeed = silverSpeedTest === undefined || silverSpeedTest == null ? '' : '(' + numberWithCommas(silverSpeedTest.data.speedMin, 0) + ' ops/sec)';
    var bronzeSpeed = bronzeSpeedTest === undefined || bronzeSpeedTest == null ? '' : '(' + numberWithCommas(bronzeSpeedTest.data.speedMin, 0) + ' ops/sec)';

    // table row header content

    var htmlTemplate =
        "<div class='row-header'>" +
            "<b>@testName@</b><br>@testType@<br><br>" +
            "<b>by speed:</b><br>" +
            "1. @goldSpeedTest@ @goldSpeed@<br>" +
            "2. @silverSpeedTest@ @silverSpeed@<br>" +
            "3. @bronzeSpeedTest@ @bronzeSpeed@<br><br>" +
            "<b>by payload:</b><br>" +
            "1. @goldPayloadTest@ @goldPayload@<br>" +
            "2. @silverPayloadTest@ @silverPayload@<br>" +
            "3. @bronzePayloadTest@ @bronzePayload@<br>" +
        "<div>";

    var html = WAVE.strHTMLTemplate(htmlTemplate,
        {
            testType: testType,
            testName: testName,
            goldSpeedTest: goldSpeedTest === undefined || goldSpeedTest == null ? '-' : goldSpeedTest.data.SerializerName,
            silverSpeedTest: silverSpeedTest === undefined || silverSpeedTest == null ? '-' : silverSpeedTest.data.SerializerName,
            bronzeSpeedTest: bronzeSpeedTest === undefined || bronzeSpeedTest == null ? '-' : bronzeSpeedTest.data.SerializerName,
            goldSpeed: goldSpeed,
            silverSpeed: silverSpeed,
            bronzeSpeed: bronzeSpeed,
            goldPayloadTest: goldPayloadTest === undefined || goldPayloadTest == null ? '-' : goldPayloadTest.data.SerializerName,
            silverPayloadTest: silverPayloadTest === undefined || silverPayloadTest == null ? '-' : silverPayloadTest.data.SerializerName,
            bronzePayloadTest: bronzePayloadTest === undefined || bronzePayloadTest == null ? '-' : bronzePayloadTest.data.SerializerName,
            goldPayload: goldPayload,
            silverPayload: silverPayload,
            bronzePayload: bronzePayload
        });

    return html;
}

// returns performance boundaries for a given set of tests 
function getDataSummary(serializersData, onlyClearTests) {

    var summary = {
        speedMin: null,
        speedMax: null,
        speedAbsoluteMax: null,
        payloadMin: null,
        payloadMax: null
    };

    serializersData.wEach(function (d) {
        
        var test = d.data;
        if (test == null)
            return;

        if (onlyClearTests && !isTestClear(test))
            return;

        if (test.speedMin != 0) {
            var speedAbs = Math.max(test.SerOpsSec, test.DeserOpsSec);
            if (summary.speedMin == null || summary.speedMin > test.speedMin)
                summary.speedMin = test.speedMin;
            if (summary.speedMax == null || summary.speedMax < test.speedMin)
                summary.speedMax = test.speedMin;
            if (summary.speedAbsoluteMax == null || summary.speedAbsoluteMax < speedAbs)
                summary.speedAbsoluteMax = speedAbs;
        }

        if (test.PayloadSize != 0) {
            if (summary.payloadMin == null || summary.payloadMin > test.PayloadSize)
                summary.payloadMin = test.PayloadSize;
            if (summary.payloadMax == null || summary.payloadMax < test.PayloadSize)
                summary.payloadMax = test.PayloadSize;
        }
    });

    return summary;
}

function createAbortedTable(aborted) {

    if (!aborted.wAny())
        return null;
                 
    var div = document.createElement('div');
    div.className = 'aborted-container';

    // header div
    var header = document.createElement('div');
    header.innerHTML = 'Aborted';
    header.className = 'aborted-header';
    div.appendChild(header);

    var table = document.createElement('table');
    table.className = 'aborted-table main-table';

    // header
    var thead = document.createElement('thead');
    var tr = document.createElement('tr');
    tr.className = 'aborted-table-column-header main-table-column-header';
    var th = document.createElement('th');
    th.innerHTML = '<b>Test</b>';
    tr.appendChild(th);
    th = document.createElement('th');
    th.innerHTML = '<b>Serializer</b>';
    tr.appendChild(th);
    th = document.createElement('th');
    th.innerHTML = '<b>Message</b>';
    tr.appendChild(th);
    thead.appendChild(tr);
    table.appendChild(thead);

    // body
    var tbody = document.createElement('tbody');
    aborted.wEach(function (r) {

        var tr = document.createElement('tr');
        tr.className = 'main-table-row';
        tr.onmouseover = function () { highlightRow(tr) };
        tr.onmouseout = function () { unhighlightRow(tr) };

        var td = document.createElement("td");
        var htmlTemplate = "<b>@testName@</b><br>@testType@";
        td.innerHTML = WAVE.strHTMLTemplate(htmlTemplate, { testName: r.TestName, testType: r.TestType });
        tr.appendChild(td);

        td = document.createElement("td");
        htmlTemplate = "<b>@serializerName@</b><br>@serializerType@";
        td.innerHTML = WAVE.strHTMLTemplate(htmlTemplate, { serializerType: r.SerializerType, serializerName: r.SerializerName });
        tr.appendChild(td);

        td = document.createElement("td");
        htmlTemplate = "@message@";
        td.innerHTML = WAVE.strHTMLTemplate(htmlTemplate, { message: r.Message });
        tr.appendChild(td);

        tbody.appendChild(tr);
    });

    table.appendChild(tbody);
    div.appendChild(table);

    return div;
}

function sortBySpeedPredicate(a, b, onlyClearTests) {
    if (typeof onlyClearTests == 'undefined')
        onlyClearTests = false;

    var aIsClear = !onlyClearTests || isTestClear(a.data);
    var bIsClear = !onlyClearTests || isTestClear(b.data);

    if (a.data == null)
        return b.data == null ? 0 : 1;
    if (!aIsClear)
        return b.data == null ? -1 : (!bIsClear ? 0 : 1);

    return (b.data == null || !bIsClear) ? -1 : (a.data.speedMin < b.data.speedMin ? 1 : -1);
}

function sortByPayloadPredicate(a, b, onlyClearTests) {
    if (typeof onlyClearTests == 'undefined')
        onlyClearTests = false;

    var aIsClear = !onlyClearTests || isTestClear(a.data);
    var bIsClear = !onlyClearTests || isTestClear(b.data);

    if (a.data == null)
        return b.data == null ? 0 : 1;
    if (!aIsClear)
        return b.data == null ? -1 : (!bIsClear ? 0 : 1);

    return (b.data == null || !bIsClear) ? -1 : (a.data.PayloadSize > b.data.PayloadSize ? 1 : -1);
}




// **************************************** UI ****************************************

function highlightRow(row) {
    row.style.border = "1px solid #808080";
    row.style.background = "#ffffe0";
}

function unhighlightRow(row) {
    row.style.border = "0px";
    row.style.background = "#ffffff";
}

// for a given value returns color which is linear mapped to performance palette
function getColor(min, max, value, reverse) {

    if (typeof reverse == 'undefined')
        reverse = false;

    if (value == 0)
        return "lightgray";
    var normed = (value - min) * (performancePalette.length - 1) / (max - min);
    var index = reverse ? performancePalette.length - normed.toFixed() - 1 : normed.toFixed();

    return performancePalette[index];
}

function arrangeVerticalOffset(elements, minOffset, maxOffset) {

    if (typeof minOffset == 'undefined')
        minOffset = 0;
    if (typeof maxOffset == 'undefined')
        maxOffset = Number.MAX_VALUE;

    var currentTopScroll = $(window).scrollTop();
    var topDelta = currentTopScroll - minOffset;
    if (topDelta < 0)
        topDelta = 0;
    else if (topDelta > maxOffset)
        topDelta = maxOffset;

    elements.each(function () {
        $(this).css("top", topDelta);
    });
};

function arrangeHorizontalOffset(elements, minOffset, maxOffset) {

    if (typeof minOffset == 'undefined')
        minOffset = 0;
    if (typeof maxOffset == 'undefined')
        maxOffset = Number.MAX_VALUE;

    var currentLeftScroll = $(window).scrollLeft();
    var leftDelta = currentLeftScroll - minOffset;

    if (leftDelta < 0)
        leftDelta = 0;
    else if (leftDelta > maxOffset)
        leftDelta = maxOffset;

    elements.each(function () {
        $(this).css("left", leftDelta);
    });
};