using System;
using System.Collections.Generic;
using System.Linq;
using InitialKata.App.Exceptions;

namespace InitialKata.App.Business
{
    public class StringCalculator
    {
        private const string DefaultDelimiter = ",";
        private const string DelimitersPrefix = "//";
        private const string DelimitersSuffix = @"\n";

        public int Add(string numbers)
        {
            if (numbers == null) throw new ArgumentNullException(nameof(numbers));
            if (string.IsNullOrWhiteSpace(numbers)) return 0;

            var delimiters = GetDelimiters(numbers);
            var body = GetBody(delimiters, numbers);

            return body.Sum();
        }
        private static ICollection<string> GetDelimiters(string numbers) => HasDelimiterHeader(numbers) ? GetDelimiterFromHeader(numbers) : new List<string>() { DefaultDelimiter };

        private static bool HasDelimiterHeader(string numbers) => numbers.StartsWith(DelimitersPrefix);

        private static ICollection<string> GetDelimiterFromHeader(string numbers)
        {
            var index = numbers.IndexOf(DelimitersSuffix, StringComparison.Ordinal);
            var header = numbers.Substring(0, index).Replace(DelimitersPrefix, string.Empty);

            var hasDelimiterFormat = header.StartsWith("[") && header.EndsWith("]");
            if (!hasDelimiterFormat) return new List<string>() { header };

            var delimiters = header
                .Split("]")
                .Where(o => !string.IsNullOrWhiteSpace(o))
                .Select(o => o.Replace("[", string.Empty))
                .ToList();

            return delimiters;
        }

        private static IEnumerable<int> GetBody(ICollection<string> delimiters, string numbers)
        {
            var payload = GetRawBody(delimiters, numbers);

            var body = payload.Select(GetNumberFromString).Where(o => o <= 1000).ToList();
            AssertNoNegatives(body);
            return body;
        }

        private static void AssertNoNegatives(IEnumerable<int> body)
        {
            var negatives = body.Where(o => o < 0).ToList();
            if (negatives.Any()) throw new NegativeException(negatives.Select(o => $"{o}").Aggregate((s1, s2) => $"{s1},{s2}"));
        }

        private static IEnumerable<string> GetRawBody(ICollection<string> delimiters, string numbers)
        {
            var payload = RemoveHeader(numbers);
            var body = SplitBody(payload, delimiters);

            return body;
        }

        private static IEnumerable<string> SplitBody(string payload, ICollection<string> delimiters)
        {
            var rootDelimiter = delimiters.First();
            delimiters.Add(@"\n");

            var body = delimiters.Aggregate(payload, (current, delimiter) => current.Replace(delimiter, rootDelimiter));
            return body.Split(rootDelimiter);
        }

        private static string RemoveHeader(string numbers)
        {
            if (!HasDelimiterHeader(numbers)) return numbers;

            var index = numbers.IndexOf(DelimitersSuffix, StringComparison.Ordinal);
            return numbers.Substring(index + 2);
        }

        private static int GetNumberFromString(string number)
        {
            try
            {
                return int.Parse(number);
            }
            catch
            {
                throw new ArgumentException("Some number in numbers are not numeric", nameof(number));
            }
        }
    }
}
