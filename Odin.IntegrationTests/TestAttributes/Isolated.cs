using System;
using System.Transactions;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Odin.Domain;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;

namespace Odin.IntegrationTests.TestAttributes
{
    public class Isolated : Attribute, ITestAction
    {
        private TransactionScope _transactionScope;
        public void BeforeTest(ITest test)
        {
            _transactionScope = new TransactionScope();
        }

        public void AfterTest(ITest test)
        {
            _transactionScope.Dispose();
        }

        public ActionTargets Targets => ActionTargets.Test;
    }
}
