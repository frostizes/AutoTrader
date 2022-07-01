//using BuisnessLogicLayer.TradingAlgo;
//using ContractEntities.Entities;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Utils.Random;

//namespace BuisnessLogicLayerTests
//{
//    [TestClass]
//    public class KeyTests
//    {
//        private Crypto _cryptoTest;
//        private List<Crypto> _cryptoList;

//        [TestInitialize]
//        public void Init()
//        {
//            _cryptoTest = new Crypto()
//            {
//                CmcKey = new CryptoId() { CmcKey = "ether" }
//            };

//            _cryptoList = new List<Crypto>()
//            {
//                new Crypto()
//                {
//                    CmcKey = new CryptoId() { CmcKey = "bitcoin" }
//                },
//                new Crypto()
//                {
//                    CmcKey = new CryptoId() { CmcKey = "ether" }
//                }
//            };
//        }

//        [TestMethod]
//        public void TestIdToString()
//        {
//            var test = _cryptoList.FirstOrDefault(c => c.CmcKey.CmcKey.Equals(_cryptoTest.CmcKey.CmcKey));
//            Assert.IsNotNull(test);
//        }
//    }
//}