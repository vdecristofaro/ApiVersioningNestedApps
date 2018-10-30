using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ApiHost.Controllers {

    [ApiVersion( "1.0" )]
    [ApiVersion( "2.0" )]
    [ApiController]
    [Route( "v{version:apiVersion}/values" )]
    public class ValuesController : ControllerBase {

        [Route( "", Name = nameof( GetValues ) )]
        [ProducesResponseType( typeof( string[] ), 200 )]
        [ProducesResponseType( typeof( string[] ), 404 )]
        [Produces( "application/json" )]
        [Consumes( "application/json" )]
        public async Task<IActionResult> GetValues() {
            return await Task.FromResult<IActionResult>( Ok( new string[] { "value 1", "value 2" } ) );
        }
    }
}
