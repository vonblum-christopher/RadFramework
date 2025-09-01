StructuredObject = function(){ };

StructuredObject.prototype.isFunctionPrimitive = function() 
{
    let getType = {};
    return this && getType.toString.call(this) === '[object Function]';
};

StructuredObject.prototype.getMemberType = function(memberName)
{
    let memberTypes = this["§§memberTypes"];

    if(memberTypes === undefined)
    {
        return;
    }

    return memberTypes[memberName];
};

StructuredObject.prototype.setMemberType = function(memberName, memberType)
{
    let memberTypes = this["§§memberTypes"];

    if(memberTypes === undefined)
    {
        memberTypes = this["§§memberTypes"] = new Object();
    } 

    memberTypes[memberName] = memberType; 
};

StructuredObject.prototype.isCustomMember = function(memberName)
{
    return this.getMemberType(memberName) !== "§§";
}

StructuredObject.prototype.setMemberType("§§memberTypes", "§§");
StructuredObject.prototype.setMemberType("getMemberType", "§§");
StructuredObject.prototype.setMemberType("setMemberType", "§§");
StructuredObject.prototype.setMemberType("isCustomMember", "§§");
StructuredObject.prototype.setMemberType("$inherits", "§§");
StructuredObject.prototype.setMemberType("$protoCtor", "§§");
StructuredObject.prototype.setMemberType("$ctor", "§§");
StructuredObject.prototype.setMemberType("isFunctionPrimitive", "§§");
StructuredObject.prototype.setMemberType("constructor", "§§");

function makeClass(classModel)
{
    classModel = Object.assign(new StructuredObject(), classModel);
    
    let baseProto = new StructuredObject; 
    
    var inherits = classModel.$inherits;

    if(inherits instanceof Function)
    {
        baseProto = Object.assign(baseProto, inherits.prototype["§§untransformedProto"]);
    }
    else if(inherits instanceof Array)
    {
        let inheritedProtos = [];
        for(let i = 0; i < inherits.length; i++)
        {
            inheritedProtos.push(inherits[i].prototype["§§untransformedProto"]);
        }
        baseProto = Object.assign.apply(baseProto, [baseProto].concat(inheritedProtos));
    }

    let ownProto = new StructuredObject;

    for(let prop in classModel)
    {
        if(!classModel.isCustomMember(prop)){
            continue;
        }

        
        ownProto[prop] = classModel[prop];
    }

    ownProto = Object.assign(new StructuredObject, baseProto, ownProto);

    ownProto["§§untransformedProto"] = Object.assign(new StructuredObject, ownProto);

    var ctor = eval(
"(function()\n\
{\n\
    function _ctor()\n\
    {\n\
        Object.assign(this, new injectThis(ownProto, this, arguments));\n\
        if(arguments.length !== 0)\n\
        {\n\
            classModel.$ctor.apply(Array.prototype.slice.call(arguments));\n\
        }\n\
        else\n\
        {\n\
            classModel.$ctor();\n\
        }\n\
    }\n\
    classModel.$protoCtor(ownProto);\n\
    return _ctor;\n\
})()");

    ctor.prototype = ownProto;

    ctor.prototype.constructor = ctor;

    return ctor;
}

function injectThis(classModel, $this, args)
{
    let transformedProto = new StructuredObject;
    for(let prop in classModel)
    {
        if(!classModel.isCustomMember(prop)){
            continue;
        }

        let member = classModel[prop];

        if(StructuredObject.prototype.isFunctionPrimitive.call(member))
        {
            var vars = { $this: $this };
            var args = [];
            var names = getParamNames(member);
            for(i = 0; i < names.length; i++){
                args[i] = names[i];
            }

            transformedProto[prop] = injectVar(member, vars, args);
            continue;
        }
    }

    return transformedProto;
}

function injectVar(func, vars, args){
    var injectionWrapper = "(function(){\n"

    var entire = func.toString(); 
    var body = entire.slice(entire.indexOf("{") + 1, entire.lastIndexOf("}"));

    injectionWrapper += "return (function __func(vars, arguments){\n"; 
    
    var i = 0;
    
    vars = Object.assign(new StructuredObject(), vars);
    
    for(v in vars)
    {
        if(!vars.isCustomMember(v))
        {
            continue;
        }

        injectionWrapper += "var " + v + " = vars[\"" + v + "\"];\n";
        i++;
    }
    
    for(i = 0; i < args.length; i++)
    {
        injectionWrapper += "var " + args[i] + " = arguments[\"" + i + "\"];\n";
    }
    
    injectionWrapper += body + "\n})(vars, arguments)})";

    return eval(injectionWrapper);
}

var STRIP_COMMENTS = /((\/\/.*$)|(\/\*[\s\S]*?\*\/))/mg;
var ARGUMENT_NAMES = /([^\s,]+)/g;
function getParamNames(func) {
    var fnStr = func.toString().replace(STRIP_COMMENTS, '');
    var result = fnStr.slice(fnStr.indexOf('(')+1, fnStr.indexOf(')')).match(ARGUMENT_NAMES);
    if(result === null)
        result = [];
    return result;
}
