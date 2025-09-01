'use strict';
var $aspect$this;
var jsWeaver = new defaultWeaver();


var aspect = makeClass(
{
    $protoCtor: ($proto) =>
    {
    },
    $ctor: () => 
    {
        console.log();
    },
    applyToMethod: (proto, method, instance) =>
    {
        if(!method.isCustomMember(method))
        {
            throw "Can not intercept non custom properties.";
        }

        instance = $this.getInterceptionTarget(instance, proto);
        
        let member = proto[method];

        if(!member.isFunctionPrimitive())
        {
            throw "Member " + method + " is not a method";
        }
        
        instance[method] = jsWeaver.interceptFunction(member, method, this.onEntry, this.onExit, this.handleError);

        instance.annotateMember(method, this);

        return this;
    },
    applyToProperty: (proto, property, instance) =>
    {
        if(!property.isCustomMember())
        {
            throw "Can not intercept non custom properties.";
        }

        instance = $this.getInterceptionTarget(instance, proto);
        
        // get member from prototype
        let targetMember = proto[property];
        
        // if member not found or already differs from prototype
        if(targetMember === undefined || targetMember !== instance[property])
        {
            targetMember = instance[property];
        }

        if(!targetMember.isFunctionPrimitive())
        {
            targetMember = jsWeaver.autoImplementProperty(instance, property, targetMember);
        }

        instance[property] = jsWeaver.interceptFunction(targetMember, property, $this.onEntry, $this.onExit, $this.handleError);

        instance.annotateMember(property, $this);

        return $this;
    },

    applyToMethods: (proto, instance) =>
    {
        instance = $this.getInterceptionTarget(instance, proto);
        
        for(let memberName in proto)
        {
            let member = instance[memberName];
                        
            if(!$this.isCustomMember(memberName) || !$this.isFunction(member))
            {
                continue;
            }
            
            instance[memberName] = jsWeaver.interceptFunction(member, memberName, $this.onEntry, $this.onExit, $this.handleError);

            instance.annotateMember(memberName, $this);
        }

        return $this;
    },
    applyToProperties: (proto, instance) =>
    {
        instance = $this.getInterceptionTarget(instance, proto);
        
        for(var memberName in proto)
        {
            let member = instance[memberName];
            
            if(!$this.isCustomMember(memberName))
            {
                continue;
            }

            if(!this.isFunction(member))
            {
                member = jsWeaver.autoImplementProperty(proto, memberName, member);
            }

            instance[memberName] = jsWeaver.interceptFunction(member, targetMemberName, $this.onEntry, $this.onExit, $this.handleError);

            instance.annotateMember(memberName, $this);
        }

        return $this;
    },
    getInterceptionTarget: (intercept, targetClass) =>
    {
        if(intercept !== undefined)
        {
            return intercept;
        }

        return targetClass;
    },
    isFunction: (functionToCheck) => 
    {
        let getType = {};
        return functionToCheck && getType.toString.call(functionToCheck) === '[object Function]';
    }
});


function defaultWeaver()
{
    this.autoImplementProperty = function(targetClass, targetProperty, defaultValue)
    {
        let targetFunction = 
    "(function()\n\
    {\n\
        if(arguments[0] === undefined)\n\
        {\n\
            return $aspect$this._" + targetProperty + ";\n\
        }\n\
        $aspect$this._" + targetProperty + " = arguments[0];\n\
    })\n";
        targetClass.setMemberType(targetProperty, "property");
        targetClass.setMemberType("_" + targetProperty, "property");
        targetClass["_" + targetProperty] = defaultValue;
        return targetClass[targetProperty] = eval(targetFunction);
    }

    this.interceptFunction = function(func, funcName, preInterceptor, postInterceptor, errorHandler)
    {
        if(undefined === (preInterceptor || postInterceptor || errorHandler))
        {
            return func;
        }

        let interceptedMethodBody = 
"(function ()\n\
{\n\
    $aspect$this = $aspect$this || this;\n";
        
        if(preInterceptor !== undefined)
        {
            interceptedMethodBody += "\tpreInterceptor(this, func, \"" + funcName + "\", arguments);\n\n";
        }
        
        interceptedMethodBody += "\tvar ret;\n\n";
        
        if(errorHandler !== undefined)
        {
            interceptedMethodBody +=
"	try\n\
    {\n\
        ret = func.apply(this, arguments);\n\
    }\n\
    catch(error)\n\
    {\n\
        ret = errorHandler(this, func, \"" + funcName + "\", arguments, error);\n\
    }\n\n";
        }
        else
        {
            interceptedMethodBody += "\tret = func.apply(this, arguments);\n\n"
        }
        
        if(postInterceptor !== undefined)
        {
            interceptedMethodBody +=
"   var overrideRet = postInterceptor(this, func, \"" + funcName + "\", arguments, ret);\n\
    \n\
    if(overrideRet !== undefined)\n\
    {\n\
        ret = overrideRet;\n\
    }\n\n"
        }
        
        interceptedMethodBody += 
    "if($aspect$this === this)\n\
    {\n\
        $aspect$this = undefined;\n\
    }\n\
    return ret;\n\
})";
        
        return eval(interceptedMethodBody);	
    }
}