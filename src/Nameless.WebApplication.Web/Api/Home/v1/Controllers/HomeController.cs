using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nameless.WebApplication.Collections.Generic;
using Nameless.WebApplication.Domain.Output;

namespace Nameless.WebApplication.Api.Home.v1.Controllers {

    public class HomeController : ApiControllerBase {

        #region Private Read-Only Fields

        private readonly IMapper _mapper;

        #endregion

        #region Public Constructors

        public HomeController(IMapper mapper) {
            _mapper = mapper;
        }

        #endregion

        #region Methods

        [HttpGet]
        public IActionResult Get() {
            var page = new Page<int>(new[] { 1, 2, 3, 4, 5 });

            var result = _mapper.Map<PageOutput<int>>(page);

            return Ok(result);
        }

        [Authorize]
        [HttpGet, Route("/list")]
        public IActionResult List() {
            return Ok(new[] { 1, 2, 3 });
        }

        #endregion
    }
}
