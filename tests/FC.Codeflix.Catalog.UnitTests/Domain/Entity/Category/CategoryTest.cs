﻿using FC.Codeflix.Catalog.Domain.Exceptions;
using FluentAssertions;
using DomainEntity = FC.Codeflix.Catalog.Domain.Entity;

namespace FC.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest
{
    private readonly CategoryTestFixture _categoryTestFixture;
    public CategoryTest(CategoryTestFixture categoryTestFixture)
    {
        _categoryTestFixture = categoryTestFixture;
    }

    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();
        var dateTimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);

        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBe(default(DateTime));
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (category.IsActive).Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithActive(bool isActive)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var dateTimeBefore = DateTime.Now;

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);

        var dateTimeAfter = DateTime.Now.AddSeconds(1);

        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBe(default(Guid));
        category.CreatedAt.Should().NotBe(default(DateTime));
        (category.CreatedAt >= dateTimeBefore).Should().BeTrue();
        (category.CreatedAt <= dateTimeAfter).Should().BeTrue();
        (category.IsActive).Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = ()
            => new DomainEntity.Category(name!, validCategory.Description);

        action.Should()
            .Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = ()
            => new DomainEntity.Category(validCategory.Name, null!);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Description should not be null");
    }

    public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfTests = 6)
    {
        var fixture = new CategoryTestFixture();
        for (int i = 0; i < numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;
            yield return new object[] {
                fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)]
            };
        }
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    [Trait("Domain", "Category - Aggregates")]

    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = ()
             => new DomainEntity.Category(invalidName, validCategory.Description);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be less than 3 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        var invalidName = String.Join(null, Enumerable.Range(1, 256).Select(_ => "a").ToArray());

        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = ()
             => new DomainEntity.Category(invalidName, validCategory.Description);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be greater than 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var invalidDescription = String.Join(null, Enumerable.Range(1, 10_001).Select(_ => "a").ToArray());

        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = ()
             => new DomainEntity.Category(validCategory.Name, invalidDescription);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Description should be greater than 10000 characters long");
    }

    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();

        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Desactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Desactivate()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);
        category.Desactivate();

        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var categoryWithNewValues = _categoryTestFixture.GetValidCategory();

        validCategory.Update(categoryWithNewValues.Name, categoryWithNewValues.Description);

        validCategory.Name.Should().Be(categoryWithNewValues.Name);
        validCategory.Description.Should().Be(categoryWithNewValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var newName = _categoryTestFixture.GetValidCategoryName();
        var currentDescription = validCategory.Description;

        validCategory.Update(newName);

        validCategory.Name.Should().Be(newName);
        validCategory.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsEmpty))]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsEmpty(string? name)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = ()
            => validCategory.Update(name!);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should not be empty or null");
    }


    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters: 10)]
    [Trait("Domain", "Category - Aggregates")]

    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        Action action = ()
             => validCategory.Update(invalidName);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be less than 3 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var invalidName = _categoryTestFixture.Faker.Lorem.Letter(256);

        Action action = () => validCategory.Update(invalidName, "Category Description");

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Name should be greater than 255 characters long");
    }


    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]

    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        var validCategory = _categoryTestFixture.GetValidCategory();

        var invalidDescription = _categoryTestFixture.Faker.Commerce.ProductDescription();
        while (invalidDescription.Length <= 10_000)
        {
            invalidDescription = $"{invalidDescription} {_categoryTestFixture.Faker.Commerce.ProductDescription()}";
        }


        Action action = ()
             => validCategory.Update("Category Name", invalidDescription);

        action.Should()
              .Throw<EntityValidationException>()
              .WithMessage("Description should be greater than 10000 characters long");
    }
}
