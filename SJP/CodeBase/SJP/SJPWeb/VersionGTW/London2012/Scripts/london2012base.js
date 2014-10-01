var swfobject = function () {
        var D = "undefined",
            r = "object",
            S = "Shockwave Flash",
            W = "ShockwaveFlash.ShockwaveFlash",
            q = "application/x-shockwave-flash",
            R = "SWFObjectExprInst",
            x = "onreadystatechange",
            O = window,
            j = document,
            t = navigator,
            T = false,
            U = [h],
            o = [],
            N = [],
            I = [],
            l, Q, E, B, J = false,
            a = false,
            n, G, m = true,
            M = function () {
                var aa = typeof j.getElementById != D && typeof j.getElementsByTagName != D && typeof j.createElement != D,
                    ah = t.userAgent.toLowerCase(),
                    Y = t.platform.toLowerCase(),
                    ae = Y ? /win/.test(Y) : /win/.test(ah),
                    ac = Y ? /mac/.test(Y) : /mac/.test(ah),
                    af = /webkit/.test(ah) ? parseFloat(ah.replace(/^.*webkit\/(\d+(\.\d+)?).*$/, "$1")) : false,
                    X = !+"\v1",
                    ag = [0, 0, 0],
                    ab = null;
                if (typeof t.plugins != D && typeof t.plugins[S] == r) {
                    ab = t.plugins[S].description;
                    if (ab && !(typeof t.mimeTypes != D && t.mimeTypes[q] && !t.mimeTypes[q].enabledPlugin)) {
                        T = true;
                        X = false;
                        ab = ab.replace(/^.*\s+(\S+\s+\S+$)/, "$1");
                        ag[0] = parseInt(ab.replace(/^(.*)\..*$/, "$1"), 10);
                        ag[1] = parseInt(ab.replace(/^.*\.(.*)\s.*$/, "$1"), 10);
                        ag[2] = /[a-zA-Z]/.test(ab) ? parseInt(ab.replace(/^.*[a-zA-Z]+(.*)$/, "$1"), 10) : 0
                    }
                } else {
                    if (typeof O.ActiveXObject != D) {
                        try {
                            var ad = new ActiveXObject(W);
                            if (ad) {
                                ab = ad.GetVariable("$version");
                                if (ab) {
                                    X = true;
                                    ab = ab.split(" ")[1].split(",");
                                    ag = [parseInt(ab[0], 10), parseInt(ab[1], 10), parseInt(ab[2], 10)]
                                }
                            }
                        } catch (Z) {}
                    }
                }
                return {
                    w3: aa,
                    pv: ag,
                    wk: af,
                    ie: X,
                    win: ae,
                    mac: ac
                }
            }(),
            k = function () {
                if (!M.w3) {
                    return
                }
                if ((typeof j.readyState != D && j.readyState == "complete") || (typeof j.readyState == D && (j.getElementsByTagName("body")[0] || j.body))) {
                    f()
                }
                if (!J) {
                    if (typeof j.addEventListener != D) {
                        j.addEventListener("DOMContentLoaded", f, false)
                    }
                    if (M.ie && M.win) {
                        j.attachEvent(x, function () {
                            if (j.readyState == "complete") {
                                j.detachEvent(x, arguments.callee);
                                f()
                            }
                        });
                        if (O == top) {
                            (function () {
                                if (J) {
                                    return
                                }
                                try {
                                    j.documentElement.doScroll("left")
                                } catch (X) {
                                    setTimeout(arguments.callee, 0);
                                    return
                                }
                                f()
                            })()
                        }
                    }
                    if (M.wk) {
                        (function () {
                            if (J) {
                                return
                            }
                            if (!/loaded|complete/.test(j.readyState)) {
                                setTimeout(arguments.callee, 0);
                                return
                            }
                            f()
                        })()
                    }
                    s(f)
                }
            }();

        function f() {
            if (J) {
                return
            }
            try {
                var Z = j.getElementsByTagName("body")[0].appendChild(C("span"));
                Z.parentNode.removeChild(Z)
            } catch (aa) {
                return
            }
            J = true;
            var X = U.length;
            for (var Y = 0; Y < X; Y++) {
                U[Y]()
            }
        }
        function K(X) {
            if (J) {
                X()
            } else {
                U[U.length] = X
            }
        }
        function s(Y) {
            if (typeof O.addEventListener != D) {
                O.addEventListener("load", Y, false)
            } else {
                if (typeof j.addEventListener != D) {
                    j.addEventListener("load", Y, false)
                } else {
                    if (typeof O.attachEvent != D) {
                        i(O, "onload", Y)
                    } else {
                        if (typeof O.onload == "function") {
                            var X = O.onload;
                            O.onload = function () {
                                X();
                                Y()
                            }
                        } else {
                            O.onload = Y
                        }
                    }
                }
            }
        }
        function h() {
            if (T) {
                V()
            } else {
                H()
            }
        }
        function V() {
            var X = j.getElementsByTagName("body")[0];
            var aa = C(r);
            aa.setAttribute("type", q);
            var Z = X.appendChild(aa);
            if (Z) {
                var Y = 0;
                (function () {
                    if (typeof Z.GetVariable != D) {
                        var ab = Z.GetVariable("$version");
                        if (ab) {
                            ab = ab.split(" ")[1].split(",");
                            M.pv = [parseInt(ab[0], 10), parseInt(ab[1], 10), parseInt(ab[2], 10)]
                        }
                    } else {
                        if (Y < 10) {
                            Y++;
                            setTimeout(arguments.callee, 10);
                            return
                        }
                    }
                    X.removeChild(aa);
                    Z = null;
                    H()
                })()
            } else {
                H()
            }
        }
        function H() {
            var ag = o.length;
            if (ag > 0) {
                for (var af = 0; af < ag; af++) {
                    var Y = o[af].id;
                    var ab = o[af].callbackFn;
                    var aa = {
                        success: false,
                        id: Y
                    };
                    if (M.pv[0] > 0) {
                        var ae = c(Y);
                        if (ae) {
                            if (F(o[af].swfVersion) && !(M.wk && M.wk < 312)) {
                                w(Y, true);
                                if (ab) {
                                    aa.success = true;
                                    aa.ref = z(Y);
                                    ab(aa)
                                }
                            } else {
                                if (o[af].expressInstall && A()) {
                                    var ai = {};
                                    ai.data = o[af].expressInstall;
                                    ai.width = ae.getAttribute("width") || "0";
                                    ai.height = ae.getAttribute("height") || "0";
                                    if (ae.getAttribute("class")) {
                                        ai.styleclass = ae.getAttribute("class")
                                    }
                                    if (ae.getAttribute("align")) {
                                        ai.align = ae.getAttribute("align")
                                    }
                                    var ah = {};
                                    var X = ae.getElementsByTagName("param");
                                    var ac = X.length;
                                    for (var ad = 0; ad < ac; ad++) {
                                        if (X[ad].getAttribute("name").toLowerCase() != "movie") {
                                            ah[X[ad].getAttribute("name")] = X[ad].getAttribute("value")
                                        }
                                    }
                                    P(ai, ah, Y, ab)
                                } else {
                                    p(ae);
                                    if (ab) {
                                        ab(aa)
                                    }
                                }
                            }
                        }
                    } else {
                        w(Y, true);
                        if (ab) {
                            var Z = z(Y);
                            if (Z && typeof Z.SetVariable != D) {
                                aa.success = true;
                                aa.ref = Z
                            }
                            ab(aa)
                        }
                    }
                }
            }
        }
        function z(aa) {
            var X = null;
            var Y = c(aa);
            if (Y && Y.nodeName == "OBJECT") {
                if (typeof Y.SetVariable != D) {
                    X = Y
                } else {
                    var Z = Y.getElementsByTagName(r)[0];
                    if (Z) {
                        X = Z
                    }
                }
            }
            return X
        }
        function A() {
            return !a && F("6.0.65") && (M.win || M.mac) && !(M.wk && M.wk < 312)
        }
        function P(aa, ab, X, Z) {
            a = true;
            E = Z || null;
            B = {
                success: false,
                id: X
            };
            var ae = c(X);
            if (ae) {
                if (ae.nodeName == "OBJECT") {
                    l = g(ae);
                    Q = null
                } else {
                    l = ae;
                    Q = X
                }
                aa.id = R;
                if (typeof aa.width == D || (!/%$/.test(aa.width) && parseInt(aa.width, 10) < 310)) {
                    aa.width = "310"
                }
                if (typeof aa.height == D || (!/%$/.test(aa.height) && parseInt(aa.height, 10) < 137)) {
                    aa.height = "137"
                }
                j.title = j.title.slice(0, 47) + " - Flash Player Installation";
                var ad = M.ie && M.win ? "ActiveX" : "PlugIn",
                    ac = "MMredirectURL=" + O.location.toString().replace(/&/g, "%26") + "&MMplayerType=" + ad + "&MMdoctitle=" + j.title;
                if (typeof ab.flashvars != D) {
                    ab.flashvars += "&" + ac
                } else {
                    ab.flashvars = ac
                }
                if (M.ie && M.win && ae.readyState != 4) {
                    var Y = C("div");
                    X += "SWFObjectNew";
                    Y.setAttribute("id", X);
                    ae.parentNode.insertBefore(Y, ae);
                    ae.style.display = "none";
                    (function () {
                        if (ae.readyState == 4) {
                            ae.parentNode.removeChild(ae)
                        } else {
                            setTimeout(arguments.callee, 10)
                        }
                    })()
                }
                u(aa, ab, X)
            }
        }
        function p(Y) {
            if (M.ie && M.win && Y.readyState != 4) {
                var X = C("div");
                Y.parentNode.insertBefore(X, Y);
                X.parentNode.replaceChild(g(Y), X);
                Y.style.display = "none";
                (function () {
                    if (Y.readyState == 4) {
                        Y.parentNode.removeChild(Y)
                    } else {
                        setTimeout(arguments.callee, 10)
                    }
                })()
            } else {
                Y.parentNode.replaceChild(g(Y), Y)
            }
        }
        function g(ab) {
            var aa = C("div");
            if (M.win && M.ie) {
                aa.innerHTML = ab.innerHTML
            } else {
                var Y = ab.getElementsByTagName(r)[0];
                if (Y) {
                    var ad = Y.childNodes;
                    if (ad) {
                        var X = ad.length;
                        for (var Z = 0; Z < X; Z++) {
                            if (!(ad[Z].nodeType == 1 && ad[Z].nodeName == "PARAM") && !(ad[Z].nodeType == 8)) {
                                aa.appendChild(ad[Z].cloneNode(true))
                            }
                        }
                    }
                }
            }
            return aa
        }
        function u(ai, ag, Y) {
            var X, aa = c(Y);
            if (M.wk && M.wk < 312) {
                return X
            }
            if (aa) {
                if (typeof ai.id == D) {
                    ai.id = Y
                }
                if (M.ie && M.win) {
                    var ah = "";
                    for (var ae in ai) {
                        if (ai[ae] != Object.prototype[ae]) {
                            if (ae.toLowerCase() == "data") {
                                ag.movie = ai[ae]
                            } else {
                                if (ae.toLowerCase() == "styleclass") {
                                    ah += ' class="' + ai[ae] + '"'
                                } else {
                                    if (ae.toLowerCase() != "classid") {
                                        ah += " " + ae + '="' + ai[ae] + '"'
                                    }
                                }
                            }
                        }
                    }
                    var af = "";
                    for (var ad in ag) {
                        if (ag[ad] != Object.prototype[ad]) {
                            af += '<param name="' + ad + '" value="' + ag[ad] + '" />'
                        }
                    }
                    aa.outerHTML = '<object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"' + ah + ">" + af + "</object>";
                    N[N.length] = ai.id;
                    X = c(ai.id)
                } else {
                    var Z = C(r);
                    Z.setAttribute("type", q);
                    for (var ac in ai) {
                        if (ai[ac] != Object.prototype[ac]) {
                            if (ac.toLowerCase() == "styleclass") {
                                Z.setAttribute("class", ai[ac])
                            } else {
                                if (ac.toLowerCase() != "classid") {
                                    Z.setAttribute(ac, ai[ac])
                                }
                            }
                        }
                    }
                    for (var ab in ag) {
                        if (ag[ab] != Object.prototype[ab] && ab.toLowerCase() != "movie") {
                            e(Z, ab, ag[ab])
                        }
                    }
                    aa.parentNode.replaceChild(Z, aa);
                    X = Z
                }
            }
            return X
        }
        function e(Z, X, Y) {
            var aa = C("param");
            aa.setAttribute("name", X);
            aa.setAttribute("value", Y);
            Z.appendChild(aa)
        }
        function y(Y) {
            var X = c(Y);
            if (X && X.nodeName == "OBJECT") {
                if (M.ie && M.win) {
                    X.style.display = "none";
                    (function () {
                        if (X.readyState == 4) {
                            b(Y)
                        } else {
                            setTimeout(arguments.callee, 10)
                        }
                    })()
                } else {
                    X.parentNode.removeChild(X)
                }
            }
        }
        function b(Z) {
            var Y = c(Z);
            if (Y) {
                for (var X in Y) {
                    if (typeof Y[X] == "function") {
                        Y[X] = null
                    }
                }
                Y.parentNode.removeChild(Y)
            }
        }
        function c(Z) {
            var X = null;
            try {
                X = j.getElementById(Z)
            } catch (Y) {}
            return X
        }
        function C(X) {
            return j.createElement(X)
        }
        function i(Z, X, Y) {
            Z.attachEvent(X, Y);
            I[I.length] = [Z, X, Y]
        }
        function F(Z) {
            var Y = M.pv,
                X = Z.split(".");
            X[0] = parseInt(X[0], 10);
            X[1] = parseInt(X[1], 10) || 0;
            X[2] = parseInt(X[2], 10) || 0;
            return (Y[0] > X[0] || (Y[0] == X[0] && Y[1] > X[1]) || (Y[0] == X[0] && Y[1] == X[1] && Y[2] >= X[2])) ? true : false
        }
        function v(ac, Y, ad, ab) {
            if (M.ie && M.mac) {
                return
            }
            var aa = j.getElementsByTagName("head")[0];
            if (!aa) {
                return
            }
            var X = (ad && typeof ad == "string") ? ad : "screen";
            if (ab) {
                n = null;
                G = null
            }
            if (!n || G != X) {
                var Z = C("style");
                Z.setAttribute("type", "text/css");
                Z.setAttribute("media", X);
                n = aa.appendChild(Z);
                if (M.ie && M.win && typeof j.styleSheets != D && j.styleSheets.length > 0) {
                    n = j.styleSheets[j.styleSheets.length - 1]
                }
                G = X
            }
            if (M.ie && M.win) {
                if (n && typeof n.addRule == r) {
                    n.addRule(ac, Y)
                }
            } else {
                if (n && typeof j.createTextNode != D) {
                    n.appendChild(j.createTextNode(ac + " {" + Y + "}"))
                }
            }
        }
        function w(Z, X) {
            if (!m) {
                return
            }
            var Y = X ? "visible" : "hidden";
            if (J && c(Z)) {
                c(Z).style.visibility = Y
            } else {
                v("#" + Z, "visibility:" + Y)
            }
        }
        function L(Y) {
            var Z = /[\\\"<>\.;]/;
            var X = Z.exec(Y) != null;
            return X && typeof encodeURIComponent != D ? encodeURIComponent(Y) : Y
        }
        var d = function () {
                if (M.ie && M.win) {
                    window.attachEvent("onunload", function () {
                        var ac = I.length;
                        for (var ab = 0; ab < ac; ab++) {
                            I[ab][0].detachEvent(I[ab][1], I[ab][2])
                        }
                        var Z = N.length;
                        for (var aa = 0; aa < Z; aa++) {
                            y(N[aa])
                        }
                        for (var Y in M) {
                            M[Y] = null
                        }
                        M = null;
                        for (var X in swfobject) {
                            swfobject[X] = null
                        }
                        swfobject = null
                    })
                }
            }();
        return {
            registerObject: function (ab, X, aa, Z) {
                if (M.w3 && ab && X) {
                    var Y = {};
                    Y.id = ab;
                    Y.swfVersion = X;
                    Y.expressInstall = aa;
                    Y.callbackFn = Z;
                    o[o.length] = Y;
                    w(ab, false)
                } else {
                    if (Z) {
                        Z({
                            success: false,
                            id: ab
                        })
                    }
                }
            },
            getObjectById: function (X) {
                if (M.w3) {
                    return z(X)
                }
            },
            embedSWF: function (ab, ah, ae, ag, Y, aa, Z, ad, af, ac) {
                var X = {
                    success: false,
                    id: ah
                };
                if (M.w3 && !(M.wk && M.wk < 312) && ab && ah && ae && ag && Y) {
                    w(ah, false);
                    K(function () {
                        ae += "";
                        ag += "";
                        var aj = {};
                        if (af && typeof af === r) {
                            for (var al in af) {
                                aj[al] = af[al]
                            }
                        }
                        aj.data = ab;
                        aj.width = ae;
                        aj.height = ag;
                        var am = {};
                        if (ad && typeof ad === r) {
                            for (var ak in ad) {
                                am[ak] = ad[ak]
                            }
                        }
                        if (Z && typeof Z === r) {
                            for (var ai in Z) {
                                if (typeof am.flashvars != D) {
                                    am.flashvars += "&" + ai + "=" + Z[ai]
                                } else {
                                    am.flashvars = ai + "=" + Z[ai]
                                }
                            }
                        }
                        if (F(Y)) {
                            var an = u(aj, am, ah);
                            if (aj.id == ah) {
                                w(ah, true)
                            }
                            X.success = true;
                            X.ref = an
                        } else {
                            if (aa && A()) {
                                aj.data = aa;
                                P(aj, am, ah, ac);
                                return
                            } else {
                                w(ah, true)
                            }
                        }
                        if (ac) {
                            ac(X)
                        }
                    })
                } else {
                    if (ac) {
                        ac(X)
                    }
                }
            },
            switchOffAutoHideShow: function () {
                m = false
            },
            ua: M,
            getFlashPlayerVersion: function () {
                return {
                    major: M.pv[0],
                    minor: M.pv[1],
                    release: M.pv[2]
                }
            },
            hasFlashPlayerVersion: F,
            createSWF: function (Z, Y, X) {
                if (M.w3) {
                    return u(Z, Y, X)
                } else {
                    return undefined
                }
            },
            showExpressInstall: function (Z, aa, X, Y) {
                if (M.w3 && A()) {
                    P(Z, aa, X, Y)
                }
            },
            removeSWF: function (X) {
                if (M.w3) {
                    y(X)
                }
            },
            createCSS: function (aa, Z, Y, X) {
                if (M.w3) {
                    v(aa, Z, Y, X)
                }
            },
            addDomLoadEvent: K,
            addLoadEvent: s,
            getQueryParamValue: function (aa) {
                var Z = j.location.search || j.location.hash;
                if (Z) {
                    if (/\?/.test(Z)) {
                        Z = Z.split("?")[1]
                    }
                    if (aa == null) {
                        return L(Z)
                    }
                    var Y = Z.split("&");
                    for (var X = 0; X < Y.length; X++) {
                        if (Y[X].substring(0, Y[X].indexOf("=")) == aa) {
                            return L(Y[X].substring((Y[X].indexOf("=") + 1)))
                        }
                    }
                }
                return ""
            },
            expressInstallCallback: function () {
                if (a) {
                    var X = c(R);
                    if (X && l) {
                        X.parentNode.replaceChild(l, X);
                        if (Q) {
                            w(Q, true);
                            if (M.ie && M.win) {
                                l.style.display = "block"
                            }
                        }
                        if (E) {
                            E(B)
                        }
                    }
                    a = false
                }
            }
        }
    }();
(function ($) {
    $.fn.superfish = function (op) {
        var sf = $.fn.superfish,
            c = sf.c,
            $arrow = $(['<span class="', c.arrowClass, '">&#9660;</span>'].join('')),
            over = function () {
                var $$ = $(this),
                    menu = getMenu($$),
                    o = sf.op;
                clearTimeout(menu.sfTimer);
                menu.sfTimer = setTimeout(function () {
                    $$.showSuperfishUl().siblings().hideSuperfishUl();
                }, o.overDelay);
            },
            out = function () {
                var $$ = $(this),
                    menu = getMenu($$),
                    o = sf.op;
                clearTimeout(menu.sfTimer);
                menu.sfTimer = setTimeout(function () {
                    o.retainPath = ($.inArray($$[0], o.$path) > -1);
                    $$.hideSuperfishUl();
                    if (o.$path.length && $$.parents(['li.', o.hoverClass].join('')).length < 1) {
                        over.call(o.$path);
                    }
                }, o.delay);
            },
            getMenu = function ($menu) {
                var menu = $menu.parents(['ul.', c.menuClass, ':first'].join(''))[0];
                sf.op = sf.o[menu.serial];
                return menu;
            },
            addArrow = function ($a) {
                $a.addClass(c.anchorClass).append($arrow.clone());
            };
        return this.each(function () {
            var $ctx = $(this),
                s = this.serial = sf.o.length,
                o = $.extend({}, sf.defaults, op);
            o.$path = $('li.' + o.pathClass, this).slice(0, o.pathLevels).each(function () {
                $(this).addClass([o.hoverClass, c.bcClass].join(' ')).filter('li:has(ul)').removeClass(o.pathClass);
            });
            sf.o[s] = sf.op = o;
            var _menus = $ctx.find('li:has(ul)');
            _menus[($.fn.hoverIntent && !o.disableHI) ? 'hoverIntent' : 'hover'](over, out).each(function () {
                if (o.autoArrows) {
                    addArrow($('>a:first-child', this));
                }
            }).not('.' + c.bcClass).hideSuperfishUl();
            _menus.find('a.sf-with-ul').focus(function () {
                over.call($(this).closest('li'));
            }).blur(function () {
                out.call($(this).closest('li'));
            });
            o.onInit.call(this);
        }).each(function () {
            var menuClasses = [c.menuClass];
            if (sf.op.dropShadows && !($.browser.msie && $.browser.version < 7)) {
                menuClasses.push(c.shadowClass);
            }
            $(this).addClass(menuClasses.join(' '));
        });
    };
    var sf = $.fn.superfish;
    sf.o = [];
    sf.op = {};
    sf.IE7fix = function () {
        var o = sf.op;
        if ($.browser.msie && $.browser.version > 6 && o.dropShadows && o.animation.opacity != undefined) {
            this.toggleClass(sf.c.shadowClass + '-off');
        }
    };
    sf.c = {
        bcClass: 'sf-breadcrumb',
        menuClass: 'sf-js-enabled',
        anchorClass: 'sf-with-ul',
        arrowClass: 'sf-sub-indicator',
        shadowClass: 'sf-shadow'
    };
    sf.defaults = {
        hoverClass: 'sfHover',
        pathClass: 'overideThisToUse',
        pathLevels: 1,
        delay: 300,
        overDelay: 150,
        animation: {
            opacity: 'show'
        },
        speed: 'normal',
        autoArrows: true,
        dropShadows: true,
        disableHI: true,
        onInit: function () {},
        onBeforeShow: function () {},
        onShow: function () {},
        onHide: function () {}
    };
    $.fn.extend({
        hideSuperfishUl: function () {
            var o = sf.op,
                not = (o.retainPath === true) ? o.$path : '';
            o.retainPath = false;
            var $ul = $(['li.', o.hoverClass].join(''), this).add(this).not(not).removeClass(o.hoverClass).find('>div.flyOut').addClass('hide');
			this.find('>div.flyOut').css('display','none'); // ATOS
            o.onHide.call($ul);
            return this;
        },
        showSuperfishUl: function () {
            var o = sf.op,
                sh = sf.c.shadowClass + '-off',
                $ul = this.addClass(o.hoverClass).find('>div.flyOut:hidden').removeClass('hide');
				this.find('>div.flyOut:hidden').css('display','block'); // ATOS
            sf.IE7fix.call($ul);
            o.onBeforeShow.call($ul);
            $ul.animate(o.animation, o.speed, function () {
                sf.IE7fix.call($ul);
                o.onShow.call($ul);
            });
            return this;
        }
    });
}(jQuery));
(function ($) {
    $.extend({
        tablesorter: new

        function () {
            var parsers = [],
                widgets = [];
            this.defaults = {
                cssHeader: "header",
                cssAsc: "headerSortUp",
                cssDesc: "headerSortDown",
                cssChildRow: "expand-child",
                sortInitialOrder: "asc",
                sortMultiSortKey: "shiftKey",
                sortForce: null,
                sortAppend: null,
                sortLocaleCompare: true,
                textExtraction: "simple",
                parsers: {},
                widgets: [],
                widgetZebra: {
                    css: ["even", "odd"]
                },
                headers: {},
                widthFixed: false,
                cancelSelection: true,
                sortList: [],
                headerList: [],
                dateFormat: "us",
                decimal: '/\.|\,/g',
                onRenderHeader: null,
                selectorHeaders: 'thead th',
                debug: false
            };

            function benchmark(s, d) {
                log(s + "," + (new Date().getTime() - d.getTime()) + "ms");
            }
            this.benchmark = benchmark;

            function log(s) {
                if (typeof console != "undefined" && typeof console.debug != "undefined") {
                    console.log(s);
                } else {
                    alert(s);
                }
            }
            function buildParserCache(table, $headers) {
                if (table.config.debug) {
                    var parsersDebug = "";
                }
                if (table.tBodies.length == 0) return;
                var rows = table.tBodies[0].rows;
                if (rows[0]) {
                    var list = [],
                        cells = rows[0].cells,
                        l = cells.length;
                    for (var i = 0; i < l; i++) {
                        var p = false;
                        if ($.metadata && ($($headers[i]).metadata() && $($headers[i]).metadata().sorter)) {
                            p = getParserById($($headers[i]).metadata().sorter);
                        } else if ((table.config.headers[i] && table.config.headers[i].sorter)) {
                            p = getParserById(table.config.headers[i].sorter);
                        }
                        if (!p) {
                            p = detectParserForColumn(table, rows, -1, i);
                        }
                        if (table.config.debug) {
                            parsersDebug += "column:" + i + " parser:" + p.id + "\n";
                        }
                        list.push(p);
                    }
                }
                if (table.config.debug) {
                    log(parsersDebug);
                }
                return list;
            };

            function detectParserForColumn(table, rows, rowIndex, cellIndex) {
                var l = parsers.length,
                    node = false,
                    nodeValue = false,
                    keepLooking = true;
                while (nodeValue == '' && keepLooking) {
                    rowIndex++;
                    if (rows[rowIndex]) {
                        node = getNodeFromRowAndCellIndex(rows, rowIndex, cellIndex);
                        nodeValue = trimAndGetNodeText(table.config, node);
                        if (table.config.debug) {
                            log('Checking if value was empty on row:' + rowIndex);
                        }
                    } else {
                        keepLooking = false;
                    }
                }
                for (var i = 1; i < l; i++) {
                    if (parsers[i].is(nodeValue, table, node)) {
                        return parsers[i];
                    }
                }
                return parsers[0];
            }
            function getNodeFromRowAndCellIndex(rows, rowIndex, cellIndex) {
                return rows[rowIndex].cells[cellIndex];
            }
            function trimAndGetNodeText(config, node) {
                return $.trim(getElementText(config, node));
            }
            function getParserById(name) {
                var l = parsers.length;
                for (var i = 0; i < l; i++) {
                    if (parsers[i].id.toLowerCase() == name.toLowerCase()) {
                        return parsers[i];
                    }
                }
                return false;
            }
            function buildCache(table) {
                if (table.config.debug) {
                    var cacheTime = new Date();
                }
                var totalRows = (table.tBodies[0] && table.tBodies[0].rows.length) || 0,
                    totalCells = (table.tBodies[0].rows[0] && table.tBodies[0].rows[0].cells.length) || 0,
                    parsers = table.config.parsers,
                    cache = {
                        row: [],
                        normalized: []
                    };
                for (var i = 0; i < totalRows; ++i) {
                    var c = $(table.tBodies[0].rows[i]),
                        cols = [];
                    if (c.hasClass(table.config.cssChildRow)) {
                        cache.row[cache.row.length - 1] = cache.row[cache.row.length - 1].add(c);
                        continue;
                    }
                    cache.row.push(c);
                    for (var j = 0; j < totalCells; ++j) {
                        cols.push(parsers[j].format(getElementText(table.config, c[0].cells[j]), table, c[0].cells[j]));
                    }
                    cols.push(cache.normalized.length);
                    cache.normalized.push(cols);
                    cols = null;
                };
                if (table.config.debug) {
                    benchmark("Building cache for " + totalRows + " rows:", cacheTime);
                }
                return cache;
            };

            function getElementText(config, node) {
                var text = "";
                if (!node) return "";
                if (!config.supportsTextContent) config.supportsTextContent = node.textContent || false;
                if (config.textExtraction == "simple") {
                    if (config.supportsTextContent) {
                        text = node.textContent;
                    } else {
                        if (node.childNodes[0] && node.childNodes[0].hasChildNodes()) {
                            text = node.childNodes[0].innerHTML;
                        } else {
                            text = node.innerHTML;
                        }
                    }
                } else {
                    if (typeof (config.textExtraction) == "function") {
                        text = config.textExtraction(node);
                    } else {
                        text = $(node).text();
                    }
                }
                return text;
            }
            function appendToTable(table, cache) {
                if (table.config.debug) {
                    var appendTime = new Date()
                }
                var c = cache,
                    r = c.row,
                    n = c.normalized,
                    totalRows = n.length,
                    checkCell = (n[0].length - 1),
                    tableBody = $(table.tBodies[0]),
                    rows = [];
                for (var i = 0; i < totalRows; i++) {
                    var pos = n[i][checkCell];
                    rows.push(r[pos]);
                    if (!table.config.appender) {
                        var l = r[pos].length;
                        for (var j = 0; j < l; j++) {
                            tableBody[0].appendChild(r[pos][j]);
                        }
                    }
                }
                if (table.config.appender) {
                    table.config.appender(table, rows);
                }
                rows = null;
                if (table.config.debug) {
                    benchmark("Rebuilt table:", appendTime);
                }
                applyWidget(table);
                setTimeout(function () {
                    $(table).trigger("sortEnd");
                }, 0);
            };

            function buildHeaders(table) {
                if (table.config.debug) {
                    var time = new Date();
                }
                var meta = ($.metadata) ? true : false;
                var header_index = computeTableHeaderCellIndexes(table);
                $tableHeaders = $(table.config.selectorHeaders, table).each(function (index) {
                    this.column = header_index[this.parentNode.rowIndex + "-" + this.cellIndex];
                    this.order = formatSortingOrder(table.config.sortInitialOrder);
                    this.count = this.order;
                    if (checkHeaderMetadata(this) || checkHeaderOptions(table, index)) this.sortDisabled = true;
                    if (checkHeaderOptionsSortingLocked(table, index)) this.order = this.lockedOrder = checkHeaderOptionsSortingLocked(table, index);
                    if (!this.sortDisabled) {
                        var $th = $(this).addClass(table.config.cssHeader);
                        if (table.config.onRenderHeader) table.config.onRenderHeader.apply($th);
                    }
                    table.config.headerList[index] = this;
                });
                if (table.config.debug) {
                    benchmark("Built headers:", time);
                    log($tableHeaders);
                }
                return $tableHeaders;
            };

            function computeTableHeaderCellIndexes(t) {
                var matrix = [];
                var lookup = {};
                var thead = t.getElementsByTagName('THEAD')[0];
                var trs = thead.getElementsByTagName('TR');
                for (var i = 0; i < trs.length; i++) {
                    var cells = trs[i].cells;
                    for (var j = 0; j < cells.length; j++) {
                        var c = cells[j];
                        var rowIndex = c.parentNode.rowIndex;
                        var cellId = rowIndex + "-" + c.cellIndex;
                        var rowSpan = c.rowSpan || 1;
                        var colSpan = c.colSpan || 1
                        var firstAvailCol;
                        if (typeof (matrix[rowIndex]) == "undefined") {
                            matrix[rowIndex] = [];
                        }
                        for (var k = 0; k < matrix[rowIndex].length + 1; k++) {
                            if (typeof (matrix[rowIndex][k]) == "undefined") {
                                firstAvailCol = k;
                                break;
                            }
                        }
                        lookup[cellId] = firstAvailCol;
                        for (var k = rowIndex; k < rowIndex + rowSpan; k++) {
                            if (typeof (matrix[k]) == "undefined") {
                                matrix[k] = [];
                            }
                            var matrixrow = matrix[k];
                            for (var l = firstAvailCol; l < firstAvailCol + colSpan; l++) {
                                matrixrow[l] = "x";
                            }
                        }
                    }
                }
                return lookup;
            }
            function checkCellColSpan(table, rows, row) {
                var arr = [],
                    r = table.tHead.rows,
                    c = r[row].cells;
                for (var i = 0; i < c.length; i++) {
                    var cell = c[i];
                    if (cell.colSpan > 1) {
                        arr = arr.concat(checkCellColSpan(table, headerArr, row++));
                    } else {
                        if (table.tHead.length == 1 || (cell.rowSpan > 1 || !r[row + 1])) {
                            arr.push(cell);
                        }
                    }
                }
                return arr;
            };

            function checkHeaderMetadata(cell) {
                if (($.metadata) && ($(cell).metadata().sorter === false)) {
                    return true;
                };
                return false;
            }
            function checkHeaderOptions(table, i) {
                if ((table.config.headers[i]) && (table.config.headers[i].sorter === false)) {
                    return true;
                };
                return false;
            }
            function checkHeaderOptionsSortingLocked(table, i) {
                if ((table.config.headers[i]) && (table.config.headers[i].lockedOrder)) return table.config.headers[i].lockedOrder;
                return false;
            }
            function applyWidget(table) {
                var c = table.config.widgets;
                var l = c.length;
                for (var i = 0; i < l; i++) {
                    getWidgetById(c[i]).format(table);
                }
            }
            function getWidgetById(name) {
                var l = widgets.length;
                for (var i = 0; i < l; i++) {
                    if (widgets[i].id.toLowerCase() == name.toLowerCase()) {
                        return widgets[i];
                    }
                }
            };

            function formatSortingOrder(v) {
                if (typeof (v) != "Number") {
                    return (v.toLowerCase() == "desc") ? 1 : 0;
                } else {
                    return (v == 1) ? 1 : 0;
                }
            }
            function isValueInArray(v, a) {
                var l = a.length;
                for (var i = 0; i < l; i++) {
                    if (a[i][0] == v) {
                        return true;
                    }
                }
                return false;
            }
            function setHeadersCss(table, $headers, list, css) {
                $headers.removeClass(css[0]).removeClass(css[1]);
                var h = [];
                $headers.each(function (offset) {
                    if (!this.sortDisabled) {
                        h[this.column] = $(this);
                    }
                });
                var l = list.length;
                for (var i = 0; i < l; i++) {
                    h[list[i][0]].addClass(css[list[i][1]]);
                }
            }
            function fixColumnWidth(table, $headers) {
                var c = table.config;
                if (c.widthFixed) {
                    var colgroup = $('<colgroup>');
                    $("tr:first td", table.tBodies[0]).each(function () {
                        colgroup.append($('<col>').css('width', $(this).width()));
                    });
                    $(table).prepend(colgroup);
                };
            }
            function updateHeaderSortCount(table, sortList) {
                var c = table.config,
                    l = sortList.length;
                for (var i = 0; i < l; i++) {
                    var s = sortList[i],
                        o = c.headerList[s[0]];
                    o.count = s[1];
                    o.count++;
                }
            }
            function multisort(table, sortList, cache) {
                if (table.config.debug) {
                    var sortTime = new Date();
                }
                var dynamicExp = "var sortWrapper = function(a,b) {",
                    l = sortList.length;
                for (var i = 0; i < l; i++) {
                    var c = sortList[i][0];
                    var order = sortList[i][1];
                    var s = (table.config.parsers[c].type == "text") ? ((order == 0) ? makeSortFunction("text", "asc", c) : makeSortFunction("text", "desc", c)) : ((order == 0) ? makeSortFunction("numeric", "asc", c) : makeSortFunction("numeric", "desc", c));
                    var e = "e" + i;
                    dynamicExp += "var " + e + " = " + s;
                    dynamicExp += "if(" + e + ") { return " + e + "; } ";
                    dynamicExp += "else { ";
                }
                var orgOrderCol = cache.normalized[0].length - 1;
                dynamicExp += "return a[" + orgOrderCol + "]-b[" + orgOrderCol + "];";
                for (var i = 0; i < l; i++) {
                    dynamicExp += "}; ";
                }
                dynamicExp += "return 0; ";
                dynamicExp += "}; ";
                if (table.config.debug) {
                    benchmark("Evaling expression:" + dynamicExp, new Date());
                }
                eval(dynamicExp);
                cache.normalized.sort(sortWrapper);
                if (table.config.debug) {
                    benchmark("Sorting on " + sortList.toString() + " and dir " + order + " time:", sortTime);
                }
                return cache;
            };

            function makeSortFunction(type, direction, index) {
                var a = "a[" + index + "]",
                    b = "b[" + index + "]";
                if (type == 'text' && direction == 'asc') {
                    return "(" + a + " == " + b + " ? 0 : (" + a + " === null ? Number.POSITIVE_INFINITY : (" + b + " === null ? Number.NEGATIVE_INFINITY : (" + a + " < " + b + ") ? -1 : 1 )));";
                } else if (type == 'text' && direction == 'desc') {
                    return "(" + a + " == " + b + " ? 0 : (" + a + " === null ? Number.POSITIVE_INFINITY : (" + b + " === null ? Number.NEGATIVE_INFINITY : (" + b + " < " + a + ") ? -1 : 1 )));";
                } else if (type == 'numeric' && direction == 'asc') {
                    return "(" + a + " === null && " + b + " === null) ? 0 :(" + a + " === null ? Number.POSITIVE_INFINITY : (" + b + " === null ? Number.NEGATIVE_INFINITY : " + a + " - " + b + "));";
                } else if (type == 'numeric' && direction == 'desc') {
                    return "(" + a + " === null && " + b + " === null) ? 0 :(" + a + " === null ? Number.POSITIVE_INFINITY : (" + b + " === null ? Number.NEGATIVE_INFINITY : " + b + " - " + a + "));";
                }
            };

            function makeSortText(i) {
                return "((a[" + i + "] < b[" + i + "]) ? -1 : ((a[" + i + "] > b[" + i + "]) ? 1 : 0));";
            };

            function makeSortTextDesc(i) {
                return "((b[" + i + "] < a[" + i + "]) ? -1 : ((b[" + i + "] > a[" + i + "]) ? 1 : 0));";
            };

            function makeSortNumeric(i) {
                return "a[" + i + "]-b[" + i + "];";
            };

            function makeSortNumericDesc(i) {
                return "b[" + i + "]-a[" + i + "];";
            };

            function sortText(a, b) {
                if (table.config.sortLocaleCompare) return a.localeCompare(b);
                return ((a < b) ? -1 : ((a > b) ? 1 : 0));
            };

            function sortTextDesc(a, b) {
                if (table.config.sortLocaleCompare) return b.localeCompare(a);
                return ((b < a) ? -1 : ((b > a) ? 1 : 0));
            };

            function sortNumeric(a, b) {
                return a - b;
            };

            function sortNumericDesc(a, b) {
                return b - a;
            };

            function getCachedSortType(parsers, i) {
                return parsers[i].type;
            };
            this.construct = function (settings) {
                return this.each(function () {
                    if (!this.tHead || !this.tBodies) return;
                    var $this, $document, $headers, cache, config, shiftDown = 0,
                        sortOrder;
                    this.config = {};
                    config = $.extend(this.config, $.tablesorter.defaults, settings);
                    $this = $(this);
                    $.data(this, "tablesorter", config);
                    $headers = buildHeaders(this);
                    this.config.parsers = buildParserCache(this, $headers);
                    cache = buildCache(this);
                    var sortCSS = [config.cssDesc, config.cssAsc];
                    fixColumnWidth(this);
                    $headers.click(function (e) {
                        var totalRows = ($this[0].tBodies[0] && $this[0].tBodies[0].rows.length) || 0;
                        if (!this.sortDisabled && totalRows > 0) {
                            $this.trigger("sortStart");
                            var $cell = $(this);
                            var i = this.column;
                            this.order = this.count++ % 2;
                            if (this.lockedOrder) this.order = this.lockedOrder;
                            if (!e[config.sortMultiSortKey]) {
                                config.sortList = [];
                                if (config.sortForce != null) {
                                    var a = config.sortForce;
                                    for (var j = 0; j < a.length; j++) {
                                        if (a[j][0] != i) {
                                            config.sortList.push(a[j]);
                                        }
                                    }
                                }
                                config.sortList.push([i, this.order]);
                            } else {
                                if (isValueInArray(i, config.sortList)) {
                                    for (var j = 0; j < config.sortList.length; j++) {
                                        var s = config.sortList[j],
                                            o = config.headerList[s[0]];
                                        if (s[0] == i) {
                                            o.count = s[1];
                                            o.count++;
                                            s[1] = o.count % 2;
                                        }
                                    }
                                } else {
                                    config.sortList.push([i, this.order]);
                                }
                            };
                            setTimeout(function () {
                                setHeadersCss($this[0], $headers, config.sortList, sortCSS);
                                appendToTable($this[0], multisort($this[0], config.sortList, cache));
                            }, 1);
                            return false;
                        }
                    }).mousedown(function () {
                        if (config.cancelSelection) {
                            this.onselectstart = function () {
                                return false
                            };
                            return false;
                        }
                    });
                    $this.bind("update", function () {
                        var me = this;
                        setTimeout(function () {
                            me.config.parsers = buildParserCache(me, $headers);
                            cache = buildCache(me);
                        }, 1);
                    }).bind("updateCell", function (e, cell) {
                        var config = this.config;
                        var pos = [(cell.parentNode.rowIndex - 1), cell.cellIndex];
                        cache.normalized[pos[0]][pos[1]] = config.parsers[pos[1]].format(getElementText(config, cell), cell);
                    }).bind("sorton", function (e, list) {
                        $(this).trigger("sortStart");
                        config.sortList = list;
                        var sortList = config.sortList;
                        updateHeaderSortCount(this, sortList);
                        setHeadersCss(this, $headers, sortList, sortCSS);
                        appendToTable(this, multisort(this, sortList, cache));
                    }).bind("appendCache", function () {
                        appendToTable(this, cache);
                    }).bind("applyWidgetId", function (e, id) {
                        getWidgetById(id).format(this);
                    }).bind("applyWidgets", function () {
                        applyWidget(this);
                    });
                    if ($.metadata && ($(this).metadata() && $(this).metadata().sortlist)) {
                        config.sortList = $(this).metadata().sortlist;
                    }
                    if (config.sortList.length > 0) {
                        $this.trigger("sorton", [config.sortList]);
                    }
                    applyWidget(this);
                });
            };
            this.addParser = function (parser) {
                var l = parsers.length,
                    a = true;
                for (var i = 0; i < l; i++) {
                    if (parsers[i].id.toLowerCase() == parser.id.toLowerCase()) {
                        a = false;
                    }
                }
                if (a) {
                    parsers.push(parser);
                };
            };
            this.addWidget = function (widget) {
                widgets.push(widget);
            };
            this.formatFloat = function (s) {
                var i = parseFloat(s);
                return (isNaN(i)) ? 0 : i;
            };
            this.formatInt = function (s) {
                var i = parseInt(s);
                return (isNaN(i)) ? 0 : i;
            };
            this.isDigit = function (s, config) {
                return /^[-+]?\d*$/.test($.trim(s.replace(/[,.']/g, '')));
            };
            this.clearTableBody = function (table) {
                if ($.browser.msie) {
                    function empty() {
                        while (this.firstChild) this.removeChild(this.firstChild);
                    }
                    empty.apply(table.tBodies[0]);
                } else {
                    table.tBodies[0].innerHTML = "";
                }
            };
        }
    });
    $.fn.extend({
        tablesorter: $.tablesorter.construct
    });
    var ts = $.tablesorter;
    ts.addParser({
        id: "text",
        is: function (s) {
            return true;
        },
        format: function (s) {
            return $.trim(s.toLocaleLowerCase());
        },
        type: "text"
    });
    ts.addParser({
        id: "digit",
        is: function (s, table) {
            var c = table.config;
            return $.tablesorter.isDigit(s, c);
        },
        format: function (s) {
            return $.tablesorter.formatFloat(s);
        },
        type: "numeric"
    });
    ts.addParser({
        id: "currency",
        is: function (s) {
            return /^[£$€?.]/.test(s);
        },
        format: function (s) {
            return $.tablesorter.formatFloat(s.replace(new RegExp(/[£$€]/g), ""));
        },
        type: "numeric"
    });
    ts.addParser({
        id: "ipAddress",
        is: function (s) {
            return /^\d{2,3}[\.]\d{2,3}[\.]\d{2,3}[\.]\d{2,3}$/.test(s);
        },
        format: function (s) {
            var a = s.split("."),
                r = "",
                l = a.length;
            for (var i = 0; i < l; i++) {
                var item = a[i];
                if (item.length == 2) {
                    r += "0" + item;
                } else {
                    r += item;
                }
            }
            return $.tablesorter.formatFloat(r);
        },
        type: "numeric"
    });
    ts.addParser({
        id: "url",
        is: function (s) {
            return /^(https?|ftp|file):\/\/$/.test(s);
        },
        format: function (s) {
            return jQuery.trim(s.replace(new RegExp(/(https?|ftp|file):\/\//), ''));
        },
        type: "text"
    });
    ts.addParser({
        id: "isoDate",
        is: function (s) {
            return /^\d{4}[\/-]\d{1,2}[\/-]\d{1,2}$/.test(s);
        },
        format: function (s) {
            return $.tablesorter.formatFloat((s != "") ? new Date(s.replace(new RegExp(/-/g), "/")).getTime() : "0");
        },
        type: "numeric"
    });
    ts.addParser({
        id: "percent",
        is: function (s) {
            return /\%$/.test($.trim(s));
        },
        format: function (s) {
            return $.tablesorter.formatFloat(s.replace(new RegExp(/%/g), ""));
        },
        type: "numeric"
    });
    ts.addParser({
        id: "usLongDate",
        is: function (s) {
            return s.match(new RegExp(/^[A-Za-z]{3,10}\.? [0-9]{1,2}, ([0-9]{4}|'?[0-9]{2}) (([0-2]?[0-9]:[0-5][0-9])|([0-1]?[0-9]:[0-5][0-9]\s(AM|PM)))$/));
        },
        format: function (s) {
            return $.tablesorter.formatFloat(new Date(s).getTime());
        },
        type: "numeric"
    });
    ts.addParser({
        id: "shortDate",
        is: function (s) {
            return /\d{1,2}[\/\-]\d{1,2}[\/\-]\d{2,4}/.test(s);
        },
        format: function (s, table) {
            var c = table.config;
            s = s.replace(/\-/g, "/");
            if (c.dateFormat == "us") {
                s = s.replace(/(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{4})/, "$3/$1/$2");
            } else if (c.dateFormat == "uk") {
                s = s.replace(/(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{4})/, "$3/$2/$1");
            } else if (c.dateFormat == "dd/mm/yy" || c.dateFormat == "dd-mm-yy") {
                s = s.replace(/(\d{1,2})[\/\-](\d{1,2})[\/\-](\d{2})/, "$1/$2/$3");
            }
            return $.tablesorter.formatFloat(new Date(s).getTime());
        },
        type: "numeric"
    });
    ts.addParser({
        id: "time",
        is: function (s) {
            return /^(([0-2]?[0-9]:[0-5][0-9])|([0-1]?[0-9]:[0-5][0-9]\s(am|pm)))$/.test(s);
        },
        format: function (s) {
            return $.tablesorter.formatFloat(new Date("2000/01/01 " + s).getTime());
        },
        type: "numeric"
    });
    ts.addParser({
        id: "metadata",
        is: function (s) {
            return false;
        },
        format: function (s, table, cell) {
            var c = table.config,
                p = (!c.parserMetadataName) ? 'sortValue' : c.parserMetadataName;
            return $(cell).metadata()[p];
        },
        type: "numeric"
    });
    ts.addWidget({
        id: "zebra",
        format: function (table) {
            if (table.config.debug) {
                var time = new Date();
            }
            var $tr, row = -1,
                odd;
            $("tr:visible", table.tBodies[0]).each(function (i) {
                $tr = $(this);
                if (!$tr.hasClass(table.config.cssChildRow)) row++;
                odd = (row % 2 == 0);
                $tr.removeClass(table.config.widgetZebra.css[odd ? 0 : 1]).addClass(table.config.widgetZebra.css[odd ? 1 : 0])
            });
            if (table.config.debug) {
                $.tablesorter.benchmark("Applying Zebra widget", time);
            }
        }
    });
})(jQuery);

;
(function ($) {
    $.ui = $.ui || {};
    $.fn.extend({
        accordion: function (options, data) {
            var args = Array.prototype.slice.call(arguments, 1);
            return this.each(function () {
                if (typeof options == "string") {
                    var accordion = $.data(this, "ui-accordion");
                    accordion[options].apply(accordion, args);
                } else if (!$(this).is(".ui-accordion")) $.data(this, "ui-accordion", new $.ui.accordion(this, options));
            });
        },
        activate: function (index) {
            return this.accordion("activate", index);
        }
    });
    $.ui.accordion = function (container, options) {
        this.options = options = $.extend({}, $.ui.accordion.defaults, options);
        this.element = container;
        $(container).addClass("ui-accordion");
        if (options.navigation) {
            var current = $(container).find("a").filter(options.navigationFilter);
            if (current.length) {
                if (current.filter(options.header).length) {
                    options.active = current;
                } else {
                    options.active = current.parent().parent().prev();
                    current.addClass("current");
                }
            }
        }
        options.headers = $(container).find(options.header);
        options.active = findActive(options.headers, options.active);
        if (options.fillSpace) {
            var maxHeight = $(container).parent().height();
            options.headers.each(function () {
                maxHeight -= $(this).outerHeight();
            });
            var maxPadding = 0;
            options.headers.next().each(function () {
                maxPadding = Math.max(maxPadding, $(this).innerHeight() - $(this).height());
            }).height(maxHeight - maxPadding);
        } else if (options.autoheight) {
            var maxHeight = 0;
            options.headers.next().each(function () {
                maxHeight = Math.max(maxHeight, $(this).outerHeight());
            }).height(maxHeight);
        }
        options.headers.not(options.active || "").next().hide();
        options.active.parent().andSelf().addClass(options.selectedClass);
        if (options.event) $(container).bind((options.event) + ".ui-accordion", clickHandler);
    };
    $.ui.accordion.prototype = {
        activate: function (index) {
            clickHandler.call(this.element, {
                target: findActive(this.options.headers, index)[0]
            });
        },
        enable: function () {
            this.options.disabled = false;
        },
        disable: function () {
            this.options.disabled = true;
        },
        destroy: function () {
            this.options.headers.next().css("display", "");
            if (this.options.fillSpace || this.options.autoheight) {
                this.options.headers.next().css("height", "");
            }
            $.removeData(this.element, "ui-accordion");
            $(this.element).removeClass("ui-accordion").unbind(".ui-accordion");
        }
    };

    function scopeCallback(callback, scope) {
        return function () {
            return callback.apply(scope, arguments);
        };
    };

    function completed(cancel) {
        if (!$.data(this, "ui-accordion")) return;
        var instance = $.data(this, "ui-accordion");
        var options = instance.options;
        options.running = cancel ? 0 : --options.running;
        if (options.running) return;
        if (options.clearStyle) {
            options.toShow.add(options.toHide).css({
                height: "",
                overflow: ""
            });
        }
        $(this).triggerHandler("change.ui-accordion", [options.data], options.change);
    }
    function toggle(toShow, toHide, data, clickedActive, down) {
        var options = $.data(this, "ui-accordion").options;
        options.toShow = toShow;
        options.toHide = toHide;
        options.data = data;
        var complete = scopeCallback(completed, this);
        options.running = toHide.size() == 0 ? toShow.size() : toHide.size();
        if (options.animated) {
            if (!options.alwaysOpen && clickedActive) {
                $.ui.accordion.animations[options.animated]({
                    toShow: jQuery([]),
                    toHide: toHide,
                    complete: complete,
                    down: down,
                    autoheight: options.autoheight
                });
            } else {
                $.ui.accordion.animations[options.animated]({
                    toShow: toShow,
                    toHide: toHide,
                    complete: complete,
                    down: down,
                    autoheight: options.autoheight
                });
            }
        } else {
            if (!options.alwaysOpen && clickedActive) {
                toShow.toggle();
            } else {
                toHide.hide();
                toShow.show();
            }
            complete(true);
        }
    }
    function clickHandler(event) {
        var options = $.data(this, "ui-accordion").options;
        if (options.disabled) return false;
        if (!event.target && !options.alwaysOpen) {
            options.active.parent().andSelf().toggleClass(options.selectedClass);
            var toHide = options.active.next(),
                data = {
                    instance: this,
                    options: options,
                    newHeader: jQuery([]),
                    oldHeader: options.active,
                    newContent: jQuery([]),
                    oldContent: toHide
                },
                toShow = options.active = $([]);
            toggle.call(this, toShow, toHide, data);
            return false;
        }
        var clicked = $(event.target);
        if (clicked.parents(options.header).length) while (!clicked.is(options.header)) clicked = clicked.parent();
        var clickedActive = clicked[0] == options.active[0];
        if (options.running || (options.alwaysOpen && clickedActive)) return false;
        if (!clicked.is(options.header)) return;
        options.active.parent().andSelf().toggleClass(options.selectedClass);
        if (!clickedActive) {
            clicked.parent().andSelf().addClass(options.selectedClass);
        }
        var toShow = clicked.next(),
            toHide = options.active.next(),
            data = {
                instance: this,
                options: options,
                newHeader: clicked,
                oldHeader: options.active,
                newContent: toShow,
                oldContent: toHide
            },
            down = options.headers.index(options.active[0]) > options.headers.index(clicked[0]);
        options.active = clickedActive ? $([]) : clicked;
        toggle.call(this, toShow, toHide, data, clickedActive, down);
        return false;
    };

    function findActive(headers, selector) {
        return selector != undefined ? typeof selector == "number" ? headers.filter(":eq(" + selector + ")") : headers.not(headers.not(selector)) : selector === false ? $([]) : headers.filter(":eq(0)");
    }
    $.extend($.ui.accordion, {
        defaults: {
            selectedClass: "selected",
            alwaysOpen: true,
            animated: 'slide',
            event: "click",
            header: "a",
            autoheight: true,
            running: 0,
            navigationFilter: function () {
                return this.href.toLowerCase() == location.href.toLowerCase();
            }
        },
        animations: {
            slide: function (options, additions) {
                options = $.extend({
                    easing: "swing",
                    duration: 300
                }, options, additions);
                if (!options.toHide.size()) {
                    options.toShow.animate({
                        height: "show"
                    }, options);
                    return;
                }
                var hideHeight = options.toHide.height(),
                    showHeight = options.toShow.height(),
                    difference = showHeight / hideHeight;
                options.toShow.css({
                    height: 0,
                    overflow: 'hidden'
                }).show();
                options.toHide.filter(":hidden").each(options.complete).end().filter(":visible").animate({
                    height: "hide"
                }, {
                    step: function (now) {
                        var current = (hideHeight - now) * difference;
                        if ($.browser.msie || $.browser.opera) {
                            current = Math.ceil(current);
                        }
                        options.toShow.height(current);
                    },
                    duration: options.duration,
                    easing: options.easing,
                    complete: function () {
                        if (!options.autoheight) {
                            options.toShow.css("height", "auto");
                        }
                        options.complete();
                    }
                });
            },
            bounceslide: function (options) {
                this.slide(options, {
                    easing: options.down ? "bounceout" : "swing",
                    duration: options.down ? 1000 : 200
                });
            },
            easeslide: function (options) {
                this.slide(options, {
                    easing: "easeinout",
                    duration: 700
                })
            }
        }
    });
})(jQuery);
(function ($) {
    $.fn.stickyBar = function (opts) {
        return $.fn.stickyBar.methods.init(this, $.extend({}, $.fn.stickyBar.defaults, opts));
    };
    $.fn.stickyBar.defaults = {
        toggleElement: '#sticky-switch',
        additionalElm: null,
        onBeforeToggleHandler: null,
        onToggle: null,
        onAfterToggle: null,
        panelElement: '#sticky-content',
        panelClosedClass: 'closed',
        panelMinHeight: 5,
        panelDefHeight: 98,
        cookie: {
            name: 'sticky',
            status: ['is-close', 'is-open'],
            expires: 15
        },
        hiddenClass: 'hide',
        reverseClass: 'reverse',
        wrapTemplate: '<div class="sticky-wrap hide" id="__jQuery_stickybarWrap"></div>',
        focusedElement: '#sticky-sportsList>ul>li',
        focusedChildElement: '.tooltip',
        tooltip: {
            url: '/externalmodules/{discipline}/sticky.html',
            paraUrl: '/externalmodules/para/{discipline}/sticky.html',
            loadingImg: '/imgml/stickybar/sticky-ico-loading.gif'
        },
        pollCfg: {
            json: '/live/library/stickybar/status.txt',
            paraJson: '/live/library/stickybar/paralympics/status.txt',
            delay: 15000
        },
        liveClass: 'onNow',
        legendElement: '.legend',
        resBasePath: '/live/library/stickybar/',
        onInit: null,
        onFocusedMouseEnter: null,
        onFocusedMouseMove: null,
        onFocusedMouseLeave: null,
        onFocusedClick: null,
        onResourcesLoaded: null
    };
    $.fn.stickyBar.methods = {
        opts: null,
        _ctx: null,
        jDataKey: '__stickybar_focusedelement_create',
        $wrap: null,
        $panel: null,
        $toggle: null,
        $toggleElement: null,
        $window: null,
        window: null,
        $sportsList: null,
        $legend: null,
        genericTs: null,
        init: function ($this, opts) {
            var self = this,
                platform = (navigator.platform || '').toLowerCase(),
                userAgent = (navigator.userAgent || '').toLowerCase();
            self.opts = opts;
            self.genericTs = new Date().getTime();
            self._ctx = $this;
            if ((platform === 'ipad' || platform === 'iphone' || platform === 'ipod') && userAgent.indexOf('os 5_0') === -1 && self._ctx.css('position') === 'fixed') {
                self._ctx.css({
                    position: 'absolute'
                });
            }
            self.$wrap = $(self.opts.wrapTemplate).appendTo(document.body);
            self.window = window;
            self.$window = $(self.window);
            self.$panel = $(self.opts.panelElement, self._ctx);
            self.isPara = self.$panel.attr('data-paralympics');
            if (self.$panel.hasClass('sticky-content-pre')) {
                self.$wrap.addClass('pre');
            }
            self.showPreGamesData();
            self.$toggleElement = $(self.opts.toggleElement, self._ctx).bind('click', function (e) {
                var $target = $(e.target);
                if (!$target.is('a')) {
                    return;
                }
                e.preventDefault();
                if (self.opts.onBeforeToggleHandler) {
                    if (typeof (self.opts.onBeforeToggleHandler.toString()) === 'object') {
                        self.opts.onBeforeToggleHandler.callback.call($target[0], self, e);
                        if (self.opts.onBeforeToggleHandler.hasOwnProperty('chained') && !self.opts.onBeforeToggleHandler.chained) {
                            return;
                        }
                    } else {
                        self.opts.onBeforeToggleHandler.call($target[0], self, e);
                    }
                }
                self.toggle.apply(self, [false]);
            });
            self.toggle.apply(self, [true]);
            return $this;
        },
        toggle: function (isOnLoad) {
            var self = this,
                _opts = self.opts,
                _ctx = self._ctx,
                _cookie;
            if (!self.$toggle) {
                self.$toggle = self.$toggleElement.find('a');
            }
            if (window.localStorage) {
                _cookie = localStorage.getItem(_opts.cookie.name);
            } else {
                _cookie = _opts.cookie.status[0];
            }
            var panelCallback = function () {
                    self.$panel.toggleClass(_opts.panelClosedClass, _cookie === _opts.cookie.status[0]);
                };
            if (!isOnLoad) {
                _cookie = _opts.cookie.status[_cookie === _opts.cookie.status[1] ? 0 : 1];
                var height = self.$panel.attr('def-height');
                var afterAnimation = false;
                if (_cookie === _opts.cookie.status[0]) {
                    height = self.opts.panelMinHeight;
                    afterAnimation = true;
                }
                if (!afterAnimation) {
                    panelCallback.apply(self);
                }
                self.$panel.animate({
                    height: height
                }, 500, 'linear', function () {
                    if (afterAnimation) {
                        panelCallback.apply(self);
                    }
                });
                if (self.opts.onToggle) {
                    self.opts.onToggle.apply(self, [_cookie]);
                }
                self.$toggle.toggleClass(self.opts.hiddenClass);
            } else {
                self.$panel[0].setAttribute('def-height', self.opts.panelDefHeight);
                if (!_cookie) {
                    _cookie = _opts.cookie.status[0];
                }
                if (_cookie !== _opts.cookie.status[1]) {
                    self.$panel.height(self.opts.panelMinHeight);
                    setTimeout(function () {
                        self.innerAfterToggle.apply(self, [true]);
                        panelCallback.apply(self);
                    }, 500);
                } else {
                    self.$toggle.toggleClass(self.opts.hiddenClass);
                    panelCallback.apply(self);
                    self.$panel.animate({
                        height: self.opts.panelDefHeight
                    }, 500, 'linear');
                    self.innerAfterToggle.apply(self);
                }
            }
            if (window.localStorage) {
                localStorage.setItem(_opts.cookie.name, _cookie, {
                    expires: _opts.cookie.expires,
                    path: '/'
                });
            }
            if (self.opts.onAfterToggle) {
                self.opts.onAfterToggle.apply(self, [_cookie, isOnLoad]);
            }
            return self;
        },
        innerAfterToggle: function (closed) {
            var self = this,
                p;
            self.$sportsList = $(self.opts.focusedElement, self._ctx);
            self.$legend = $(self.opts.legendElement, self._ctx);
            self.$legend.nativeText = self.$legend.html();
            if (self.opts.additionalElm) {
                for (p in self.opts.additionalElm) {
                    if (p) {
                        self.opts.additionalElm[p] = $(self.opts.additionalElm[p]);
                    }
                }
            }
            self.setFocused();
            if (self.opts.onInit) {
                self.opts.onInit.apply(self, [localStorage.getItem(self.opts.cookie.name)]);
            }
            self.loadResources.apply(self);
            if (!closed) {
                self._onPoll.apply(self, [self._ctx]);
            } else {
                self.$toggleElement.bind('mouseenter', function () {
                    setTimeout(function () {
                        self._onPoll.apply(self, [self._ctx]);
                    }, 200);
                    $(this).unbind('mouseenter');
                });
            }
        },
        resources_to_load: 2,
        loadResources: function () {
            var self = this,
                $countryWrap = self._ctx.find('#sticky-countries-wrap'),
                $disciplineWrap = self._ctx.find('#sticky-sports-wrap');
            if (!$countryWrap.length) {
                return;
            }
            if ($countryWrap.html().length > 10) {
                return;
            }
            basePath = self.opts.resBasePath + (self.isPara ? 'paralympics/' : '');
            self._loadInnerResources(basePath + '_countries.html', function (data) {
                $countryWrap.html(['<!--googleoff:all--><div><label for="c1filterCountry" class="hidden">Country</label><select id="c1filterCountry">', data, '</select></div><div><label for="c2filterCountry" class="hidden">Country</label><select id="c2filterCountry">', data, '</select></div><!--googleon:all-->'].join(''));
            });
            self._loadInnerResources(basePath + '_disciplines.html', function (data) {
                $disciplineWrap.html(['<!--googleoff:all--><div><label for="d1filterSport" class="hidden">Sport</label><select id="d1filterSport">', data, '</select></div><div><label for="d2filterSport" class="hidden">Sport</label><select id="d2filterSport">', data, '</select></div><!--googleon:all-->'].join(''));
            });
        },
        _loadInnerResources: function (url, callback) {
            var self = this;
            $.ajax({
                type: 'get',
                url: url,
                success: function (data) {
                    if (callback) {
                        callback.apply(self, [data]);
                    }
                    self.resources_to_load--;
                    if (self.resources_to_load <= 0 && self.opts.onResourcesLoaded) {
                        self.opts.onResourcesLoaded.apply(self);
                    }
                }
            });
        },
        setFocused: function () {
            var self = this;
            if (!self.opts.onFocusedMouseEnter) {
                self.opts.onFocusedMouseEnter = self.onFocusedMouseEnter;
            }
            if (!self.opts.onFocusedMouseMove) {
                self.opts.onFocusedMouseMove = self.onFocusedMouseMove;
            }
            if (!self.opts.onFocusedMouseLeave) {
                self.opts.onFocusedMouseLeave = self.onFocusedMouseLeave;
            }
            if (!self.opts.onFocusedClick) {
                self.opts.onFocusedClick = self.onFocusedClick;
            }
            self.$legend.bind('mouseenter', function () {
                if (self.xhr) {
                    self.xhr.abort();
                }
                self.__hideWrap();
            });
            self.$sportsList.unbind('mouseenter mouseleave blur mousemove click').bind('mouseenter mouseleave blur mousemove click', function (e) {
                var $this = $(this);
                switch (e.type) {
                case 'mouseenter':
                    self.__onFocusedMouseEnter.apply($this, [e, self]);
                    break;
                case 'mouseleave':
                case 'blur':
                    self.opts.onFocusedMouseLeave.apply($this, [e, self]);
                    self.__lastEvt = null;
                    break;
                case 'mousemove':
                    self.opts.onFocusedMouseMove.apply($this, [e, self]);
                    break;
                case 'click':
                    self.opts.onFocusedClick.apply($this, [e, self]);
                    break;
                }
            });
        },
        __onFocusedMouseEnter: function (e, mThis) {
            var $this = this;
            var $child = $this.find(mThis.opts.focusedChildElement);
            if ($child.length) {
                mThis.opts.onFocusedMouseEnter.apply($this, [e, $child, mThis]);
            }
        },
        __modifier: {
            x: 0,
            y: -6
        },
        __tm: null,
        xhr: null,
        onFocusedMouseEnter: function (e, $child, mThis) {
            var $this = $(this);
            var $inner = $child.find('.tooltipInner');
            mThis.$wrap.html($child.html()).removeClass(mThis.opts.hiddenClass);
            var completeCallback = function (jqXHR, textStatus) {
                    if (!jqXHR || jqXHR.status == 0) {
                        return;
                    }
                    var _$this = $(this);
                    var _$child = _$this.find(mThis.opts.focusedChildElement);
                    if (textStatus == 'error' || jqXHR.responseText.indexOf('urlmapper_Errors') != -1) {
                        _$child.find('.tooltipInner').html('Information not available');
                    }
                    mThis.$wrap.html(_$child.html());
                };
            if (mThis.xhr) {
                mThis.xhr.abort();
            }
            if ($inner.text() === '' || $inner.text() === ' ') {
                if (!$inner.find('#sticky-loading-image').length) {
                    $inner.append('<div id="sticky-loading-image"><img style="display:block;width:32px;height:32px;margin:0 auto;padding:2px 0 4px;" src="' + mThis.opts.tooltip.loadingImg + '"/></div>');
                }
                var _url = (mThis.isPara ? (mThis.opts.tooltip.paraUrl || mThis.opts.tooltip.url) : mThis.opts.tooltip.url).replace('{discipline}', $this.find('a:first').attr('class'));
                mThis.xhr = $.ajax({
                    type: 'GET',
                    url: _url + '?ign=' + mThis.genericTs,
                    context: $this,
                    success: function (data) {
                        $(this).find(mThis.opts.focusedChildElement).find('.tooltipInner').html(data);
                    },
                    complete: completeCallback
                });
            } else {
                completeCallback.apply(this);
            }
            mThis.__setCssMouseEvt(e, $this, mThis);
            _gaq.push(['_trackEvent', 'sticky bar', 'popup', $this.find('a:first').attr('class')]);
        },
        onFocusedMouseMove: function (e, mThis) {
            return mThis.__setCssMouseEvt(e, $(this), mThis);
        },
        onFocusedMouseLeave: function (e, mThis) {
            mThis.__hideWrap();
            if (mThis.xhr) {
                mThis.xhr.abort();
            }
        },
        onFocusedClick: function (e, mThis) {},
        __setCssMouseEvt: function (e, $this, mThis) {
            var _bottom = (mThis.$window.height() - e.clientY) - mThis.__modifier.y;
            var _left = e.clientX + mThis.__modifier.x;
            if (($this.offset().left + $this.width() + mThis.$wrap.width()) > mThis.$window.width()) {
                _left -= mThis.$wrap.width();
                mThis.$wrap.addClass(mThis.opts.reverseClass);
            } else {
                mThis.$wrap.removeClass(mThis.opts.reverseClass);
            }
            mThis.$wrap.css({
                bottom: _bottom,
                left: _left
            });
            return mThis.$wrap;
        },
        __hideWrap: function () {
            return this.$wrap.removeAttr('style').addClass(this.opts.hiddenClass);
        },
        poll: function ($this, data) {
            var self = this;
            var liveLegend = false;
            $.ajax({
                dataType: 'json',
                type: 'GET',
                url: (self.isPara ? self.opts.pollCfg.paraJson || self.opts.pollCfg.json : self.opts.pollCfg.json) + '?ign=' + (new Date().getTime()),
                success: function (json) {
                    var $json = $(json),
                        p;
                    if (!$json.length) {
                        return;
                    }
                    var _$json = $json[0];
                    for (p in _$json) {
                        if (p && p.indexOf('debug_') === -1) {
                            var v = _$json[p];
                            var isLive = v === '1';
                            var $li = self.$sportsList.find('.' + p.toLowerCase()).parents('li:first');
                            var wasLive = $li.hasClass(self.opts.liveClass);
                            if ((wasLive && !isLive) || !wasLive && isLive) {
                                $li.find('.tooltipInner').html('');
                                self.genericTs = new Date().getTime();
                            }
                            $li[isLive ? 'addClass' : 'removeClass'](self.opts.liveClass);
                            if (isLive) {
                                liveLegend = true;
                            }
                        }
                    }
                    if (self.$legend.length) {
                        var _native = self.$legend.nativeText;
                        if (liveLegend) {
                            self.$legend.html('<a href="/live/index.html">' + _native + '</a>').addClass(self.opts.liveClass);
                        } else {
                            self.$legend.html(_native).removeClass(self.opts.liveClass);
                        }
                    }
                    self._onPoll.apply(self, [$this, true]);
                }
            });
        },
        _onPoll: function ($this, recursive) {
            var self = this;
            if (!recursive) {
                self.poll.apply(self, [$this]);
                return;
            }
            self.__tm = setTimeout(function () {
                self.poll.apply(self, [$this]);
            }, self.opts.pollCfg.delay);
        },
        showPreGamesData: function () {
            var $stickypre = $('.sticky-content-pre');
            if ($stickypre.length) {
                var now = new Date();
                var OlympicsStart = new Date('2012-07-25');
                var nowMonth = now.getMonth() + 1;
                var nowDay = now.getDate();
                nowDay = (nowDay < 10) ? '0' + nowDay : nowDay;
                $stickypre.find('.day').text(nowDay);
                var month = '';
                switch (nowMonth) {
                case 1:
                    month = 'January';
                    break;
                case 2:
                    month = 'February';
                    break;
                case 3:
                    month = 'March';
                    break;
                case 4:
                    month = 'April';
                    break;
                case 5:
                    month = 'May';
                    break;
                case 6:
                    month = 'June';
                    break;
                case 7:
                    month = 'July';
                    break;
                case 8:
                    month = 'August';
                    break;
                }
                if (month.length) {
                    $stickypre.find('.month').text(month);
                }
                var cdDays = $('.jcountdown-dd').text().replace(/^[0]+/g, "");
                $stickypre.find('.daysto25').text(cdDays);
            }
        }
    };
}(jQuery));
window.$core = jQuery.extend({}, window.$core, {
    myolympics: function ($) {
        $.fn.myolympics = function (options) {
            var $ctx = this,
                ispara = $ctx.attr('data-paralympics'),
                countryKey = 'mycountries' + (ispara ? '_para' : ''),
                discKey = 'mysports' + (ispara ? '_para' : ''),
                $cookieC = localStorage.getItem(countryKey),
                $cookieS = localStorage.getItem(discKey),
                hiddenClass = 'hide',
                showSelect = false,
                $stickyselect = $ctx.find('.stickyselect'),
                $myolympics = $('.myolympics'),
                $stickycountry1 = $('#stickycountry1'),
                $stickycountry2 = $('#stickycountry2'),
                $stickydisc1 = $('#stickydisc1'),
                $stickydisc2 = $('#stickydisc2'),
                $selDisc1 = $("#d1filterSport"),
                $selDisc2 = $("#d2filterSport"),
                $selCountry1 = $('#c1filterCountry'),
                $selCountry2 = $('#c2filterCountry');
            if (($cookieC !== null || $cookieS !== null) && ($cookieC !== '_' || $cookieS != '_')) {
                $stickyselect.addClass(hiddenClass);
                $myolympics.removeClass(hiddenClass);
                if ($cookieC !== null) {
                    var countries = $cookieC.split('_'),
                        $country1 = countries[0],
                        $country2 = countries[1];
                    if ($country1) {
                        $stickycountry1.html('<img src="/imgml/flags/s/' + $country1 + '.png" alt="' + $country1 + '" width="23" height="15" />' + $country1);
                    }
                    if ($country2) {
                        $stickycountry2.html('<img src="/imgml/flags/s/' + $country2 + '.png" alt="' + $country2 + '" width="23" height="15" />' + $country2);
                    }
                    $selCountry1.find('option[data-code="' + $country1 + '"]').attr('selected', 'selected');
                    $selCountry2.find('option[data-code="' + $country2 + '"]').attr('selected', 'selected');
                }
                if ($cookieS !== null) {
                    var sports = $cookieS.split('_'),
                        $sport1 = sports[0],
                        $sport2 = sports[1],
                        $optDisc1 = $selDisc1.find('option[data-seo="' + $sport1 + '"]'),
                        $optDisc2 = $selDisc2.find('option[data-seo="' + $sport2 + '"]');
                    if ($sport1 !== '') {
                        $stickydisc1.text($optDisc1.text());
                        if ($stickydisc1.text().length > 20) {
                            $stickydisc1.css("letter-spacing", "-1px");
                        }
                        $optDisc1.attr('selected', 'selected');
                    }
                    if ($sport2 !== '') {
                        $stickydisc2.text($optDisc2.text());
                        if ($stickydisc2.text().length > 20) {
                            $stickydisc2.css("letter-spacing", "-1px");
                        }
                        $optDisc2.attr('selected', 'selected');
                    }
                }
            }
            $('#stickystart').click(function (e) {
                $stickyselect.addClass(hiddenClass);
                $('.choosecountries').removeClass(hiddenClass);
                _gaq.push(['_trackEvent', 'sticky bar', 'personalizeStart']);
                e.preventDefault();
            });
            $('#stickystep1').click(function (e) {
                $('.choosecountries').addClass(hiddenClass);
                $('.choosesports').removeClass(hiddenClass);
                var country1 = $selCountry1.val(),
                    country2 = $selCountry2.val();
                if (country1 && country1.length > 0) {
                    _gaq.push(['_trackEvent', 'sticky bar', 'personalizeCountry1', country1]);
                }
                if (country2 && country2.length > 0) {
                    _gaq.push(['_trackEvent', 'sticky bar', 'personalizeCountry2', country2]);
                }
                e.preventDefault();
            });
            $('#stickystep2').click(function (e) {
                var sport1 = $selDisc1.val(),
                    sport2 = $selDisc2.val();
                if (sport1 !== '') {
                    $stickydisc1.text($selDisc1.find(':selected').text());
                    if ($stickydisc1.text().length > 20) {
                        $stickydisc1.css("letter-spacing", "-1px");
                    }
                }
                if (sport2 !== '') {
                    $stickydisc2.text($selDisc2.find(':selected').text());
                    if ($stickydisc2.text().length > 20) {
                        $stickydisc2.css("letter-spacing", "-1px");
                    }
                }
                e.preventDefault();
                $('.choosesports,.stickyselect').addClass(hiddenClass);
                $myolympics.removeClass(hiddenClass);
                localStorage.setItem(discKey, sport1 + '_' + sport2, {
                    expires: 60,
                    path: '/'
                });
                var country1 = $selCountry1.val(),
                    country2 = $selCountry2.val();
                if (country1) {
                    $stickycountry1.html('<img src="/imgml/flags/s/' + country1 + '.png" alt="' + country1 + '" width="23" height="15" />' + country1);
                } else {
                    $stickycountry1.html('');
                }
                if (country2) {
                    $stickycountry2.html('<img src="/imgml/flags/s/' + country2 + '.png" alt="' + country2 + '" width="23" height="15" />' + country2);
                } else {
                    $stickycountry2.html('');
                }
                localStorage.setItem(countryKey, country1 + '_' + country2, {
                    expires: 60,
                    path: '/'
                });
                if (sport1 && sport1.length > 0) {
                    _gaq.push(['_trackEvent', 'sticky bar', 'personalizeSport1', sport1]);
                }
                if (sport2 && sport2.length > 0) {
                    _gaq.push(['_trackEvent', 'sticky bar', 'personalizeSport2', sport2]);
                }
                var options = (sport1.length > 0 ? 1 : 0) + (sport2.length > 0 ? 1 : 0) + (country1.length > 0 ? 1 : 0) + (country2.length > 0 ? 1 : 0);
                _gaq.push(['_trackEvent', 'sticky bar', 'personalizeComplete', options]);
                $.execIfExists('select[id$="filterSport"],select[id$="filterCountry"]', function () {
                    var $this = $(this);
                    $this.find('.fave').removeClass('fave');
                    $this.mySelect();
                });
                $.execIfExists('.customTabs', function () {
                    var $this = $(this),
                        $parent = $this.closest('.wrapTabs').parent(),
                        genericTabOuter = $this.find('.tabItem.first').outerHtml();
                    $this.html(genericTabOuter);
                    $this.find('.tabItem').addClass('current');
                    $parent.find('div[class*="tabCustom"]').not('.tabCustom-all').remove();
                    $parent.find('.tabCustom-all').removeClass('hidden').addClass('current');
                    $this.myTabs();
                });
                $.execIfExists('#schedule_gridWrap', function () {
                    var $this = $(this);
                    $('#my-schedule_sportsList').replaceWith('');
                    $('#my-schedule_grid').replaceWith('');
                    $this.mySchedule();
                });
            });
            $('#stickyedit').click(function (e) {
                $('.myolympics,.stickytitle').addClass(hiddenClass);
                $('.choosecountries').removeClass(hiddenClass);
                e.preventDefault();
            });
        };
    },
    myTabs: function ($) {
        $.fn.myTabs = function (options) {
            var ispara = $('#sticky-myOly').attr('data-paralympics'),
                opts = $.extend({}, $.fn.myTabs.defaults, options),
                countries = [],
                disciplines = [],
                countryKey = 'mycountries' + (ispara ? '_para' : ''),
                discKey = 'mysports' + (ispara ? '_para' : ''),
                cookieCountries = localStorage.getItem(countryKey),
                cookieSports = localStorage.getItem(discKey),
                tabsHtml = [],
                country = '',
                discipline = '';
            if (!cookieCountries && !cookieSports) {
                return this;
            }
            countries = cookieCountries.split('_');
            disciplines = cookieSports.split('_');
            var lcountries = countries.length,
                ldisciplines = disciplines.length,
                toload = lcountries + ldisciplines,
                seo = '',
                tabContent = '',
                $this = this,
                setAjax = function (typebase, key, idx, alt) {
                    tabContent += ['<div class="tabCustom-', (alt || key), ' hidden"></div>'].join('');
                    $.ajax({
                        type: 'get',
                        url: [typebase, key, opts.base, '_tab.html'].join(''),
                        dataType: 'html',
                        success: function (data) {
                            tabsHtml[idx] = data;
                        },
                        complete: function () {
                            toload--;
                            if (toload <= 0) {
                                $.fn.myTabs.methods.init($this, opts, tabsHtml, tabContent, ispara);
                            }
                        }
                    });
                },
                $c1options = $('#c1filterCountry>option'),
                $c2options = $('#c2filterCountry>option'),
                isFr = (window.location.pathname.indexOf('/fr') === 0);
            while (lcountries) {
                country = countries[lcountries - 1];
                if (!country || country === 'undefined') {
                    lcountries--;
                    toload--;
                    continue;
                }
                var $optCountry1 = $c1options.filter('[data-code="' + country + '"]'),
                    $optCountry2 = $c2options.filter('[data-code="' + country + '"]');
                seo = '';
                if ($optCountry1.is(':selected')) {
                    seo = $optCountry1.attr('data-seo');
                } else if ($optCountry2.is(':selected')) {
                    seo = $optCountry2.attr('data-seo');
                }
                if (!seo) {
                    lcountries--;
                    toload--;
                    continue;
                }
                setAjax.apply(this, [(ispara ? '/paralympics' : '') + opts.typeBase.country + (isFr ? '/country=' : ''), seo, lcountries - 1, country]);
                lcountries--;
            }
            while (ldisciplines) {
                discipline = disciplines[ldisciplines - 1];
                if (!discipline || discipline === 'undefined') {
                    ldisciplines--;
                    toload--;
                    continue;
                }
                setAjax.apply(this, [(ispara ? '/paralympics' : '') + opts.typeBase.disc, discipline, 2 + ldisciplines - 1]);
                ldisciplines--;
            }
        };
        $.fn.myTabs.methods = {
            init: function ($this, opts, tabsHtml, tabContent, ispara) {
                if (!tabsHtml.length) {
                    return;
                }
                var xhr = null;
                return $this.each(function () {
                    var $ctx = $(this),
                        $tabs = $ctx.append(tabsHtml.join('')),
                        $wrapTabs = $tabs.parent().removeClass(opts.hiddenClass),
                        $customAll = $wrapTabs.next('.tabCustom-all').after(tabContent);
                    $wrapTabs.data('custom-file', $ctx.attr('data-file'));
                    $wrapTabs.closest('.box').addClass('custom-tabbed');
                    $wrapTabs.unbind('click').bind('click', function (e) {
                        if (xhr) {
                            xhr.abort();
                        }
                        e.preventDefault();
                        var $target = $(e.target),
                            $a = $target,
                            $li = $target.closest('li'),
                            tabType = '';
                        if ($target.is('img') || $target.is('span.icon')) {
                            $a = $target.closest('a');
                        } else if ($target.is('li')) {
                            $li = $target;
                            $a = $target.find('a');
                        }
                        tabType = $li.searchCssClass('tabtype-');
                        if (!$a.is('a')) {
                            return;
                        }
                        var $this = $(this),
                            tabContentSelector = $a.attr('href').replace('#', '.'),
                            hasHref = tabContentSelector.indexOf('http') === 0,
                            href = window.location.href;
                        $content = $this.siblings(hasHref ? tabContentSelector.replace(href, '') : tabContentSelector), type = $this.searchCssClass('type-'), dataFile = $this.data('custom-file'), url = dataFile ? dataFile + '.html' : opts.typeUrl[type], isFr = (window.location.pathname.indexOf('/fr') === 0);
                        base = (ispara ? '/paralympics' : '') + opts.typeBase[tabType] + (isFr && tabType == 'country' ? 'country=' : '');
                        var callback = function ($li) {
                                this.find('.tabItem.current').removeClass(opts.currentClass).end().siblings('.' + opts.currentClass).removeClass(opts.currentClass).addClass(opts.hiddenClass);
                                $content.removeClass(opts.hiddenClass).addClass(opts.currentClass);
                                $li.addClass(opts.currentClass);
                            };
                        if (!base) {
                            callback.apply($this, [$li]);
                            return;
                        }
                        if ($content.html().length) {
                            callback.apply($this, [$li]);
                            return;
                        }
                        xhr = $.ajax({
                            type: 'get',
                            url: [base, $a.attr('data-seo'), opts.base, url].join(''),
                            context: $content,
                            complete: function (xhr) {
                                if (!xhr && !xhr.responseText) {
                                    return;
                                }
                                var html = xhr.responseText,
                                    $this = $(this);
                                if (html.length > 1) {
                                    $this.html(html);
                                }
                                var cls = $this.searchCssClass('tabCustom-', {
                                    rawVal: true
                                }),
                                    _$wrap = $this.parent().find('.wrapTabs');
                                callback.apply(_$wrap, [_$wrap.find('a[href="' + (hasHref ? href : '') + '#' + cls + '"]').closest('li')]);
                                $.execIfExists($this.find(".jcarousel"), "jCarousel", window.jcarouselCfg);
                            }
                        });
                    });
                });
            }
        };
        $.fn.myTabs.defaults = {
            base: '/library/custom/',
            typeBase: {
                disc: '/disciplines/discipline=',
                country: '/country/'
            },
            typeUrl: {
                news: '_latestnews.html',
                photos: '_latestpictures.html',
                galleries: '_latestgalleries.html'
            },
            hiddenClass: 'hidden',
            currentClass: 'current'
        };
    },
    mySelect: function ($) {
        $.fn.mySelect = function (options) {
            var ispara = $('#sticky-myOly').attr('data-paralympics'),
                opts = $.extend({}, $.fn.mySelect.defaults, options),
                countries = [],
                disciplines = [],
                countryKey = 'mycountries' + (ispara ? '_para' : ''),
                discKey = 'mysports' + (ispara ? '_para' : ''),
                cookieCountries = localStorage.getItem(countryKey),
                cookieSports = localStorage.getItem(discKey);
            if (!cookieCountries && !cookieSports) {
                return this;
            }
            countries = cookieCountries.split('_');
            disciplines = cookieSports.split('_');
            return this.filter(function () {
                return $(this).closest('#sticky-myOly').length === 0;
            }).find('option').each(function () {
                var dataValue = this.getAttribute('data-value'),
                    dataSeo = this.getAttribute('data-seo') || 'undefined',
                    valueLower = 'undefined',
                    txt = this.text.toLowerCase();
                if (dataValue) {
                    valueLower = dataValue.toLowerCase();
                }
                if (valueLower === countries[0] || valueLower === countries[1] || txt === disciplines[0] || txt === disciplines[1] || dataSeo === disciplines[0] || dataSeo === disciplines[1]) {
                    this.className += ' ' + opts.selectedClass;
                }
            });
        };
        $.fn.mySelect.defaults = {
            selectedClass: 'fave'
        };
    },
    mySchedule: function ($) {
        $.fn.mySchedule = function (options) {
            var isbydisc = this.attr('data-bydisc'),
                ispara = $('#sticky-myOly').attr('data-paralympics'),
                opts = $.extend({}, options),
                disciplines = [],
                discKey = 'mysports' + (ispara ? '_para' : ''),
                cookieSports = localStorage.getItem(discKey),
                l = 2,
                i = 0,
                $sportsList = [],
                $scheduleGrid = [],
                $faveDisc, $faveRow, addClass = false;
            if (isbydisc === '1') {
                return this;
            }
            if (!cookieSports || cookieSports === '_') {
                return this;
            }
            disciplines = cookieSports.split('_');
            $sportsList = $('#schedule_sportsList');
            $scheduleGrid = $('#schedule_rows');
            for (i; i < l; i++) {
                if (!disciplines[i].length) {
                    continue;
                }
                var disc = $('#d' + (i + 1) + 'filterSport>option[data-seo="' + disciplines[i] + '"]').attr('data-code');
                if (!disc || !disc.length) {
                    continue;
                }
                var $faveDiscClone = $sportsList.find('#disc-' + disc).clone(true),
                    $faveRowClone = $scheduleGrid.find('#row-' + disc).clone(true);
                if ($faveDiscClone.length) {
                    if (!$faveDisc) {
                        $faveDisc = $('<ul id="fave-schedule-disc"/>').prependTo($sportsList);
                        $faveDisc.wrap('<div id="my-schedule_sportsList"/>');
                    }
                    if (!$faveRow) {
                        $faveRow = $('<ul id="fave-schedule-row"/>').insertBefore($scheduleGrid);
                        $faveRow.wrap('<div id="my-schedule_grid"/>');
                    }
                    $faveDiscClone.attr('id', $faveDiscClone.attr('id') + '_fave');
                    $faveDiscClone.appendTo($faveDisc);
                    $faveRowClone.attr('id', $faveRowClone.attr('id') + '_fave');
                    $faveRowClone.appendTo($faveRow);
                    addClass = true;
                }
            }
            if (addClass) {
                this.addClass('my-schedule');
            }
        };
    }
});
(function ($) {
    $.fn.jLightbox = function (opts) {
        var self = this.jLightbox.methods.coreInit($.extend({}, $.fn.jLightbox.defaults, opts));
        return this.each(function () {
            var $this = $(this);
            $this.jLightbox.methods.init.apply(self, [$this]);
        });
    };
    $.fn.jLightbox.methods = {
        opts: null,
        $ctx: null,
        actualImage: 0,
        $overlay: null,
        $wrap: null,
        $innerWrap: null,
        $img: null,
        $btnPrev: null,
        $btnNext: null,
        $loading: null,
        $caption: null,
        $galleryTitle: null,
        isPhotolist: false,
        $title: null,
        $full: null,
        $imageWrap: null,
        $imageDataBox: null,
        $currentNumber: null,
        $nav: null,
        $close: null,
        $activeElement: null,
        $playPause: null,
        sizes: null,
        scrolls: null,
        $body: null,
        $html: null,
        $document: null,
        $hideable: null,
        $imagesList: null,
        paused: false,
        key: null,
        coreInit: function (opts) {
            var self = this;
            self.$body = $('body');
            self.$html = $('html');
            self.$document = $(document);
            self.$hideable = $('embed,object,select');
            self.opts = opts;
            self.paused = !opts.autoplay;
            if (!self.opts.onKeyUp) {
                self.opts.onKeyUp = self.onKeyUp;
            }
            self._setHtmlElement('$overlay', self.opts.overlay, '<div id="jquery-overlay" class="jlightbox-shadow ' + self.opts.hiddenClass + '"></div>');
            self._setHtmlElement('$wrap', self.opts.wrap, '<div id="jquery-lightbox" class="' + self.opts.hiddenClass + '"><div id="lightbox-container-image-box"><div id="lightbox-container-image"><img id="__jLightbox-image"/><div id="lightbox-nav"><a href="#" id="lightbox-nav-btnPrev">&nbsp;</a><a href="#" id="lightbox-nav-btnNext">&nbsp;</a></div><div id="lightbox-loading"><a href="#" id="lightbox-loading-link"><img src="' + self.opts.imageLoading + '"/></a></div></div><div id="lightbox-container-image-data-box"><div id="lightbox-container-image-data"><div id="lightbox-image-details"><span id="lightbox-image-details-caption"></span><span id="lightbox-image-details-currentNumber"></span></div><div id="__jLightbox-close"></div></div></div></div></div>');
            self.$innerWrap = $('#lightbox-container-image-box');
            self.$img = $('#__jLightbox-image');
            self.$loading = $('#lightbox-loading');
            self.$caption = $('#lightbox-image-details-caption');
            self.$imageWrap = $('#lightbox-container-image');
            self.$imageDataBox = $('#lightbox-container-image-data-box');
            self.$currentNumber = $('#lightbox-image-details-currentNumber');
            self.$nav = $('#lightbox-nav');
            self.$btnPrev = $('#lightbox-nav-btnPrev').prop('next', -1);
            self.$btnNext = $('#lightbox-nav-btnNext').prop('next', 1);
            self.$title = $('#lightbox-title').hide();
            self.$wrap.bind('click', function (e) {
                var $target = $(e.target);
                if (!$target.parents('#jquery-lightbox').length) {
                    self.finish();
                }
            });
            self.$close = $('#__jLightbox-close').add(self.$overlay).bind('click', function (e) {
                e.preventDefault();
                self.finish();
            });
            $(window).resize(function () {
                self.sizes = self._copyOf_getPageSize();
                self.scrolls = self._copyOf_getPageScroll();
                self._setBaseSizes();
            });
            if (self.opts.onInit) {
                self.opts.onInit.apply(self, arguments);
            }
            return self.sync();
        },
        init: function ($this) {
            var self = this;
            self.$ctx = $this;
            self.key = ['jlightbox-key', new Date().getTime()].join('');
            self.$ctx.find(self.opts.itemSelector).bind('click', function (e) {
                e.preventDefault();
                var $item = $(this);
                var $self = self._getData($item);
                $self.actualImage = $self.$ctx.find(self.opts.itemSelector).index($item);
                $self.$hideable.css({
                    'visibility': 'hidden'
                });
                $self.onClick.apply($self, arguments);
                $self.sync($item);
            });
            self._setFilter();
            self.$imagesList = self.$ctx.find(self.opts.photoSelector);
            return self.sync();
        },
        finish: function () {
            var self = this;
            self.$overlay.add(self.$wrap).attr('style', '');
            self.$document.unbind('keyup keypress');
            self.$btnPrev.add(self.$btnNext).unbind('click');
            self.$hideable.css({
                'visibility': 'visible'
            });
            self.$nav.add(self.$close).add(self.$currentNumber).hide();
            if (self.opts.onFinish) {
                self.opts.onFinish.apply(self, arguments);
            }
        },
        show: function () {
            var self = this;
            self.$loading.hide();
            if (self.$currentNumber.is(':hidden')) {
                self.$currentNumber.show();
            }
            if (self.$nav.is(':hidden')) {
                self.$nav.add(self.$close).show();
                if (self.opts.onSummaryInit) {
                    self.opts.onSummaryInit.apply(self);
                    self.$full = $('#lightbox-full');
                }
                document.getElementById('lightbox-nav-landing').focus();
            }
            if (self.$title.is(':hidden')) {
                self.$title.show();
            }
            self.$img.fadeIn(function () {
                self._showData.apply(self, arguments);
                self._setNavigation.apply(self, arguments);
            });
            self._preloadNeighbor();
        },
        setImage: function (callback) {
            var self = this;
            if (self.opts.onBeforeImageSet) {
                self.opts.onBeforeImageSet.apply(self, arguments);
            }
            self.$loading.show();
            self.$btnNext.add(self.$btnPrev).unbind('hover').addClass(self.opts.disabledClass);
            self.$caption.hide();
            self.$currentNumber[0].innerHTML = ([self.actualImage + 1, ' / ', self.$imagesList.length].join(''));
            var o = self.$imagesList[self.actualImage];
            if (o.className.indexOf(self.opts.summaryFilter) === -1) {
                var imgPreloader = new Image();
                imgPreloader.onload = function () {
                    var w = imgPreloader.width;
                    var h = imgPreloader.height;
                    if (!w || w === 1) {
                        w = self.opts.fallback.width;
                    }
                    if (!h || h === 1) {
                        h = self.opts.fallback.height;
                    }
                    self.$img.attr('src', (o.getAttribute('data-href') || o.href)).attr('width', w).attr('height', h);
                    self._setImageBoxSizes(w, h);
                    imgPreloader.onload = function () {};
                    if (callback) {
                        callback.apply(self);
                    }
                    if (self.opts.onAfterImageSet) {
                        self.opts.onAfterImageSet.apply(self, arguments);
                    }
                };
                imgPreloader.src = (o.getAttribute('data-href') || o.href);
                self.$img.hide();
            } else {
                self.$img.hide();
                self.$loading.hide();
                self._setNavigation.apply(self);
                if (self.opts.onSummary) {
                    self.opts.onSummary.apply(self, [o]);
                }
            }
            return self;
        },
        timedPlay: function () {
            var self = this;
            self._timedPlay(self.paused);
        },
        _timedPlay: function (paused) {
            var self = this;
            if (window.__jtm) {
                self.pause.apply(self);
            }
            if (paused) {
                if (self.$playPause && self.$playPause.length && !self.$playPause.hasClass('jlightbox-pause')) {
                    self.$playPause.addClass('jlightbox-pause');
                }
                return;
            }
            window.__jtm = setTimeout(function () {
                self._moveNext.apply(self, [function () {
                    self._timedPlay.apply(self);
                }]);
            }, self.opts.playSpeed);
        },
        restart: function () {
            var self = this;
            if (self.paused) {
                return;
            }
            self.timedPlay.apply(self, arguments);
        },
        pause: function () {
            clearTimeout(window.__jtm);
            window.__jtm = null;
        },
        sync: function ($o) {
            var self = this;
            self._toData(self.$btnPrev.add(self.$btnNext));
            if (self.$ctx) {
                self._toData(self.$ctx.find(self.opts.itemSelector));
            }
            if ($o) {
                self._toData($o);
            }
            return self;
        },
        onClick: function () {
            var self = this;
            self.$galleryTitle = self.$ctx.find('.gallery-title');
            if (!self.$galleryTitle.length) {
                self.$galleryTitle = self.$ctx.parents('.box:first').find('.bH').find('h3');
                self.isPhotolist = true;
            }
            self.$title.text(self.$galleryTitle.text());
            self.$document.bind('keyup keypress', function () {
                var _self = $(this).data('active-self');
                if (_self.$wrap.is(':visible')) {
                    _self.opts.onKeyUp.apply(_self, arguments);
                } else {
                    self.$document.unbind('keyup keypress');
                }
            });
            self.$btnPrev.add(self.$btnNext).bind('click', function (e) {
                e.preventDefault();
                var $btn = $(this);
                if ($btn.prop('next') === 1) {
                    self._moveNext.apply(self);
                } else {
                    self._movePrev.apply(self);
                }
            });
            self.setImage();
            self.scrolls = self._copyOf_getPageScroll();
            self.sizes = self._copyOf_getPageSize();
            self._setBaseSizes();
            self.$overlay.css({
                opacity: self.opts.overlayOpacity,
                backgroundColor: self.opts.overlayBgColor
            }).fadeIn();
            self.$wrap.show();
            self.$document.data('active-self', self);
            if (self.opts.onClick) {
                self.opts.onClick.apply(self, arguments);
            }
        },
        onKeyUp: function (e) {
            var code = e.keyCode || e.which;
            var self = this;
            if (e.type === 'keyup') {
                switch (code) {
                case 27:
                    self.finish();
                    break;
                case 39:
                case 78:
                    self._moveNext.apply(self, [self.opts.onNextKey]);
                    break;
                case 37:
                case 80:
                    self._movePrev.apply(self, [self.opts.onPrevKey]);
                    break;
                case 9:
                case 16:
                    self.$activeElement = $(document.activeElement);
                    break;
                case 32:
                    if (self.$activeElement && self.$activeElement.length) {
                        if (self.$activeElement.is(self.$close)) {
                            self.finish();
                        } else if (self.$activeElement.is(self.$btnNext)) {
                            self._moveNext.apply(self, [self.opts.onNextKey]);
                        } else if (self.$activeElement.is(self.$btnPrev)) {
                            self._movePrev.apply(self, [self.opts.onPrevKey]);
                        } else if (self.$activeElement.is(self.$full)) {
                            window.location.href = self.$full.attr('href');
                        } else if (self.opts.onSpacebarKey) {
                            self.opts.onSpacebarKey.apply(self);
                        }
                    }
                    break;
                }
            } else {
                switch (code) {
                case 38:
                case 40:
                case 33:
                case 34:
                case 36:
                case 35:
                case 32:
                    e.preventDefault();
                    break;
                }
            }
        },
        _toData: function ($o) {
            $o.parent().data('self', $.extend({}, this));
        },
        _getData: function ($o) {
            return $o.parent().data('self');
        },
        _moveNext: function (trueCallback) {
            var self = this;
            if (!(self.actualImage + 1 >= self.$imagesList.length)) {
                self.actualImage++;
                self.setImage(trueCallback);
            } else if (!trueCallback) {
                self.pause.apply(self, arguments);
            }
        },
        _movePrev: function (callback) {
            var self = this;
            if (!(self.actualImage - 1 < 0)) {
                self.actualImage--;
                self.setImage();
            }
            if (callback) {
                callback.apply(self);
            }
        },
        _setFilter: function () {
            var self = this,
                p;
            var _filter = self.opts.filter;
            if (!_filter.selector) {
                return;
            }
            if (!self.$ctx.is(_filter.selector)) {
                return;
            }
            for (p in _filter) {
                if (p !== 'selector' && self.opts.hasOwnProperty(p)) {
                    self.opts[p] = _filter[p];
                }
            }
        },
        _setImageBoxSizes: function (imgWidth, imgHeight, altCallback) {
            var self = this;
            var currentWidth = self.$innerWrap.width();
            var currentHeight = self.$innerWrap.height();
            var $c = self.$imageWrap;
            var paddingTop = 10,
                paddingBottom = 10,
                paddingLeft = 10,
                paddingRight = 10;
            var sizes = self.opts.paddingSize;
            if (sizes) {
                if (typeof (sizes) === typeof ([])) {
                    paddingTop = sizes[0];
                    paddingRight = sizes[1];
                    paddingBottom = sizes[2];
                    paddingLeft = sizes[3];
                } else {
                    paddingBottom = paddingLeft = paddingRight = paddingTop = sizes;
                }
            } else if (paddingTop = parseInt($c.css('padding-top'), 10)) {
                paddingRight = parseInt($c.css('padding-right'), 10);
                paddingBottom = parseInt($c.css('padding-bottom'), 10);
                parseInt($c.css('padding-left'), 10);
            }
            var width = (imgWidth + (paddingLeft + paddingRight));
            var height = (imgHeight + (paddingTop + paddingBottom));
            var diffW = currentWidth - width;
            var diffH = currentHeight - height;
            if ((diffW == 0) && (diffH == 0)) {
                setTimeout(function () {
                    if (altCallback) {
                        altCallback.apply(self);
                    } else {
                        self.show();
                    }
                }, 100);
            } else {
                self.$innerWrap.parent().css({
                    width: width,
                    height: height
                });
                self.$innerWrap.animate({
                    width: width,
                    height: height
                }, self.opts.resizeSpeed, function () {
                    if (altCallback) {
                        altCallback.apply(self);
                    } else {
                        self.show();
                    }
                });
            }
            self.$imageDataBox.css({
                width: imgWidth
            });
        },
        _setBaseSizes: function () {
            var self = this;
            self.$overlay.css({
                width: self.sizes[0],
                height: self.sizes[1]
            });
            self.$wrap.css({
                top: self.scrolls[1] + (self.sizes[3] / 10),
                left: self.scrolls[0]
            });
        },
        _setHtmlElement: function (prop, value, defContent) {
            if (value) {
                if (value.indexOf('#') === 0 || value.indexOf('.') === 0) {
                    this[prop] = $(value);
                } else {
                    var re = /#{[a-zA-Z]*}/g;
                    var m = value.match(re);
                    var i = m.length;
                    while (i) {
                        var r = m[i - 1];
                        value = value.replace(r, this.opts[r.substring(2, r.length - 1)]);
                        i--;
                    }
                    this[prop] = $(value).appendTo('body');
                }
            } else {
                this[prop] = $(defContent).appendTo('body');
            }
        },
        _showData: function () {
            var self = this;
            var $photodata = $(self.opts.photoSelector, self.$ctx).eq(self.actualImage).find(".photo-data");
            if (self.$imagesList[self.actualImage] && $photodata.length) {
                self.$caption.html($photodata.html()).show();
            }
        },
        _setNavigation: function () {
            var self = this;
            if (self.actualImage > 0) {
                self.$btnPrev.removeClass(self.opts.disabledClass);
            }
            if (self.actualImage < (self.$imagesList.length - 1)) {
                self.$btnNext.removeClass(self.opts.disabledClass);
            }
        },
        _preloadNeighbor: function () {
            var self = this;
            if ((self.$imagesList.length - 1) > self.actualImage) {
                var objNext = new Image();
                objNext.src = self.$imagesList[self.actualImage + 1].href;
            }
            if (self.actualImage > 0) {
                var objPrev = new Image();
                objPrev.src = self.$imagesList[self.actualImage - 1].href;
            }
        },
        _copyOf_getPageSize: function () {
            var xScroll, yScroll;
            if (window.innerHeight && window.scrollMaxY) {
                xScroll = window.innerWidth + window.scrollMaxX;
                yScroll = window.innerHeight + window.scrollMaxY;
            } else if (document.body.scrollHeight > document.body.offsetHeight) {
                xScroll = document.body.scrollWidth;
                yScroll = document.body.scrollHeight;
            } else {
                xScroll = document.body.offsetWidth;
                yScroll = document.body.offsetHeight;
            }
            var windowWidth, windowHeight;
            if (self.innerHeight) {
                if (document.documentElement.clientWidth) {
                    windowWidth = document.documentElement.clientWidth;
                } else {
                    windowWidth = self.innerWidth;
                }
                windowHeight = self.innerHeight;
            } else if (document.documentElement && document.documentElement.clientHeight) {
                windowWidth = document.documentElement.clientWidth;
                windowHeight = document.documentElement.clientHeight;
            } else if (document.body) {
                windowWidth = document.body.clientWidth;
                windowHeight = document.body.clientHeight;
            }
            if (yScroll < windowHeight) {
                pageHeight = windowHeight;
            } else {
                pageHeight = yScroll;
            }
            if (xScroll < windowWidth) {
                pageWidth = xScroll;
            } else {
                pageWidth = windowWidth;
            }
            arrayPageSize = [pageWidth, pageHeight, windowWidth, windowHeight];
            return arrayPageSize;
        },
        _copyOf_getPageScroll: function () {
            var xScroll, yScroll;
            if (self.pageYOffset) {
                yScroll = self.pageYOffset;
                xScroll = self.pageXOffset;
            } else if (document.documentElement && document.documentElement.scrollTop) {
                yScroll = document.documentElement.scrollTop;
                xScroll = document.documentElement.scrollLeft;
            } else if (document.body) {
                yScroll = document.body.scrollTop;
                xScroll = document.body.scrollLeft;
            }
            return [xScroll, yScroll];
        }
    };
    $.fn.jLightbox.defaults = {
        itemSelector: '.thumb',
        photoSelector: '.lbPhotoItem',
        hiddenClass: 'hide',
        disabledClass: 'disabled',
        summaryFilter: 'jlightbox-summary',
        dataSelector: '.lbWrap',
        fallback: {
            width: 638,
            height: 359
        },
        filter: {
            selector: null
        },
        autoplay: true,
        playSpeed: 3000,
        excludesFromClick: null,
        paddingSize: null,
        resizeSpeed: 400,
        overlay: null,
        overlayBgColor: '#000',
        overlayOpacity: 0.8,
        imageLoading: '/imgml/lightbox/lightbox-ico-loading.gif',
        imageBtnClose: '/imgml/lightbox/lightbox-btn-close.gif',
        wrap: null,
        onInit: null,
        onClick: null,
        onFinish: null,
        onSummary: null,
        onSummaryInit: null,
        onBeforeImageSet: null,
        onAfterImageSet: null,
        onNextKey: null,
        onPrevKey: null,
        onCancel: null,
        onNext: null,
        onPrev: null,
        onKeyUp: null,
        onSpacebarKey: null
    };
}(jQuery));
(function ($) {
    $.fn.tooltip = function (options) {
        var defaults = {
            debug: false,
            console: false,
            tSelector: null,
            ajaxHref: true,
            cssClass: null,
            position: 'auto',
            cSelector: '#jquery-tooltip-content',
            isInner: false,
            imgLoading: '/images/ajax-loader.gif',
            lazyLoadImg: false,
            click: {
                fire: false,
                callback: null,
                once: false
            }
        };
        var options = $.extend(defaults, options);
        var _dHeight = $(document).height();
        var _dWidth = $(document).width();
        _appendDebug();
        var _t = _getTooltip();
        var $this;
        var _cExists = false;
        var _img = _getImage();
        var _tm = null;
        return this.each(function () {
            $this = $(this);
            var _c = _getContentSelector();
            _cExists = __exists(_c);
            if (options.click.fire) {
                $this.data('tooltip_once', options.click.once);
                if (options.click.once && $this.data('tooltip_fired')) {
                    return;
                }
                $this.bind('click.tooltip', function (evt) {
                    evt.preventDefault();
                    if (__exists(_c)) {
                        if (_c.html().length > 1) {
                            _t.html(_c.html());
                            _setCss(_t, evt).show(0);
                            var _$this = $(this);
                            _$this.data('tooltip_fired', true);
                            if (_$this.data('tooltip_once') && $this.data('tooltip_fired')) {
                                _$this.unbind('click.tooltip');
                            }
                            setTimeout(function () {
                                _t.hide();
                                if (options.click.callback) {
                                    options.click.callback.apply(this);
                                }
                            }, 1000);
                        }
                    }
                });
                return;
            }
            $this.bind('mouseenter.tooltip', function (evt) {
                _setClass();
                $(this).bind('mousemove.tooltip', function (evt) {
                    if (_img && _img.is(':visible')) {
                        _setCss(_img, evt);
                    } else {
                        _setCss(_t, evt);
                    }
                });
                var callback = function () {
                        if (options.lazyLoadImg) {
                            _c.html(_lazyLoad(_c.html()));
                        }
                        _setCss(_t, evt).show(0);
                    };
                if (options.ajaxHref) {
                    var _url = null;
                    if (options.isInner) {
                        _url = _c.html();
                    }
                    if (!_url) {
                        _url = $(evt.target).attr('href');
                    }
                    if (_url) {
                        _setCss(_img, evt).show();
                        $.get(_url, function (data) {
                            _img.hide();
                            _t.html(data);
                            callback();
                        });
                    } else {
                        callback();
                    }
                } else if (__exists(_c)) {
                    if (_c.html().length > 1) {
                        _t.html(_c.html());
                        callback();
                    }
                }
            }).bind('mouseleave.tooltip', function () {
                $(this).unbind('mousemove.tooltip');
                _t.hide();
            });
        });

        function _lazyLoad(_html) {
            var result = _html,
                re = new RegExp(/\$.img{src:'+([/\w\.-:]*)'}/gim),
                _m = re.exec(_html),
                i;
            if (_m) {
                for (i = 0; i < _m.length; i++) {
                    if (_m.indexOf('$.img') != -1) {
                        return;
                    }
                    _html = _html.replace(_m[i - 1], '<img src="' + _m[i] + '"/>');
                    _t.html(_html);
                    if (_cExists) {
                        result = _html;
                    }
                }
                _m = re.exec(result);
                if (_m) {
                    result = _lazyLoad(result);
                }
            }
            return result;
        };

        function _appendDebug() {
            if (options.debug && !__exists('#debug')) {
                $('body').append('<div id="debug"></div>');
            }
        };

        function _getTooltip() {
            var _o = null;
            if (options.tSelector) {
                _o = $(tSelector);
            } else if (!(_o = __exists('#jquery-tooltip'))) {
                _o = $('<div id="jquery-tooltip" style="display:none"></div>').appendTo('body');
            }
            return _o;
        };

        function _getImage() {
            var _o = null;
            if (options.ajaxHref) {
                if (!(_o = __exists('#jquery-tooltip-loader'))) {
                    _o = $('<img src="' + options.imgLoading + '" id="jquery-tooltip-loader" style="position:absolute;display:none"/>').appendTo('body');
                }
            }
            return _o;
        };

        function _setClass() {
            _t.attr('class', '');
            if (options.cssClass) {
                _t.addClass(options.cssClass);
            }
        };

        function _debug(s, append) {
            if (options.debug) {
                var _a = '';
                if (append) {
                    _a = $('#debug').html();
                }
                $('#debug').html(_a + s);
            } else if (options.console) {
                console.log(s);
            }
        };

        function _setCss(o, evt) {
            var result = {};
            switch (options.position) {
            case 'auto':
                var _left = evt.pageX + 15;
                var _top = evt.pageY + 10;
                var _cLeft = evt.pageX + o.outerWidth();
                if (_cLeft > _dWidth) {
                    _left = evt.pageX - o.outerWidth() - 7;
                }
                var _cTop = evt.pageY + _t.outerHeight();
                if (_cTop > _dHeight) {
                    _top = evt.pageY - o.outerHeight() + 5;
                }
                result = {
                    left: _left,
                    top: _top
                };
                break;
            case 'bl':
                result = {
                    left: evt.pageX - o.outerWidth() - 7,
                    top: evt.pageY + 10
                };
                break;
            case 'bc':
                result = {
                    left: evt.pageX - (o.outerWidth() / 2),
                    top: evt.pageY + 10
                };
                break;
            case 'br':
                result = {
                    left: evt.pageX + 15,
                    top: evt.pageY + 10
                };
                break;
            case 'rc':
                result = {
                    left: evt.pageX + 15,
                    top: evt.pageY - (o.outerHeight() / 2)
                };
                break;
            case 'tr':
                result = {
                    left: evt.pageX + 15,
                    top: evt.pageY - o.outerHeight() + 5
                };
                break;
            case 'tc':
                result = {
                    left: evt.pageX - (o.outerWidth() / 2),
                    top: evt.pageY - o.outerHeight() + 5
                };
                break;
            case 'tl':
                result = {
                    left: evt.pageX - o.outerWidth() - 7,
                    top: evt.pageY - o.outerHeight() + 5
                };
                break;
            case 'lc':
                result = {
                    left: evt.pageX - o.outerWidth() - 7,
                    top: evt.pageY - (o.outerHeight() / 2)
                };
                break;
            }
            _debug(result.left + ' ' + result.top + ' ');
            return o.css(result);
        };

        function _getContentSelector() {
            return options.isInner ? $($this).find(options.cSelector) : $(options.cSelector);
        }
        function __exists(sel) {
            var _s = $(sel);
            return _s.length > 0 ? _s : null;
        }
    };
}(jQuery));
(function ($) {
    $.fn.jPhotoReader = function (opts) {
        return $.fn.jPhotoReader.methods.init(this, $.extend({}, $.fn.jPhotoReader.defaults, opts));
    };
    $.fn.jPhotoReader.defaults = {
        carouselSelector: '#items-carousel',
        singleItemSelector: '.thumbs-single-item',
        imageSelector: '#frame img.photo',
        linkSelector: '#frame a.nL',
        currentCssClass: 'current',
        visibleItem: 4,
        scrollItems: 4,
        horizontal: false,
        autoplay: false,
        dataCfg: {
            selector: '#frame .figcaption',
            ajaxUrl: '/_galleries/library/galleryid={0}/photodetails.html',
            onSuccessLoad: null
        }
    };
    $.fn.jPhotoReader.methods = {
        xhr: null,
        init: function ($this, opts) {
            var self = this;
            self.$data = $(opts.dataCfg.selector);
            self.opts = opts;
            self.$ctx = $this;
            self.$carousel = $this.find(opts.carouselSelector);
            $this.find('#jcarouselReader-btn-playPause').bind('click', function (e) {
                e.preventDefault();
                $thisbutton = $(this);
                $thisbutton.toggleClass('jcarouselReader-play');
                if ($thisbutton.hasClass('jcarouselReader-play')) {
                    $thisbutton.removeClass('jcarouselReader-pause');
                    $thisbutton.attr('title', 'Play');
                    opts.autoplay = false;
                    $thisbutton.html('Play');
                    clearTimeout(self.$timer);
                    return;
                }
                $thisbutton.addClass('jcarouselReader-pause');
                $thisbutton.attr('title', 'Pause');
                opts.autoplay = true;
                self.autoMove();
                $thisbutton.html('Pause');
            });
            self.$carousel.bind('click', function (e) {
                self.move.apply(self, [e]);
            });
            $.execIfExists(self.$carousel, 'jCarousel', {
                horizontal: opts.horizontal,
                visible: opts.visibleItem,
                scroll: opts.scrollItems
            });
            if (opts.autoplay) {
                self.autoMove();
            }
            return $this;
        },
        autoMove: function () {
            var self = this;
            if (self.$timer) {
                clearTimeout(self.$timer);
            }
            self.$timer = setTimeout(function (s) {
                var idx = self.$carousel.find('.' + self.opts.currentCssClass).index();
                if (!idx) {
                    idx = 0;
                }
                idx++;
                if (idx > self.$carousel.find(self.opts.singleItemSelector).length - 1) {
                    idx = 0;
                }
                if (idx % self.opts.visibleItem == 0) {
                    self.$carousel.data('__jcarousel').inner.scroll(idx, true);
                }
                clearTimeout(s);
                self.move.apply(self, [null, idx, self.autoMove]);
            }, 5000);
        },
        move: function (e, idx, callback) {
            var self = this;
            var transitionSpeed = 100;
            var $target = !isNaN(idx) ? self.$carousel.find('.' + self.opts.singleItemSelector + ':eq(' + idx + ')') : $(e.target);
            if (!($target.is(self.opts.singleItemSelector) || $target.parents(self.opts.singleItemSelector).length)) {
                return;
            }
            if (e) {
                e.preventDefault();
            }
            if (!$target.is(self.opts.singleItemSelector)) {
                $target = $target.parents(self.opts.singleItemSelector);
            }
            self.$carousel.find(self.opts.singleItemSelector).removeClass(self.opts.currentCssClass);
            $target.addClass(self.opts.currentCssClass);
            var changeItemCallback = function () {
                    var $a = $target.find('a:first');
                    var href = $a.attr('data-href') || $a.attr('href');
                    self.$ctx.find(self.opts.linkSelector).attr('href', $a.attr('href'));
                    var imgPreloader = new Image();
                    imgPreloader.onload = function () {
                        self.$ctx.find(self.opts.imageSelector).attr('src', imgPreloader.src);
                        imgPreloader.onload = function () {};
                        if (self.opts.autoplay) {
                            self.autoMove.apply(self);
                        } else {
                            clearTimeout(self.$timer);
                        }
                    };
                    imgPreloader.src = href;
                };
            if ($target[0].dataValue && $target[0].dataValue.length > 0) {
                self.$data.html($target[0].dataValue);
                self.$data.fadeOut(transitionSpeed, function () {
                    changeItemCallback();
                    self.$data.fadeIn(transitionSpeed);
                });
                return;
            }
            if (self.xhr) {
                self.xhr.abort();
            }
            self.xhr = $.ajax({
                url: self.opts.dataCfg.ajaxUrl.replace('{0}', $target.searchCssClass('item-')),
                type: 'GET',
                success: function (html) {
                    if (html.length > 1) {
                        self.$data.fadeOut(transitionSpeed, function () {
                            changeItemCallback();
                            self.$data.html(html);
                            $target[0].dataValue = html;
                            if (self.opts.dataCfg.onSuccessLoad) {
                                self.opts.dataCfg.onSuccessLoad.apply(self, [html]);
                            }
                            self.$data.fadeIn(transitionSpeed);
                        });
                    }
                }
            });
        }
    };
}(jQuery));
(function ($) {
    $.fn.medalStandings = function (options) {
        return $.fn.medalStandings.methods.init(this, $.extend({}, $.fn.medalStandings.defaults, options));
    };
    $.fn.medallists = function (options) {
        return $.fn.medallists.methods.init(this, $.extend({}, $.fn.medallists.defaults, options));
    };
    $.fn.historicalMedalStandings = function (options) {
        return $.fn.historicalMedalStandings.methods.init(this, $.extend({}, $.fn.historicalMedalStandings.defaults, options));
    };
    $.fn.medalStandings.methods = {
        init: function ($this, opts) {
            var $ctx = $this;
            if ($ctx.is(".sortable")) {
                $.execIfExists($ctx, 'tablesorter', {
                    headers: {
                        0: {
                            sorter: false
                        },
                        1: {
                            sorter: false
                        },
                        2: {
                            sorter: false
                        },
                        3: {
                            sorter: false
                        },
                        4: {
                            sorter: false
                        },
                        5: {
                            sorter: false
                        },
                        6: {
                            sorter: false
                        },
                        7: {
                            sorter: false
                        },
                        9: {
                            sorter: false
                        },
                        10: {
                            sorter: false
                        },
                        11: {
                            sorter: false
                        }
                    },
                    widgets: ['zebra']
                });
                $ctx.find('.view').removeClass('header').addClass('{sorter:false}nosort');
                var $barColumns = $ctx.find('td:nth-child(6)').add($ctx.find('th.all_medals')).hide();
                var $barTableItems = $ctx.find('th.medal,td.gold,td.silver,td.bronze');
                $ctx.find('.switchTable').bind('click', function (e) {
                    var $target = $(e.target);
                    if (!($target.is('a'))) {
                        return true;
                    }
                    e.preventDefault();
                    var $this = $(this);
                    var $active = $this.find('.active');
                    if ($active.is($target)) {
                        return;
                    }
                    var toggleMethodName = $target.is('.show_grid') ? 'hide' : 'show';
                    $this.find('a').removeClass('active');
                    $target.addClass('active');
                    $barColumns[toggleMethodName]();
                    toggleMethodName = toggleMethodName == 'hide' ? 'show' : 'hide';
                    $barTableItems[toggleMethodName]();
                });
            }
        }
    };
    $.fn.historicalMedalStandings.methods = {
        init: function ($this, opts) {
            var $ctx = $this;
            if ($ctx.is(".sortable")) {
                $.execIfExists($ctx, 'tablesorter', {
                    headers: {
                        0: {
                            sorter: false
                        },
                        1: {
                            sorter: false
                        },
                        2: {
                            sorter: false
                        },
                        3: {
                            sorter: false
                        },
                        4: {
                            sorter: false
                        },
                        5: {
                            sorter: false
                        },
                        6: {
                            sorter: false
                        },
                        8: {
                            sorter: false
                        },
                        9: {
                            sorter: false
                        },
                        10: {
                            sorter: false
                        }
                    },
                    widgets: ['zebra']
                });
                $ctx.find('.view').removeClass('header').addClass('{sorter:false}nosort');
                var $barColumns = $ctx.find('td:nth-child(5)').add($ctx.find('th.all_medals')).hide();
                var $barTableItems = $ctx.find('th.medal,td.gold,td.silver,td.bronze');
                $ctx.find('.switchTable').bind('click', function (e) {
                    var $target = $(e.target);
                    if (!($target.is('a'))) {
                        return true;
                    }
                    e.preventDefault();
                    var $this = $(this);
                    var $active = $this.find('.active');
                    if ($active.is($target)) {
                        return;
                    }
                    var toggleMethodName = $target.is('.show_grid') ? 'hide' : 'show';
                    $this.find('a').removeClass('active');
                    $target.addClass('active');
                    $barColumns[toggleMethodName]();
                    toggleMethodName = toggleMethodName == 'hide' ? 'show' : 'hide';
                    $barTableItems[toggleMethodName]();
                });
            }
        }
    };
    $.fn.medallists.methods = {
        init: function ($this, opts) {
            var $ctx = $this;
            if ($ctx.is(".sortable")) {
                $.execIfExists($ctx, 'tablesorter', {
                    headers: {
                        0: {
                            sorter: false
                        },
                        1: {
                            sorter: false
                        },
                        2: {
                            sorter: false
                        }
                    },
                    widgets: ['zebra']
                });
            }
        }
    };
}(jQuery));
window.$core = jQuery.extend({}, window.$core, {
    jCarousel: function ($) {
        $.fn.jCarousel = function (options) {
            return this.each(function () {
                var $this = $(this);
                $this.data('__jcarousel', $.fn.jCarousel.methods.init($this, $.extend({}, $.fn.carousels.defaults, options)));
            });
        };
        $.fn.jCarousel.methods = {
            inner: null,
            $container: null,
            $clip: null,
            opts: null,
            $timer: null,
            $child: null,
            childLength: null,
            init: function ($this, opts) {
                var self = $.extend({}, $.fn.carousels.methods.coreInit($this, opts));
                self.inner = $.extend({}, this);
                if (self.childLength < 2) {
                    if (self.opts.itemFirstInCallback) {
                        self.opts.itemFirstInCallback.apply(self, [self.$ctx, self.$child.eq(0), -1]);
                    }
                    return;
                }
                self.inner.opts = opts;
                self.inner.$child = self.$child;
                self.inner.childLength = self.childLength;
                self.inner.$container = self.$ctx.closest('.' + self.opts.container);
                self.inner.$clip = self.$ctx.closest('.' + self.getSelector(self.opts.clip));
                var datainjectNav = self.$ctx.attr('data-injectnav');
                if (datainjectNav) {
                    self.opts.injectNav = datainjectNav == 'true';
                }
                if (self.opts.injectNav) {
                    self.inner.$nav = $('<a href="#prev" class="' + self.opts.hiddenClass + ' ' + self.opts.prev + ' ' + self.getSelector(self.opts.prev) + '" style="display:block;"></a><a href="#next" class="' + self.opts.next + ' ' + self.getSelector(self.opts.next) + '" style="display:block;"></a>').appendTo(self.inner.$container);
                    self.inner.$container.bind('click', function (e) {
                        var _$this = $(e.target);
                        if (_$this.is('.' + self.opts.prev) || _$this.is('.' + self.opts.next)) {
                            e.preventDefault();
                            var carousel = _$this.closest('.' + self.opts.container).find('.' + self.opts.carouselClass);
                            self.inner.move.apply(self, [_$this.is('.' + self.opts.prev), _$this.closest('.' + self.opts.container), carousel]);
                        }
                    });
                    self.inner.setNavigation.apply(self, [self.inner.$container, self.$ctx]);
                }
                self.$ctx.data('__jcarousel', self);
                if (self.opts.initCallback) {
                    self.opts.initCallback.apply(self, [self.$ctx]);
                }
                return self;
            },
            move: function (prev, container, carousel, isScroll, animateDone, refresh) {
                var self = this;
                carousel.stop(true, false);
                if (refresh) {
                    self.refresh(true);
                }
                var idx = carousel.data('actual-idx');
                if (!idx) {
                    idx = 0;
                }
                if (!isScroll) {
                    idx += self.opts.scroll * (prev ? -1 : 1);
                }(self.inner || self)['setNavigation'].apply(self, [container, carousel, idx]);
                carousel.data('actual-idx', idx);
                var $li = self.$child.eq(idx);
                var posProperty = self.getPosProperty();
                var liPos = $li.position()[posProperty];
                var carouselPos = carousel.position()[posProperty];
                if (Math.abs(carouselPos) === liPos) {
                    return;
                }
                var animObj = {};
                animObj[posProperty] = '-' + (liPos) + 'px';
                carousel.animate(animObj, 'normal', function () {
                    if (self.opts.itemFirstInCallback && !isScroll) {
                        self.opts.itemFirstInCallback.apply(self, [$(this), $li, idx]);
                    }
                    if (animateDone) {
                        animateDone.apply(self, [$(this), $li, idx]);
                    }
                });
            },
            scroll: function (index, invokeItemCallback, callback, refresh) {
                var self = this;
                var carousel = self.$container.find('.' + self.opts.carouselClass);
                var actualIdx = carousel.data('actual-idx');
                carousel.data('actual-idx', index);
                self.move.apply(self, [actualIdx > index, self.$container, carousel, true, function ($this, $li, idx) {
                    if (invokeItemCallback && self.opts.itemFirstInCallback) {
                        self.opts.itemFirstInCallback.apply(self, [$this, $li, idx]);
                    }
                    if (callback) {
                        callback.apply(self);
                    }
                },
                refresh]);
            },
            setNavigation: function (container, carousel, idx) {
                if (typeof (idx) == typeof (undefined)) {
                    idx = carousel.data('actual-idx') || 0;
                }
                var self = this;
                self.$prev = self.$prev || container.find('.' + self.opts.prev);
                self.$next = self.$next || container.find('.' + self.opts.next);
                if (idx > self.childLength) {
                    idx = self.childLength;
                }
                var page = Math.floor(idx / self.opts.scroll) + 1,
                    maxPage = self.childLength / self.opts.scroll;
                if (maxPage <= 1) {
                    self.$prev.add(self.$next).addClass(self.opts.hiddenClass);
                    return;
                }
                if (Math.floor(maxPage) < maxPage) {
                    maxPage = Math.floor(maxPage) + 1;
                }
                if (page <= 1) {
                    self.$prev.addClass(self.opts.hiddenClass);
                    self.$next.removeClass(self.opts.hiddenClass);
                } else if (page === maxPage) {
                    self.$prev.removeClass(self.opts.hiddenClass);
                    self.$next.addClass(self.opts.hiddenClass);
                } else {
                    self.$prev.add(self.$next).removeClass(self.opts.hiddenClass);
                }
            },
            prev: function (refresh) {
                this.__move(true, refresh);
            },
            next: function (refresh) {
                this.__move(false, refresh);
            },
            __move: function (prev, refresh) {
                var self = this;
                var carousel = self.$container.find('.' + self.opts.carouselClass);
                self.move.apply(self, [prev, self.$container, carousel, false, false, refresh]);
            },
            getPosProperty: function () {
                return this.opts.horizontal ? 'left' : 'top';
            },
            refresh: function (alsoWidth) {
                var self = this;
                if (!self.$ctx) {
                    self.$ctx = self.$container.find('.' + self.opts.carouselClass);
                }
                self.$child = self.$ctx.children(self.opts.child).css('float', 'left').addClass(self.opts.item);
                self.childLength = self.$child.length;
                if (self.inner) {
                    self.inner.$child = self.$child;
                    self.inner.childLength = self.childLength;
                }
                if (alsoWidth) {
                    if (!self.childSingleWidth) {
                        self.childSingleWidth = self.$child.outerWidth(true);
                    }
                    self.$ctx.width(Math.ceil(self.$child.length * self.childSingleWidth));
                }
            }
        };
        $.fn.jCarousel.defaults = {};
    },
    circularCarousel: function ($) {
        $.fn.circularCarousel = function (opts) {
            return this.each(function () {
                $.fn.circularCarousel.methods.init($(this), $.extend({}, $.fn.carousels.defaults, opts));
            });
        };
        $.fn.circularCarousel.methods = {
            inner: null,
            init: function ($this, opts) {
                var self = $.extend({}, $.fn.carousels.methods.coreInit($this, opts));
                self.inner = $.extend({}, this);
                self.inner.$child = self.$child.removeClass(opts.hiddenClass);
                if (self.opts.timed) {
                    self.inner.timed.apply(self, [self.$ctx]);
                }
                self.$container = self.$ctx.closest('.' + self.opts.container);
                if (self.opts.randomStart) {
                    var min = 0;
                    var max = self.$child.length - 2;
                    var randomIndex = Math.floor(Math.random() * (max - min + 1)) + min;
                    var randomLeft = '-' + randomIndex * self.childSingleWidth + 'px';
                    self.$ctx.css('left', randomLeft);
                    var _childForCssClass = self.inner.$child.eq(randomIndex);
                    if (_childForCssClass.find('div.article').length) {
                        self.$container.removeClass('pr bl or pk');
                        self.$container.addClass(_childForCssClass.find('div.article').parent().attr('class'));
                    }
                }
                if (self.opts.injectNav) {
                    if (self.opts.timed) {
                        $nav = $('<div class="jcarousel-control navigator"><a href="#prev" class=" prev ' + self.opts.prev + ' ' + self.getSelector(self.opts.prev) + '" style="display:block;">Previous</a><a href="#playPause" class="btn jcarousel-pause" id="jcarousel-btn-playPause" title="Pause">Pause</a><a href="#next" class=" next ' + self.opts.next + ' ' + self.getSelector(self.opts.next) + '" style="display:block;">Next</a></div>').appendTo(self.$container);
                    } else {
                        $nav = $('<a href="#prev" class=" ' + self.opts.prev + ' ' + self.getSelector(self.opts.prev) + '" style="display:block;">Previous</a><a href="#next" class=" ' + self.opts.next + ' ' + self.getSelector(self.opts.next) + '" style="display:block;">Next</a>').appendTo(self.$container);
                    }
                    self.$container.find('.' + self.opts.prev).bind('click', function (e) {
                        e.preventDefault();
                        self.inner.onPrevClick.apply(self, [$this]);
                    });
                    self.$container.find('.' + self.opts.next).bind('click', function (e) {
                        e.preventDefault();
                        self.inner.onNextClick.apply(self, [$this]);
                    });
                    self.$container.find('#jcarousel-btn-playPause').bind('click', function (e) {
                        e.preventDefault();
                        $thisbutton = $(this);
                        if ($thisbutton.hasClass('jcarousel-pause')) {
                            self.inner.onPauseClick.apply(self, [$this]);
                            $thisbutton.html('Play').attr('title', 'Play');
                        } else {
                            self.inner.timed.apply(self, [$this]);
                            $thisbutton.html('Pause').attr('title', 'Pause');
                        }
                        $thisbutton.toggleClass('jcarousel-pause jcarousel-play');
                    });
                }
                self.inner.$clip = self.$ctx.closest('.' + self.getSelector(self.opts.clip));
            },
            move: function (carousel) {
                var self = this;
                var idx = carousel.data('actual-idx');
                if (!idx) {
                    idx = 0;
                }
                idx++;
                carousel.data('actual-idx', idx);
                var left = '-=' + self.childSingleWidth + 'px';
                var newLeft = parseInt(carousel.css('left'), 10) - self.childSingleWidth;
                if (Math.abs(newLeft) >= carousel.outerWidth()) {
                    left = '-' + carousel.outerWidth() + 'px';
                }
                var childPosition = Math.abs(newLeft / self.childSingleWidth);
                var _childForCssClass = $(carousel).find(self.opts.child + ':eq(' + (childPosition) + ')');
                if (_childForCssClass.find('div.article').length != 0) {
                    self.$container.removeClass('pr bl or pk');
                    self.$container.addClass(_childForCssClass.find('div.article').parent().attr('class'));
                }
                carousel.animate({
                    left: left
                }, 'normal', function () {
                    var $this = $(this);
                    var _child = $this.find(self.opts.child + ':eq(' + (idx - 1) + ')').clone().appendTo($this);
                    var _parentChild = $this.find(self.opts.child + ':eq(' + (idx) + ')');
                    $this.width($this.outerWidth() + self.childSingleWidth);
                    self.inner.timed.apply(self, [$this]);
                });
            },
            onNextClick: function (carousel) {
                var self = this;
                var idx = carousel.data('actual-idx');
                if (!idx) {
                    idx = 0;
                }
                idx++;
                carousel.data('actual-idx', idx);
                self.inner.onPauseClick.apply(self, [carousel]);
                var $playStopButton = self.$container.find('#jcarousel-btn-playPause');
                $playStopButton.html('Play');
                $playStopButton.removeClass('jcarousel-pause');
                if (!$playStopButton.hasClass('jcarousel-play')) {
                    $playStopButton.addClass('jcarousel-play');
                }
                $playStopButton.attr('title', 'Play');
                var left = '-=' + self.childSingleWidth + 'px';
                var newLeft = parseInt(carousel.css('left'), 10) - self.childSingleWidth;
                if (Math.abs(newLeft) >= carousel.outerWidth()) {
                    left = '-' + carousel.outerWidth() + 'px';
                }
                var childPosition = Math.abs(newLeft / self.childSingleWidth);
                var _childForCssClass = $(carousel).find(self.opts.child + ':eq(' + (childPosition) + ')');
                if (_childForCssClass.find('div.article').length != 0) {
                    self.$container.removeClass('pr bl or pk');
                    self.$container.addClass(_childForCssClass.find('div.article').parent().attr('class'));
                }
                carousel.animate({
                    left: left
                }, 'normal', function () {
                    var $this = $(this);
                    var _child = $this.find(self.opts.child + ':eq(' + (idx - 1) + ')').clone().appendTo($this);
                    $this.width($this.outerWidth() + self.childSingleWidth);
                });
            },
            onPrevClick: function (carousel) {
                var self = this;
                var idx = carousel.data('actual-idx');
                if (!idx) {
                    idx = 0;
                }
                idx--;
                carousel.data('actual-idx', idx);
                self.inner.onPauseClick.apply(self, [carousel]);
                var $playStopButton = self.$container.find('#jcarousel-btn-playPause');
                $playStopButton.html('Play');
                $playStopButton.removeClass('jcarousel-pause');
                if (!$playStopButton.hasClass('jcarousel-play')) {
                    $playStopButton.addClass('jcarousel-play');
                }
                $playStopButton.attr('title', 'Play');
                var _child = carousel.find(self.opts.child + ':eq(' + (self.$child.length - 1) + ')').clone();
                carousel.prepend(_child);
                carousel.width(carousel.outerWidth() + self.childSingleWidth);
                var left = '+=' + self.childSingleWidth + 'px';
                var newLeft = parseInt(carousel.css('left'), 10) - self.childSingleWidth;
                var carouselLeft = parseInt($(carousel).css('left'), 10) - self.childSingleWidth;
                $(carousel).css('left', carouselLeft);
                var childPosition = Math.abs(newLeft / self.childSingleWidth) - 1;
                var _childForCssClass = $(carousel).find(self.opts.child + ':eq(' + (childPosition) + ')');
                if (_childForCssClass.find('div.article').length != 0) {
                    self.$container.removeClass('pr bl or pk');
                    self.$container.addClass(_childForCssClass.find('div.article').parent().attr('class'));
                }
                carousel.animate({
                    left: left
                }, 'normal', function () {
                    var $this = $(this);
                });
            },
            onPauseClick: function (carousel) {
                var self = this;
                clearTimeout(self.$timer);
            },
            timed: function (carousel) {
                var self = this;
                self.$timer = setTimeout(function (s) {
                    clearTimeout(s);
                    self.inner.move.apply(self, [carousel]);
                }, self.opts.delay);
            }
        }
    }
});
(function ($) {
    $.fn.carousels = {};
    $.fn.carousels.methods = {
        opts: null,
        $ctx: null,
        $child: null,
        $timer: null,
        childSingleWidth: -1,
        childLength: -1,
        coreInit: function ($this, opts) {
            var self = this;
            self.opts = opts;
            var $list = $this;
            var isUl = $this.is('ul');
            if (!isUl) {
                $list = $this.find('ul:first');
            }
            self.$child = $list.find('>' + self.opts.child).filter(function () {
                return !($(this).find('.summary-wrap').length);
            }).css('float', 'left').addClass(self.opts.item);
            self.childLength = self.$child.length;
            self.$ctx = $list.addClass(opts.carouselClass).data('actual-idx', 0).css({
                padding: 0,
                margin: 0,
                top: 0,
                position: 'relative'
            });
            var property = self.getProperty();
            if (isUl) {
                $list.wrap('<div class="' + self.opts.container + ' ' + self.getSelector(self.opts.container) + '" style="position:relative;display:block"><div class="' + self.opts.clip + ' ' + self.getSelector(self.opts.clip) + '" style="position:relative;overflow:hidden;"></div></div>');
            } else {
                $this.addClass(opts.container).css({
                    position: 'relative',
                    display: 'block'
                });
                $list.wrap('<div class="' + self.opts.clip + ' ' + self.getSelector(self.opts.clip) + '" style="position:relative;overflow:hidden;"></div>');
            }
            self.childSingleWidth = self.$child.outerWidth(true);
            $list[property](Math.ceil(self.$child.length * self.childSingleWidth));
            return self;
        },
        refresh: function () {
            var self = this;
            self.$child = self.$ctx.children(self.opts.child).css('float', 'left').addClass(self.opts.item);
            self.childLength = self.$child.length;
            if (self.inner) {
                self.inner.$child = self.$child;
                self.inner.childLength = self.childLength;
            }
        },
        getSelector: function (elm) {
            return elm + '-' + (this.opts.horizontal ? 'horizontal' : 'vertical');
        },
        getProperty: function () {
            return this.opts.horizontal ? 'width' : 'height';
        },
        getPosProperty: function () {
            return this.opts.horizontal ? 'left' : 'top';
        }
    };
    $.fn.carousels.defaults = {
        child: 'li',
        carouselClass: 'jcarousel-list',
        clip: 'jcarousel-clip',
        container: 'jcarousel-container',
        hiddenClass: 'hidden',
        timer: null,
        timed: true,
        delay: 2500,
        prev: 'jcarousel-prev',
        next: 'jcarousel-next',
        item: 'jcarousel-item',
        horizontal: true,
        scroll: 1,
        visible: 1,
        randomStart: false,
        itemFirstInCallback: null,
        itemLastInCallback: null,
        initCallback: null,
        ajaxUrl: null,
        injectNav: true
    };
}(jQuery));
window.$core = jQuery.extend({}, window.$core, {
    accSwitcher: function ($) {
        $.fn.accSwitcher = function () {
            var $this = this,
                $cookie = localStorage.getItem('acc');
            if (!$cookie) {
                var defValue = 'acc-stdfonts_acc-stdsize';
                localStorage.setItem("acc", defValue, {
                    expires: 15,
                    path: '/'
                });
                $cookie = defValue;
            }
            var values = $cookie.split('_'),
                selectorFont = '#' + values[0],
                selectorSize = '#' + values[1];
            if (selectorFont === '#acc-default') {
                selectorFont = '#acc-stdfonts';
            }
            if (selectorSize === '#acc-default') {
                selectorSize = '#acc-stdsize';
            }
            $(selectorFont).closest('li').addClass('selected');
            $(selectorSize).closest('li').addClass('selected');
            $this.click(function (e) {
                var $target = $(e.target);
                if ($target.is('span')) {
                    e.preventDefault();
                    var $ul = $target.parents('ul[id]:first');
                    $ul.find('li').removeClass('selected');
                    $target.parents('li:first').addClass('selected');
                    var cookieValues = localStorage.getItem('acc').split('_');
                    var id = e.target.id;
                    if ($ul.attr('id').indexOf('fontsize') !== -1) {
                        cookieValues[0] = id;
                    } else if ($ul.attr('id').indexOf('styleswitch') !== -1) {
                        cookieValues[1] = id;
                        if (id == "acc-highvisi") {
                            selector = "acc-largestfonts";
                        } else {
                            selector = "acc-stdfonts";
                        }
                        cookieValues[0] = selector;
                        $('#' + selector).closest('li').closest('ul').find('.selected').removeClass('selected').end().end().addClass('selected');
                    }
                    $('html').removeClass(function (index, classes) {
                        var m = classes.match(/acc-\w+/g) || [];
                        return m.join(" ");
                    }).addClass(cookieValues.join(' '));
                    localStorage.setItem('acc', cookieValues.join('_'), {
                        expires: 15,
                        path: '/'
                    });
                }
            });
        };
    },
	accLoader: function ($) {  // ATOS
        $.fn.accLoader = function () {
				
				$cookie = localStorage.getItem('acc');
				if ($cookie){
					var cookieValues = $cookie.split('_');
					$('html').removeClass(function (index, classes) {
						var m = classes.match(/acc-\w+/g) || [];
						return m.join(" ");
					}).addClass(cookieValues.join(' '));
				}
		}
	},
    select: function ($) {
        $.fn.select = function (opts) {
            return $.fn.select.methods.init(this, $.extend({}, $.fn.select.defaults, opts));
        };
        $.fn.select.methods = {
            change: null,
            opts: null,
            __init: null,
            data: null,
            $ctx: null,
            $bC: null,
            init: function ($this, opts) {
                var self = this;
                self.opts = opts;
                self.$ctx = $this;
                self.$bC = $this.closest('.bC');
                if (opts.change) {
                    self.change = opts.change;
                    $this.unbind().bind('change', function () {
                        opts.change.apply(self, [this]);
                    });
                }
                if (opts.init) {
                    self.__init = opts.init;
                    self.__init.apply(self);
                }
                if (!self.opts.defaulValue) {
                    $this.val(self.opts.defaulValue);
                }
                return $this;
            }
        };
        $.fn.select.defaults = {
            change: null,
            init: null,
            defaulValue: null
        };
    },
    equalHeight: function ($) {
        $.fn.equalHeight = function (opts) {
            return this.each(function () {
                var $this = $(this);
                $.fn.equalHeight.methods.exec($this, $.extend({}, $.fn.equalHeight.defaults, opts));
            });
        };
        $.fn.equalHeight.methods = {
            exec: function ($ctx, opts) {
                if (opts.excludeSelector && $ctx.find(opts.excludeSelector).length) {
                    return $ctx;
                }
                var self = this;
                i = opts.selectors.length;
                while (i) {
                    var selector = opts.selectors[i - 1];
                    var $children = $ctx.find(selector);
                    if ($children.length < 2) {
                        i--;
                        continue;
                    }
                    var $last = $ctx.find('.last');
                    if ($last.length > 1) {
                        var perRow = $last.eq(0).index();
                        $last.each(function (idx, item) {
                            var maxIdx = perRow;
                            var minIdx = 0;
                            if (idx > 0) {
                                var page = $(item).closest('[class*=page-]').searchCssClass('page-');
                                maxIdx = $(item).index() + (page && page > 1 ? ((page - 1) * perRow) + perRow : 0);
                                minIdx = maxIdx - perRow;
                            }
                            if (minIdx > -1) {
                                self.innerExec.apply(self, [$children.slice(minIdx, maxIdx + 1), opts]);
                            }
                        });
                    } else {
                        self.innerExec.apply(self, [$children, opts]);
                    }
                    i--;
                }
                return $ctx;
            },
            innerExec: function ($children, opts) {
                if (!$children.length) {
                    return;
                }
                var maxHeight = -1;
                var self = this;
                $children.each(function () {
                    var $this = $(this);
                    var th = $this.height();
                    if (th > maxHeight) {
                        maxHeight = th;
                    }
                });
                $children.css((opts.toEm ? 'height' : 'min-height'), (opts.toEm ? (maxHeight / 16) + 'em' : maxHeight));
            }
        };
        $.fn.equalHeight.defaults = {
            selectors: ['.gallery-title,.athleteName', '.gallery-description'],
            toEm: false
        };
    },
    menu: function ($) {
        $.fn.menu = function (opts) {
            return $.fn.menu.methods.init(this, $.extend({}, $.fn.menu.defaults, opts));
        };
        $.fn.menu.methods = {
            $ctx: null,
            opts: null,
            init: function ($this, opts) {
                var self = this;
                if (!$this.length) {
                    return $this;
                }
                self.opts = opts;
                self.$ctx = $this;
                self.setItem(document.location.pathname);
            },
            setItem: function (path) {
                var self = this;
                var noSlashPath = path[path.length - 1] == '/' ? path.substr(0, path.length - 1) : path,
                    noIndexPath = path.replace('/index.html', ''),
                    $exactItems = self.$ctx.find('li a[href="' + path + '"]').add(self.$ctx.find('li a[href="' + noIndexPath + '"]')).add(self.$ctx.find('li a[href="' + noSlashPath + '"]')).add(self.$ctx.find('li a[href="' + path + (path[path.length - 1] != '/' ? '/' : '') + 'index.html"]')).attr('data-found', 'true'),
                    exactWithVs = [],
                    $vsItems = [];
                if (path.indexOf('=') !== -1) {
                    exactWithVs = self.$ctx.find('li a[href*="="][data-found="true"]');
                    if (!exactWithVs.length) {
                        $vsItems = self.$ctx.find('li a[href*="="]').filter(function () {
                            var $this = $(this),
                                _ulDom, p = this.parentNode;
                            while (true) {
                                if (p.nodeName && p.nodeName.toLowerCase() === 'ul') {
                                    _ulDom = p;
                                    break;
                                }
                                p = p.parentNode;
                            }
                            if (_ulDom && $(_ulDom).find('li a[data-found="true"]').length) {
                                return false;
                            }
                            var href = $this.attr('href');
                            if (href[href.length - 1] == '/') {
                                if (href.indexOf('/index.html') == -1) {
                                    href += 'index.html';
                                }
                            } else if (href.indexOf('.html') == -1) {
                                href += '/index.html';
                            }
                            var stripHref = self.stripValueset(href),
                                stripPath = self.stripValueset(path);
                            var result = stripHref == stripPath;
                            if (!result) {
                                result = stripHref == (stripPath + (stripPath[stripPath.length - 1] === '/' ? '' : '/') + 'index.html');
                            }
                            return result;
                        });
                        self.setDataFound($vsItems);
                    }
                }
                var $itemsWithScore = [],
                    _host = 'http:/' + '/' + window.location.host,
                    $ulFound = self.$ctx.find('li a[data-found="true"]').closest('ul'),
                    ullength = $ulFound.length;
                self.$ctx.find('li a[href!="#"]').each(function (i) {
                    var $this = $(this),
                        href = this.getAttribute('href');
                    if (!(href && href.length)) {
                        return;
                    }
                    href = href.replace(_host, '');
                    if (href.indexOf('http:\/\/') === 0) {
                        return;
                    }
                    if (ullength) {
                        var $ul, _isSameUl = false,
                            _ulDom, p = this.parentNode;
                        while (true) {
                            if (p.nodeName && p.nodeName.toLowerCase() === 'ul') {
                                _ulDom = p;
                                break;
                            }
                            p = p.parentNode;
                        }
                        $ulFound.each(function () {
                            if (_isSameUl) {
                                return;
                            }
                            if (this === _ulDom) {
                                _isSameUl = true;
                            }
                        });
                        if (_isSameUl) {
                            return;
                        }
                    }
                    if (href[href.length - 1] === '/') {
                        if (href.indexOf('/index.html') === -1) {
                            href += 'index.html';
                        }
                    } else {
                        href += '/index.html';
                    }
                    if (href.indexOf('{') !== -1) {
                        return;
                    }
                    if (href === path) {
                        self.setDataFound($this);
                        return;
                    }
                    var urlParts = path.split('/'),
                        l = urlParts.length,
                        hrefParts = href.split('/'),
                        hl = hrefParts.length;
                    if (urlParts[l - 1] === '') {
                        urlParts[l - 1] = 'index.html';
                    }
                    if (hl > l) {
                        return;
                    }
                    if (hrefParts[hl - 1] === '') {
                        hrefParts[hl - 1] = 'index.html';
                    }
                    var arrObj = self.getItem(urlParts, hrefParts);
                    if (!arrObj.found && arrObj.hasVs && !exactWithVs.length) {
                        arrObj = self.getItem(urlParts, hrefParts, true);
                        if (arrObj.found) {
                            $itemsWithScore.push({
                                item: $this,
                                score: arrObj.score
                            });
                        }
                    }
                    found = arrObj.found;
                    if (arrObj.found) {
                        $itemsWithScore.push({
                            item: $this,
                            score: arrObj.score
                        });
                    }
                });
                $itemsWithScore = $itemsWithScore.length > 1 ? $itemsWithScore.sort(function (a, b) {
                    var aScore = a.score;
                    var bScore = b.score;
                    if (aScore > bScore) {
                        return aScore - bScore;
                    }
                    return bScore - aScore;
                }) : ($itemsWithScore.length == 1 ? $itemsWithScore : []);
                var $itemsScore = [];
                if ($itemsWithScore.length > 1) {
                    var $itemMaxScore = $itemsWithScore[0].item,
                        i;
                    $itemsScore.push($itemMaxScore[0]);
                    var il = $itemsWithScore.length;
                    for (i = 0; i < il; i++) {
                        var _$item = $itemsWithScore[i].item;
                        if (_$item.closest('ul').is($itemMaxScore.closest('ul'))) {
                            continue;
                        }
                        $itemsScore.push(_$item[0]);
                    }
                } else if ($itemsWithScore.length == 1) {
                    $itemsScore.push($itemsWithScore[0].item[0]);
                }
                var $parent = $exactItems.add($vsItems).add($itemsScore).closest("li"),
                    $alias = !$parent.length ? self.$ctx.find('li a[alias]') : [];
                var dataRel = self.getDataRel($exactItems, $vsItems, $itemsScore, $alias);
                var drl = dataRel.length;
                if (drl) {
                    while (drl + 1) {
                        $parent = $parent.add(self.$ctx.find('li a[href="' + dataRel[drl - 1] + '"]').add(self.$ctx.find('li a[href="' + dataRel[drl - 1] + 'index.html"]')).closest('li'));
                        drl--;
                    }
                }
                $parent.not(self.opts.excludeSelector).addClass(self.opts.currentClass);
            },
            getDataRel: function () {
                var dataRel = [],
                    i;
                if (!arguments.length) {
                    return dataRel;
                }
                for (i = 0; i < arguments.length; i++) {
                    $.each(arguments[i], function () {
                        dataRel.push($(this).closest('li').attr('data-rel'));
                    });
                }
                return dataRel;
            },
            setDataFound: function ($item) {
                $item.attr('data-found', 'true');
            },
            getItem: function (urlParts, hrefParts, replaceVs) {
                var self = this,
                    result = {
                        found: false,
                        hasVs: false,
                        score: -1
                    },
                    hl = hrefParts.length,
                    newHref = hrefParts.join('/');
                if (replaceVs && newHref.indexOf('=') !== -1) {
                    newHref = self.stripValueset(hrefParts);
                }
                var newPath = urlParts.join('/');
                if (replaceVs && newPath.indexOf('=') !== -1) {
                    newPath = self.stripValueset(urlParts);
                }
                var hasVs = newPath.indexOf('{');
                while (newPath.length) {
                    var tPath = newPath + (newPath.indexOf('.htm') == -1 ? '/index.html' : '');
                    var tHref = newHref + (newHref.indexOf('.htm') == -1 ? '/index.html' : '');
                    if (tHref === tPath) {
                        result.found = true;
                        result.score = lidx;
                        return result;
                    }
                    var lidx = newPath.lastIndexOf('/');
                    if (lidx === -1) {
                        return result;
                    }
                    newPath = newPath.substr(0, lidx);
                }
                result.hasVs = hasVs;
                return result;
            },
            stripValueset: function (href) {
                if (typeof (href) !== 'string') {
                    href = href.join('/');
                }
                if (href.indexOf('=') === -1) {
                    return href;
                }
                var pattern = /([\w\d\s\-]*)=[\w\d\s\-]*/i,
                    vs = href.match(pattern);
                if (!vs.length) {
                    return href;
                }
                return href.replace(vs[0], '{' + vs[1] + '}');
            }
        };
        $.fn.menu.defaults = {
            currentClass: 'current',
            excludeSelector: null
        };
    },
    jWatch: function ($) {
        $.fn.jWatch = function (msTimer) {
            $.fn.jWatch.methods.calc(this, msTimer || 60000);
        };
        $.fn.jWatch.methods = {
            calc: function (ctx, msTimer) {
                var self = this,
                    dt = new Date(),
                    hour = dt.getUTCHours() + 1,
                    suffix = 'AM',
                    isPm = false,
                    minutes = dt.getUTCMinutes(),
                    firstTimer = msTimer,
                    ms = dt.getUTCSeconds() * 1000;
                if (hour > 12) {
                    hour = hour - 12;
                    isPm = true;
                    suffix = 'PM';
                }
                if (hour < 10) {
                    hour = ('0' + hour);
                }
                if (minutes < 10) {
                    minutes = '0' + minutes;
                }
                if (firstTimer > ms) {
                    firstTimer = firstTimer - ms;
                }
                window.__jwatch = setTimeout(function () {
                    self.calc.apply(self, [ctx, msTimer]);
                }, firstTimer);
                ctx.html(hour + ':' + minutes + ' ' + suffix);
            }
        };
    },
    jCountdown: function ($) {
        $.fn.jCountdown = function (opts) {
            return $.fn.jCountdown.methods.init(this, $.extend({}, $.fn.jCountdown.defaults, opts));
        };
        $.fn.jCountdown.methods = {
            one_day: 86400000,
            init: function ($this, opts) {
                if (!opts.startDate) {
                    var olympic = $this.searchCssClass('olympic-');
                    if (!olympic) {
                        return;
                    }
                    var isOlympic = olympic == 'true';
                    opts.startDate = isOlympic ? Date.UTC(2012, 6, 27, 20, 0, 0) : Date.UTC(2012, 7, 29, 19, 30, 0);
                }
                var self = this,
                    actualDt = new Date(),
                    nowmsec = Date.UTC(actualDt.getFullYear(), actualDt.getUTCMonth(), actualDt.getUTCDate(), actualDt.getUTCHours(), actualDt.getUTCMinutes(), actualDt.getUTCSeconds(), actualDt.getUTCMilliseconds()),
                    diffTicks = (opts.startDate - nowmsec) > 0 ? opts.startDate - nowmsec : 0,
                    diffDays = diffTicks / this.one_day,
                    diffFloorDays = Math.floor(diffDays),
                    diffHours = (diffDays - diffFloorDays) * 24,
                    diffFloorHours = Math.floor(diffHours),
                    diffMinutes = (diffHours - diffFloorHours) * 60,
                    diffFloorMinutes = Math.floor(diffMinutes),
                    diffSeconds = 60 - actualDt.getSeconds();
                diffSeconds = diffSeconds > 0 ? diffSeconds : 60;
                self.$dd = $('.jcountdown-dd', $this);
                self.$hh = $('.jcountdown-hh', $this);
                self.$mm = $('.jcountdown-mm', $this);
                self.setTimer(diffFloorDays, diffFloorHours, diffFloorMinutes + 1, diffSeconds * 1000);
            },
            setTimer: function (actualDays, actualHours, actualMinutes, timeout) {
                var self = this;
                if (window.__jctm) {
                    clearTimeout(window.__jctm);
                }
                window.__jctm = setTimeout(function () {
                    var newMinute = actualMinutes - 1;
                    var newHour = actualHours;
                    var newDays = actualDays;
                    if (newMinute < 0) {
                        newHour--;
                        newMinute = 59;
                    }
                    if (newMinute === 60) {
                        newHour--;
                        newMinute = 0;
                    }
                    if (newHour < 0) {
                        newDays--;
                        newHour = 23;
                    }
                    if (newHour == 24) {
                        newHour = 0;
                        newDays++;
                    }
                    self.setTimer.apply(self, [newDays, newHour, newMinute]);
                }, timeout || 60000);
                if (actualMinutes === 60) {
                    actualMinutes = 0;
                    actualHours--;
                }
                if (actualHours < 0) {
                    actualHours = 23;
                    actualDays--;
                }
                var padDay = (actualDays >= 100 ? actualDays : (actualDays >= 10 ? '0' + actualDays : '00' + actualDays)) + '',
                    padHour = (actualHours >= 10 ? actualHours : '0' + actualHours) + '',
                    padMinute = (actualMinutes >= 10 ? actualMinutes : '0' + actualMinutes) + '',
                    arrDay = padDay.split(''),
                    l = arrDay.length,
                    _html = '',
                    i = 0;
                for (i; i < l; i++) {
                    _html += ['<span class="digit', (i == l - 1 ? ' last' : ''), '">', arrDay[i], '</span>'].join('');
                }
                self.$dd.html(_html);
                _html = '';
                var arrHour = padHour.split('');
                l = arrHour.length;
                for (i = 0; i < l; i++) {
                    _html += ['<span class="digit', (i == l - 1 ? ' last' : ''), '">', arrHour[i], '</span>'].join('');
                }
                self.$hh.html(_html);
                _html = '';
                var arrMinute = padMinute.split('');
                l = arrMinute.length;
                for (i = 0; i < l; i++) {
                    _html += ['<span class="digit', (i == l - 1 ? ' last' : ''), '">', arrMinute[i], '</span>'].join('');
                }
                self.$mm.html(_html);
            }
        };
        $.fn.jCountdown.defaults = {
            startDate: null
        };
    },
    jTabs: function ($) {
        $.fn.jTabs = function (opts) {
            var href = window.location.href;
            return this.each(function () {
                $.fn.jTabs.methods.init($(this), $.extend({}, $.fn.jTabs.defaults, opts), href);
            });
        };
        $.fn.jTabs.methods = {
            opts: null,
            $contents: null,
            $items: null,
            init: function ($ctx, opts, href) {
                var $items = $ctx.find('.' + opts.tabs.itemClass),
                    tabsLength = $items.length;
                if (tabsLength === 0) {
                    return $ctx;
                }
                var self = this,
                    arrValueset = self.getValueset(href),
                    vsLength = arrValueset ? arrValueset.length : 0,
                    vsValues = [],
                    idxCurrent = -1,
                    tabCurrentClass = opts.tabs.currentClass;
                self.opts = opts;
                self.$items = $items;
                if (vsLength) {
                    $items.each(function (idx) {
                        var $this = $(this),
                            isCurrent = $this.hasClass(tabCurrentClass),
                            _$title = $(opts.tabs.titleSelector, this),
                            dataVs = _$title.attr('data-vs');
                        if (dataVs) {
                            var values = dataVs.split('|');
                            var valuesL = values.length,
                                i2 = 0;
                            for (i2; i2 < valuesL; i2++) {
                                if (isCurrent) {
                                    break;
                                }
                                var vs = values[i2],
                                    i = 0;
                                for (i; i < vsLength; i++) {
                                    var keypair = arrValueset[i].split('=');
                                    if (keypair[0] !== vs) {
                                        continue;
                                    }
                                    isCurrent = true;
                                    vsValues.push([keypair[1], '/' + keypair.join('=') + '/']);
                                    break;
                                }
                            }
                            if (idxCurrent > -1 && vsLength > 1) {
                                isCurrent = false;
                            }
                        }
                        if (isCurrent) {
                            idxCurrent = idx;
                        }
                    });
                }
                if (idxCurrent === -1) {
                    idxCurrent = 0;
                }
                self.$contents = $ctx.find(opts.tabs.contentSelector).attr('tabindex', '-1').addClass(opts.hiddenClass);
                self.$contents.eq(idxCurrent).attr('tabindex', '').removeClass(opts.hiddenClass);
                var $currents = $ctx.find('.' + tabCurrentClass),
                    currentLength = $currents.length,
                    valuesLength = vsValues.length,
                    i = valuesLength;
                while (i) {
                    var keypair = vsValues[i - 1],
                        $opt = self.$contents.find('option[value="' + keypair[0] + '"]');
                    if ($opt.length) {
                        $opt.attr('selected', 'selected');
                    }
                    $opt = self.$contents.find('option[value*="' + keypair[1] + '"]');
                    if ($opt.length) {
                        $opt.attr('selected', 'selected');
                    }
                    i--;
                }
                var wrapClass = '.' + opts.tabs.itemClass,
                    currentClass = opts.currentClass;
                if (!currentLength) {
                    $ctx.find('li:first').addClass(tabCurrentClass);
                } else {
                    $currents.removeClass(tabCurrentClass);
                    $items.eq(idxCurrent).addClass(tabCurrentClass);
                }
                if (opts.live || $ctx.attr('data-live') === '1') {
                    var _$ctx = (opts.parentSelector ? $(opts.parentSelector) : $ctx.parent());
                    _$ctx.data('__self', self);
                    _$ctx.on('click', opts.liveSelector, function (e) {
                        self.onClick.apply(self, [e]);
                    });
                } else {
                    $ctx.data('__self', self);
                    $ctx.bind('click', function (e) {
                        var _self = $(this).data('__self');
                        if (_self) {
                            _self.onClick.apply(_self, [e]);
                        }
                    });
                }
                return $ctx;
            },
            onClick: function (e) {
                var self = this,
                    target = e.target,
                    $target = $(target),
                    $parent = $target.parent();
                if (!$target.is('a')) {
                    $target = $target.closest('a');
                    $parent = $target.parent();
                }
                if (!($target.is('a') && $parent.is('.' + self.opts.tabs.itemClass))) {
                    return true;
                }
                var ref = $target.attr('href') || $target.attr('data-ref') || $target.attr('rel');
                if (!ref) {
                    return true;
                }
                if (ref.indexOf('http:') != -1) {
                    ref = ref.substr(ref.indexOf('#'));
                }
                $.fn.jTabs.selectedRef = ref;
                self.$contents.addClass(self.opts.hiddenClass).filter(ref).removeClass(self.opts.hiddenClass);
                self.$items.removeClass(self.opts.currentClass);
                $parent.addClass(self.opts.currentClass);
                e.preventDefault();
            },
            getValueset: function (href) {
                var pattern = /([\w]*=[\w-]*)/gi;
                return href.match(pattern);
            }
        };
        $.fn.jTabs.defaults = {
            tabs: {
                ulClassPrefix: 'tab-',
                currentClass: 'current',
                itemClass: 'tabItem',
                wrapSelector: '.tabWrap',
                titleSelector: '.tabTitle',
                contentSelector: '.tabContent'
            },
            currentClass: 'current',
            hiddenClass: 'hidden',
            live: false,
            parentSelector: null,
            liveSelector: null
        };
    },
    changeRedirect: function ($) {
        $.fn.changeRedirect = function () {
            var $this = this,
                pathname = document.location.pathname;
            newpath = pathname.lastIndexOf('/') == pathname.length ? pathname + 'index.html' : (pathname.indexOf('index.html') == -1 ? pathname + '/index.html' : pathname), i = 0;
            $this.find('option[value="' + pathname + '"],option[value="' + newpath + '"]').attr('selected', 'selected');
            $this.bind('change', function () {
                var url = $(this).val(),
                    matches = url.match(/({[\w]*})/gi),
                    ml;
                if (matches && (ml = matches.length)) {
                    for (i = ml; i > 0; i--) {
                        var m = matches[i - 1];
                        if (!m.length) {
                            continue;
                        }
                        var stripm = m.replace('{', '').replace('}', ''),
                            parts = pathname.split('/'),
                            pl = parts.length,
                            i2;
                        for (i2 = pl; i2 > 0; i2--) {
                            var p = parts[i2 - 1];
                            if (!p.length) {
                                continue;
                            }
                            if (p.indexOf(stripm + '=') !== 0) {
                                continue;
                            }
                            url = url.replace(m, p);
                            break;
                        }
                    }
                }
                if (url) {
                    window.location = url;
                }
                return false;
            });
        };
    },
    buttonExpander: function ($) {
        $.fn.buttonExpander = function (opts) {
            return $.fn.buttonExpander.methods.init(this, $.extend({}, $.fn.buttonExpander.defaults, opts));
        };
        $.fn.buttonExpander.methods = {
            opts: null,
            $ctx: null,
            init: function ($this, opts) {
                var self = this;
                self.opts = opts;
                self.$ctx = $this;
                if (opts.elementToExpand != null && $(opts.elementToExpand).length != 0) {
                    $this.unbind().bind('click', function (e) {
                        var _this = $(this);
                        _this.closest('.jcarousel').height('auto');
                        var _elToExpand = _this.closest('.newsComment').find(opts.elementToExpand).toggle();
                        if (!_elToExpand.is(':visible')) {
                            _this.html('Expand to read more');
                        } else {
                            _this.html('Click to collapse the text');
                        }
                        e.preventDefault();
                    });
                }
                return $this;
            }
        };
        $.fn.buttonExpander.defaults = {
            elementToExpand: null,
            textElementOpened: 'Click to collapse the text',
            textElementClosed: 'Expand to read more'
        };
    },
    zebraTable: function ($) {
        $.fn.zebraTable = function (options) {
            return $.fn.zebraTable.methods.init(this, $.extend({}, options));
        };
        $.fn.zebraTable.methods = {
            init: function ($this, opts) {
                var $ctx = $this;
                var $tbl = $ctx;
                if (!$ctx.is('table')) {
                    $tbl = $ctx.find('table');
                }
                $tbl.find('tr:odd').addClass("odd").end().find('tr:even').addClass("even");
                if ($tbl.is(".sortable")) {
                    $.execIfExists($tbl, 'tablesorter', {
                        headers: {
                            0: {
                                sorter: false
                            },
                            1: {
                                sorter: false
                            },
                            2: {
                                sorter: false
                            }
                        },
                        widgets: ['zebra']
                    });
                }
            }
        };
    },
    minLength: function ($) {
        $.fn.minLength = function (opts) {
            return this.each(function () {
                $.fn.minLength.methods.init($(this), $.extend({}, $.fn.minLength.defaults, opts));
            });
        };
        $.fn.minLength.methods = {
            init: function ($this, opts) {
                $this.bind('click', function (e) {
                    var tmpSearchText = $(this).parent().find(opts.txtSelector).val();
                    if (tmpSearchText.length < opts.minLength) {
                        alert(opts.errMessage);
                        e.preventDefault();
                    }
                });
            }
        };
        $.fn.minLength.defaults = {
            txtSelector: '.searchText',
            errMessage: 'Please insert at least three characters in the search field',
            minLength: 3
        };
    },
    publicationsSort: function ($) {
        $.fn.publicationsSort = function () {
            return $.fn.publicationsSort.methods.init(this);
        };
        $.fn.publicationsSort.methods = {
            $dataWrap: null,
            init: function ($this) {
                var self = this;
                self.$dataWrap = $('#publicationsList');
                var parts = window.location.href.replace('#', '').split('/');
                if (parts[parts.length - 1].indexOf('.htm') !== -1) {
                    parts = parts.splice(0, parts.length - 1);
                }
                var baseurl = parts.join('/') + '/library/_latest-';
                var _isAsc = null;
                var _lib = null;
                var sortOrder = null;
                return $this.bind('click', function (e) {
                    e.preventDefault();
                    $this.removeClass('selected');
                    var _$this = $(this).addClass('selected');
                    _isAsc = _$this.hasClass("asc");
                    _lib = _$this.attr('id').toLocaleLowerCase().indexOf('title') != -1 ? 'title' : 'theme';
                    sortOrder = _isAsc ? 'desc' : 'asc';
                    var url = _lib + '-' + sortOrder + '.html';
                    switch (_isAsc) {
                    case true:
                        _$this.removeClass('asc').addClass('desc');
                        break;
                    default:
                        _$this.removeClass('desc').addClass('asc');
                        break;
                    }
                    self._load.apply(self, [$this, _lib + sortOrder, baseurl + url]);
                });
            },
            _load: function ($sort, key, _url) {
                var self = this;
                var storedVal = $sort[key];
                if (storedVal && storedVal.length > 0) {
                    self.$dataWrap.html(storedVal);
                    return;
                }
                $.ajax({
                    type: "get",
                    url: _url,
                    success: function (result) {
                        self.$dataWrap.html(result);
                        $sort[key] = result;
                    },
                    error: function () {}
                });
            }
        };
    },
    external: function ($) {
        $.fn.external = function () {
            var host = window.location.host || window.location.hostname;
            var $o = this;
            var $f = $o.not('.internal,.external,.twitter-share-button,.twitter-follow-button').filter(function () {
                var href = this.getAttribute('href');
                if (!href) {
                    return false;
                }
                if (href.indexOf('http') !== -1) {
                    return false;
                }
                var exthost = href.match(/http[s]?:\/\/[\w\d\.\-]*/gi);
                if (!(exthost && exthost.length)) {
                    return false;
                }
                var basehost = exthost[0].split('/');
                return host !== (basehost[basehost.length - 1]);
            }).addClass('external');
            $o.not('.ldn-popup,.internal').bind('click', function (e) {
                e.preventDefault();
                var title = this.title.replace(/ /g, "").replace(/-/g, "").replace(/–/g, "").replace(/_/g, "").replace(/'/g, "").replace(/\(/g, "").replace(/\)/g, "");
                try {
                    var w = window.open(this.href, title);
                } catch (err) {
                    document.location = this.href;
                }
                w.focus();
            });
        };
    },
    preloadPhoto: function ($) {
        $.fn.preloadPhoto = function (opts) {
            var emptySrc = (opts && opts.emptySrc) || '/imgml/void.png';
            return this.find('img').each(function () {
                return $.fn.preloadPhoto.exec.apply(this, [emptySrc]);
            });
        };
        $.fn.preloadPhoto.exec = function (emptySrc) {
            var img = this;
            var imgPreloader = new Image(),
                src = img.src;
            imgPreloader.onload = function () {
                if (imgPreloader.width === 1 && imgPreloader.height === 1) {
                    img.src = emptySrc;
                }
                imgPreloader.onload = function () {};
            };
            imgPreloader.src = src;
        };
    },
    convertMeasure: function ($) {
        $.fn.convertMeasure = function () {
            var measure = localStorage.getItem('orpCommon_convert');
            if (!measure) {
                return this;
            }
            return this.each(function () {
                var $this = $(this),
                    v = this.innerHTML;
                if (v && v !== '-') {
                    if ($this.hasClass('or-h-conv')) {
                        var cm, ft, inch;
                        if (measure === 'metric') {
                            if (!$this.data('metric_h')) {
                                $this.data('metric_h', v);
                            } else {
                                this.innerHTML = $this.data('metric_h');
                            }
                        } else if (measure === 'imperial') {
                            $this.data('metric_h', v);
                            cm = parseFloat(v.substr(0, 4)) * 100;
                            ft = Math.round(cm / 30.48);
                            inch = parseInt((cm % 30.48) / 12, 10);
                            this.innerHTML = ft + '\' ' + (inch + (inch < 10 ? '0' : '')) + '\'\'';
                        }
                    } else if ($this.hasClass('or-w-conv')) {
                        if (measure === 'metric') {
                            if (!$this.data('metric_w')) {
                                $this.data('metric_w', v);
                            } else {
                                this.innerHTML = $this.data('metric_w');
                            }
                        } else if (measure === 'imperial') {
                            var kg = parseFloat(v.split(' ')[0]);
                            $this.data('metric_w', v);
                            this.innerHTML = parseInt(kg * 2.2) + ' lbs';
                        }
                    }
                }
            });
        };
    },
    selectAutoWidth: function ($) {
        $.fn.selectAutoWidth = function () {
            var userAgent = window.navigator.userAgent.toLowerCase();
            if (userAgent.indexOf('msie 8.') !== -1 || userAgent.indexOf('msie 7.') !== -1) {
                this.bind('mouseenter', function () {
                    var $el = $(this),
                        origWidth = $el.outerWidth();
                    if ($el.data('no-resize')) {
                        return;
                    }
                    if (!$el.data('origWidth')) {
                        $el.data('origWidth', origWidth);
                    }
                    $el.css('width', 'auto');
                    if ($el.outerWidth() < origWidth) {
                        $el.data('no-resize', '1');
                        $el.width($el.data('origWidth'));
                    }
                }).bind('mouseleave blur change', function () {
                    var $el = $(this);
                    if ($el.data('no-resize')) {
                        return;
                    }
                    $el.width($el.data('origWidth'));
                });
            }
            return this;
        };
    },
    toggleVisibility: function ($) {
        $.fn.toggleVisibility = function () {
            return this.unbind('click.toggle').bind('click.toggle', function (e) {
                e.preventDefault();
                var $this = $(this);
                var elemToToggle = $this.parent().find(".related");
                if (!elemToToggle.length) {
                    return;
                }
                var $tagList = $this.closest('.tagList');
                $tagList.find('.related:visible').not(elemToToggle).hide();
                $(elemToToggle).toggle();
                $this.toggleClass('down-arrow up-arrow');
            });
        }
    }
});
(function ($) {
    $.fn.setCurrentItem = function (opts) {
        return $.fn.setCurrentItem.methods.init(this, $.extend({}, $.fn.setCurrentItem.defaults, opts));
    };
    $.fn.setCurrentItem.methods = {
        opts: null,
        init: function ($this, opts) {
            var self = this;
            self.opts = opts;
            var idLi = "";
            if ($this.attr("id") != undefined) {
                idLi = $this.attr("id").replace(self.opts.scope, "item");
            }
            var current = $("#lcg-left li.current");
            if ($("#" + idLi).length != 0) {
                if (current.length != 0) {
                    current.removeClass("current").addClass("sub");
                }
                $("li#" + idLi).addClass("current");
            }
            if (current.length != 0 && $("#lcg-left li.current ul").length != 0) {
                current.addClass("sub");
            }
        }
    };
    $.fn.setCurrentItem.defaults = {
        scope: 'mnu-link'
    };
}(jQuery));
(function ($) {
    $.fn.radioSetup = function () {
        var name = "rdb-freetext";
        var results = new RegExp('[\\?&]' + name + '=([^&#]*)').exec(window.location.href);
        if (!results) {
            return 0;
        }
        if (results[1] === 'c') {
            $('.ftr_contains input').attr('checked', true);
            $('.ftr_starts input').attr('checked', false);
        } else {
            $('.ftr_contains input').attr('checked', false);
            $('.ftr_starts input').attr('checked', true);
        }
    };
}(jQuery));
(function ($) {
    $.fn.countryAthletesfilters = function (opts) {
        return $.fn.countryAthletesfilters.methods.init(this, $.extend({}, $.fn.countryAthletesfilters.defaults, opts));
    };
    $.fn.countryAthletesfilters.methods = {
        opts: null,
        $ctx: null,
        $list: null,
        $itemsCount: null,
        init: function ($this, opts) {
            var self = this;
            self.$ctx = $this;
            self.opts = opts;
            self.$list = $(opts.listWrap);
            self.$itemsCount = $(opts.itemsCount);
            self.$rbFilters = self.$ctx.find(opts.rbFilters).bind('click', function () {
                var $this = $(this);
                var value = $this.val();
                self.resetFilters.apply(self, ['gender']);
                if (value == 'all') {
                    self.clearFilter.apply(self);
                    return;
                }
                self.applyFilter.apply(self, ['.g-', value]);
            });
            self.$initials = self.$ctx.find(opts.initialWrap).bind('click', function (e) {
                var $target = $(e.target);
                if (!$target.is('a')) {
                    return true;
                }
                $target.addClass(opts.currentClass);
                e.preventDefault();
                var value = $target.text().toLowerCase();
                self.resetFilters.apply(self, ['initials']);
                if (value == 'all') {
                    self.clearFilter.apply(self);
                    return;
                }
                self.applyFilter.apply(self, ['.i-', value]);
            });
            self.$discFilter = self.$ctx.find(opts.discFilter).bind('change', function (e) {
                e.preventDefault();
                var value = $(this).val();
                self.resetFilters.apply(self, ['discipline']);
                if (!value) {
                    self.clearFilter.apply(self);
                    return;
                }
                self.applyFilter.apply(self, ['.d-', value]);
            });
            self.$freeText = self.$ctx.find(opts.freeTextWrap).find('a').bind('click', function (e) {
                e.preventDefault();
                var value = $(this).parent().find('input').val();
                self.resetFilters.apply(self, ['freetext']);
                if (!value) {
                    self.clearFilter.apply(self);
                    return;
                }
                self.applyFilter.apply(self, ['[class*="f-' + value + '"]', '']);
            }).end().find('input');
            self.$rbDefault = self.$rbFilters.find('gender-default');
            self.$initialsDefault = self.$initials.find('initial-default');
            self.$discDefault = self.$discFilter.find('option:first');
            self.resetFilters.apply(self);
            return $this;
        },
        clearFilter: function () {
            var self = this;
            self.$list.find('li.' + self.opts.hiddenClass).removeClass(self.opts.hiddenClass);
            self.$itemsCount.text(self.opts.itemsText + ' ' + self.$list.find('li').length);
        },
        applyFilter: function (selector, value) {
            var self = this;
            self.$list.find('li' + selector + value).removeClass(self.opts.hiddenClass);
            self.$list.find('li:not(' + selector + value + ')').addClass(self.opts.hiddenClass);
            self.$itemsCount.text(self.opts.itemsText + ' ' + self.$list.find('li:not(.' + self.opts.hiddenClass + ')').length);
        },
        resetFilters: function (notFilter) {
            var self = this;
            if (notFilter != 'freetext') {
                self.$freeText.val('');
            }
            if (notFilter != 'gender') {
                self.$rbFilters.find('input[checked]').attr('checked', '');
                self.$rbDefault.attr('checked', 'checked');
            }
            if (notFilter != 'initials') {
                self.$initials.find('a').removeClass(self.opts.currentClass);
                self.$initialsDefault.addClass(self.opts.currentClass);
            }
            if (notFilter != 'discipline') {
                self.$discFilter.find('option[selected]').attr('selected', '');
                self.$discDefault.attr('selected', 'selected');
            }
        }
    };
    $.fn.countryAthletesfilters.defaults = {
        rbFilters: '.genderFilters input[type="radio"]',
        freeTextWrap: '.freeTextAthletesSearch',
        initialWrap: '#alphaFilters',
        discFilter: '#athletefilterSport',
        listWrap: '#wrapAthletesList',
        itemsCount: '.athletesTotalNumber',
        itemsText: "Total number of athletes",
        hiddenClass: 'hidden',
        currentClass: 'current'
    };
}(jQuery));
(function ($) {
    $.fn.openPopupWindow = function () {
        var _dataAttr = $(this).attr('data-attr');
        var _params = 'scrollbars=no,menubar=no,toolbar=no,resizable=no';
        var _url = $(this).attr('href');
        if (_dataAttr != null && _dataAttr.length > 0) {
            _params += _dataAttr;
        }
        $(this).click(function (e) {
            e.preventDefault();
            window.open(_url, 'smartAgent', _params);
        });
    };
    $.fn.forceHref = function () {
        var href = window.location.href;
        var hashIdx = href.indexOf('#');
        if (hashIdx == -1) {
            return this;
        }
        href = href.substr(0, hashIdx);
        var path = window.location.pathname;
        return this.bind('click', function () {
            var $this = $(this);
            var origUrl = $this.attr('href');
            var iHashIdx = origUrl.indexOf('#');
            if (iHashIdx == -1) {
                return true;
            }
            var url = origUrl.substr(0, iHashIdx);
            if (url.indexOf('http:\/\/') != -1 && url == href) {
                window.location.href = origUrl;
                window.location.reload(true);
            } else if (url == path) {
                window.location.href = origUrl;
                window.location.reload(true);
            }
        });
    };
}(jQuery));
(function ($) {
    $.fn.videoPlayerEmbed = function () {
        var id = $(this).attr("id");
        if (id !== undefined && id.indexOf("_") > 0) {
            var values = id.split("_");
            var programmeId = values[0];
            var playersize = values[1];
            var url = "http://players.mediafreedom.twofourdigital.net/locog/script/config.js?id=" + programmeId;
            if (playersize == 'large') {
                width = 700;
                height = 480;
            } else {
                width = 448;
                height = 307;
            }
            var flashvars = {
                config: url,
                autostart: "true"
            };
            var params = {
                allowscriptaccess: "always",
                allowFullscreen: "true",
                opacity: "transparent",
                quality: "high",
                align: "middle",
                wmode: "transparent"
            };
            var attributes = {
                id: 'flashPlayer',
                name: 'flashPlayer'
            };
            swfobject.embedSWF("http://players.mediafreedom.twofourdigital.net/swf/player.swf", "videoplayer", width, height, "10.0.42.34", "/swf/expressinstall.swf", flashvars, params, attributes);
        }
    };
}(jQuery));
(function ($) {
    $.fn.outerHtml = function () {
        return this[0].outerHTML || new XMLSerializer().serializeToString(this[0]);
    };
}(jQuery));
(function ($) {
    $.fn.searchCssClass = function (pattern, config) {
        if (!this.length) {
            return undefined;
        }
        var cfg = $.extend({}, $.fn.searchCssClass.defaults, config),
            o = this[0],
            cls = o.className.split(' ');
        if (!cls.length) {
            return undefined;
        }
        if (cls[0].indexOf(pattern) === 0) {
            return $.fn.searchCssClass.retrieveValue(cls[0], cfg);
        }
        var i = cls.length;
        while (i) {
            var cl = cls[i - 1];
            if (cl.indexOf(pattern) === 0) {
                return $.fn.searchCssClass.retrieveValue(cl, cfg);
            }
            i--;
        }
        return undefined;
    };
    $.fn.searchCssClassList = function (pattern, config) {
        var cfg = $.extend({}, $.fn.searchCssClass.defaults, config);
        var i = this.length;
        var result = [];
        while (i) {
            var o = this[i - 1];
            var cls = o.className.split(' ');
            var i2 = cls.length;
            if (!i2) {
                continue;
            }
            if (cls[0].indexOf(pattern) === 0) {
                result.push($.fn.searchCssClass.retrieveValue(cls[0], cfg));
            }
            while (i2) {
                var cl = cls[i2 - 1];
                if (cl.indexOf(pattern) === 0) {
                    result.push($.fn.searchCssClass.retrieveValue(cl, cfg));
                }
                i2--;
            }
        }
        return result;
    };
    $.fn.searchCssClass.defaults = {
        splitChar: '-',
        index: 1,
        rawVal: false
    };
    $.fn.searchCssClass.retrieveValue = function (value, config) {
        if (config.rawVal) {
            return value;
        }
        return value.split(config.splitChar)[config.index];
    };
}(jQuery));
(function ($) {
    $.execIfExists = function (object, pluginName, pluginOpts, windowProp) {
        if (!object) {
            return undefined;
        }
        var $o, type = (typeof (object)).toString().toLowerCase();
        switch (type) {
        case 'string':
            $o = $(object);
            break;
        case 'object':
            if (!object.jquery) {
                $o = $(object);
            } else {
                $o = object;
            }
            break;
        default:
            $o = object;
            break;
        }
        if (!$o.length) {
            return $o;
        }
        if (typeof (pluginName) === typeof (function () {})) {
            pluginName.apply($o);
            return $o;
        }
        if (pluginName in window.$core && !window.$core[pluginName].lazyInit) {
            window.$core[pluginName].lazyInit = true;
            window.$core[pluginName](jQuery);
        }
        if (!pluginName in $.fn) {
            return $o;
        }
        try {
            $o[pluginName](pluginOpts);
        } catch (e) {
            if (window.console) {
                var error = '',
                    p;
                for (p in e) {
                    error += p + ':' + e[p] + '\n';
                }
                window.console.error(pluginName + '\n' + e + '\n' + error);
            }
        }
        if (windowProp) {
            window[windowProp] = $o;
        }
        return $o;
    };
}(jQuery));
var $doc = document;
var countDownOpts = {};
if ($doc.getElementById('torchrelay-oly')) {
    countDownOpts.startDate = Date.UTC(2012, 6, 27, 20, 0, 0);
} else if ($doc.getElementById('torchrelay-para')) {
    countDownOpts.startDate = Date.UTC(2012, 7, 24, 7, 0, 0);
}
$.execIfExists('html', 'accLoader'); // ATOS
$.execIfExists('.jcountdown', 'jCountdown', countDownOpts);
$.execIfExists('.js-watch', 'jWatch');
$.execIfExists($doc.getElementById('photoGallery'), 'jPhotogallery', {
    visibleThumbs: 6,
    onItemSet: function (movement) {
        var self = this;
        if (!self.carousel) {
            self.queue.apply(self, ['onItemSet', movement]);
            return;
        }
        var itemsCount = self.opts.visibleThumbs;
        var page = Math.floor(self.actualIdx / itemsCount) + 1;
        if (page != self.$ctx.data('actual-page')) {
            self.$ctx.data('actual-page', page);
            self.pause();
            var idx = (itemsCount * (page - 1));
            var __c = self.carousel.data('__jcarousel');
            if (__c) {
                __c.inner.scroll(idx, null, function () {
                    self.timedPlay();
                });
            }
        }
        $.execIfExists(".lcg-toggle-visibility", 'toggleVisibility');
    },
    onPreInit: function () {
        var self = this;
        self.$itemsCarousel = $('#items-carousel');
        self.isLazyLoading = self.$itemsCarousel.attr('data-count') != null && self.$itemsCarousel.attr('data-href') != null;
    },
    onInit: function (initCallback) {
        var self = this;
        self.$ctx.data('actual-page', 1);
        var dataCount = self.$itemsCarousel.attr('data-count') || -1;
        var dataHref = self.$itemsCarousel.attr('data-href');
        var totItems = self.$itemsCarousel.find('li').length;
        var sizeCallback = function (tot, callback) {
                $.execIfExists(self.$itemsCarousel, 'jCarousel', {
                    scroll: 6,
                    visible: self.opts.visibleThumbs,
                    itemFirstInCallback: function (carousel, li, index) {
                        if (index < 0) {
                            index = 0;
                        }
                        if (!self.carousel) {
                            self.carousel = $.extend({}, carousel);
                            if (self.dequeue.apply(self, ['onItemSet'])) {
                                return;
                            }
                        }
                        var itemsCount = self.opts.visibleThumbs;
                        var page = Math.floor(index / itemsCount) + 1;
                        if (page != self.$ctx.data('actual-page')) {
                            self.$ctx.data('actual-page', page);
                            self.onNavigate.apply(self, [true, self.carousel.children('.jcarousel-item:eq(' + index + ')')]);
                        }
                    }
                }, '$galleryCarousel');
                self.carousel = $.extend({}, window.$galleryCarousel);
                var page = Math.floor(self.actualIdx / totItems) + 1;
                if (self.actualIdx > 0 && self.carousel.data('__jcarousel') && page != self.$ctx.data('actual-page')) {
                    self.carousel.data('__jcarousel').inner.scroll(self.actualIdx);
                }
                if (callback) {
                    callback.apply(this);
                }
            };
        if (dataCount && dataHref) {
            var lazyItems = [];
            $.ajax({
                type: 'get',
                dataType: 'json',
                url: dataHref,
                success: function (items) {
                    var idx, item, basepath;
                    for (idx in items) {
                        item = items[idx];
                        if (!item) {
                            continue;
                        }
                        basepath = ['/mm', item.bp, '/', item.id].join('');
                        lazyItems.push(['<li data-seo="', item.seo, '" class="jlazy item-', item.id, ' gallery-single-item"><a data-fullscreen="', [basepath, '_MFULL.jpg'].join(''), '" href="', [basepath, '_M01.jpg'].join(''), '"><span class="top-bar">&nbsp;</span><img width="90" height="63" src="', [basepath, '_M05.jpg'].join(''), '"/></a></li>'].join(''));
                    }
                    self.$itemsCarousel.append(lazyItems.join(''));
                    totItems = dataCount || totItems;
                    self.setItems();
                    self.initCallback.apply(self);
                    self.onNavigate.apply(self, [0]);
                    sizeCallback.apply(self, [totItems, function () {
                        this.$itemsWrapUl.fadeIn('slow');
                    }]);
                },
                error: function () {
                    sizeCallback.apply(self, [totItems]);
                }
            });
        } else {
            sizeCallback.apply(self, [totItems]);
        }
    },
    onItemNotFound: function () {
        var self = this;
        if (self.carousel && self.carousel.data('__jcarousel')) {
            self.carousel.data('__jcarousel').refresh();
        }
    }
});
window.jcarouselCfg = {
    scroll: 1,
    visible: 1,
    initCallback: function (carousel) {
        var self = this,
            ajaxUrl = self.opts.ajaxUrl;
        carousel.data('actual-page', 1).find('li.hidden').removeClass('hidden');
        var $box = carousel.closest('.jcarousel-container').parent();
        if (!$box.length) {
            $box = carousel.closest('.bC');
        }
        var _control = $box.next('.jcarousel-control'),
            $placeholder = carousel.find('.page-placeholder');
        if (!_control.length) {
            _control = $box.find('.jcarousel-control');
        }
        if ($placeholder.length) {
            $placeholder.replaceWith('');
        }
        var pages = _control.find('.sliderNav').find('.slide').length;
        _control.find('.prev').addClass('jcarousel-prev-disabled').end().bind('click', function (e) {
            var $target = $(e.target);
            var $b = $(this).prev('.jcarousel-container');
            if (!$b.length) {
                $b = $(this).parent().find('.jcarousel-container');
            }
            var $c = $b.find('.' + self.opts.carouselClass),
                $parent;
            if ($target.hasClass('prev')) {
                e.preventDefault();
                var actualPage = $c.data('actual-page'),
                    page = actualPage - 1,
                    $newPage = $c.find('.page-' + page);
                if (ajaxUrl && !($newPage.length && $newPage.text().length > 0)) {
                    $.ajax({
                        type: 'get',
                        url: ajaxUrl + page,
                        success: function (data) {
                            if (data.length > 0) {
                                $c.find('.page-placeholder,.page-' + page).replaceWith('');
                                $c.find('.page-' + actualPage).before(data);
                                $c.append('<li class="page-placeholder"/>');
                                $c.data('__jcarousel').inner.prev(true);
                            }
                        }
                    });
                } else {
                    $c.data('__jcarousel').inner.prev();
                }
                $parent = $target.parent();
                $parent.find(".current").removeClass("current");
                $parent.find("li.slide").eq(carousel.last - 1).addClass("current");
                $c.data('actual-page', $c.data('actual-page') - 1);
                $c.data('offset-top', window.$scrollable.scrollTop());
            } else if ($target.hasClass('next')) {
                e.preventDefault();
                var actualPage = $c.data('actual-page'),
                    page = actualPage + 1,
                    $newPage = $c.find('.page-' + page);
                if (ajaxUrl && !($newPage.length && $newPage.text().length > 0)) {
                    $.ajax({
                        type: 'get',
                        url: ajaxUrl + page,
                        success: function (data) {
                            if (data.length > 0) {
                                $c.find('.page-placeholder,.page-' + page).replaceWith('');
                                $c.find('.page-' + actualPage).after(data);
                                if (page < pages) {
                                    $c.append('<li class="page-placeholder"/>');
                                }
                                $c.data('__jcarousel').inner.next(true);
                            }
                        }
                    });
                } else {
                    $c.data('__jcarousel').inner.next();
                }
                $parent = $target.parent();
                $parent.find(".current").removeClass("current");
                $parent.find("li.slide").eq(carousel.last - 1).addClass("current");
                $c.data('actual-page', $c.data('actual-page') + 1);
                $c.data('offset-top', window.$scrollable.scrollTop());
            } else if ($target.is('a')) {
                e.preventDefault();
                var actualPage = $c.data('actual-page'),
                    newPage = parseInt($target.text().replace("item", "").replace(" ", ""), 10),
                    $newPage = $c.find('.page-' + newPage);
                if (ajaxUrl && !($newPage.length && $newPage.text().length > 0)) {
                    var maxIdx = newPage > actualPage ? newPage : actualPage,
                        minIdx = newPage < actualPage ? newPage : actualPage,
                        $lastFound = [];
                    for (minIdx + 1; minIdx < maxIdx; minIdx++) {
                        if ($c.find('.page-' + minIdx).length) {
                            continue;
                        }
                        $c.find('.page-' + (minIdx - 1)).after('<li class="page-' + minIdx + '"><div class="torchbearersWrap"/></li>');
                    }
                    $.ajax({
                        type: 'get',
                        url: ajaxUrl + newPage,
                        success: function (data) {
                            $c.find('.page-placeholder,.page-' + newPage).replaceWith('');
                            $c.find('.page-' + (newPage - 1)).after(data);
                            if (newPage < pages) {
                                $c.append('<li class="page-placeholder"/>');
                            }
                            $c.data('__jcarousel').inner.scroll(newPage - 1, true, null, true);
                        }
                    });
                } else {
                    $c.data('__jcarousel').inner.scroll(newPage - 1, true);
                }
                $parent = $target.parent();
                $parent.parent().find(".current").removeClass("current");
                $parent.addClass("current");
                $c.data('actual-page', newPage);
                $c.data('offset-top', window.$scrollable.scrollTop());
            }
        });
        carousel.bind('keydown', function (e) {
            var code = e.keyCode || e.which;
            if (!(code == 9)) {
                return;
            }
            var $this = $(this);
            var $activeElement = $(document.activeElement);
            var $activeParent = $activeElement.parents('.jcarousel-item:first');
            var _page = $activeParent.searchCssClass('page-');
            if (!_page) {
                return;
            }
            _page = parseInt(_page, 10);
            carousel.data('actual-idx', _page - 1);
            var actualPage = $this.data('actual-page');
            if (actualPage < _page) {
                var domA = carousel.find('.page-' + actualPage).find('a[id*="page-' + actualPage + '"]')[0];
                if (domA) {
                    domA.focus();
                }
            }
            var $m04 = $activeElement.parents('.m_04');
            var idx = $m04.index();
            var maxIdx = $activeParent.find('.m_04').length;
            var $b = $this.closest('.jcarousel-container').parent();
            if (!$b.length) {
                $b = $this.closest('.bC');
            }
            var $carouselControl = $b.next('.jcarousel-control');
            if (!$carouselControl.length) {
                $carouselControl.find('.jcarousel-control');
            }
            var $prev = $carouselControl.find('.prev');
            var $next = $carouselControl.find('.next');
            if (idx == maxIdx && !e.shiftKey) {
                var _length = $this.find('.jcarousel-item').length;
                _page = _page + 1;
                if (_page > _length) {
                    return;
                }
                e.preventDefault();
                $carouselControl.find(".current").removeClass("current");
                $carouselControl.find("li.slide").eq(_page - 1).addClass("current");
                if (_page == 1) {
                    $prev.addClass("jcarousel-prev-disabled");
                }
                if (_page > 1) {
                    $prev.removeClass("jcarousel-prev-disabled");
                }
                if (_page == _length) {
                    $next.addClass("jcarousel-prev-disabled");
                }
                if (_page < _length) {
                    $next.removeClass("jcarousel-prev-disabled");
                }
                carousel.data('__jcarousel').inner.scroll(_page - 1, true);
            }
            if (idx <= 0 && e.shiftKey && _page > 1) {
                _page = _page - 1;
                if (_page < 0) {
                    return;
                }
                e.preventDefault();
                $carouselControl.find(".current").removeClass("current");
                $carouselControl.find("li.slide").eq(_page - 1).addClass("current");
                if (_page == 1) {
                    $prev.addClass("jcarousel-prev-disabled");
                }
                if (_page > 1) {
                    $prev.removeClass("jcarousel-prev-disabled");
                }
                if (_page == _length) {
                    $next.addClass("jcarousel-prev-disabled");
                }
                if (_page < _length) {
                    $next.removeClass("jcarousel-prev-disabled");
                }
                carousel.data('__jcarousel').inner.scroll(_page - 1, true);
            }
            $this.data('actual-page', _page);
            $this.data('offset-top', null);
            $this.data('keydown', true);
        });
    },
    itemFirstInCallback: function (carousel, li, index) {
        var $box = carousel.closest('.jcarousel-container').parent();
        if (!$box.length) {
            $box = carousel.closest('.bC');
        }
        if (carousel.data('actual-page') > 1) {
            index = carousel.data('actual-page');
        }
        if (carousel.data('keydown')) {
            var domA = carousel.find('.page-' + index).find('a[id*="page-' + index + '"]')[0];
            if (domA) {
                domA.focus();
                var offsetTop = carousel.data('offset-top');
                if (offsetTop) {
                    window.$scrollable.scrollTop(offsetTop);
                }
            }
        }
        var $control = $box.next('.jcarousel-control');
        if (!$control.length) {
            $control = $box.find('.jcarousel-control');
        }
        var $prev = $control.find('.prev');
        var $next = $control.find('.next');
        if (index == -1) {
            $prev.add($next).addClass("jcarousel-prev-disabled");
            $control.addClass('hidden');
            return;
        }
        if (index <= 1) {
            $prev.addClass("jcarousel-prev-disabled");
        }
        if (index > 1) {
            $prev.removeClass("jcarousel-prev-disabled");
        }
        if (index >= this.childLength) {
            $next.addClass("jcarousel-prev-disabled");
            carousel.data('__jcarousel').inner.scroll(this.childLength - 1);
        }
        if (index < this.childLength) {
            $next.removeClass("jcarousel-prev-disabled");
        }
        var $sliderNavList = $next.parent().find(".sliderNav").find("li");
        $sliderNavList.removeClass("current");
        var idx = index - 1;
        if (idx < 0) {
            idx = 0;
        }
        $sliderNavList.eq(idx).addClass("current");
    },
    injectNav: false
};
window.jcarouselSingleCfg = {
    scroll: 1,
    visible: 1,
    initCallback: function (carousel) {
        var dataAttr = carousel.attr('data-attr');
        if (dataAttr && dataAttr.length > 0) {
            switch (dataAttr) {
            case 'no-bg':
                carousel.data('parent-bg', carousel.parent().css('background-image'));
                carousel.parent().parent().find('.jcarousel-next,.jcarousel-prev').bind('click', function () {
                    $(this).parent().find('.jcarousel-clip').css('background-image', 'none');
                });
                break;
            }
        }
        carousel.find('li.hidden').removeClass('hidden');
    },
    itemLastInCallback: function (carousel) {
        var dataAttr = carousel.attr('data-attr');
        if (dataAttr && dataAttr.length > 0) {
            switch (dataAttr) {
            case 'no-bg':
                if (carousel.data('parent-bg')) {
                    carousel.parent().css('background-image', carousel.data('parent-bg'));
                }
                break;
            }
        }
    }
};
window.jcarousel2Cfg = {
    scroll: 2,
    visible: 2,
    initCallback: function (carousel) {
        carousel.list.find('li.hidden').removeClass('hidden');
    }
};
$.execIfExists('.field', function () {
    this.each(function () {
        var __this = this;
        var $this = $(__this);
        var default_value = __this.value;
        $this.focus(function () {
            var _this = this;
            if (_this.value === default_value) {
                _this.value = '';
            }
        });
        $this.blur(function () {
            var _this = this;
            if (_this.value === '') {
                _this.value = default_value;
            }
        });
    });
});
$.execIfExists('.tabcordion-wrap', function () {
    var $ctx = this,
        $tabCordion = $ctx.find('.tabcordion'),
        $tabsNav = $ctx.find('.tabs-nav');
    $ctx.find('.tabNext,.tabLive,.tabRes').bind('click', function (e) {
        e.preventDefault();
        var $target = $(e.target),
            selector = '.tab-next';
        if ($target.is('a') || $target.is('span')) {
            $target = $target.closest('li');
        }
        $tabsNav.find('.current').removeClass('current');
        $target.addClass('current');
        if ($target.hasClass('tabLive')) {
            selector = '.tab-live';
        }
        if ($target.hasClass('tabRes')) {
            selector = '.tab-res';
        }
        $tabCordion.addClass('hidden');
        $(selector).removeClass('hidden');
    });
    $tabCordion.find('.noItem').each(function () {
        var $this = $(this);
        if ($this.closest('.tab-res').length) {
            $ctx.find('.tabRes,.tab-res').hide();
        }
        if ($this.closest('.tab-next').length) {
            $ctx.find('.tabNext,.tab-next').hide();
            $tabsNav.find('.current').removeClass('current');
            $ctx.find('.tabRes').addClass('current');
            $tabCordion.addClass('hidden');
            $ctx.find('.tab-res').removeClass('hidden');
        }
        if ($this.closest('.tab-live').length) {
            $ctx.find('.tabLive,.tab-live').hide();
            $tabsNav.find('.current').removeClass('current');
            $ctx.find('.tabNext').addClass('current');
        } else {
            $tabsNav.find('.current').removeClass('current');
            $ctx.find('.tabLive').addClass('current');
            $tabCordion.addClass('hidden');
            $ctx.find('.tab-live').removeClass('hidden');
        }
    });
    $ctx.find('li').removeClass('current');
    $tabCordion.addClass('hidden');
    if ($('.tab-live .liveData').length) {
        $ctx.find('.tabLive').addClass('current');
        $ctx.find('.tab-live').removeClass('hidden');
    } else if ($('.tab-next .liveData').length) {
        $ctx.find('.tabNext').addClass('current');
        $ctx.find('.tab-next').removeClass('hidden');
    } else if ($('.tab-res .liveData').length) {
        $ctx.find('.tabRes').addClass('current');
        $ctx.find('.tab-res').removeClass('hidden');
    }
});
$.execIfExists('#schedule_gridWrap,#schedule_listWrap', 'schedule');
$.execIfExists('#overall_eventMedallists,.famousOlympians', 'zebraTable');
$.execIfExists('.bC', 'equalHeight');
$.execIfExists('.equalHeights', 'equalHeight', {
    selectors: ['.bH', '.bC']
});
$.execIfExists('.b-tabs', 'equalHeight', {
    selectors: ['.tabContent']
});
$.execIfExists('.newsHp', 'equalHeight', {
    selectors: ['.cols-2>li']
});
$.execIfExists('.eventsEqualHeights ', 'equalHeight', {
    selectors: ['.listGender ul']
});
$.execIfExists($doc.getElementById('countriesAthletesFilters'), 'countryAthletesfilters');
$.execIfExists('#lcg-sm-form', function () {
    $('#lcg-sm-form').validate();
    jQuery.extend(jQuery.validator.messages, {
        required: "Required field",
        email: "Please insert a correct email address"
    });
});
$.execIfExists(".sw", function () {
    this.bind('click', function (e) {
        e.preventDefault();
        var _wrap = $(this).closest('.or-legend-wrap');
        _wrap.find(".sw").toggleClass('hide');
        _wrap.find(".or-legend-content").toggleClass('show');
    });
});
$.execIfExists('.publications-sort', 'publicationsSort');
$.execIfExists('.torchBearerPhoto', 'preloadPhoto');
$.execIfExists($doc.getElementById('communityByRegionfilterRegion'), 'select', {
    change: function (domThis) {
        var region = $(domThis).find('option:selected').val();
        ajaxUrltoCall = '/torch-relay/' + region + '/library/_getCommunity.html';
        $.ajax({
            type: "get",
            url: ajaxUrltoCall
        }).success(function (xml) {
            var selectedRegionSeoName = $(domThis).children("option:selected").val();
            $('#communityByRegionfilterCommunity').empty().append(xml).removeAttr('disabled');
        });
        return false;
    }
});
if (window.addthis) {
    addthis.init();
}
$.execIfExists('#athlete_surname_search', 'radioSetup');
$.execIfExists('#athlete_surname_search,#btnFreeText,#btnPubCode', 'minLength');
$.execIfExists('.videoPlayerWrapper', 'videoPlayerEmbed');
$.execIfExists('.vert-tab', 'menu', {
    currentClass: 'or-current'
});
if ($.orpCommon) {
    $.execIfExists('.or-livestop', function () {
        var $this = this;
        if ($.orpCommon.isStopped()) {
            var $a = $this.find('a').text('Play');
            $a.closest('li').toggleClass('pause play');
        }
        $this.bind('click', function (e) {
            var $target = $(e.target),
                txt = 'Play';
            e.preventDefault();
            if ($.orpCommon.isStopped()) {
                $.orpCommon.refresh();
                txt = 'Pause';
            } else {
                $.orpCommon.stop();
            }
            $target.text(txt);
            $target.closest('li').toggleClass('pause play');
        });
    });
    $.execIfExists('.or-convertmeasure', function () {
        var $this = this,
            _convert = localStorage.getItem('orpCommon_convert');
        if (_convert) {
            $this.find('.current').removeClass('current').end().find('.' + _convert).addClass('current');
        }
        $this.bind('click', function (e) {
            e.preventDefault();
            var $target = $(e.target),
                convert = undefined;
            if ($target.is('.current')) {
                return;
            }
            if ($target.is('.metric')) {
                convert = 'metric';
            } else if ($target.is('.imperial')) {
                convert = 'imperial';
            }
            if (!convert) {
                return;
            }
            $(this).find('.current').removeClass('current');
            $target.addClass('current');
            localStorage.setItem('orpCommon_convert', convert);
            $.execIfExists('.or-h-conv,.or-w-conv', 'convertMeasure');
        });
    });
    var $orpToggle = $('.or-toggle,.or-expandall');
    if ($orpToggle.length && !($orpToggle.toggleLoadData && $orpToggle.toggleLoadData.isInit)) {
        $orpToggle.toggleLoadData();
    }
    $.override('AthleteExpansion.onGetAJContent', function (s) {
        if ($.fn.jTabs) {
            var $tabs = $('.sbContainer,#tt-match-wrapper').find('.jtabs').add(s.find('.jtabs')),
                ref = $.fn.jTabs.selectedRef,
                $parent, $items;
            if (ref) {
                $items = $tabs.find('.tabItem');
                $parent = $items.find('>a[href="' + ref + '"]').parent();
                $tabs.find('.tabContent').addClass('hidden').filter(ref).removeClass('hidden');
                $items.removeClass('current');
                $parent.addClass('current');
            }
        }
    });
    $.override('AthleteExpansion.onSwapHtml', function (div) {
        $.execIfExists(div.find('.jtabs'), 'jTabs');
        $.execIfExists(div.find('.timeline').find('li'), 'tooltip', {
            isInner: true,
            cSelector: '.tt',
            ajaxHref: false,
            position: 'bc',
            lazyLoadImg: true,
            cssClass: 'timeline-tooltip'
        });
        $.execIfExists(div.find('.or-h-conv,.or-w-conv'), 'convertMeasure');
    });
    $.execIfExists($doc.getElementById('or-eventNavContainer'), 'tubeNav');
    $.execIfExists($doc.getElementById('or-event'), 'phaseNavMenu');
    $.execIfExists('.scoreProg', 'ShowTimeline');
}
setTimeout(function () {
    $.execIfExists('#lcg-lev0menu,#lcg-lev2menu', 'superfish', {}, '$menus');
    if (!window.$menus) {
        window.$menus = $();
    }
    $.execIfExists(window.$menus.add('#lcg-lev3menu'), 'menu', {
        excludeSelector: '.sportHub'
    });
    $.execIfExists('.mnu-link', 'setCurrentItem');
    var $photoreader = $doc.getElementById('photoReader');
    if ($photoreader) {
        var opts = {};
        if ($photoreader.className.indexOf('newsReader') > -1) {
            opts = {
                autoplay: true,
                dataCfg: {
                    selector: '#frame ',
                    ajaxUrl: '/_news/library/article={0}/photo.html'
                }
            };
        }
        $.execIfExists($photoreader, 'jPhotoReader', opts);
    }
    $.execIfExists('.overall_medals', 'medalStandings');
    $.execIfExists('.historicalMedals', 'historicalMedalStandings');
    $.execIfExists($doc.getElementById('overall_medallists'), function () {
        if (!$doc.getElementById('rdbAll')) {
            return;
        }
        var $overall_medallists = this,
            $overall_medallists_M = $('#overall_medallists_M'),
            $overall_medallists_W = $('#overall_medallists_W');
        $("#rdbAll").click(function () {
            $overall_medallists_M.add($overall_medallists_W).addClass('hidden');
            $overall_medallists.removeClass('hidden');
        });
        $("#rdbMen").click(function () {
            $overall_medallists_W.add($overall_medallists).addClass('hidden');
            $overall_medallists_M.removeClass('hidden');
        });
        $("#rdbWomen").click(function () {
            $overall_medallists_M.add($overall_medallists).addClass('hidden');
            $overall_medallists_W.removeClass('hidden');
        });
    });
    $.execIfExists('.lbPhotoItem', 'forceHref');
    $.execIfExists($doc.getElementById('mascot-carousel'), 'circularCarousel', {
        injectNav: true,
        timed: false,
        randomStart: true
    });
    $.execIfExists('#related-Carousel,#article-Carousel,.jcarousel-single-simple,#venue-Carousel', 'jCarousel', window.jcarouselSingleCfg);
    $.execIfExists(".jcarousel", "jCarousel", window.jcarouselCfg);
    $.execIfExists(".jcarousel-double", "jCarousel", window.jcarousel2Cfg);
    $.execIfExists($doc.getElementById('anch-events-live'), function () {
        this.bind('click', function () {
            $(this).parent().find('.evt-flyOut').toggle();
        });
    });
    $.execIfExists(".lcg-toggle-visibility", 'toggleVisibility');
}, 50);
setTimeout(function () {
    $.execIfExists('.athlete_photo_tooltip,.oldtorch', 'tooltip', {
        isInner: true,
        cSelector: 'span',
        ajaxHref: false,
        position: 'tr',
        lazyLoadImg: true
    });
    $.execIfExists('.timeline li', 'tooltip', {
        isInner: true,
        cSelector: '.tt',
        ajaxHref: false,
        position: 'bc',
        lazyLoadImg: true,
        cssClass: 'timeline-tooltip'
    });
    $.execIfExists('#publicFunders-Carousel,#commercialPartners-Carousel', 'circularCarousel', {
        injectNav: false
    });
    $.execIfExists('.bar', 'tooltip', {
        isInner: true,
        cSelector: 'div,span',
        ajaxHref: false
    });
    $.execIfExists('.tbl-toggle', function () {
        this.bind('click', function (e) {
            var $this = $(this),
                selector = $this.attr('href'),
                $elm = $(selector),
                $toggle = $('#related-' + (selector.substr(1)));
            if (!$elm.length) {
                return;
            }
            if ($toggle.length) {
                if (!$toggle.data('copied')) {
                    $toggle.html('<td colspan="6">' + $elm.html() + '</td>');
                    $toggle.data('copied', true);
                }
            } else {
                $toggle = $elm;
            }
            $toggle.toggleClass('hidden');
            $this.toggleClass('or-plus or-minus');
            var html = '+';
            if ($this.hasClass('or-minus')) {
                html = '-';
            }
            $this.html('<span>' + html + '</span>');
            e.preventDefault();
        });
    });
    $.execIfExists('.overall_medalist', 'medallists');
}, 100);
setTimeout(function () {
    $.execIfExists('.jtabs', 'jTabs');
    //$.execIfExists('a[href^="http"],.external', 'external'); ATOS
    $.execIfExists($doc.getElementById('lcg-accLinks'), 'accSwitcher');
    $.execIfExists('#atthevenuefilterLocation,#atthevenuefilterSport', 'select', {
        change: function ($this) {
            var _sport = $("#atthevenuefilterSport").val();
            var _venue = $('#atthevenuefilterLocation').val();
            var _url = "/spectators/venues/library/";
            if (_venue.length > 0) {
                _url += ("location=" + _venue + "/");
            }
            if (_sport.length > 0) {
                _url += ("discipline=" + _sport + "/");
            }
            _url += "_venueslist.html";
            if (location.pathname.indexOf("/paralympics") == 0) {
                _url = "/paralympics" + _url;
            }
            $.ajax({
                type: "get",
                url: _url,
                success: function (result) {
                    $('#venuesList').replaceWith(result);
                },
                error: function () {}
            });
            return false;
        },
        defaulValue: ""
    });
    $.execIfExists($doc.getElementById('galleryfilterSport'), 'select', {
        change: function (domThis) {
            var optValue = domThis.value;
            if (!optValue.length) {
                return;
            }
            var self = this;
            var seoName = "";
            var start = optValue.indexOf("discipline=");
            var stop = 0;
            if (start != -1) {
                start = optValue.indexOf("=", start);
                stop = optValue.indexOf("/", start);
                seoName = optValue.substring((start + 1), stop);
            }
            var $iconSelect = $('#iconSelect').html(optValue).removeClass().addClass('discIcon ' + seoName);
            $.ajax({
                type: "get",
                url: optValue,
                success: function (result) {
                    var $row = self.$bC.find('.row:eq(1)');
                    if (!$row.length) {
                        $row = $('<div class="row"></div>').appendTo(self.$bC);
                    }
                    $row[0].innerHTML = result.length ? result : '';
                    $.execIfExists($row.find('.jcarousel'), 'jCarousel', window.jcarouselCfg);
                    $.execIfExists($row, 'equalHeight');
                },
                error: function () {}
            });
        },
        init: function () {
            var discref = $('#discref').html(),
                $iconSelect = $('#iconSelect');
            if (!$iconSelect.length) {
                $iconSelect = $('<div id="iconSelect"></div>').insertAfter(this.$ctx);
            }
            $iconSelect.html(discref).removeClass().addClass('discIcon ' + discref);
            this.$ctx.find('option[value *="=' + discref + '/"]').attr('selected', 'selected');
        }
    });
    $.execIfExists($doc.getElementById('broadcastersfilterCountry'), 'select', {
        change: function ($this) {
            if ($this.value == "" || self.data == null || self.data == undefined) {
                return false;
            }
            var myObject = $.parseJSON(data);
            if (myObject.root.country.length == 0) {
                return false;
            }
            var inner = "";
            var exit = false;
            $.each(myObject.root.country, function (i, _country) {
                if (_country.seoname != undefined && _country.seoname.toLowerCase() == $this.value.toLowerCase().replace(' ', '') && _country.broadcaster != undefined) {
                    if (Object.prototype.toString.call(_country.broadcaster) === '[object Array]') {
                        $.each(_country.broadcaster, function (j, broadcaster) {
                            inner += "<li>";
                            inner += broadcaster;
                            inner += "</li>";
                            exit = true;
                        });
                    } else {
                        inner += "<li>";
                        inner += _country.broadcaster;
                        inner += "</li>";
                        exit = true;
                    }
                }
                return !exit;
            });
            if (inner.length != 0) {
                inner = "<ul>" + inner + "</ul>";
            } else {
                inner = "No broadcaster found for the country you selected";
                inner = "<span>" + inner + "</span>";
            }
            if ($("#broadcastersContainer").length == 0) {
                $("#" + $this.id).after("<div id='broadcastersContainer'>" + inner + "</div>");
            } else {
                $("#broadcastersContainer").html(inner);
            }
            return false;
        },
        init: function () {
            $.ajax({
                url: "/spectators/library/_broadcasterlist.html",
                success: function (data) {
                    self.data = data;
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    return false;
                }
            });
        }
    });
    $.execIfExists('.lbWrap', 'jLightbox', {
        autoplay: false,
        wrap: '<div id="jquery-lightbox" class="galleryView #{hiddenClass}"><div id="lightbox-wrap"><a id="lightbox-nav-landing" href="#landing" tabindex="1"></a><div id="lightbox-title"></div><div id="lightbox-container-image-box"><div id="lightbox-container-image"><img id="__jLightbox-image"/></div><div id="lightbox-loading"><a href="#loading" id="lightbox-loading-link"><img src="#{imageLoading}"/></a></div><div id="lightbox-container-image-data-box"><div id="lightbox-container-image-data"><div id="lightbox-image-details"><div id="lightbox-image-details-caption"></div></div></div></div></div><a href="#prev" id="lightbox-nav-btnPrev" class="gallery-nav-btn" tabindex="4">Previous</a><a href="#next" id="lightbox-nav-btnNext" class="gallery-nav-btn" tabindex="5">Next</a><div id="lightbox-nav"><div class="navWrap"><div class="full"><a id="lightbox-full" tabindex="3">Full gallery</a></div><div class="arrows"><a href="#play-pause" id="lightbox-nav-btnPlayPause" tabindex="6">Play</a></div><div class="pages"><span id="lightbox-image-details-currentNumber"></span></div></div></div><a href="#close" id="__jLightbox-close" tabindex="2">Close</a></div></div>',
        excludesFromClick: '#lightbox-nav-btnPlayPause',
        onInit: function () {
            var self = this;
            self.$playPause = $('#lightbox-nav-btnPlayPause').removeClass(self.opts.disabledClass).bind('click', function (e) {
                e.preventDefault();
                var $this = $(this);
                if ($this.hasClass(self.opts.disabledClass)) {
                    return;
                }
                var $self = self.$document.data('active-self');
                $this.toggleClass('jlightbox-pause');
                if (($self.paused = $this.hasClass('jlightbox-pause'))) {
                    self.pause.apply();
                    $this.html("Stop");
                } else {
                    $self.timedPlay.apply($self);
                    $this.html('Play');
                }
                $self.$innerWrap.data('paused', $self.paused);
            });
            self.pause.apply(self);
        },
        onSpacebarKey: function () {
            var self = this;
            if (self.$activeElement.is(self.$playPause)) {
                var $this = self.$playPause;
                $this.toggleClass('jlightbox-pause');
                if ((self.paused = $this.hasClass('jlightbox-pause'))) {
                    self.pause.apply(self);
                    $this.html('Stop');
                } else {
                    self.timedPlay.apply(self);
                    $this.html('Play');
                }
                self.$innerWrap.data('paused', self.paused);
            }
        },
        onClick: function () {
            var self = this;
            self.$playPause.removeClass('jlightbox-pause');
            self.timedPlay.apply(self);
        },
        onAfterImageSet: function () {
            var self = this;
            self.$wrap.find('div.summary-wrap').replaceWith('');
            self.$playPause.removeClass(self.opts.disabledClass);
        },
        onFinish: function () {
            var self = this;
            self.$wrap.find('div.summary-wrap').replaceWith('');
            self.pause.apply(self);
        },
        onSummaryInit: function () {
            var self = this;
            var $summary = self.$imagesList.filter('.' + self.opts.summaryFilter).parents('div.summary-wrap');
            if ($summary.length) {
                var href = self.$imagesList.eq(0).attr('data-full');
                if (!href) {
                    href = self.isPhotolist ? self.$galleryTitle.find('a').attr('href') || self.$imagesList.eq(0).attr('href') : $summary.find('.full').attr('href');
                }
                if (self.isPhotolist) {
                    $('#lightbox-full').text('More photos');
                }
                if (href) {
                    $('#lightbox-full').attr('href', href).removeClass(self.opts.hiddenClass);
                } else {
                    $('#lightbox-full').addClass(self.opts.hiddenClass);
                }
            }
        },
        onSummary: function (domImage) {
            var self = this.$document.data('active-self');
            var $parent = self.$img.parent();
            if (!$parent.find('div.summary-wrap').length) {
                $parent.append($(domImage).parents('div.summary-wrap').clone());
                var $summary = $parent.find('div.summary-wrap');
                var fw = self.opts.fallback.width;
                var fh = self.opts.fallback.height;
                self._setImageBoxSizes(fw, fh, function () {
                    var w = fw - parseInt($summary.css('paddingLeft'), 10);
                    var h = fh - parseInt($summary.css('paddingTop'), 10);
                    $summary.css({
                        width: w,
                        height: h
                    });
                });
            }
            self.paused = true;
            self.$playPause.addClass('jlightbox-pause ' + self.opts.disabledClass).html('Play');
            self.pause.apply(self);
        }
    });
    var ajaxCfg = $.extend({}, window.jcarouselCfg, {
        ajaxUrl: '/paralympics/torch-relay/torchbearers/_torchbearers.htmx?pageN=',
        itemFirstInCallback: function (carousel, li, index) {
            window.jcarouselCfg.itemFirstInCallback.apply(this, arguments);
            carousel.preloadPhoto();
        }
    });
    $.execIfExists($doc.getElementById('wrapTorchBearersList'), 'jCarousel', ajaxCfg);
}, 150);
setTimeout(function () {
    $.execIfExists($doc.getElementById('lcg-stickybar'), 'stickyBar', {
        additionalElm: {
            $footer: '#lcg-footer'
        },
        onFocusedClick: function (e, othis, mThis) {
            var $a = this.find('a:eq(0)');
            if (!$a.length) {
                return;
            }
            var label = $a.attr('class') + "-" + $a.attr("href").replace(new RegExp("/", "g"), "");
            _gaq.push(['_trackEvent', 'sticky bar', 'click', label]);
        },
        onAfterToggle: function (state, isOnLoad) {
            var $footer = this.opts.additionalElm.$footer;
            if (state === 'is-open') {
                $footer.addClass('sticky-open').css('padding-bottom', '');
                $($doc).bind('keyup', function (e) {
                    if (e.keyCode === 9) {
                        var $target = $(e.target);
                        if ($target.closest('#sticky-content').length) {
                            return;
                        }
                        var stickyOffset = window.$sticky.offset();
                        var targetOffset = $target.offset();
                        if (targetOffset.top >= stickyOffset.top) {
                            window.$scrollable.animate({
                                scrollTop: targetOffset.top - 200
                            }, 400);
                        }
                    }
                });
            } else {
                if (typeof ($footer) == typeof ([])) {
                    $footer.removeClass('sticky-open');
                }
                $($doc).unbind('keyup');
            }
        },
        onResourcesLoaded: function () {
            $.execIfExists($doc.getElementById('sticky-myOly'), 'myolympics');
            $.execIfExists('#schedule_gridWrap', 'mySchedule');
            setTimeout(function () {
                $.execIfExists('.customTabs', 'myTabs');
                $.execIfExists('select[id$="filterSport"],select[id$="filterCountry"]', 'mySelect');
            }, 15);
        }
    }, '$sticky');
    $.execIfExists('.search select:not(.noRedirect),.navigate-to select', 'changeRedirect');
    $.execIfExists('.ldn-popup', 'openPopupWindow');
    $.execIfExists('.buttonExpand>a', 'buttonExpander', {
        elementToExpand: '.newsHiddenBody'
    });
    window.$scrollable = $('body,html');
}, 200);
$('#atthevenuefilterLocation').ready(function () {
    if (location.hash == '#road-events') {
        $.ajax({
            type: "get",
            url: "/spectators/venues/library/location=4/_venueslist.html",
            success: function (result) {
                $("#atthevenuefilterLocation").val('4');
                $('#venuesList').replaceWith(result);
            },
            error: function () {}
        });
    }
});


// ATOS
$(document).ready(function () {

	var domBody = $doc.getElementsByTagName('body')[0];
	domBody.className = domBody.className.replace('no-js', '');

    // Following lines are to rebind events after ajax update of the update panel
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        $.execIfExists($doc.getElementById('lcg-accLinks'), 'accSwitcher');
    });
});
