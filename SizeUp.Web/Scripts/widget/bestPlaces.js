(function () {
    var me = {};

    me.wigetSource = '/widget/bestPlaces';
    me.defaultColor = '#fff';
    me.defaultWidth = '750';
    me.defaultMinWidth = '750';



    var createIframe = function () {
        var loc = getScriptLocation();
        var iframe = document.createElement("iframe");
        var script = getThisScript();
        iframe["frameBorder"] = "0";
        iframe["border"] = "0";
        iframe.style["border"] = "0";
        iframe.style["background"] = loc.query['color'] || me.defaultColor;
        iframe.style["overflow"] = 'hidden';
        iframe.style["background"] = loc.query['color'] || me.defaultColor;
        iframe.style["overflow"] = 'hidden';

        iframe["width"] = (loc.query['width'] || me.defaultWidth) + 'px';
        iframe.style["min-width"] = (me.defaultMinWidth) + 'px';
        iframe.style["minWidth"] = (me.defaultMinWidth) + 'px';

        iframe.style["display"] = 'block';
        iframe.id = "sizeup_iframe";
        iframe["scrolling"] = 'no';
        var hash = script.src.substring(script.src.indexOf('#') + 1, script.src.length);
        var src = loc.protocol + '://' + loc.host + (loc.port ? ':' + loc.port : '') + me.wigetSource + '/' + loc.hash['industry'] + '#' + hash;
        iframe.src = src;
        script.parentNode.insertBefore(iframe, script.parentNode.firstChild);
        me.iframe = iframe;
    };

    var createCopyright = function () {
        var script = getThisScript();
        var loc = getScriptLocation();
        var children;
        var span = null;
        var anchor = null;
        children = script.parentNode.childNodes;
        for (var x = 0; x < children.length; x++) {
            if (children[x].nodeName.toLowerCase() == 'span') {
                span = children[x];
            }
        }

        children = span.childNodes;
        for (var x = 0; x < children.length; x++) {
            if (children[x].nodeName.toLowerCase() == 'a') {
                anchor = children[x];
            }
        }

        var textNode = document.createTextNode("Copyright © ");
        span.insertBefore(textNode, anchor);
        textNode = document.createTextNode(" Patent Pending.");
        span.appendChild(textNode);

        var terms = document.createElement("a");
        terms.setAttribute("href", loc.protocol + '://' + loc.host + (loc.port ? ':' + loc.port : '') + "/product/terms/");
        terms.setAttribute("target", "_blank");
        terms.innerHTML = "Terms of Use";
        span.appendChild(terms);

        span.style["width"] = ((loc.query['width'] || me.defaultWidth) - 10) + 'px';
        span.style["height"] = '20px';
        span.style["backgroundColor"] = '#F8F8F8';
        span.style["borderTop"] = '2px solid #EEEEEE';
        span.style["padding"] = '5px';
        span.style["textAlign"] = 'right';
        span.style["fontSize"] = '12px';
        span.style["fontFamily"] = '"Trebuchet MS",Arial, Verdana, Sans-Serif';
        span.style["color"] = '#606060';
        span.style["display"] = 'block';

        anchor.style["textDecoration"] = 'none';
        anchor.style["color"] = '#e78500';

        terms.style["textDecoration"] = 'none';
        terms.style["color"] = '#e78500';
        terms.style["margin"] = '0 0 0 5px';

        me.footer = span;
    };

    var getThisScript = function () {
        // get latest script added to the page (this script)
        var scripts = document.getElementsByTagName("script");
        var index = scripts.length - 1;
        return scripts[index];
    };

    var getScriptLocation = function () {
        var script = getThisScript();
        var src = script.src;
        var chop = function (str) {
            var hash = {};
            str = str.replace(' ', '');
            if (str.length > 0) {
                var vars = str.split("&");
                for (var i = 0; i < vars.length; i++) {
                    var pair = vars[i].split("=");
                    hash[pair[0]] = unescape(pair[1]);
                }
            }
            return hash;
        };
        var match = src.match(/^((https?|ftp):\/)?\/?((.*?)(:(.*?)|)@)?((www\.)?([^:\/\s]+))(:([^\/]*))?((\/\w+)*\/)([-\w.]+[^#?\s]*)?(\?([^#]*))?(#(.*))?$/);
        match = match || new Array(20);
        var ret = {
            protocol: match[2],
            host: match[7],
            domain: match[9],
            port: match[11],
            path: match[12] + match[14],
            query: chop(match[16] ? match[16] : ''),
            hash: chop(match[18] ? match[18] : '')
        };
        return ret;
    };

    var receiveMessage = function (e) {
        var obj = {};
        var msg = new String(e.data).split(':');
        var tokens;
        if (msg[0] == 'resizeIframe') {
            obj = parseTokens(msg[1]);
            resize(obj);
        }
    };

    var parseTokens = function (msg) {
        var obj = {};
        var tokens = msg.split('&');
        for (var x in tokens) {
            var t = tokens[x].split('=');
            obj[t[0]] = decodeURI(t[1]);
        }
        return obj;
    };

    var resize = function(obj){
        me.iframe.width = obj.width;
        me.iframe.height = obj.height;
        me.footer.style["width"] = (obj.width - 10) + 'px';
    };

    var init = function () {
        createCopyright();
        createIframe();

        if (window.addEventListener) {
            window.addEventListener("message", receiveMessage);
        }
        if (window.attachEvent) {
            window.attachEvent("onmessage", receiveMessage);
        }
    };
    init();
})();









