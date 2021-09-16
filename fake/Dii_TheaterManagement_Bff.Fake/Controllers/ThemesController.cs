using FakeTheaterBff.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FakeTheaterBff.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThemesController : ControllerBase
    {
        // GET: api/<ThemesController>
        [HttpGet]
        public IEnumerable<WebSiteTheme> Get()
        {
            foreach (string themeName in Enum.GetNames(typeof(ThemeType)))
            {
                yield return new WebSiteTheme() { Name = themeName };
            };
        }
    }
}
