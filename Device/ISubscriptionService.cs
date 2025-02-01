using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HowMuch
{
    public interface ISubscriptionService
    {
        Task<bool> ValidateSubscription(string paymentToken);
        Task<bool> Subscribe();
    }
}
