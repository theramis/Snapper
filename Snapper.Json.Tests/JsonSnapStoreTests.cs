using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Newtonsoft.Json.Linq;
using Snapper.Core;
using Xunit;

namespace Snapper.Json.Tests
{
    public class JsonSnapStoreTests
    {
        private readonly ISnapStore _byteSnapStore;
        private readonly Mock<IFileSystem> _fileSystemMock;

        public JsonSnapStoreTests()
        {
            _fileSystemMock = new Mock<IFileSystem>();
            _byteSnapStore = new JsonSnapStore(_fileSystemMock.Object);
        }

        [Fact]
        public void GetSnap_FileDoesNotExist()
        {
            _fileSystemMock.Setup(f => f.FileExists(It.IsAny<string>()))
                .Returns(null);
            Assert.Null(_byteSnapStore.GetSnap("path"));
        }

        [Fact]
        public void StoreSnap_FileDoesNotExist()
        {
            _fileSystemMock.Setup(f => f.GetFolderPath(It.IsAny<string>()))
                .Returns("c:\\folder");

            _byteSnapStore.StoreSnap("path", "data");

            _fileSystemMock.Verify(f => f.CreateFolder(It.IsAny<string>()), Times.Once);
            _fileSystemMock.Verify(f => f.WriteTextToFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            _fileSystemMock.Verify(f => f.GetFolderPath(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void StoreAndGet()
        {
            var jsonSnapStore = new JsonSnapStore(new InMemoryFileSystem());

            var obj = new
            {
                Int = 10,
                IntList = new List<int> { 21, 22 },
                String = "randomstring"
            };

            jsonSnapStore.StoreSnap("path", obj);

            var newObj = jsonSnapStore.GetSnap("path");

            var dummyObj = newObj as JObject;

            Assert.Equal(obj.String, dummyObj["String"]);
            Assert.Equal(obj.Int, dummyObj["Int"]);
            Assert.Equal(obj.IntList.Count, dummyObj["IntList"].Count());

            for (var i = 0; i < obj.IntList.Count ; i++)
            {
                Assert.Equal(obj.IntList[i], dummyObj["IntList"][i]);
            }
        }

        private class InMemoryFileSystem : IFileSystem
        {
            private string Data;

            public bool FileExists(string filePath)
                => true;

            public string ReadTextFromFile(string filePath)
                => Data;

            public void WriteTextToFile(string filePath, string text)
                => Data = text;

            public void CreateFolder(string folderPath)
            {
            }

            public string GetFolderPath(string filePath)
                => "";
        }
    }
}
