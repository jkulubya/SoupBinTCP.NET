using System;
using SoupBinTCP.NET.Messages;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var message = new Debug("Hi");
            Assert.Equal(3, message.Length);
        }
        
        [Fact]
        public void Test2()
        {
            var message = new Debug("HiHiHiHiHiHiHiHiHiHiHiHiHiHi");
            Assert.Equal(29, message.Length);
        }
    }
}