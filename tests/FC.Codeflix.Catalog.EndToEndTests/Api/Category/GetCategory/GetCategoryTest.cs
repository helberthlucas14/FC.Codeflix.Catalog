using FC.Codeflix.Catalog.Api.ApiModels.Response;
using FC.Codeflix.Catalog.Application.UseCases.Category.Common;
using FC.Codeflix.Catalog.EndToEndTests.Extensions.DateTime;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.GetCategory
{
    [Collection(nameof(GetCategoryTestFixture))]
    public class GetCategoryTest : IDisposable
    {
        private readonly GetCategoryTestFixture _fixture;

        public GetCategoryTest(GetCategoryTestFixture fixture)
            => _fixture = fixture;


        [Fact(DisplayName = nameof(GetCategory))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task GetCategory()
        {
            var exampleCategoryList = _fixture.GetExampleCategoriesList();
            await _fixture.Persistence.InsertList(exampleCategoryList);
            var exampleCategory = exampleCategoryList[10];

            var (response, output) = await _fixture.ApiClient
               .Get<ApiResponse<CategoryModelOutput>>
                (
                    $"/categories/{exampleCategory.Id}"
                );

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status200OK);
            output.Should().NotBeNull();
            output.Data.Should().NotBeNull();
            output.Data.Id.Should().Be(exampleCategory.Id);
            output.Data.Name.Should().Be(exampleCategory.Name);
            output.Data.Description.Should().Be(exampleCategory.Description);
            output.Data.IsActive.Should().Be(exampleCategory.IsActive);
            output.Data.CreatedAt.TrimMillisseconds().Should().Be(exampleCategory.CreatedAt.TrimMillisseconds());
        }


        [Fact(DisplayName = nameof(ThrowWhenNotFound))]
        [Trait("EndToEnd/API", "Category/Get - Endpoints")]
        public async Task ThrowWhenNotFound()
        {
            var exampleCategoryList = _fixture.GetExampleCategoriesList();
            await _fixture.Persistence.InsertList(exampleCategoryList);
            var radomGuid = Guid.NewGuid();

            var (response, output) = await _fixture.ApiClient
               .Get<ProblemDetails>
                (
                    $"/categories/{radomGuid}"
                );

            response.Should().NotBeNull();
            response.StatusCode.Should().Be((HttpStatusCode)StatusCodes.Status404NotFound);
            output.Should().NotBeNull();
            output.Status.Should().Be((int)StatusCodes.Status404NotFound);
            output.Title.Should().Be("Not Found");
            output.Detail.Should().Be($"Category '{radomGuid}' not found.");
            output.Type.Should().Be($"NotFound");
        }

        public void Dispose() => _fixture.CleanPersistence();

    }
}
