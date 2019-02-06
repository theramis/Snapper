using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace Snapper.Core.Tests
{
    public class ByteSnapStoreTests
    {
        private readonly ISnapStore _byteSnapStore;
        private readonly Mock<IFileSystem> _fileSystemMock;

        public ByteSnapStoreTests()
        {
            _fileSystemMock = new Mock<IFileSystem>();
            _byteSnapStore = new ByteSnapStore(_fileSystemMock.Object);
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
            var byteSnapStore = new ByteSnapStore(new InMemoryFileSystem());

            var obj = new DummyObject
            {
                Int = 10,
                IntList = new List<int> { 21, 22 },
                String = "randomstring"
            };

            byteSnapStore.StoreSnap("path", obj);

            var newObj = byteSnapStore.GetSnap("path");

            var dummyObj = newObj as DummyObject;
            Assert.Equal(obj.String, dummyObj?.String);
            Assert.Equal(obj.Int, dummyObj?.Int);
            Assert.Equal(obj.IntList.Count, dummyObj?.IntList.Count);

            for (var i = 0; i < obj.IntList.Count ; i++)
            {
                Assert.Equal(obj.IntList[i], dummyObj?.IntList[i]);
            }
        }

        [Serializable]
        private class DummyObject
        {
            public string String { get; set; }
            public int Int { get; set; }
            public List<int> IntList { get; set; }
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
