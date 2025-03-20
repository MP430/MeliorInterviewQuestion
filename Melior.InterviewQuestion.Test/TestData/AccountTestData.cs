using Melior.InterviewQuestion.Types;

namespace Melior.InterviewQuestion.Test.TestData
{
    internal static class AccountTestData
    {
        internal static Account Item
        {
            get => new Account()
            {
                AccountNumber = "123",
                AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps,
                Balance = 1000,
                Status = AccountStatus.Live,
            };
        }
    }
}
