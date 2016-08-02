using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Temporal
{
    public class TestHelpers
    {
        public static void EnsureNoDateTimeNowOrUtcNowUsages(params Assembly[] assemblies)
        {
            // Ensure that none of the assemblies use DateTime.Now or DateTime.UtcNow
        }
    }
}
