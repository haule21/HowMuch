using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowMuch
{
    public partial class MessageSenderStock : ValueChangedMessage<string>
    {
        public MessageSenderStock(string value) : base(value)
        {

        }
    }
}
