using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMDM_Network_Protocol.Enums
{
    public enum Channels : byte
    {
        Set3Axis = 0x00,
        Set6Axis = 0x01,
        Set10Axis = 0x02
    }
}
