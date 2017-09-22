using System;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Odin.Data.Persistence;
using Odin.IntegrationTests.Helpers;

namespace Odin.IntegrationTests.TestAttributes
{
    public class CleanData : Attribute, ITestAction
    {
        private readonly ApplicationDbContext _context;
        public CleanData()
        {
            _context = new ApplicationDbContext();
        }

        public void BeforeTest(ITest test)
        {
            OrderHelper.ClearIntegrationOrders(_context);
        }

        public void AfterTest(ITest test)
        {
            OrderHelper.ClearIntegrationOrders(_context);
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}
