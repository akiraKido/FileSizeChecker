using System;
using FileSizeChecker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class HistoryTests
    {
        [TestMethod]
        public void TestMethod1 ()
        {
            var history = new HistoryManager();
            Assert.AreEqual( history.CanReturn, false );
            Assert.AreEqual( history.CanMoveNext, false );
            history.Push( "hoge" );
            Assert.AreEqual( history.CanReturn, false );
            Assert.AreEqual( history.CanMoveNext, false );
            history.Push( "fuga" );
            Assert.AreEqual( history.CanReturn, true );
            Assert.AreEqual( history.CanMoveNext, false );
            history.Push( "moge" );

            Assert.AreEqual( history.Current, "moge" );
            var actual = history.Back();
            Assert.AreEqual( actual, "fuga" );
            Assert.AreEqual( history.Current, "fuga" );
            Assert.AreEqual( history.CanReturn, true );
            Assert.AreEqual( history.CanMoveNext, true );

            actual = history.Back();
            Assert.AreEqual( actual, "hoge" );
            Assert.AreEqual( history.Current, "hoge" );
            Assert.AreEqual( history.CanReturn, false );
            Assert.AreEqual( history.CanMoveNext, true );
            
            history.Push( "gogo" );

        }
    }
}
