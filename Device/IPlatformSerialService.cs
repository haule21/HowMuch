using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HowMuch
{
    public interface IPlatformSerialService
    {
        Task<string> GetSerialNumber();
    }
}
