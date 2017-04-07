// Embed code

//        (function(i,s,o,g,r,a,m){i['ClinkPixel']=r;i[r]=i[r]||function(){
//        (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
//        m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
//        })(window,document,'script','https://clink-azfuncdemopixel.azurewebsites.net/pixel.js','px');
//
//        px('create', 'UA-XXXXX-Y');
//        px('send', 'pageview');

(function () {
    var px = window.px;
    var queue = px.q;

    var settings = px.settings = {};

    px.init = function (data) {
        settings.trackingId = data.shift();
    };

    px.track = function (data) {
        var params = data.shift() || {};
        params.url = window.location.href;
        params.settings = settings;

        var xhr = new XMLHttpRequest();
        xhr.open('POST', 'https://marc-azfuncdemo.azurewebsites.net/api/pixel', true);
        xhr.setRequestHeader('Content-Type', 'application/json');
        xhr.send(JSON.stringify(params));
    };

    queue.forEach(function (cmd) {
        cmd = [].slice.call(cmd);

        var method = cmd.shift();

        if (px[method]) {
            px[method](cmd);
        }
    });
})();