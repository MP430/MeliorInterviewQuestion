using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Types;

namespace Melior.InterviewQuestion.Services
{
    public class PaymentService : IPaymentService
    {
        readonly IAccountDataStore AccountDataStore;

        public PaymentService(IAccountDataStore _AccountDataStore)
        {
            AccountDataStore = _AccountDataStore;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            MakePaymentResult result = new();
            Account account = AccountDataStore.GetAccount(request.DebtorAccountNumber);
            if (account == null)
            {
                result.Success = false;
                return result;
            }

            // In the original logic here, there's actually no way for the result to be successful.
            // I've changed the logic to fix that. Hopefully that's what was intended.
            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                        result.Success = false;

                    break;

                case PaymentScheme.FasterPayments:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                        result.Success = false;

                    if (account.Balance < request.Amount)
                        result.Success = false;

                    break;

                case PaymentScheme.Chaps:
                    if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                        result.Success = false;

                    if (account.Status != AccountStatus.Live)
                        result.Success = false;

                    break;

                default:
                    result.Success = false;
                    break;
            }

            if (result.Success)
            {
                account.Balance -= request.Amount;
                AccountDataStore.UpdateAccount(account);
            }

            return result;
        }
    }
}
