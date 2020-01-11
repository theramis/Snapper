using System.Runtime.CompilerServices;
using Snapper.Attributes;
using Xunit;

namespace Snapper.Tests
{
    /// <summary>
    ///     This class contains tests which test that the nuisances of newtonsoft are avoided in Snapper
    /// </summary>
    [StoreSnapshotsPerClass]
    public class SnapperNewtonsoftNuisancesTests
    {
        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        // Related issue https://github.com/JamesNK/Newtonsoft.Json/issues/862
        public void DateTimeIsParsedAsStringBySnapper_FileSnapshot()
        {
            const string snapshot = "{" +
                                        "\"Key\" : \"2010-12-31T00:00:00+00:00\"" +
                                    "}";

            snapshot.ShouldMatchSnapshot();
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        // Related issue https://github.com/JamesNK/Newtonsoft.Json/issues/862
        public void DateTimeIsParsedAsStringBySnapper_InlineSnapshot()
        {
            const string snapshot = "{" +
                                    "\"Key\" : \"2010-12-31T00:00:00+00:00\"" +
                                    "}";

            snapshot.ShouldMatchInlineSnapshot("{" +
                                               "\"Key\" : \"2010-12-31T00:00:00+00:00\"" +
                                               "}");
        }
    }
}
