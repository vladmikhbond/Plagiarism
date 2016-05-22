using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Plagiarism;
using System.IO;

namespace UnitTestPlagiarism
{
    [TestClass]
    public class UnitTestXComparer
    {

        [TestMethod]
        public void Find_1()
        {
            string book = File.ReadAllText(@"Resources\Text1.txt");
            var sample = "Шаблон проектирования программы в виде трех слабо связанных между собой частей: модели, представления и контроллера известен в программировании давно";
            int res = XComparer.Find(book, sample, 0);
            Assert.AreEqual(259140, res);
        }


        [TestMethod]
        public void Find_2()
        {
            int L = 10000, D = 1000;
            string book = File.ReadAllText(@"Resources\Text1.txt");
            var sample = book.Substring(L, D);
            int res = XComparer.Find(book, sample, 0);
            Assert.AreEqual(L, res);
        }


        [TestMethod]
        public void ExtStart_1()
        {
            var book = "01234567890123456789";
            var manu = "00034567890123400";
            var xc = new XComparer(manu, 5);
            int res = xc.ExtStart(book, 5, 5);
            Assert.AreEqual(3, res);
        }

        [TestMethod]
        public void ExtStart_2()
        {
            var book = "01234567890123456789";
            var manu = "0001234567890123400";
            var xc = new XComparer(manu, 5);
            int res = xc.ExtStart(book, 5, 7);
            Assert.AreEqual(2, res);
        }

        [TestMethod]
        public void ExtStart_3()
        {
            var book = "0001234567890123456789";
            var manu = "01234567890123400";
            var xc = new XComparer(manu, 5);
            int res = xc.ExtStart(book, 7, 5);
            Assert.AreEqual(0, res);
        }

        [TestMethod]
        public void ExtFinish_1()
        {
            var book = "01234567890123456789";
            var manu = "00034567890123400";
            var xc = new XComparer(manu, 5);
            int res = xc.ExtFinish(book, 10, 10);
            Assert.AreEqual(15, res);
        }

        [TestMethod]
        public void ExtFinish_2()
        {
            var book = "01234567890123456789";
            var manu = "0000034567890123400";
            var xc = new XComparer(manu, 5);
            int res = xc.ExtFinish(book, 10, 12);
            Assert.AreEqual(17, res);
        }

        [TestMethod]
        public void ExtFinish_3()
        {
            var book = "0001234567890123456789";
            var manu = "00034567890123400";
            var xc = new XComparer(manu, 5);
            int res = xc.ExtFinish(book, 12, 10);
            Assert.AreEqual(15, res);
        }

        [TestMethod]
        public void Compare_1()
        {
            var manu = "00034567890123400";
            var book = "01234567890123456789";
            var xc = new XComparer(manu, 5);
            var res = xc.Compare(book);

            Assert.AreEqual(3, res[0].ManStart);
            Assert.AreEqual(12, res[0].Length);
        }

        [TestMethod]
        public void Compare_2()
        {
            var book = "01234567890123456789";
            var manu = "0000034567890123400";
            var xc = new XComparer(manu, 5);
            var res = xc.Compare(book);

            Assert.AreEqual(5, res[0].ManStart);
            Assert.AreEqual(12, res[0].Length);
        }

        [TestMethod]
        public void Compare_3()
        {
            var book = "0001234567890123456789";
            var manu = "00034567890123400";
            var xc = new XComparer(manu, 5);
            var res = xc.Compare(book);

            Assert.AreEqual(3, res[0].ManStart);
            Assert.AreEqual(12, res[0].Length);
        }

        [TestMethod]
        public void Compare_4()
        {
            int L = 10000, D = 1000;
            string book = File.ReadAllText(@"Resources\Text1.txt");
            var manu = new string('0', L) + book.Substring(L, D) + new string('0', L);
            var xc = new XComparer(manu, 100);
            var res = xc.Compare(book);

            Assert.AreEqual(L, res[0].ManStart);
            Assert.AreEqual(D, res[0].Length);
        }

    }
}
