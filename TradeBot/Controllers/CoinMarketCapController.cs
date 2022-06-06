using System.Collections.Generic;
using BuisnessLogicLayer.CoinMarketCap;
using ContractEntities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ISSHost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinMarketCapController : ControllerBase, ICoinMarketCapController
    {
        private readonly ICoinMarketCapBL _coinMarketCapBl;

        public CoinMarketCapController(ICoinMarketCapBL coinMarketCapBl)
        {
            _coinMarketCapBl = coinMarketCapBl;
        }

        [HttpGet("getallcryptos")]
        public List<Crypto> GetCryptosList()
        {
            return _coinMarketCapBl.GetAllCryptos();
        }

        [HttpGet("getcryptodetail{id}")]
        public Crypto GetCryptoDetail([FromRoute] string id)
        {
            return _coinMarketCapBl.GetCryptoDetail(new CryptoId() { Id = id });
        }
    }
}
