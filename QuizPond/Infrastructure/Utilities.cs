using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace QuizPond.Infrastructure
{
    public static class Utilities
    {
        public static bool IsNumber(this string aNumber)
        {
            BigInteger temp_big_int;
            var is_number = BigInteger.TryParse(aNumber, out temp_big_int);
            return is_number;
        }
    }
}
