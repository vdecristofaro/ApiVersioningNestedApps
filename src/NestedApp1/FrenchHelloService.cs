// ItalianHelloService.cs
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
using Common;

namespace NestedApp1 {
    public class FrenchHelloService : IHelloService {
        public FrenchHelloService() {
        }

        public void SayHello() {
            Console.WriteLine( "Salut Salut Salut Salut Salut Salut " );
        }
    }
}
