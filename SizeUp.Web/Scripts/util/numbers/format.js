
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
            return num + ord;
        },
        abbreviate: function (num) {
            if (num < 10) {
                return this.addCommas(pValue.toFixed(1));
            }
            else if (num >= 10 && num < 10000) {
                return this.addCommas(Math.round(num));
            }
            else if (num >= 10000 && num < 100000) {
                return this.addCommas((num / 1000).toFixed(1)) + "K";
            }
            else if (num >= 100000 && num < 1000000) {
                return this.addCommas(Math.round(num / 1000)) + "K";
            }
            else if (num >= 1000000 && num < 1000000000) {
                return this.addCommas((num / 1000000).toFixed(1)) + "M";
            }
            else if (num >= 1000000000) {
                return this.addCommas((num / 1000000000).toFixed(1)) + "B";
            }
        }
    };
})();