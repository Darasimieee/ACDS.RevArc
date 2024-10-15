using System;
using System.Text.RegularExpressions;

namespace ACDS.RevBill.Helpers
{
    public class EnumerationUtility
    {
        public static string GenerateNigerianPhoneNumber()
        {
            Random random = new Random();
            int prefixIndex = random.Next(NigerianPhonePrefixes.Length);
            string prefix = NigerianPhonePrefixes[prefixIndex];

            // Generate the remaining 8 digits
            string remainingDigits = random.Next(10000000, 99999999).ToString();

            // Pad the remaining digits with leading zeros if necessary
            remainingDigits = remainingDigits.PadLeft(8, '0');

            string number = prefix + remainingDigits;

            return number;
        }

        public static string ExtractPayerID(string input)
        {
            string pattern = @"\b(N-\d+)\b";
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                return match.Value;
            }

            return null;
        }

        private static readonly string[] NigerianPhonePrefixes = {
            "080", "081", "070", "090", "091", "071", "0802", "0803", "0804",
            "0805", "0806", "0807", "0808", "0809", "0810", "0811", "0812",
            "0813", "0814", "0815", "0816", "0817", "0818", "0819", "0902",
            "0903", "0904", "0905", "0906", "0907", "0908", "0909", "0910",
            "0911", "0912", "0913", "0914", "0915", "0916", "0917", "0918",
            "0919", "0701", "0702", "0703", "0704", "0705", "0706", "0707",
            "0708", "0709"
        };
    }
}