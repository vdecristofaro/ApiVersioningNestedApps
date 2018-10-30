
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NestedApp.Controllers {

    [ApiVersion( "1.0" )]
    [ApiController]
    [Route( "v{version:apiVersion}/nested" )]
    public class NestedController : Controller {
        public NestedController() {
        }

        [HttpGet]
        [Route( "", Name = nameof( GetNestedValues ) )]
        [ProducesResponseType( typeof( string[] ), 200 )]
        [ProducesResponseType( typeof( string[] ), 404 )]
        [Produces( "application/json" )]
        [Consumes( "application/json" )]
        public async Task<IActionResult> GetNestedValues() {
            return await Task.FromResult<IActionResult>( Ok( new string[] { "nested value 1", "nested value 2" } ) );
        }

    }
}
