﻿using FC.Codeflix.Catalog.Api.ApiModels.Category;
using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.Application.UseCases.Category.UpdateCategory;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.UpdateCategory
{
    [Collection(nameof(UpdateCategoryApiTestFixture))]
    public class UpdateCategoryApiTest : IDisposable
    {
        private readonly UpdateCategoryApiTestFixture _fixture;

        public UpdateCategoryApiTest(UpdateCategoryApiTestFixture fixture)
              => _fixture = fixture;

        [Fact(DisplayName = nameof(UpdateCategory))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        public async void UpdateCategory()
        {
            var exampleCategoryList = _fixture.GetExampleCategoriesList();
            await _fixture.Persistence.InsertList(exampleCategoryList);
            var exampleCategory = exampleCategoryList[10];
            var input = _fixture.GetExampleInput();

            var (response, output) = await _fixture.ApiClient
                .Put<ApiResponse<CategoryModelOutput>>
                (
                    $"/categories/{exampleCategory.Id}",
                    input
                );

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Data.Id.Should().Be(exampleCategory.Id);
            output.Data.Name.Should().Be(input.Name);
            output.Data.Description.Should().Be(input.Description);
            output.Data.IsActive.Should().Be((bool)input.IsActive);
            var dbCategory = await _fixture.Persistence
                    .GetById(exampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Id.Should().Be(exampleCategory.Id);
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((bool)input.IsActive);
        }


        [Fact(DisplayName = nameof(UpdateCategoryOnlyName))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        public async void UpdateCategoryOnlyName()
        {
            var exampleCategoryList = _fixture.GetExampleCategoriesList();
            await _fixture.Persistence.InsertList(exampleCategoryList);
            var exampleCategory = exampleCategoryList[10];
            var input = new UpdateCategoryApiInput(
                _fixture.GetValidCategoryName()
                );

            var (response, output) = await _fixture.ApiClient
                .Put<ApiResponse<CategoryModelOutput>>
                (
                    $"/categories/{exampleCategory.Id}",
                    input
                );

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Data.Id.Should().Be(exampleCategory.Id);
            output.Data.Name.Should().Be(input.Name);
            output.Data.Description.Should().Be(exampleCategory.Description);
            output.Data.IsActive.Should().Be((bool)exampleCategory.IsActive);
            var dbCategory = await _fixture.Persistence
                    .GetById(exampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Id.Should().Be(exampleCategory.Id);
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(exampleCategory.Description);
            dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive);
        }

        [Fact(DisplayName = nameof(UpdateCategoryNameAndDescription))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        public async void UpdateCategoryNameAndDescription()
        {
            var exampleCategoryList = _fixture.GetExampleCategoriesList();
            await _fixture.Persistence.InsertList(exampleCategoryList);
            var exampleCategory = exampleCategoryList[10];
            var input = new UpdateCategoryApiInput(
                _fixture.GetValidCategoryName(),
                _fixture.GetValidCategoryDescription()
                );

            var (response, output) = await _fixture.ApiClient
                .Put<ApiResponse<CategoryModelOutput>>
                (
                    $"/categories/{exampleCategory.Id}",
                    input
                );

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Data.Id.Should().Be(exampleCategory.Id);
            output.Data.Name.Should().Be(input.Name);
            output.Data.Description.Should().Be(input.Description);
            output.Data.IsActive.Should().Be((bool)exampleCategory.IsActive);
            var dbCategory = await _fixture.Persistence
                    .GetById(exampleCategory.Id);

            dbCategory.Should().NotBeNull();
            dbCategory.Id.Should().Be(exampleCategory.Id);
            dbCategory.Name.Should().Be(input.Name);
            dbCategory.Description.Should().Be(input.Description);
            dbCategory.IsActive.Should().Be((bool)exampleCategory.IsActive);
        }

        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        public async void ErrorWhenNotFound()
        {
            var exampleCategoryList = _fixture.GetExampleCategoriesList();
            await _fixture.Persistence.InsertList(exampleCategoryList);
            var randomGuid = Guid.NewGuid();
            var input = _fixture.GetExampleInput();

            var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>
                (
                    $"/categories/{randomGuid}",
                    input
                );

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output.Title.Should().Be("Not Found");
            output.Type.Should().Be("NotFound");
            output.Status.Should().Be((int)StatusCodes.Status404NotFound);
            output.Detail.Should().Be($"Category '{randomGuid}' not found.");
        }

        [Theory(DisplayName = nameof(ErrorWhenCantInstantiateAggregate))]
        [MemberData(nameof(UpdateCategoryApiTestDataGenerator.GetInvalidInputs),
            MemberType = typeof(UpdateCategoryApiTestDataGenerator))]
        [Trait("EndToEnd/API", "Category/Update - Endpoints")]
        public async void ErrorWhenCantInstantiateAggregate(
            UpdateCategoryApiInput input,
            string expectedDetails
            )
        {
            var exampleCategoryList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoryList);
            var exampleCategory = exampleCategoryList[10];

            var (response, output) = await _fixture.ApiClient.Put<ProblemDetails>
                (
                    $"/categories/{exampleCategory.Id}",
                    input
                );

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.UnprocessableEntity);
            output.Should().NotBeNull();
            output.Title.Should().Be("One or more validation erros ocurred");
            output.Type.Should().Be("UnprocessableEntity");
            output.Status.Should().Be((int)HttpStatusCode.UnprocessableEntity);
            output.Detail.Should().Be(expectedDetails);
        }

        public void Dispose() => _fixture.CleanPersistence();
    }
}
