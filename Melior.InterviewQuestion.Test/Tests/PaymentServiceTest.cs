using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Services;
using Melior.InterviewQuestion.Test.TestData;
using Melior.InterviewQuestion.Types;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

namespace Melior.InterviewQuestion.Test.Tests
{
    [TestClass]
    public sealed class PaymentServiceTest
    {
        Account account = null;
        MakePaymentRequest request = null;
        IAccountDataStore dataStore = null;
        PaymentService service = null;

        [TestInitialize]
        public void Initialize()
        {
            account = AccountTestData.Item;
            request = MakePaymentRequestTestData.Item;
            dataStore = Substitute.For<IAccountDataStore>();
            service = new(dataStore);
            dataStore.GetAccount(Arg.Any<string>()).Returns(account);
        }

        [TestMethod]
        public void MakePayment_Success()
        {
            var result = service.MakePayment(request);

            Assert.IsTrue(result.Success);
            dataStore.Received().UpdateAccount(Arg.Any<Account>());
        }

        [TestMethod]
        public void MakePayment_InsufficientFunds()
        {
            request.PaymentScheme = PaymentScheme.FasterPayments;
            request.Amount = 5000;
            var result = service.MakePayment(request);

            Assert.IsFalse(result.Success);
            dataStore.DidNotReceive().UpdateAccount(Arg.Any<Account>());
        }

        [TestMethod]
        public void MakePayment_InactiveAccount()
        {
            MakePaymentResult result;

            request.PaymentScheme = PaymentScheme.Chaps;
            account.Status = AccountStatus.Disabled;
            result = service.MakePayment(request);
            Assert.IsFalse(result.Success);
            dataStore.DidNotReceive().UpdateAccount(Arg.Any<Account>());

            request.PaymentScheme = PaymentScheme.Chaps;
            account.Status = AccountStatus.InboundPaymentsOnly;
            result = service.MakePayment(request);
            Assert.IsFalse(result.Success);
            dataStore.DidNotReceive().UpdateAccount(Arg.Any<Account>());
        }

        [TestMethod]
        public void MakePayment_NoAccount()
        {
            dataStore.GetAccount(Arg.Any<string>()).ReturnsNull();
            var result = service.MakePayment(request);

            Assert.IsFalse(result.Success);
            dataStore.DidNotReceive().UpdateAccount(Arg.Any<Account>());
        }

        [TestMethod]
        public void MakePayment_PaymentSchemeNotAllowed()
        {
            MakePaymentResult result;

            request.PaymentScheme = PaymentScheme.Bacs;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            result = service.MakePayment(request);
            Assert.IsFalse(result.Success);
            dataStore.DidNotReceive().UpdateAccount(Arg.Any<Account>());

            request.PaymentScheme = PaymentScheme.FasterPayments;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            result = service.MakePayment(request);
            Assert.IsFalse(result.Success);
            dataStore.DidNotReceive().UpdateAccount(Arg.Any<Account>());

            request.PaymentScheme = PaymentScheme.Chaps;
            account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
            result = service.MakePayment(request);
            Assert.IsFalse(result.Success);
            dataStore.DidNotReceive().UpdateAccount(Arg.Any<Account>());
        }

        [TestMethod]
        public void MakePayment_InvalidPaymentScheme()
        {
            request.PaymentScheme = (PaymentScheme)(-1);
            var result = service.MakePayment(request);
            Assert.IsFalse(result.Success);
            dataStore.DidNotReceive().UpdateAccount(Arg.Any<Account>());
        }
    }
}
