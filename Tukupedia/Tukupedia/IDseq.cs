using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tukupedia
{
    class IDseq
    {
        BigInteger num;
        BigInteger M;

        public IDseq(long seed, long first_prime, long second_prime)
        {
            num = seed;
            M = new BigInteger(first_prime) * new BigInteger(second_prime);
        }

        public string nextId()
        {
            string id = toBase64Converter();
            num = (num * num) % M;

            return id;
        }

        private string toBase64Converter()
        {
            int Base = 36;
            string digit = "0123456789abcdefghijklmnopqrstuvwxyz";

            BigInteger temp = num;
            string res = "";
            while(temp > 0)
            {
                res += digit[(int)(temp % Base)];
                temp /= Base;
            }

            return res;
        }
    }
}
