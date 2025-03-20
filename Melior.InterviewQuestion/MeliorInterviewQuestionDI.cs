using System.Configuration;
using Melior.InterviewQuestion.Data;
using Melior.InterviewQuestion.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Melior.InterviewQuestion
{
    public static class MeliorInterviewQuestionDI
    {
        public static void AddMeliorInterviewQuestion(this IServiceCollection services)
        {
            // I've decided to use DI for this check, but this could also go inside the AccountDataStore instead.
            // How different are the two data stores really?
            // I don't know with the information I have, but if they're very similar they should reuse logic instead of this.
            if (ConfigurationManager.AppSettings["DataStoreType"] == "Backup")
                services.AddSingleton<IAccountDataStore, BackupAccountDataStore>();
            else
                services.AddSingleton<IAccountDataStore, AccountDataStore>();

            services.AddSingleton<IPaymentService, PaymentService>();
        }
    }
}
