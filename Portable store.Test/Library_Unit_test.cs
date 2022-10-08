using Portable_store.Models;
using Xunit;
using Xunit.Abstractions;


namespace Portable_store.Test
{
    public class Library_Unit_test
    {
        #region Constructors
        public Library_Unit_test(ITestOutputHelper output)
        {
            this.output = output;
        }
        #endregion

        #region Variables
        private readonly ITestOutputHelper output;
        #endregion

        #region Methods
        [Theory]
        [InlineData("*")]
        [InlineData("Rufus")]
        public async void Delete(string app_name)
        {
            var progress = new Progress<Progress_info_Model>(p =>
            {
                output.WriteLine(p.Details);
            });
            int success = 0;

            var applications_info = await Library.List_Async(app_name, progress);

            if (applications_info.Count == 0)
                throw new Exception("Not application was found!");

            foreach (var application_info in applications_info)
                success += await Library.Delete_Async(application_info, progress) ? 1 : 0;

            Assert.True(success == applications_info.Count);
        }

        [Theory]
        [InlineData("*")]
        [InlineData("Rufus")]
        public async void List(string app_name)
        {
            var progress = new Progress<Progress_info_Model>(p =>
            {
                output.WriteLine(p.Details);
            });

            var applications_info = await Library.List_Async(app_name, progress);

            output.WriteLine(string.Join(", ", applications_info));

            Assert.True(applications_info.Count > 0);
        }

        [Theory]
        [InlineData("*")]
        [InlineData("Rufus")]
        public async void Run(string app_name)
        {
            var progress = new Progress<Progress_info_Model>(p =>
            {
                output.WriteLine(p.Details);
            });
            int success = 0;

            var applications_info = await Library.List_Async(app_name, progress);

            if (applications_info.Count == 0)
                throw new Exception("Not application was found!");

            foreach (var application_info in applications_info)
                success += Library.Run_Async(application_info, progress) ? 1 : 0;

            Assert.True(success == applications_info.Count);
        }
        #endregion
    }
}
