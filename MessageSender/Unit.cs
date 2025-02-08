using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowMuch
{
    public partial class MessageSenderUnit : ValueChangedMessage<string>
    {
        public MessageSenderUnit(string value) : base(value)
        {

        }
    }
}
