﻿using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.DeleteCategory
{
    [Collection(nameof(DeleteCategoryApiTestFixture))]
    public class DeleteCategoryApiTest : IDisposable
    {
        private readonly DeleteCategoryApiTestFixture _fixture;

        public DeleteCategoryApiTest(DeleteCategoryApiTestFixture fixture)
                => _fixture = fixture;

        [Fact(DisplayName = nameof(DeleteCategory))]
        [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
        public async void DeleteCategory()
        {
            var exampleCategoryList = _fixture.GetExampleCategoriesList();
            await _fixture.Persistence.InsertList(exampleCategoryList);
            var exampleCategory = exampleCategoryList[10];

            var (response, output) = await _fixture.ApiClient.Delete<object>
                (
                    $"/categories/{exampleCategory.Id}"
                );

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status204NoContent);
            output.Should().BeNull();
            var persistenceCategory = await _fixture.Persistence
                .GetById(exampleCategory.Id);
            persistenceCategory.Should().BeNull();
        }


        [Fact(DisplayName = nameof(ErrorWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Delete - Endpoints")]
        public async void ErrorWhenNotFound()
        {
            var exampleCategoryList = _fixture.GetExampleCategoriesList(20);
            await _fixture.Persistence.InsertList(exampleCategoryList);
            var radomGuid = Guid.NewGuid();

            var (response, output) = await _fixture.ApiClient.Delete<ProblemDetails>
                (
                    $"/categories/{radomGuid}"
                );

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output.Title.Should().Be("Not Found");
            output.Type.Should().Be("NotFound");
            output.Status.Should().Be((int)StatusCodes.Status404NotFound);
            output.Detail.Should().Be($"Category '{radomGuid}' not found.");

        }
        public void Dispose() => _fixture.CleanPersistence();
    }
}
