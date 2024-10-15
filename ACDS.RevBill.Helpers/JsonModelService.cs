using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
using ACDS.RevBill.Shared.DataTransferObjects.Enumeration;
using ACDS.RevBill.Shared.DataTransferObjects.Billing;
using ACDS.RevBill.Shared.DataTransferObjects.Reporting;
using ACDS.RevBill.Shared.DataTransferObjects.Reporting.Agency;
using System.Linq.Expressions;
using ACDS.RevBill.Entities;
using ACDS.RevBill.Entities.Models;
using Azure;
using MathNet.Numerics.LinearAlgebra.Factorization;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;


namespace ACDS.RevBill.Helpers
{
    public class JsonModelService
    {
        private readonly string _owner = "FCTRS";
        private readonly string _owner2 = "KJ";
        private readonly string _baseUrl = "https://services.ebs-rcm.com/prCallProcfctrs.asmx/prDBConnect";
        private readonly PID _pidConfig;
        private readonly string _operation = "PostTring";

        /// <summary>
        /// Calls soap base web service in Asynchronously
        /// </summary>
        /// <param name="storedProcedure">The <see cref="System.string"/> of the stored procedure and parameter </param>
        /// <returns>An instance of <see cref="System.Data.DataSet"/> from the SOAP Service </returns>
        public async Task<DataSet> QueryProcessorAsync(string storedProcedure)

        {

            try
            {

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;


                using var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromSeconds(1000);
                var encoding = new ASCIIEncoding();

                var operation = $"{_operation}=FCTRS.dbo.prWebAgency '300',null,null, null";
                var client = new RestClient();

                var request = new RestRequest(_baseUrl, Method.Post);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("PostTring", "FCTRS.dbo.prWebAgency '300',null,null, null");

                var response2 = await client.ExecuteAsync(request);


                var content = response2.Content;
                //var response = await httpClient.PostAsync(_baseUrl, null);

                ////response.EnsureSuccessStatusCode();

                //var responseStream = await response.Content.ReadAsStreamAsync();

                //var responseString = await new StreamReader(responseStream, Encoding.ASCII).ReadToEndAsync();

                var xmlReader = XmlReader.Create(new StringReader(content));

                var dataset = new DataSet();

                dataset.ReadXml(xmlReader);

                return dataset;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DataSet QueryProcessor(string query)
        {
            try
            {
                HttpWebRequest httpReq = (HttpWebRequest)WebRequest.Create(_baseUrl);

                ASCIIEncoding encoding = new ASCIIEncoding();

                string postTring = "PostTring=" + HttpUtility.UrlEncode(query);

                byte[] postData = encoding.GetBytes(postTring);
                httpReq.ContentType = "application/x-www-form-urlencoded";
                httpReq.Method = "POST";
                httpReq.ContentLength = postData.Length;
                Stream reqStrm = httpReq.GetRequestStream();
                reqStrm.Write(postData, 0, postData.Length);
                reqStrm.Close();

                 HttpWebResponse httpResp = (HttpWebResponse)httpReq.GetResponse();
                StreamReader respStrm = new StreamReader(httpResp.GetResponseStream(), Encoding.ASCII);
                string lReturnValue = respStrm.ReadToEnd();
                httpResp.Close();
                respStrm.Close();
                XmlReader xmlReader = XmlReader.Create(new StringReader(lReturnValue));

                DataSet ds = new DataSet();
                ds.ReadXml(xmlReader);

                return ds;
            }
            catch (Exception ex)
            {

            }
            return new DataSet();
        }
        public async Task<DataSet> QueryProcesser(string query)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, _baseUrl);
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("PostTring", query));
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();

            // Convert to DataSet
            return ConvertToDataSet(responseString);

        }
        public static DataSet ConvertToDataSet(string responseString)
        {
            var dataSet = new DataSet();

            using (var stringReader = new System.IO.StringReader(responseString))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    dataSet.ReadXml(xmlReader);
                }
            }

            return dataSet;
        }
        public IEnumerable<VerifyPidResponseDto> VerifyPayerId(PayerIdEnumerationDto verifyPid)
        {
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prgetname '{verifyPid.PayerId}'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<VerifyPidResponseDto> emptyModel = new List<VerifyPidResponseDto>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<VerifyPidResponseDto>>();

            return models;
        }
        public async Task<PIDResponse> CreatePIDWithBioData(CustomerEnumerationDto pidEntity)
        {
            var pidCreationUrl = $"{_pidConfig.BASE_URL}/Interface/pidcreation";
            var pidCreationHash = EncryptionUtility.CreateSHA512(_pidConfig.KEY + pidEntity.Phone + pidEntity.Email + pidEntity.Address + _pidConfig.STATE);

            //map hash, state and client id
            pidEntity.Hash = pidCreationHash;
            pidEntity.State = _pidConfig.STATE;
            pidEntity.ClientId = _pidConfig.CLIENT_ID;

            //generate PID
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, pidCreationUrl);

            //serialize payload 
            var payload = JsonConvert.SerializeObject(pidEntity);
            var content = new StringContent(payload, null, "application/json");
            request.Content = content;

            //get response
            var response = await client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync(); // here you can read response as string

            //deserialise response
            PIDResponse output = JsonConvert.DeserializeObject<PIDResponse>(responseContent);

            return output;
        }
        public IEnumerable<GetTaxPayerByPhoneNumberResponseDto> GetCustomerDetailsByPhoneNumber(GetTaxPayerRequestDto getTaxPayer)
        {
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prSMSEmail2_secure '{getTaxPayer.Param}'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<GetTaxPayerByPhoneNumberResponseDto> emptyModel = new List<GetTaxPayerByPhoneNumberResponseDto>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<GetTaxPayerByPhoneNumberResponseDto>>();

            return models;
        }

        public IEnumerable<GetTaxPayerByNameResponseDto> GetCustomerDetailsByName(GetTaxPayerRequestDto getTaxPayer)
        {
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prPersFindName_e '{getTaxPayer.Param}'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<GetTaxPayerByNameResponseDto> emptyModel = new List<GetTaxPayerByNameResponseDto>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<GetTaxPayerByNameResponseDto>>();

            return models;
        }

        public IEnumerable<GetTaxPayerByEmailResponseDto> GetCustomerDetailsByEmail(GetTaxPayerRequestDto getTaxPayer)
        {
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prSMSEmail_secure '{getTaxPayer.Param}'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<GetTaxPayerByEmailResponseDto> emptyModel = new List<GetTaxPayerByEmailResponseDto>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<GetTaxPayerByEmailResponseDto>>();

            return models;
        }

        public IEnumerable<GetTaxPayerByEmailResponseDto> CreatePIDUsingBVN(GetTaxPayerRequestDto getTaxPayer)
        {
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prCreatePayerIDIndiv_BVN '{getTaxPayer.Param}'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<GetTaxPayerByEmailResponseDto> emptyModel = new List<GetTaxPayerByEmailResponseDto>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<GetTaxPayerByEmailResponseDto>>();

            return models;
        }
        public IEnumerable<GenerateBillResponse> GenerateBillReference(GenerateBillRequest generateBillRequest)
        {
            String[] PayerID = generateBillRequest.PayerID.Split("-");
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prWebtellerPostCustomer_Direct '{PayerID[0]}',{PayerID[1]},'{generateBillRequest.AgencyRef}','{generateBillRequest.RevCode}',{generateBillRequest.Amount},'{generateBillRequest.EntryDate}','{generateBillRequest.AssessRef}','{generateBillRequest.AppliedDate}',{generateBillRequest.Year},null,null,null,null,null,null,null,'RevBillNew','{generateBillRequest.BillReference}','{generateBillRequest.HarmonizedBillReference}',null,'RevBillNew','{generateBillRequest.PropertyAddress}'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<GenerateBillResponse> emptyModel = new List<GenerateBillResponse>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<GenerateBillResponse>>();

            return models;
        }
        public async Task<IEnumerable<GetAgencyResponse>> GetEBSAgencyByCode(string agencycode)
        {

            var response = QueryProcessorAsync($"{_owner}.dbo.prWebAgency '{agencycode}',null,null, null").GetAwaiter().GetResult();

            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prWebAgency '{agencycode}',null,null, null");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<GetAgencyResponse> emptyModel = new List<GetAgencyResponse>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<GetAgencyResponse>>();

            return models;
        }


        public IEnumerable<GetRevenueResponse> GetEBSRevenueByCode(string agencycode)
        {

            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prWebRevHead2 '{agencycode}'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<GetRevenueResponse> emptyModel = new List<GetRevenueResponse>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<GetRevenueResponse>>();

            return models;
        }
        public IEnumerable<GetBankCodeResponse> GetBankCodes()
        {
            DataSet JsonFileName = QueryProcessor($"lasg.dbo.prBankActive null");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<GetBankCodeResponse> emptyModel = new List<GetBankCodeResponse>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<GetBankCodeResponse>>();

            return models;
        }

        public IEnumerable<UpdateBillResponse> UpdateBillonEbs(int editorId, UpdateBillRequest updateBillRequest)
        {

            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prWebtellerEditAssmt_New '{updateBillRequest.BillReference}','{updateBillRequest.Amount}',{editorId},null,null");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<UpdateBillResponse> emptyModel = new List<UpdateBillResponse>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<UpdateBillResponse>>();
            return models;
        }

        public IEnumerable<HarmonizedBillReferenceResponseDto> GetHarmonizedBillReferences(HarmonizedBillReferenceRequestDto billReferenceRequestDto)
        {
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prWebtellerSelectCustomer_Bulk '{billReferenceRequestDto.HarmonizedBillReference}',null,'070','NGN'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<HarmonizedBillReferenceResponseDto> emptyModel = new List<HarmonizedBillReferenceResponseDto>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<HarmonizedBillReferenceResponseDto>>();

            return models;
        }

        public IEnumerable<CorporatePayerIDResponse> CreateCorporatePID(CorporatePayerIDRequest generatePID)
        {
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prCreatePayerIDCoy_New {1912077}, '{generatePID.CompanyName}', '{generatePID.Email}', '{generatePID.PhoneNumber}', null, '{generatePID.Address}', null, null, 'OCC', 'Coy', {12}, '{generatePID.DateofIncorporation}', null, null");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (string.IsNullOrWhiteSpace(json) || json.Length <= 2)
            {
                return new List<CorporatePayerIDResponse>();
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();
            if (outputData == null || !outputData.Any())
            {
                return new List<CorporatePayerIDResponse>(); // Return an empty list if no output data found
            }

            // Map the JArray to a list of CorporatePayerIDResponse
            var models = outputData.ToObject<List<CorporatePayerIDResponse>>();

            return models;
        }

        public IEnumerable<BankCollectionResponse> CollectionByBank(BankCollectionRequest request)
        {
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prBankDailyCollection_New_e '{request.uCode}', '{request.ipAddress}', {request.StartDate}, {request.EndDate}, '{request.StartEntryID}', '{request.EndEntryID}', '{request.AcctType}', '{request.Bank}', '{request.RevCode}', {request.AgencyRef}, '{request.Owner}', '{request.BankAcct}'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<BankCollectionResponse> emptyModel = new List<BankCollectionResponse>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var models = outputData.ToObject<List<BankCollectionResponse>>();

            return models;
        }

        public async Task<IEnumerable<AgencyYearlyCollectionResponse>> GetAgencyYearlyCollections(AgencyYearlyCollectionRequest getresult)
        {
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prCollAgency_Year_e {getresult.TrendYear}, '{getresult.AgencyRef}', '{_owner2}'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<AgencyYearlyCollectionResponse> emptyModel = new List<AgencyYearlyCollectionResponse>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();

            var collection = outputData.ToObject<List<AgencyYearlyCollectionResponse>>();
            return collection;
        }
        public async Task<IEnumerable<object>> GetAgencyQuarterlyCollections(AgencyQuarterlyCollectionRequest getquarterlyresult)
        {
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prCollAgency_Quarterly_e {getquarterlyresult.TrendYear},{getquarterlyresult.Qrter}, '{getquarterlyresult.AgencyRef}', '{_owner2}'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<AgencyQuarterlyCollectionResponse> emptyModel = new List<AgencyQuarterlyCollectionResponse>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();
            if (getquarterlyresult.Qrter != 1)
            {
                switch (getquarterlyresult.Qrter)
                {
                    case 2:
                        var result2 = outputData.ToObject<List<AgencyQuarter2>>();
                        return result2;

                    case 3:
                        var result3 = outputData.ToObject<List<AgencyQuarter3>>();
                        return (result3);

                    default:
                        var result4 = outputData.ToObject<List<AgencyQuarter4>>();
                        return result4;
                }
            }
            var collection = outputData.ToObject<List<AgencyQuarterlyCollectionResponse>>();

            return collection;
        }
        public async Task<IEnumerable<object>> GetAgencyBiAnnualCollection(AgencyBiAnnualCollectionRequest bianualCol)
        {
            DataSet JsonFileName = QueryProcessor($"{_owner}.dbo.prCollAgency_BiAnnual_e {bianualCol.TrendYear},{bianualCol.BiAnnual}, '{bianualCol.AgencyRef}', '{_owner2}'");
            string json = JsonConvert.SerializeObject(JsonFileName, Newtonsoft.Json.Formatting.Indented);

            if (json.Length <= 2)
            {
                List<AgencyBiAnnualCollectionResponse> emptyModel = new List<AgencyBiAnnualCollectionResponse>();
                return emptyModel;
            }

            var project = (JObject)JsonConvert.DeserializeObject(json);
            var outputData = project["OutputData"].Value<JArray>();
            if (bianualCol.BiAnnual != 1)
            {
                var collection2 = outputData.ToObject<List<AgencyBiAnnual2>>();
                return collection2;
            }
            var collection = outputData.ToObject<List<AgencyBiAnnualCollectionResponse>>();

            return collection;
        }


    }
}