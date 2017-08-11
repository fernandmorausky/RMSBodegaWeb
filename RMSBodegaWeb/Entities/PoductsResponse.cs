using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace RMSBodegaWeb.Entities
{
   public class PoductsResponse
   {
      public   List<ProductResponse> ListProduct { get; set; }
   }
   public struct ProductResponse
   {
      public String SKU;
      public Boolean allowed;
      public String Available;

   }
}