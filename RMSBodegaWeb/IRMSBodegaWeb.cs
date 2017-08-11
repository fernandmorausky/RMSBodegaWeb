using RMSBodegaWeb.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RMSBodegaWeb
{
   [ServiceContract]
   public interface IRMSBodegaWeb
   {

      #region CreditLimit
      [OperationContract]
      [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,   RequestFormat = WebMessageFormat.Json,
                   UriTemplate = "/getLimitCredit_Employee")]
      EmployeeCredit getLimitCredit_Employee(EmailRequest email);

    
      #endregion



      [OperationContract]
      [WebInvoke(Method = "POST", ResponseFormat = WebMessageFormat.Json,  RequestFormat = WebMessageFormat.Json,
                   UriTemplate = "/getAcumulatedProductsByEmployee")]
      PoductsResponse getAcumulatedProductsByEmployee(PoductsRequest oProductsRequest);



      [OperationContract]
      [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
                        UriTemplate = "/ChangeCreditlimit")]
      Boolean ChangeCreditlimit(ChangeCreditLimit oChange);



      [OperationContract]
      [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json,
                        UriTemplate = "/StartBatbyParam")]
      BatResponse StartBatbyParam(BatParams Param);


   } 
}
