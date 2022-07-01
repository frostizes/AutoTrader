using BuisnessLogicLayer.TradingAlgo;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Random;

namespace BuisnessLogicLayerTests
{
    [TestClass]
    public class RSIAlgoTests
    {
        private ITrader _trader;
        private FixedSizedList<double> _queue;

        [TestInitialize]
        public void Init()
        {
            _trader = new Trader();
            _queue = new FixedSizedList<double>();
        }

        [TestMethod]
        public void TestQueue()
        {
            //Assert.AreEqual(_queue.First(),1.3);
            //Assert.AreEqual(_queue.Last(),5.3);
            //foreach (var VARIABLE in _queue)
            //{
            //    Assert.AreEqual(_queue.First(),1.3);
            //}
        }
    }
}