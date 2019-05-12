using InitialKata.App.Business;
using InitialKata.App.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace InitialKata.Test.Business
{
    public class StringCalculatorTest
    {
        private readonly StringCalculator _sc;

        public StringCalculatorTest() => _sc = new StringCalculator();

        [Fact]
        public void Add_WhenNull_ThenThrowException() => Assert.Throws<ArgumentNullException>(() => _sc.Add(null));

        [Fact]
        public void Add_WhenEmptyString_ThenReturnZero()
        {
            var result = _sc.Add(string.Empty);

            Assert.Equal(0, result);
        }

        [Fact]
        public void Add_WhenNotNumber_ThenThrowException() => Assert.Throws<ArgumentException>(() => _sc.Add("a"));

        [Fact]
        public void Add_WhenOneNumber_ThenThatNumber()
        {
            const int number = 5;

            var result = _sc.Add($"{number}");

            Assert.Equal(number, result);
        }

        [Fact]
        public void Add_WhenTwoNumbers_ThenResultItsSum()
        {
            var numbers = new List<int> { 5, 4 };

            var result = _sc.Add(numbers.Select(o => $"{o}").Aggregate((s1, s2) => $"{s1},{s2}"));
            
            Assert.Equal(numbers.Sum(), result);
        }

        [Fact]
        public void Add_WhenSomeNumbers_ThenResultItsSum()
        {
            var numbers = new List<int> { 5, 4, 32, 23, 6, 32 };

            var result = _sc.Add(numbers.Select(o => $"{o}").Aggregate((s1, s2) => $"{s1},{s2}"));

            Assert.Equal(numbers.Sum(), result);
        }

        [Fact]
        public void Add_WhenInputHasNewLines_ThenResultItsSum()
        {
            const string numbers = @"1\n2,3";

            var result = _sc.Add(numbers);

            Assert.Equal(6, result);
        }

        [Fact]
        public void Add_WhenInputHasNewLines_ThenThrowException()
        {
            const string numbers = @"1,\n";

            Assert.Throws<ArgumentException>(() => _sc.Add(numbers));
        }
        
        [Fact]
        public void Add_WhenInputHasDelimiter_ThenParseItAndSum()
        {
            const string delimiter = ";";

            var sumList = new List<int> { 5, 4, 32, 23, 6, 32 };
            var aggregate = sumList.Select(o => $"{o}").Aggregate((s1, s2) => $"{s1}{delimiter}{s2}");
            var numbers = $@"//{delimiter}\n{aggregate}";

            var result = _sc.Add(numbers);
            Assert.Equal(sumList.Sum(), result);
        }
        
        [Fact]
        public void Add_WhenInputHasDelimiterWithDelimitation_ThenParseItAndSum()
        {
            const string delimiter = ";";

            var sumList = new List<int> { 5, 4, 32, 23, 6, 32 };
            var aggregate = sumList.Select(o => $"{o}").Aggregate((s1, s2) => $"{s1}{delimiter}{s2}");
            var numbers = $@"//[{delimiter}]\n{aggregate}";

            var result = _sc.Add(numbers);
            Assert.Equal(sumList.Sum(), result);
        }
        
        [Fact]
        public void Add_WhenInputHasMultipleDelimiters_ThenParseItAndSum()
        {
            var numbers = $@"//[*][%]\n1*2%3";

            var result = _sc.Add(numbers);
            Assert.Equal(6, result);
        }
        
        [Fact]
        public void Add_WhenDelimiterWithAnyLength_ThenParseItAndSum()
        {
            const string delimiter = "***";

            var sumList = new List<int> { 5, 4, 32, 23, 6, 32 };
            var aggregate = sumList.Select(o => $"{o}").Aggregate((s1, s2) => $"{s1}{delimiter}{s2}");
            var numbers = $@"//{delimiter}\n{aggregate}";

            var result = _sc.Add(numbers);
            Assert.Equal(sumList.Sum(), result);
        }
        
        [Fact]
        public void Add_WhenNegativeInput_ThenThrowException()
        {
            var numbers = new List<int> { -5 };
            var aggregate = numbers.Select(o => $"{o}").Aggregate((s1, s2) => $"{s1},{s2}");
            var ex = Assert.Throws<NegativeException>(() => _sc.Add(aggregate));
            Assert.Equal("negatives not allowed - [-5]", ex.Message);
        }
        
        [Fact]
        public void Add_WhenNegativeInputs_ThenThrowExceptionWithMessage()
        {
            var numbers = new List<int> { -5, 5, -10 };
            var aggregate = numbers.Select(o => $"{o}").Aggregate((s1, s2) => $"{s1},{s2}");
            var ex = Assert.Throws<NegativeException>(() => _sc.Add(aggregate));
            Assert.Equal("negatives not allowed - [-5,-10]", ex.Message);
        }
        
        [Fact]
        public void Add_WhenGreaterThan1000_ThenIgnoreIt()
        {
            var numbers = new List<int> { 5, 1000, 1001 };
            var aggregate = numbers.Select(o => $"{o}").Aggregate((s1, s2) => $"{s1},{s2}");
            var result =_sc.Add(aggregate);
            Assert.Equal(1005, result);
        }
    }
}
