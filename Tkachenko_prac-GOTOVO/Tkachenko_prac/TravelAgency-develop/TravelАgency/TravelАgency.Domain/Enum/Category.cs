using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelАgency.Domain.Enum
{
    using System.ComponentModel;

    namespace TravelАgency.Domain.Enum
    {
        public enum Category
        {
            [Description("Таблетки")]
            Tablets = 0,

            [Description("Сиропы")]
            Syrups = 1,

            [Description("Мази и гели")]
            Ointments = 2,

            [Description("Витамины")]
            Vitamins = 3,

            [Description("Антибиотики")]
            Antibiotics = 4
        }
    }
}

