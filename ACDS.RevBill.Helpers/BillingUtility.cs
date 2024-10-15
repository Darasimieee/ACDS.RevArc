using System;

namespace ACDS.RevBill.Helpers
{
    public class BillingUtility
	{
        private static readonly Random random = new Random();
        public static string GenerateBillReference(string value, int id)
		{
            //Random random = new Random();

            // Generate a random number with 7 digits, excluding zero
            int part1 = random.Next(1, 9) * 1000000 + random.Next(1, 9) * 100000 + random.Next(1, 9) * 10000 +
                        random.Next(1, 9) * 1000 + random.Next(1, 9) * 100 + random.Next(1, 9) * 10 +
                        random.Next(1, 9);

            int currentYear = DateTime.Now.Year;
            int lastTwoDigits = currentYear % 100;

            // Combine the parts with the user-supplied value to form the reference string
            //string billReference = $"{value}{part1}{0}{id}";
            string billReference = $"{value}{part1}{lastTwoDigits}{0}{id}";

            return billReference;
        }

        public static string GenerateHarmonizedBillReference(string value, int id)
        {
           // Random random = new Random();

            // Generate a random number with 6 digits, excluding zero
            int part1 = random.Next(1, 9) * 1000000 +
                        random.Next(1, 9) * 100000 +
                        random.Next(1, 9) * 10000 +
                        random.Next(1, 9) * 1000 +
                        random.Next(1, 9) * 100 +
                        random.Next(1, 9);

            int currentYear = DateTime.Now.Year;
            int lastTwoDigits = currentYear % 100;

            // Combine the parts with the user-supplied value to form the reference string
            string billReference = $"{value}{part1}{lastTwoDigits}{0}{id}";

            return billReference;
        }
    

    public static string GenerateHarmonizedBillReferenceForAutoGeneration(string payerid, string value)
        {
            // Find the index of the payerid in the value string
            int payerIdIndex = value.IndexOf(payerid);

            // Find the index of the first occurrence of zero after the payerid
            int zeroIndex = value.IndexOf('0', payerIdIndex);

            // Extract the values between payerid and zero
            string extractedValues = value.Substring(payerIdIndex + payerid.Length, zeroIndex - payerIdIndex - payerid.Length);

            // Generate random digits to replace the extracted values
            //Random random = new Random();
            string randomDigits = new string(Enumerable.Repeat("123456789", extractedValues.Length)
                                                       .Select(s => s[random.Next(s.Length)]).ToArray());

            // Replace the extracted values with the random digits in the value string
            string billReference = value.Remove(payerIdIndex + payerid.Length, extractedValues.Length)
                                        .Insert(payerIdIndex + payerid.Length, randomDigits);

            return billReference;
        }
    }
}