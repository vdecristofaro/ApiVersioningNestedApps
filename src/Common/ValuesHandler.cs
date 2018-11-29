// ValueHandler.cs
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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Common {
    public class ValuesHandler : IRequestHandler<ValueRequest, IEnumerable<string>> {
        public async Task<IEnumerable<string>> Handle( ValueRequest request, CancellationToken cancellationToken ) {
            var results = new List<string>();
            for ( int i = 1; i < 6; i++ ) {
                results.Add( $"Nested App {request.Index} - Value {i}" );
            }
            return await Task.FromResult( results );
        }
    }
}
