using System.Collections.Generic;
using System.Threading.Tasks;
using BuisnessLogicLayer.CoinMarketCap;
using ContractEntities.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace IISHost.Controllers
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
        public async Task<List<Crypto>> GetCryptosList()
        {
            return await _coinMarketCapBl.GetAllCryptos();
        }

        [HttpGet("getcryptodetail{id}")]
        public async Task<Crypto> GetCryptoDetail([FromRoute] string id)
        {
            return await _coinMarketCapBl.GetCryptoDetail(id);
        }
    }
}
