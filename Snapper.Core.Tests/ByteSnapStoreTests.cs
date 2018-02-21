using System;
using System.Collections.Generic;
using Xunit;

namespace Snapper.Core.Tests
{
    public class ByteSnapStoreTests
    {
        [Fact]
        public void ObjectToString_StringToObject()
        {
            var obj = new DummyObject
            {
                Int = 10,
                IntList = new List<int> { 21, 22 },
                String = "randomstring"
            };

            var objString = ByteSnapStore.ObjectToString(obj);
            var newObj = ByteSnapStore.StringToObject(objString);

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
    }
}
