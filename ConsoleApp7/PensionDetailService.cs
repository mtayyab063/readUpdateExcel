using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;

namespace ConsoleApp7
{
    public static class PensionDetailService
    {
        private static string APIUrl = "https://integrate.gsb.government.ae/gateway/getPensionerDetails_GPSSA/1.0/customers/";
        public static async Task<RequiredData> GetDataWithAuthentication(string eida)
        {
            var excelData = new RequiredData();
            try
            {
                var authCredential = Encoding.UTF8.GetBytes("MOHREPrdConsumer2:mohre@consumer$2");
                using (var client = new HttpClient())
                {


                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authCredential));
                    client.DefaultRequestHeaders.Add("GSB-APIKey", "3f2619b0-01ea-11ed-a803-9102dfae9fee");
                    client.BaseAddress = new Uri(APIUrl);
                    HttpResponseMessage response = await client.GetAsync(APIUrl + eida + "/personal-profile/");

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var readTask = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var rawResponse = readTask.GetAwaiter().GetResult();
                            Response r = JsonConvert.DeserializeObject<Response>(rawResponse);
                            if (r.Succeeded)
                            {
                                excelData.IsInsured = (string.IsNullOrEmpty(Convert.ToString(r.Data.PersonalDetails.IsInsured)) ? "No Data" : Convert.ToString(r.Data.PersonalDetails.IsInsured));
                                excelData.IsPensionior = (string.IsNullOrEmpty(Convert.ToString(r.Data.PersonalDetails.IsPensioner)) ? "No Data" : Convert.ToString(r.Data.PersonalDetails.IsPensioner));
                                excelData.IsBenificiary = (string.IsNullOrEmpty(Convert.ToString(r.Data.PersonalDetails.IsBeneficiary)) ? "No Data" : Convert.ToString(r.Data.PersonalDetails.IsBeneficiary));
                            }
                            else
                            {
                                excelData.IsInsured = "";
                                excelData.IsPensionior = "";
                                excelData.IsBenificiary = "";
                            }
                            //Console.WriteLine(rawResponse);
                        }
                        catch (TaskCanceledException ex)
                        {
                            Console.WriteLine(ex);
                        }

                    }
                }
                //return excelData;
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine(ex);
            }
            return excelData;
        }

        public class RequiredData
        {
            public string IsBenificiary { get; set; }
            public string IsInsured { get; set; }
            public string IsPensionior { get; set; }
        }

        public partial class Response
        {
            [JsonProperty("succeeded")]
            public bool Succeeded { get; set; }

            [JsonProperty("data")]
            public Data Data { get; set; }
        }

        public partial class Data
        {
            [JsonProperty("personalDetails")]
            public PersonalDetails PersonalDetails { get; set; }

            [JsonProperty("insuredDetails")]
            public InsuredDetails InsuredDetails { get; set; }

            [JsonProperty("pensionerPensionDetails")]
            public object PensionerPensionDetails { get; set; }

            [JsonProperty("beneficiaryPensionDetails")]
            public object BeneficiaryPensionDetails { get; set; }
        }

        public partial class InsuredDetails
        {
            [JsonProperty("employmentStartDate")]
            public string EmploymentStartDate { get; set; }

            [JsonProperty("employerArabicName")]
            public string EmployerArabicName { get; set; }

            [JsonProperty("employerEnglishName")]
            public string EmployerEnglishName { get; set; }

            [JsonProperty("sector")]
            public string Sector { get; set; }

            [JsonProperty("licenseNumber")]
            public object LicenseNumber { get; set; }

            [JsonProperty("companyCode")]
            //[JsonConverter(typeof(string))]
            public string CompanyCode { get; set; }

            [JsonProperty("salaryDetails")]
            public SalaryDetails SalaryDetails { get; set; }

            [JsonProperty("contributionDetails")]
            public ContributionDetails ContributionDetails { get; set; }
        }

        public partial class ContributionDetails
        {
            [JsonProperty("totalContributionAmount")]
            public string TotalContributionAmount { get; set; }

            [JsonProperty("employerContributionAmount")]
            public string EmployerContributionAmount { get; set; }

            [JsonProperty("lastContributionDate")]
            public string LastContributionDate { get; set; }
        }

        public partial class SalaryDetails
        {
            [JsonProperty("basicSalary")]
            public string BasicSalary { get; set; }

            [JsonProperty("deductionAmount")]
            public string DeductionAmount { get; set; }

            [JsonProperty("husingAllowance")]
            public string HusingAllowance { get; set; }

            [JsonProperty("socialAllowance")]
            public string SocialAllowance { get; set; }

            [JsonProperty("childAllowance")]
            public string ChildAllowance { get; set; }

            [JsonProperty("costOfLiving")]
            public string CostOfLiving { get; set; }

            [JsonProperty("otherAllowance")]
            public string OtherAllowance { get; set; }

            [JsonProperty("totalSalary")]
            public string TotalSalary { get; set; }
        }

        public partial class PersonalDetails
        {
            [JsonProperty("nationalId")]
            public string NationalId { get; set; }

            [JsonProperty("fullNameArabic")]
            public string FullNameArabic { get; set; }

            [JsonProperty("fullNameEnglish")]
            public string FullNameEnglish { get; set; }

            [JsonProperty("isInsured")]
            public bool IsInsured { get; set; }

            [JsonProperty("isPensioner")]
            public bool IsPensioner { get; set; }

            [JsonProperty("isBeneficiary")]
            public bool IsBeneficiary { get; set; }

            [JsonProperty("salutation")]
            public object Salutation { get; set; }

            [JsonProperty("gender")]
            public string Gender { get; set; }

            [JsonProperty("maritalStatus")]
            public string MaritalStatus { get; set; }

            [JsonProperty("birthDate")]
            public string BirthDate { get; set; }

            [JsonProperty("nationality")]
            public string Nationality { get; set; }

            [JsonProperty("cellphone")]
            //[JsonConverter(typeof(string))]
            public string Cellphone { get; set; }

            [JsonProperty("email")]
            public string Email { get; set; }
        }
    }
}
