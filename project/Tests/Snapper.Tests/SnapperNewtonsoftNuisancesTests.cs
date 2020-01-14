using System;
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
        public void DateTimeIsParsedAsStringBySnapper_UsingStringSnapshot_FileSnapshot()
        {
            const string snapshot = "{" +
                                        "\"Key\" : \"2010-12-31T00:00:00+00:00\"" +
                                    "}";

            snapshot.ShouldMatchSnapshot();
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        // Related issue https://github.com/JamesNK/Newtonsoft.Json/issues/862
        public void DateTimeIsParsedAsStringBySnapper_UsingObjectSnapshot_FileSnapshot()
        {
            var snapshot = new
            {
                Key = new DateTime(2010, 12, 31, 0, 0, 0, DateTimeKind.Utc)
            };

            snapshot.ShouldMatchSnapshot();
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        // Related issue https://github.com/JamesNK/Newtonsoft.Json/issues/862
        public void DateTimeIsParsedAsStringBySnapper_UsingStringSnapshot_InlineSnapshot()
        {
            const string snapshot = "{" +
                                    "\"Key\" : \"2010-12-31T00:00:00+00:00\"" +
                                    "}";

            snapshot.ShouldMatchInlineSnapshot("{" +
                                               "\"Key\" : \"2010-12-31T00:00:00+00:00\"" +
                                               "}");
        }

        [Fact]
        [MethodImpl(MethodImplOptions.NoInlining)]
        // Related issue https://github.com/JamesNK/Newtonsoft.Json/issues/862
        public void DateTimeIsParsedAsStringBySnapper_UsingObjectSnapshot_InlineSnapshot()
        {
            var snapshot = new
            {
                Key = new DateTime(2010, 12, 31, 0, 0, 0, DateTimeKind.Utc)
            };

            snapshot.ShouldMatchInlineSnapshot("{" +
                                               "\"Key\" : \"2010-12-31T00:00:00Z\"" +
                                               "}");
        }
    }
}
