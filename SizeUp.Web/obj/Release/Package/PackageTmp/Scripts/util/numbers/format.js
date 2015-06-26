
(function () {
    sizeup.core.namespace('sizeup.util.numbers.format');
    sizeup.util.numbers.format =  {
        addCommas: function(number) {
            number += '';
            x = number.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        },
        percentage: function (number, places) {
            if (places == null || typeof places == 'undefined') {
                places = 0;
            }
            return this.round(number, places).toFixed(places) + '%';
        },
        round: function(num, dec) {
            var result = Math.round(num*Math.pow(10,dec))/Math.pow(10,dec);
            return result;
        },
        ordinal:function(num)
        {
            var n = num % 100;
            var suff = ["th", "st", "nd", "rd", "th"]; // suff for suffix
            var ord= n<21?(n<4 ? suff[n]:suff[0]): (n%10>4 ? suff[0] : suff[n%10]);
            return this.addCommas(num) + ord;
        },
        sigFig: function (num, sigFigs) {
            if (sigFigs == null || typeof sigFigs == 'undefined') {
                sigFigs = 1;
            }
            var n = new Number(num);
            return n.toPrecision(sigFigs);
        },
        abbreviate: function (num, places) {
            if (places == null || typeof places == 'undefined') {
                places = 1;
            }
            num = Math.round(num);

            if (num < 10) {
                return Math.round(num);
            }
            else if (num >= 10 && num <= 9999) {
                return this.addCommas(num);
            }
            else if (num >= 10000 && num <= 999999) {
                var n = num / 1000;
                n = this.round(n, places);
                return n + "K";
            }
            else if (num >= 1000000 && num <= 999999999) {
                var n = num / 1000000;
                n = this.round(n, places);
                return n + "M";
            }
            else if (num >= 1000000000 && num <= 999999999999) {
                var n = num / 1000000000;
                n = this.round(n, places);
                return n + "B";
            }
            else if (num >= 1000000000000) {
                var n = num / 1000000000000;
                n = this.round(n, places);
                return n + "T";
            }
        }
    };
})();