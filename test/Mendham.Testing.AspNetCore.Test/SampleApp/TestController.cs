using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.AspNetCore.Test.SampleApp
{
    [Route("")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }


        [HttpGet]
        public async Task<ActionResult> GetValueAsync()
        {
            int value =await _testService.GetValue();

            return Ok(value);
        }

        [HttpPost]
        public async Task<ActionResult> TakeActionAsync([FromBody]string value)
        {
            await _testService.TakeAction(value);

            return NoContent();
        }

        [HttpPost("conditional")]
        public async Task<ActionResult> ConditionalActionAsync([FromBody]string value)
        {
            var result = await _testService.TakeAction(value);

            return Ok(result);
        }

    }
}
