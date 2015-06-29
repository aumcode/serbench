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



// ******************************** Data aggregate functions *******************************

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

    // if there are many tests, calculate avarage data
    var count = benchTests.wCount();
    if (count > 1) {
        benchTests.wSkip(1).wEach(function (t) {
            benchTest.PayloadSize += t.PayloadSize;
            benchTest.SerExceptions += t.SerExceptions;
            benchTest.SerAborts += t.SerAborts;
            benchTest.SerDurationMs += t.SerDurationMs;
            benchTest.SerOpsSec += t.SerOpsSec;
            benchTest.DeserExceptions += t.DeserExceptions;
            benchTest.DeserAborts += t.DeserAborts;
            benchTest.DeserDurationMs += t.DeserDurationMs;
            benchTest.DeserOpsSec += t.DeserOpsSec;
        });
        benchTest.SerDurationMs = benchTest.SerDurationTicks / 1000;
        benchTest.SerOpsSec /= count;
        benchTest.DeserDurationMs = benchTest.DeserDurationTicks / 1000;
        benchTest.DeserOpsSec /= count;
    }
    benchTest.runCount = count;
    benchTest.speed = parseFloat(benchTest.SerOpsSec) < parseFloat(benchTest.DeserOpsSec) ? benchTest.SerOpsSec : benchTest.DeserOpsSec;

    return benchTest;
};
                    
// returns information about best serializers for a given test (by speed and payload)
function createTestSummary(testName, testType, cellDatas){

    var payloadOrdered = WAVE.arrayWalkable(cellDatas).wWhere(function (d) { return d !== null && d.PayloadSize != 0; })
                                                      .wOrder(function (a, b) { return a.PayloadSize > b.PayloadSize ? 1 : a.PayloadSize < b.PayloadSize ? -1 : 0; })
                                                      .wToArray();
    var goldPayloadTest = payloadOrdered.length == 0 ? null : payloadOrdered[0];
    var silverPayloadTest = payloadOrdered.length == 0 ? null :
                        payloadOrdered.length == 1 ? payloadOrdered[0] : payloadOrdered[1];
    var bronzePayloadTest = payloadOrdered.length == 0 ? null :
                        payloadOrdered.length == 1 ? payloadOrdered[0] :
                        payloadOrdered.length == 2 ? payloadOrdered[1] : payloadOrdered[2];

    var speedOrdered = WAVE.arrayWalkable(cellDatas).wWhere(function (d) { return d !== null && d.speed != 0; })
                                                    .wOrder(function (a, b) { return a.speed < b.speed ? 1 : a.speed > b.speed ? -1 : 0; })
                                                    .wToArray();
    var goldSpeedTest = speedOrdered.length == 0 ? null : speedOrdered[0];
    var silverSpeedTest = speedOrdered.length == 0 ? null :
                      speedOrdered.length == 1 ? speedOrdered[0] : speedOrdered[1];
    var bronzeSpeedTest = speedOrdered.length == 0 ? null :
                      speedOrdered.length == 1 ? speedOrdered[0] :
                      speedOrdered.length == 2 ? speedOrdered[1] : speedOrdered[2];

    var htmlTemplate =
        "<div class='row-header'>" +
            "<b>@testName@</b><br>@testType@<br><br>" +
            "<b>by speed:</b><br>" +
            "1. @goldSpeedTest@ (@goldSpeed@ ops/sec)<br>" +
            "2. @silverSpeedTest@ (@silverSpeed@ ops/sec)<br>" +
            "3. @bronzeSpeedTest@ (@bronzeSpeed@ ops/sec)<br><br>" +
            "<b>by payload:</b><br>" +
            "1. @goldPayloadTest@ (@goldPayload@ byte(s))<br>" +
            "2. @silverPayloadTest@ (@silverPayload@ byte(s))<br>" +
            "3. @bronzePayloadTest@ (@bronzePayload@ byte(s))<br>" +
        "<div>";

    var html = WAVE.strHTMLTemplate(htmlTemplate,
        {
            testType: testType,
            testName: testName,
            goldSpeedTest: goldSpeedTest.SerializerName,
            silverSpeedTest: silverSpeedTest.SerializerName,
            bronzeSpeedTest: bronzeSpeedTest.SerializerName,
            goldPayloadTest: goldPayloadTest.SerializerName,
            silverPayloadTest: silverPayloadTest.SerializerName,
            bronzePayloadTest: bronzePayloadTest.SerializerName,
            goldSpeed: numberWithCommas(goldSpeedTest.speed, 0),
            silverSpeed: numberWithCommas(silverSpeedTest.speed, 0),
            bronzeSpeed: numberWithCommas(bronzeSpeedTest.speed, 0),
            goldPayload: numberWithCommas(goldPayloadTest.PayloadSize, 0),
            silverPayload: numberWithCommas(silverPayloadTest.PayloadSize, 0),
            bronzePayload: numberWithCommas(bronzePayloadTest.PayloadSize, 0)
        });

    return html;
}

// returns performance boundaries for a given set of tests 
function getDataSummary(cellDatas){

    var speedMin = null;
    var speedMax = null;
    var payloadMin = null;
    var payloadMax = null;

    WAVE.arrayWalkable(cellDatas).wEach(function (d) {
        
        if (d == null || d.PayloadSize == 0 || d.speed == 0)
            return;

        var speed = parseFloat(d.SerOpsSec) < parseFloat(d.DeserOpsSec) ? d.SerOpsSec : d.DeserOpsSec;
        if (speedMin == null || speedMin > speed)
            speedMin = speed;
        if (speedMax == null || speedMax < speed)
            speedMax = speed;
        if (payloadMin == null || payloadMin > d.PayloadSize)
            payloadMin = d.PayloadSize;
        if (payloadMax == null || payloadMax < d.PayloadSize)
            payloadMax = d.PayloadSize;
    });

    return { speedMin: speedMin, speedMax: speedMax, payloadMin: payloadMin, payloadMax: payloadMax };
}

// for a given value returns color which is linear mapped to performance palette
function getColor(min, max, value, reverse) {

    if (value == 0)
        return "lightgray";
    var normed = (value - min) * (performancePalette.length - 1) / (max - min);
    var index = reverse ? performancePalette.length - normed.toFixed() - 1 : normed.toFixed();

    return performancePalette[index];
}