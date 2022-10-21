using System;
using System.Linq;
using PhoneNumberChecker.Models;
using Microsoft.AspNetCore.Mvc;
using PhoneNumbers;
using System.Data;

namespace PhoneNumberChecker.Controllers
{
    public class PhoneController : Controller
    {
        private static PhoneNumberUtil phoneUtil;
        private PhoneNumber phoneNumber;
        private string countryCodeSeleted;

        public PhoneController()
        {
            phoneUtil = PhoneNumberUtil.GetInstance();
        }

        public IActionResult Verify()
        {
            var model = new PhoneNumberCheckViewModel()
            {
                CountryCodeSelected = "AU"
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Verify(PhoneNumberCheckViewModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Parse the number to check into a PhoneNumber object.
                    phoneNumber = phoneUtil.Parse(model.PhoneNumber, model.CountryCodeSelected);
                    countryCodeSeleted = model.CountryCodeSelected;
               
                    ModelState.FirstOrDefault(x => x.Key == nameof(model.IsValid)).Value.RawValue =
                        phoneUtil.IsValidNumberForRegion(phoneNumber, model.CountryCodeSelected);
                    ModelState.FirstOrDefault(x => x.Key == nameof(model.IsPossible)).Value.RawValue =
                        phoneUtil.IsPossibleNumber(phoneNumber);
                    ModelState.FirstOrDefault(x => x.Key == nameof(model.PhoneType)).Value.RawValue =
                     phoneUtil.GetNumberType(phoneNumber);
                    ModelState.FirstOrDefault(x => x.Key == nameof(model.InternationalFormat)).Value.RawValue =
                        phoneUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL);

                    // The submitted value has to be returned as the raw value.
                    ModelState.FirstOrDefault(x => x.Key == nameof(model.CountryCodeSelected)).Value.RawValue =
                        model.CountryCodeSelected;
                    ModelState.FirstOrDefault(x => x.Key == nameof(model.PhoneNumber)).Value.RawValue =
                        model.PhoneNumber;

                    return View(model);
                }
                catch (NumberParseException npex)
                {
                    // If PhoneNumberUtil throws an error, add it to the list of ModelState errors.
                    ModelState.AddModelError(npex.ErrorType.ToString(), npex.Message);
                }
            }
            return View(model);
        }

        public ActionResult DownloadCSV()
        {
            DataTable dataTable = CsvCreator.CreateDataTable(
                 phoneUtil.IsValidNumberForRegion(phoneNumber, countryCodeSeleted),
                 phoneUtil.IsPossibleNumber(phoneNumber),
                 phoneUtil.GetNumberType(phoneNumber).ToString(),
                 phoneUtil.Format(phoneNumber, PhoneNumberFormat.INTERNATIONAL)
             );

            string filePath = "PhoneNumberChecker" + DateTime.Today.ToString("yyyy-MM-dd") + ".csv";

            var output = dataTable.ToCsvByteArray();

            return new FileContentResult(output, "text/csv")
            {
                FileDownloadName = filePath
            };
        }
    }
}