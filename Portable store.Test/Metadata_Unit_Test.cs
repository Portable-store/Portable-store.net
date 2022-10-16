using Microsoft.VisualStudio.TestPlatform.Utilities;
using Portable_store.Enums;
using Portable_store.Models;
using Xunit;
using Xunit.Abstractions;

namespace Portable_store.Test
{
    public class Metadata_Unit_Test
    {
        #region Constructors
        public Metadata_Unit_Test(ITestOutputHelper output)
        {
            this.output = output;
        }
        #endregion

        #region Variables
        private readonly ITestOutputHelper output;
        #endregion

        #region Methods
        [Fact]
        public async void Create()
        {
            var progress = new Progress<Progress_info_Model>(p =>
            {
                output.WriteLine(p.Details);
            });

            var metadata = new Application_metadata_Model(
                "Test",
                "This is a test metadata!",
                Source_type_Enum.DirectLink,
                new Application_version_Model[]
                {
                    new Application_version_Model(
                        null,
                        "Test.exe",
                        null),
                    new Application_version_Model(
                        "Alternative test",
                        null,
                        "AltTest.exe",
                        "Alternative/AltTest.exe",
                        null),
                });

            var path = await Metadata.Create_Async(metadata, progress);

            output.WriteLine(path);

            Assert.NotNull(path);
        }


        [Theory]
        [InlineData(".\\AppData\\Caches\\Metadatas\\Rufus.json")]
        public async void Read(string metadata_path)
        {
            var progress = new Progress<Progress_info_Model>(p =>
            {
                output.WriteLine(p.Details);
            });

            var metadata = await Metadata.Read_Async(metadata_path, progress);

            if (metadata != null)
                output.WriteLine(metadata.To_long_string());

            Assert.NotNull(metadata);
        }
        #endregion
    }
}
