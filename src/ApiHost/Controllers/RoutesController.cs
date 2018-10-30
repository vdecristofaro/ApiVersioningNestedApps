using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiHost.Controllers {

    [ApiVersion( "1.0" )]
    [ApiController]
    [Route( "v{version:apiVersion}/routes" )]
    public class RoutesController : Controller {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

        public RoutesController( IActionDescriptorCollectionProvider actionDescriptorCollectionProvider ) {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
        }

        [HttpGet]
        [Route( "", Name = nameof( Index ) )]
        public async Task<IActionResult> Index() {
            var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.Select( x => new {
                Action = x.RouteValues[ "Action" ],
                Controller = x.RouteValues[ "Controller" ],
                x.AttributeRouteInfo?.Name,
                x.AttributeRouteInfo?.Template,
                x.ActionConstraints,
                x.RouteValues,
                x.AttributeRouteInfo
            } ).ToList();

            return await Task.FromResult<IActionResult>( Ok( routes ) );
        }
    }
}
