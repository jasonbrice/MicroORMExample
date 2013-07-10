using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroORMTest{
    //[Table("foo")]
    public class foo
    {
        //[Column("id")]
        //[Identifier(IdentifierStrategy.DbGenerated)]
        public int Id { get; set; }

        //[Column("name")]
        public string name { get; set; }
    }
    
}
