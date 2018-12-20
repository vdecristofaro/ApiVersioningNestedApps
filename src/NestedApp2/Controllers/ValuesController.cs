// ValuesController.cs
//
// Author: Vincenzo De Cristofaro <vdecristofaro@domecsolutions.com>
//
// Copyright 2015 -2018 Domec SpA(c) 2018 2015 -2018 Domec SpA
//
// Domec(r), Domec logos, Domec Tools logos, their taglines and the look, feel and trade 
// dress of the Service are the exclusive trademarks, service marks, trade dress and logos 
// of Domec SpA(r). All other trademarks, service marks, trade dress, and logos used on 
// the Service are the trademarks, service marks, trade dress, and logos of their 
// respective owners.
//
//
using System;
using System.Threading.Tasks;
using Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace NestedApp2.Controllers {

    [ApiVersion( "2.0" )]
    [ApiController]
    [Route( "v{version:apiVersion}/values2" )]
    public class ValuesController : NestedApp2Controller {
        private readonly IHelloService _helloService;
        private readonly IMediator _mediator;
        private readonly IGlobalHelloService _globalHelloService;

        public ValuesController( IHelloService helloService, IMediator mediator, IGlobalHelloService globalHelloService ) {
            _helloService = helloService;
            _mediator = mediator;
            _globalHelloService = globalHelloService;
        }

        [HttpGet]
        [Route( "", Name = nameof( GetNestedApp2Values ) )]
        [ProducesResponseType( typeof( string[] ), 200 )]
        [ProducesResponseType( typeof( string[] ), 404 )]
        [Produces( "application/json" )]
        [Consumes( "application/json" )]
        public async Task<IActionResult> GetNestedApp2Values() {
            _helloService.SayHello();
            _globalHelloService.SayHello();

            var request = new ValueRequest() {
                Index = 2
            };
            var results = await _mediator.Send( request );
            return await Task.FromResult<IActionResult>( Ok( results ) );
        }

    }
}
