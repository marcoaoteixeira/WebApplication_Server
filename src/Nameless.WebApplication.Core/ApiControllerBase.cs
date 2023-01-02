using Microsoft.AspNetCore.Mvc;

namespace Nameless.WebApplication {

    [ApiController]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class ApiControllerBase : ControllerBase {

    }
}