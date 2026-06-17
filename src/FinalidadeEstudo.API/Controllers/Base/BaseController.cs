using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace FinalidadeEstudo.API.Controllers.Base;
[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseController : ControllerBase
{
}
