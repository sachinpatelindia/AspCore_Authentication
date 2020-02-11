using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreAuthentication.CustomPolicyProvider
{

    public class SecurityLevelAttribute:AuthorizeAttribute
    {
        public SecurityLevelAttribute(int level)
        {
            Policy = $"{DynamicPolycies.SecurityLevel}.{level}";
        }
    }
    public class CustomAuthorizatonPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public CustomAuthorizatonPolicyProvider(IOptions<AuthorizationOptions> options):base(options)
        {

        }
        public override Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            foreach(var customPolicy in DynamicPolycies.Get())
            {
                if(policyName.StartsWith(customPolicy))
                {
                   // var policy = new AuthorizationPolicyBuilder().Build();
                    return Task.FromResult(DynamicAuthorizePolicyFactory.Create(policyName));
                }
            }

            return base.GetPolicyAsync(policyName);
        }
    }


    public static class DynamicAuthorizePolicyFactory
    {
        public static AuthorizationPolicy Create(string policyName)
        
        {
            var parts = policyName.Split(',');
            var type = parts.First();
            var value = parts.Last();
            switch (type)
            {
                case DynamicPolycies.Rank:
                    return new AuthorizationPolicyBuilder()
                        .RequireClaim("Rank", value)
                        .Build();
                case DynamicPolycies.SecurityLevel:
                    return new AuthorizationPolicyBuilder()
                        .AddRequirements(new SecurityLevelRequirement(Convert.ToInt32(value)))
                        .Build();
                default:
                    return null;
            }
        }
    }

    public class SecurityLevelHandler : AuthorizationHandler<SecurityLevelRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SecurityLevelRequirement requirement)
        {
            var claimmValue =Convert.ToInt32( context.User.Claims.FirstOrDefault(x => x.Type == DynamicPolycies.SecurityLevel)?.Value??"0" );
            if (requirement.Level <= claimmValue)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
    public class SecurityLevelRequirement:IAuthorizationRequirement
    {
        public int Level { get; set; }
        public SecurityLevelRequirement(int level)
        {
            Level = level;
        }
    }
    public static class DynamicPolycies
    {
        public static IEnumerable<string> Get()
        {
            yield return SecurityLevel;
            yield return Rank;
        }
        public const string SecurityLevel = "SecurityLevel";
        public const string Rank = "Rank";
    }
}
