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
        short length;

        public IDseq(ulong seed, ulong first_prime, ulong second_prime, short size)
        {
            num = seed;
            M = new BigInteger(first_prime) * new BigInteger(second_prime);
            length = size;
        }

        public string nextId()
        {
            string id = toBase36Converter();
            num = (num * num) % M;

            return id;
        }

        private string toBase36Converter()
        {
            int Base = 36;
            string digit = "0123456789abcdefghijklmnopqrstuvwxyz";

            BigInteger temp = num;
            string res = "";
            for(int i = 0; i < length; i++)
            {
                res += digit[(int)(temp % Base)];
                temp /= Base;
            }

            return res;
        }
    }
}
