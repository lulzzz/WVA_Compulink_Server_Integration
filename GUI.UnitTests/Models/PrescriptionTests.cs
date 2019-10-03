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

            string actual = prescription.Patient;
            string expected = "Banner, Bruce";

            Assert.AreEqual(expected, actual);
        }

    }
}
