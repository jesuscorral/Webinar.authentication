using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WebinarAutentication.Authentication
{
    public class WebinarAuthroiztionRequirement : IAuthorizationRequirement
    {
        public string SomeProperty { get; set; }

        public string AnnotherProperty { get; set; }
    }

    public class WebinarAuthroiztionRequirementHandler 
        : AuthorizationHandler<WebinarAuthroiztionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, WebinarAuthroiztionRequirement requirement) {
            var resource = context.Resource;
            var prop = requirement.SomeProperty;

            //context.Fail();

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
