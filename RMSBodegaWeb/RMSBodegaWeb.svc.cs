using RMSBodegaWeb.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RMSBodegaWeb
{
   public class RMSBodegaWeb : IRMSBodegaWeb
   {
      struct BatOptionsWCF
      {

         public const String Employee = "0";
         public const String Products = "1";
         public const String Campaigne = "2";
      }
      #region Webservices
      private String connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];
      private Stopwatch stopwatch = new Stopwatch();

      public EmployeeCredit getLimitCredit_Employee(EmailRequest Email)
      {
          
         Log.writeLog("INICIO llamado a WC: getLimitCredit_Employee , LongDate: " + DateTime.Now.ToLongDateString() + ", LongTime: " + DateTime.Now.ToLongTimeString(), System.Diagnostics.EventLogEntryType.Information);
         EmployeeCredit LimitCredit = new Entities.EmployeeCredit();
         LimitCredit = getLimitCredit(Email.CountryAbbrev, Email.Email);
         return LimitCredit;
      }

      public Boolean ChangeCreditlimit( ChangeCreditLimit oChange)
      {
         Log.writeLog("INICIO llamado a WC: ChangeCreditlimit , LongDate: " + DateTime.Now.ToLongDateString() +", LongTime: " + DateTime.Now.ToLongTimeString() , System.Diagnostics.EventLogEntryType.Information);
         stopwatch.Start();
         Boolean bResut = true;

         loadConnectionString(oChange.CountryAbbrev);
         Log.writeLog(String.Format(" Duracion en cargar el connection String  : {0:hh\\:mm\\:ss}", stopwatch.Elapsed), System.Diagnostics.EventLogEntryType.Information);
         //stopwatch.Restart();
         Log.writeLog("Ejecucion de Store Procedure, LongDate: " + DateTime.Now.ToLongDateString() + ", LongTime: " + DateTime.Now.ToLongTimeString(), System.Diagnostics.EventLogEntryType.Information);
         DataAccess.NET.RemoteData oRemoteData = new DataAccess.NET.RemoteData();
         DataAccess.NET.RemoteParameterCollection oParams = new DataAccess.NET.RemoteParameterCollection();
         DataSet ds = new DataSet();

         oRemoteData.CommandTimeOut=0;
            oRemoteData.ConnectionTimeOut=0;
         String sRecordSource = "Set_Planilla";
         try
         {
            oRemoteData.ConnectionString = connectionString; 

            DataAccess.NET.RemoteParameter firstParam = new DataAccess.NET.RemoteParameter("@Email", 0, ParameterDirection.Input, SqlDbType.Text, oChange.Email.ToString());
            oParams.Add(firstParam);
            DataAccess.NET.RemoteParameter secondParam = new DataAccess.NET.RemoteParameter("@ChangePlanilla", 0, ParameterDirection.Input, SqlDbType.Float , Double.Parse(oChange.PlanillaChanged.ToString()));
            oParams.Add(secondParam);

            if (oRemoteData.GetDataSetSP("custm_BelcorpSetPlanilla",  oParams, sRecordSource, ref ds) == 0)
            {
               bResut = false;
            }
            else
            {
               if (ds.Tables[0].Rows[0]["existed"].ToString().Trim() =="0")
               {
                  bResut = false;
               }
               else
               {
                  bResut = true;
               }
            }
            
         }
         catch (Exception ex)
         {
            Log.writeLog(" Message : " + ex.Message + " , StackTrace : " + ex.StackTrace + "PROC. = custm_BelcorpSetPlanilla", System.Diagnostics.EventLogEntryType.Error);
            bResut = false;
         }
         finally
         {
            ds = null;
            oRemoteData = null;
            oParams = null;
            Log.writeLog(String.Format(" Duracion Total  : {0:hh\\:mm\\:ss}", stopwatch.Elapsed), System.Diagnostics.EventLogEntryType.Information);
            stopwatch.Stop();
         }
         return bResut;
      }

      public PoductsResponse getAcumulatedProductsByEmployee(PoductsRequest oProductsRequest)
      {
         Log.writeLog("INICIO llamado a WC: getAcumulatedProductsByEmployee , LongDate: " + DateTime.Now.ToLongDateString() + ", LongTime: " + DateTime.Now.ToLongTimeString(), System.Diagnostics.EventLogEntryType.Information);
         stopwatch.Start();
         loadConnectionString(oProductsRequest.CountryAbbrev);
         Log.writeLog(String.Format(" Duracion en cargar connection String  : {0:hh\\:mm\\:ss}", stopwatch.Elapsed), System.Diagnostics.EventLogEntryType.Information);
         
         PoductsResponse oResponse = new PoductsResponse();
         oResponse.ListProduct = new List<ProductResponse>();
         DataAccess.NET.RemoteData oRemoteData = new DataAccess.NET.RemoteData();
         DataAccess.NET.RemoteParameterCollection oParams = new DataAccess.NET.RemoteParameterCollection();
         String ListSkus = String.Empty;
         DataSet dsResult = new DataSet();
         try
         {
            oRemoteData.ConnectionString = connectionString;
            foreach (ProductRequest oProductRequest in oProductsRequest.ListProduct)
            {
               ListSkus += oProductRequest.SKU + ",";
            }
            if (oProductsRequest.ListProduct.Count > 0)
            {
               ListSkus = ListSkus.Substring(0, ListSkus.Length - 1);
            }

            DataAccess.NET.RemoteParameter firstParam = new DataAccess.NET.RemoteParameter("@SKUList", 0, ParameterDirection.Input, SqlDbType.Text, ListSkus.ToString());
            oParams.Add(firstParam);
            DataAccess.NET.RemoteParameter secondParam = new DataAccess.NET.RemoteParameter("@Email", 0, ParameterDirection.Input, SqlDbType.Text, oProductsRequest.EmailCustomer.ToString());
            oParams.Add(secondParam);
            DataAccess.NET.RemoteParameter thirdParam = new DataAccess.NET.RemoteParameter("@WebId", 0, ParameterDirection.Input, SqlDbType.Int, oProductsRequest.WebId);
            oParams.Add(thirdParam);

            oRemoteData.GetDataSetSP("custm_BelcorpGetAcumulatedProductsByEmployee", oParams, "AcumulatedProductsByEmployee", ref dsResult);
            foreach (DataRow dr in dsResult.Tables[0].Rows)
            {
               ProductResponse opProductResponse = new ProductResponse();
               opProductResponse.allowed = Double.Parse(dr["AvailableQty"].ToString()) >= oProductsRequest.getTotal(dr["SKU"].ToString());
               opProductResponse.SKU = dr["SKU"].ToString();
               opProductResponse.Available = dr["AvailableQty"].ToString();
               oResponse.ListProduct.Add(opProductResponse);
            }

         }
         catch (Exception ex)
         {
            Log.writeLog(" Message : " + ex.Message + " , StackTrace : " + ex.StackTrace + "PROC = custm_BelcorpGetAcumulatedProductsByEmployee", System.Diagnostics.EventLogEntryType.Error);

            throw;
         }
         finally
         {
            oRemoteData = null;
            dsResult = null;
            oParams = null;
            ListSkus = null;
            Log.writeLog(String.Format(" Duracion Total  : {0:hh\\:mm\\:ss}", stopwatch.Elapsed), System.Diagnostics.EventLogEntryType.Information);
            stopwatch.Stop();
         }
         return oResponse;
      }

      public BatResponse StartBatbyParam(BatParams Param)
      {
         Process proc = new  Process();
         String batFilePath = System.Configuration.ConfigurationManager.AppSettings["batFilePath"];
         String batFileNameEmployee = System.Configuration.ConfigurationManager.AppSettings["batFileNameEmployee"];
         String batFileNameProducts = System.Configuration.ConfigurationManager.AppSettings["batFileNameProducts"]; 
         String batFileNameCampaigne = System.Configuration.ConfigurationManager.AppSettings["batFileNameCampaigne"];
         BatResponse objResponse = new BatResponse();
         try
         {
            Log.writeLog("INFO  StartBatbyParam  -  Parma:" + Param.BatName  , EventLogEntryType.Information);
            switch (Param.BatName)
            {
               case  BatOptionsWCF.Employee:
                  proc.StartInfo.FileName = batFilePath + "\\" + batFileNameEmployee;
                  break;
               case  BatOptionsWCF.Products  :
                  proc.StartInfo.FileName = batFilePath + "\\" + batFileNameProducts;
                  break;
               case  BatOptionsWCF.Campaigne:
                  proc.StartInfo.FileName = batFilePath + "\\" + batFileNameCampaigne;
                  break; 
               default:
                  objResponse.resul = false;
                  objResponse.Mensage = "Parametro incorrecto:" + Param.BatName;
                  throw new Exception("Parametro incorrecto:" + Param.BatName);
            }
            proc.StartInfo.WorkingDirectory = batFilePath;
            proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();
            objResponse.resul = true;
            objResponse.Mensage = "Ejecutado correctamente";
         }
         catch (Exception ex)
         {
            Log.writeLog("ERROR:" + ex.Message + "StackTrace" + ex.StackTrace , EventLogEntryType.Error);
            objResponse.resul = false;
            objResponse.Mensage = "ERROR:" + ex.Message + "StackTrace" + ex.StackTrace;
         }
         finally
         {
            objResponse.Mensage += "\n " + proc.StartInfo.FileName;
         }
         return objResponse;
      }

      #endregion


      #region Functions
      private void loadConnectionString(String CountryAbbrev)
      {
         DataAccess.NET.RemoteData oRemoteData = new DataAccess.NET.RemoteData();
         DataSet ds = new DataSet();
         String SQuery = String.Empty;
         String sDataSource = String.Empty;
         String sDBName = String.Empty;
         try
         {
            oRemoteData.ConnectionString = this.connectionString;
            SQuery = "select * from belcorpdata..country where abbrev	='" + CountryAbbrev + "'";
            oRemoteData.GetDataSetQry(SQuery, "", ref ds);
            sDataSource = ds.Tables[0].Rows[0]["DataSource"].ToString();
            sDBName = ds.Tables[0].Rows[0]["DBName"].ToString();
         }
         catch (Exception ex)
         {
            Log.writeLog(" Message : " + ex.Message + " , StackTrace : " + ex.StackTrace + " Query : " + SQuery, System.Diagnostics.EventLogEntryType.Error);
            throw new Exception("Fallo coneccion Admin", ex);
         }
         finally
         {
            oRemoteData = null;
            ds = null;
         }
         this.connectionString = "uid=RetailUser;password=retail;database=" + sDBName + ";server=" + sDataSource + ";Connect Timeout=180;pooling=false";
      }

      private EmployeeCredit getLimitCredit(String CountryAbbrev, String Email)
      {
         EmployeeCredit LimitCredit = new EmployeeCredit();
         String sQuery = string.Empty;
         DataAccess.NET.RemoteData oDataAccess = new DataAccess.NET.RemoteData();
         DataSet dsLimit = new DataSet();
         try
         {
            Log.writeLog("Iniciando obtencion de conectionString  para " + CountryAbbrev + " , LongDate: " + DateTime.Now.ToLongDateString() + ", LongTime: " + DateTime.Now.ToLongTimeString(), System.Diagnostics.EventLogEntryType.Information);
            stopwatch.Start();
            loadConnectionString(CountryAbbrev);
            Log.writeLog(String.Format(" Duracion Total  : {0:hh\\:mm\\:ss}", stopwatch.Elapsed), System.Diagnostics.EventLogEntryType.Information);
            stopwatch.Restart();
            oDataAccess.ConnectionString = this.connectionString;
            Log.writeLog("conectando a la base de datos "+ connectionString + " , LongDate: " + DateTime.Now.ToLongDateString() + ", LongTime: " + DateTime.Now.ToLongTimeString(), System.Diagnostics.EventLogEntryType.Information);
            sQuery = "     select isnull(ct.CreditLimit,0)-isnull(ct.TotalRewards,0) as CreditAvailable from " + "CUSTOMER  c   ";
            sQuery += "    inner join Customer_totals ct on c.customerNo= ct.customerNo   ";
            sQuery += "    where CustomerType='E' and Email='" + Email + "' and StatusCode='A'     ";
            oDataAccess.GetDataSetQry(sQuery, "LimitCredit", ref dsLimit);
            Log.writeLog(String.Format(" Duracion Total  : {0:hh\\:mm\\:ss}", stopwatch.Elapsed), System.Diagnostics.EventLogEntryType.Information);
            LimitCredit.CreditAvailable = dsLimit.Tables[0].Rows[0]["CreditAvailable"].ToString().Trim(); 
         }
         catch (Exception ex)
         {
            Log.writeLog(" Message : " + ex.Message + " , StackTrace : " + ex.StackTrace + " sQuery : " + sQuery, System.Diagnostics.EventLogEntryType.Error);
         }
         finally
         {
            oDataAccess = null;
            dsLimit = null;
         }
         return LimitCredit;
      } 
      #endregion
   }
}
