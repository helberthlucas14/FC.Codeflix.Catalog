namespace FC.Codeflix.Catalog.EndToEndTests.Api.Category.CreateCategory
{
    public class CreateCategoryApiTestDataGenerator
    {
        public static IEnumerable<object[]> GetInvalidInputs()
        {
            var fixture = new CreateCategoryApiTestFixture();
            var invalidInputsList = new List<object[]>();
            var totalInvalidCases = 3;

            for (int i = 0; i < totalInvalidCases; i++)
            {
                switch (i % totalInvalidCases)
                {
                    case 0:
                        var input = fixture.GetExampleInput();
                        input.Name = fixture.GetInvalidINameTooShortName();
                        invalidInputsList.Add(new object[] {
                        input,
                        "Name should be less than 3 characters long"
                        });
                        break;

                    case 1:
                        var input2 = fixture.GetExampleInput();
                        input2.Name = fixture.GetInvalidITooLongName();
                        invalidInputsList.Add(new object[] {
                         input2,
                        "Name should be greater than 255 characters long"
                        });
                        break;

                    case 2:
                        var input3 = fixture.GetExampleInput();
                        input3.Description = fixture.GetInvalidDescriptionTooLong();
                        invalidInputsList.Add(new object[] {
                        input3,
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
