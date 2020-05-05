using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using BLL;

namespace CurrencyTests
{
    [TestClass]
    public class UnitTests
    {
        private Calculation curr = new Calculation();
        private const Decimal Amount = 9;
        private const Decimal Expected = 9.264751615997579M;
        private string date = "2013-12-24";
        [TestMethod]
        public void CalculationTest()
        {
            var result = curr.FromToAmount("NOK", "SEK", Amount, date);
            Assert.AreEqual(result, Expected);
        }
    }
}
