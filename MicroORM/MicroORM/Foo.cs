using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroORM.PetaPoco
{
   public class foo
    {
       public int id{get;set;}
       public string name { get; set; }
    }

}
namespace MicroORM.MicroLite
{
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
