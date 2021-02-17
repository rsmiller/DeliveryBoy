using DeliveryBoy.BusinessLayer.Models.Dto;
using DeliveryBoy.BusinessLayer.Service;
using DeliveryBoy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryBoyApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipController : ControllerBase
    {
        private ITipService _Service;

        public TipController(ITipService tipService)
        {
            _Service = tipService;
        }

        [HttpGet]
        public TipDto Get(GetTipModel model)
        {
            var response = _Service.GetTip(model);

            return response.Data;
        }

        [HttpPost]
        public StatusCodeResult Post([FromBody] EnterTipModel model)
        {
            var response = _Service.EnterTip(model);

            if (response.Success)
                return Ok();
            else
                return BadRequest();
        }
    }
}
