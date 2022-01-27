using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/employee")]
    public class EmptyController : ControllerBase
    {
        /// <summary>
        /// Для теста производительности, возвращает текущую дату
        /// </summary>
        /// <returns></returns>
        [HttpGet("datetime")]
        public ActionResult GetDatetime()
        => Ok(DateTime.Now);

    }
}
