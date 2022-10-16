using Portable_store.Models;
using Xunit;
using Xunit.Abstractions;

namespace Portable_store.Test
{
    public class Store_Unit_test
    {
        #region Constructors
        public Store_Unit_test(ITestOutputHelper output)
        {
            this.output = output;
        }
        #endregion

        #region Variables
        private readonly ITestOutputHelper output;
        #endregion

        #region Methods
        [Theory]
        [InlineData("*", true)]
        [InlineData("Rufus", false)]
        public async void Download(string app_name, bool should_fail)
        {
            var progress = new Progress<Progress_info_Model>(p =>
            {
                output.WriteLine(p.Details);
            });

            var metadatas = await Store.Search_Async(app_name, progress);
            int success = 0;

            foreach (var metadata in metadatas)
            {
                var version = Metadata.Get_compatible_versions(metadata).First();
                success += await Store.Download_Async(metadata, version, progress) ? 1 : 0;
            }

            if (should_fail)
                Assert.True(success < metadatas.Count);
            else
                Assert.True(success == metadatas.Count);
        }

        [Theory]
        [InlineData("*", true)]
        [InlineData("Rufus", false)]
        public async void Search(string app_name, bool should_fail)
        {
            var progress = new Progress<Progress_info_Model>(p =>
            {
                output.WriteLine(p.Details);
            });

            var metadatas = await Store.Search_Async(app_name, progress);

            //Output
            output.WriteLine(string.Join(", ", metadatas));


            if (should_fail)
                Assert.True(metadatas.Count == 0);
            else
                Assert.True(metadatas.Count > 0);
        }

        [Theory]
        [InlineData("*", true)]
        [InlineData("Rufus", false)]
        public async void Update(string app_name, bool should_fail)
        {
            /*var progress = new Progress<Progress_info_Model>(p =>
            {
                output.WriteLine(p.Details);
            });

            var metadatas = await Store.Search_Async(app_name, progress);
            int success = 0;

            foreach (var metadata in metadatas)
            {
                var version = Metadata.Get_compatible_version(metadata).First();
                success += await Store.Update_Async(metadata, progress) ? 1 : 0;
            }


            //Output
            //output.WriteLine(string.Join(", ", metadatas));

            Assert.True(metadatas != should_fail);*/
        }


        [Fact]
        public async void Refresh()
        {
            Assert.True( await Store.Refresh_Async() );
        }
        #endregion
    }
}