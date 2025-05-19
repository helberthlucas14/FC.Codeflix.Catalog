using Bogus;
using FC.Codeflix.Catalog.Domain.Exceptions;
using FC.Codeflix.Catalog.Domain.Validation;
using FluentAssertions;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Validation
{
    public class DomainValidationTest
    {
        private Faker Faker { get; set; } = new Faker();

        [Fact(DisplayName = nameof(NotNullOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOk()
        {
            var value = Faker.Commerce.ProductName();
            Action action = () =>
            {
                DomainValidation.NotNull(value, "value");
            };

            action.Should().NotThrow();
        }

        [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullThrowWhenNull()
        {
            string? value = null;
            Action action = () =>
            {
                DomainValidation.NotNull(value, "FieldName");
            };

            action.Should().Throw<EntityValidationException>()
                           .WithMessage("FieldName should not be null");
        }

        [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        [Trait("Domain", "DomainValidatio - Validation")]
        public void NotNullOrEmptyThrowWhenEmpty(string? target)
        {
            string fieldName = Faker.Commerce.ProductName().Trim();
            Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

            action.Should().Throw<EntityValidationException>()
                          .WithMessage($"{fieldName} should not be empty or null");
        }

        [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        public void NotNullOrEmptyOk()
        {
            string fieldName = Faker.Commerce.ProductName().Trim();
            var target = Faker.Commerce.ProductName();

            Action action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(MinLengthOk))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanMin), parameters: 10)]
        public void MinLengthOk(string target, int minLength)
        {
            string fieldName = Faker.Commerce.ProductName().Trim();
            Action action = () => DomainValidation.MinLength(target, minLength, fieldName);

            action.Should().NotThrow();
        }

        [Theory(DisplayName = nameof(MinLengthThrownWhenLess))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesSmallerThanMin), parameters: 10)]
        public void MinLengthThrownWhenLess(string target, int minLength)
        {
            string fieldName = Faker.Commerce.ProductName().Trim();
            Action action = () => DomainValidation.MinLength(target, minLength, fieldName);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be less than {minLength} characters long");
        }

        [Theory(DisplayName = nameof(MaxLengthThrownWhenGreater))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesGreaterThanMax), parameters: 10)]
        public void MaxLengthThrownWhenGreater(string target, int maxLength)
        {
            string fieldName = Faker.Commerce.ProductName().Trim();
            Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);

            action.Should()
                .Throw<EntityValidationException>()
                .WithMessage($"{fieldName} should be greater than {maxLength} characters long");
        }

        [Theory(DisplayName = nameof(MaxLengthOK))]
        [Trait("Domain", "DomainValidation - Validation")]
        [MemberData(nameof(GetValuesLessThanMax), parameters: 10)]
        public void MaxLengthOK(string target, int maxLength)
        {
            string fieldName = Faker.Commerce.ProductName().Trim();
            Action action = () => DomainValidation.MaxLength(target, maxLength, fieldName);

            action.Should().NotThrow();
        }

        public static IEnumerable<object[]> GetValuesSmallerThanMin(int numberOfTests)
        {
            yield return new object[] { "123456", 10 };
            var faker = new Faker();
            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length + (new Random().Next(1, 20));
                yield return new object[] { example, minLength };
            }
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMin(int numberOfTests)
        {
            yield return new object[] { "123456", 6 };
            var faker = new Faker();
            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var example = faker.Commerce.ProductName();
                var minLength = example.Length - (new Random().Next(1, 5));
                yield return new object[] { example, minLength };
            }
        }

        public static IEnumerable<object[]> GetValuesGreaterThanMax(int numberOfTests)
        {
            yield return new object[] { "123456", 5 };
            var faker = new Faker();
            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length - (new Random().Next(1, 5));
                yield return new object[] { example, maxLength };
            }
        }

        public static IEnumerable<object[]> GetValuesLessThanMax(int numberOfTests)
        {
            yield return new object[] { "123456", 6 };
            var faker = new Faker();
            for (int i = 0; i < (numberOfTests - 1); i++)
            {
                var example = faker.Commerce.ProductName();
                var maxLength = example.Length + (new Random().Next(1, 5));
                yield return new object[] { example, maxLength };
            }
        }
    }
}
