using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiVRoom.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Clerk_Id { get; set; } = string.Empty;
        public int ChannelSettings_Id { get; set; }
        public bool IsPremium { get; set; } = false;
        public bool SubscribedOnMySubscriptionChannelActivity { get; set; } = true;
        public bool SubscribedOnActivityOnMyChannel { get; set; } = true;
        public bool SubscribedOnRecomendedVideo { get; set; } = true;
        public bool SubscribedOnOnActivityOnMyComments { get; set; } = true;
        public bool SubscribedOnOthersMentionOnMyChannel { get; set; } = true;
        public bool SubscribedOnShareMyContent { get; set; } = true;
        public bool SubscribedOnPromotionalContent { get; set; } = true;

        public bool SubscribedOnMainEmailNotifications { get; set; } = true;
       
        public bool EmailSubscribedOnMySubscriptionChannelActivity { get; set; } = false;
        public bool EmailSubscribedOnActivityOnMyChannel { get; set; } = false;
        public bool EmailSubscribedOnRecomendedVideo { get; set; } = false;
        public bool EmailSubscribedOnOnActivityOnMyComments { get; set; } = false;
        public bool EmailSubscribedOnOthersMentionOnMyChannel { get; set; } = false;
        public bool EmailSubscribedOnShareMyContent { get; set; } = false;
        public bool EmailSubscribedOnPromotionalContent { get; set; } = false;

    }
}
