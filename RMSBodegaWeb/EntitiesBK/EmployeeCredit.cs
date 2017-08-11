using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSBodegaWeb.Entities
{
   [DataContract]
   public class EmployeeCredit
   {
      #region  Properties
      [DataMember]
      public String CreditAvailable { get; set; } 
      #endregion
   }

}