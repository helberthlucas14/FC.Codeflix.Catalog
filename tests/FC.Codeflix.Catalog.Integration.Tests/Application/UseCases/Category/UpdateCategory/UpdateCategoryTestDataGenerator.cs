﻿namespace FC.Codeflix.Catalog.Integration.Tests.Application.UseCases.Category.UpdateCategory
{
    public class UpdateCategoryTestDataGenerator
    {
        public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
        {
            var fixture = new UpdateCategoryTestFixture();
            for (int i = 0; i < times; i++)
            {
                var exampleCategory = fixture.GetExampleCategory();
                var exampleInput = fixture.GetValidInput(exampleCategory.Id);

                yield return new object[]{
                    exampleCategory,exampleInput
                };
            }
        }

        public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
        {
            var fixture = new UpdateCategoryTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 3;

            for (int i = 0; i < times; i++)
            {
                switch (i % totalInvalidCases)
                {
                    case 0:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputShortName(),
                        "Name should be less than 3 characters long"
                        });
                        break;

                    case 1:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputTooLongName(),
                        "Name should be greater than 255 characters long"
                        });
                        break;

                    case 2:
                        invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputTooLongDescription(),
                        "Description should be greater than 10000 characters long"
                        });
                        break;

                    default:
                        break;
                }
            }
            return invalidInputsList;
        }
    }

}
