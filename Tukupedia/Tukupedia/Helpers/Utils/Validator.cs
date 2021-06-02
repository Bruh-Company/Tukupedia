using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Tukupedia.Helpers.Utils
{
    static class Validator
    {
        public static bool Email(string str)
        {
            string patt = @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$";
            return (new Regex(patt)).IsMatch(str);
        }

        public static bool PhoneNumber(string str)
        {
            string patt = @"^[+]*[(]{0,1}[0-9]{1,4}[)]{0,1}[-\s\./0-9]*$";
            return (new Regex(patt)).IsMatch(str);
        }

        public static bool Password(string str)
        {
            /**
             *  between 8 and 40 characters, contain at least one digit
             *  and one alphabetic character, and must not contain
             *  special characters
             */
            string patt = @"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,40})$";
            return (new Regex(patt)).IsMatch(str);
        }

        public static bool Name(string str)
        {
            string patt = @"^[a-zA-Z''-'\s]{1,40}$";
            return (new Regex(patt)).IsMatch(str);
        }

        public static bool Numeric(string str)
        {
            string patt = @"^[0-9]+$";
            return (new Regex(patt)).IsMatch(str);
        }
    }
}
