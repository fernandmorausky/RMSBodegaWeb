using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSBodegaWeb.Entities
{
   public class EmailRequest
   {
      public String Email { get; set; }
      public String CountryAbbrev { get; set; }
   }
}