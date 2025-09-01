
// create the aspect/interceptor
// i used OopJs function makeClass from https://github.com/ChristopherVonBlum/RadFramework.Libraries.OopJs
// aspect.js also depends on OopJs at runtime so make sure its embedded
var serviceProxyInterceptor = makeClass(
{
    $inherits: aspect,
    $protoCtor: ($proto) =>
    {
    },
    $ctor: () =>
    {
    },
    onExit: (instance, func, funcName, params, ret) =>
    {
        var asyn = false;
        if(params.length === 2){
            asyn = params[1];
        }
        
        var obj;
        
        if(params.length >= 1)
        {
            obj = JSON.stringify(params[0]);
        }
        
        var res = postAjaxCall(funcName, obj, instance.baseUri, instance.clientId, asyn);
        
        if(asyn){
            return Promise.resolve(res).then(JSON.parse)
        }
        
        return JSON.parse(res);
    },
    handleError: (instance, func, funcName, params, error) =>
    {
        console.log("error (" + error + ") occured in " + funcName)
    }
});

// create the proxy so we can do controller request
proxy = createInteractionServiceProxy("https://localhost:5001/DesktopApi/", clientId);

// call the intercepted objects method resulting in a ajax post at serviceProxyInterceptor.onExit
proxy = serviceProxy.YourControllerMethod();

var serviceProxy = (function () {
    _serviceProxy.prototype = new StructuredObject;
    var $proto = _serviceProxy.prototype;

    $proto.$protoCtor = function () {
    };

    $proto.$ctor = function (baseUri, clientId) {
        this.baseUri = baseUri;
        this.clientId = clientId;
        // let your backend return all method names in a controller(asp.net users can use reflection here)
        // get supported methods
        var methods = JSON.parse(postAjaxCall("GetMethods", undefined, baseUri, clientId));

        // create dummmy methods
        for (i = 0; i < methods.length; i++) {
            this[methods[i]] = function (body) {};
        }

        // let aspect.js inject the interceptors
        new serviceProxyInterceptor()
            .applyToMethods(this);
    };

    function _serviceProxy(baseUri, clientId) {
        this.$ctor.call(this, baseUri, clientId);
    }
    
    $proto.$protoCtor();

    return _serviceProxy;
})();

function createInteractionServiceProxy(baseUri, clientId) {
    return new serviceProxy(baseUri, clientId);
}

// this will be called by your aspect
function postAjaxCall(method, data, baseUri, clientId, asyn) {
    var req = new XMLHttpRequest();
    req.open('post', baseUri + method, asyn);
    req.setRequestHeader('Content-Type', 'application/json');
    req.setRequestHeader("clientId", clientId);
        
    if(asyn){
        var task = new Promise(function (resolve, reject) {
            // Setup our listener to process compeleted requests
            req.onreadystatechange = function () {

                // Only run if the request is complete
                if (req.readyState !== 4) return;

                // Process the response
                if (req.status >= 200 && req.status < 300) {
                    // If successful
                    resolve(req.response);
                } else {
                    // If failed
                    reject({
                        status: req.status,
                        statusText: req.statusText
                    });
                }
            };
        });

        req.send(data);
        
        return task;
    }

    req.send(data);
    
    return req.response;
}
