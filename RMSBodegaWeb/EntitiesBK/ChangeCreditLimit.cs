﻿using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSBodegaWeb.Entities
{
   [DataContract]
   public class ChangeCreditLimit
   {
      [DataMember]
      public String Email { get; set; }
      [DataMember]
      public String PlanillaChanged { get; set; }
      [DataMember]
      public String CountryAbbrev { get; set; }
   }
}