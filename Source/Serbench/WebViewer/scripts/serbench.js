function numberWithCommas(x, round) {
    if (round)
        x = x.toFixed(0);

    var parts = x.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
};