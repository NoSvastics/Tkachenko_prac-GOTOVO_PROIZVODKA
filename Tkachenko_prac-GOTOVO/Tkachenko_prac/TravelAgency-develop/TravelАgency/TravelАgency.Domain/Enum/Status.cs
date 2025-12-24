using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelАgency.Domain.Enum
{
    public enum Status
    {
        [Description("Не рассмотрено")]
        NotConsidered = 0,

        [Description("Одобрено")]
        Approved,

        [Description("Отказано")]
        Denied,

    }
}
