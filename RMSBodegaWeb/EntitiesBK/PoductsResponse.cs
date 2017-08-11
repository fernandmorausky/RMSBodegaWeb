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
   public class PoductsResponse
   {
      [DataMember]
      public   List<ProductResponse> ListProduct { get; set; }
   }
   [DataContract]
   public struct ProductResponse
   {
      [DataMember]
      public String SKU;
      [DataMember]
      public Boolean allowed;
      [DataMember]
      public String Available;

   }
}