using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CSI.Models;

namespace GUI.UnitTests.Models
{
    [TestClass]
    public class PrescriptionTests
    {
        [TestMethod]
        public void Prescription_Patient_EqualsFirstNamePlusLastName()
        {
            var prescription = new Prescription()
            {
                FirstName = "Bruce",
                LastName = "Banner"
            };

            string actualValue = prescription.Patient;
            string expectedValue = "Banner, Bruce";

            Assert.AreEqual(actualValue, expectedValue);
        }

    }
}
