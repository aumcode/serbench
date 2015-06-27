function numberWithCommas(x, round) {
    if (typeof round == 'undefined')
        round = false;
    if (round)
        x = x.toFixed(0);

    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
};

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