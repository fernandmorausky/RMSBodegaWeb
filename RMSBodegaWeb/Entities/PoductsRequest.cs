using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMSBodegaWeb.Entities
{
   public class PoductsRequest
   {
      public String EmailCustomer { get; set; }
      public String WebId { get; set; }
      public string CountryAbbrev { get; set; }
      public List<ProductRequest> ListProduct { get; set; }
       
      public Double getTotal(String sku)
      {
         Double dTotal  =0.0;
         foreach (ProductRequest product in this.ListProduct)
         {
            if (product.SKU == sku)
            {
               dTotal = Double.Parse(product.Total);
            }
         }
         return dTotal;
      }

   }
   public struct ProductRequest
   {
      public String SKU;
      public String Total;
   }
}