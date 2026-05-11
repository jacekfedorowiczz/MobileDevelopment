using Microsoft.AspNetCore.Mvc;
using MobileDevelopment.API.Interfaces;

namespace MobileDevelopment.API.Attributes
{
    public class MobileControllerAttribute : RouteAttribute, IIgnoreAntiforgery
    {
        public MobileControllerAttribute(string template = "[controller]")
                : base($"api/v{{version:apiVersion}}/mobile/{template}")
        {
            this.Order = 1000;
        }
    }
}

