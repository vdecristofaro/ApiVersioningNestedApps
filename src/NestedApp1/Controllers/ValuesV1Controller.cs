
using System;
using System.Threading.Tasks;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NestedApp1.Controllers {

    [ApiVersion( "1.0" )]
    [ApiController]
    [Route( "v{version:apiVersion}/values" )]
    public class ValuesV1Controller : NestedApp1Controller {
        private readonly IHelloService _helloService;
        private readonly IMediator _mediator;
        private readonly IGlobalHelloService _globalHelloService;

        public ValuesV1Controller( IHelloService helloService, IMediator mediator, IGlobalHelloService globalHelloService ) {
            _helloService = helloService;
            _mediator = mediator;
            _globalHelloService = globalHelloService;
        }

        [HttpGet]
        [Route( "", Name = nameof( GetNestedApp1V1Values ) )]
        [ProducesResponseType( typeof( string[] ), 200 )]
        [ProducesResponseType( typeof( string[] ), 404 )]
        [Produces( "application/json" )]
        [Consumes( "application/json" )]
        public async Task<IActionResult> GetNestedApp1V1Values() {
            _helloService.SayHello();
            _globalHelloService.SayHello();
            var request = new ValueRequest() {
                Index = 1
            };
            var results = await _mediator.Send( request );
            return await Task.FromResult<IActionResult>( Ok( results ) );
        }

    }
}
