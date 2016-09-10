using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mendham.Testing.AspNetCore.Test.SampleApp
{
    [Route("test")]
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

        [HttpPost("noaction")]
        public Task<ActionResult> TakeNoActionAsync()
        {
            return Task.FromResult<ActionResult>(NoContent());
        }
    }
}
