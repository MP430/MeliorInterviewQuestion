using Melior.InterviewQuestion.Types;

namespace Melior.InterviewQuestion.Test.TestData
{
    internal static class MakePaymentRequestTestData
    {
        internal static MakePaymentRequest Item
        {
            get => new MakePaymentRequest()
            {
                Amount = 100,
                CreditorAccountNumber = "123",
                DebtorAccountNumber = "456",
                PaymentDate = DateTime.Parse("2025-03-20 10:00"),
                PaymentScheme = PaymentScheme.Bacs,
            };
        }
    }
}
