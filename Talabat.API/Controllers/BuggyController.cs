using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;
using Talabat.Repository.Contexts;

namespace Talabat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
                                    // This Controller Just For Showing Types Of Errors
    public class BuggyController : ControllerBase
    {
        private readonly TalabatContext _dbContext;

        public BuggyController(TalabatContext dbContext)
        {
            _dbContext = dbContext;
        }
                                // EndPoint =>  NotFound()
         // baseUrl/Api/Buggy/Notfound
        [HttpGet("Notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbContext.Products.Find(100);
            if (product is null ) return NotFound(new ApiResponse(404));
            return Ok(product);
        }

                                // EndPoint => internal Server Error
        // baseUrl/api/buggy/ServerError
        [HttpGet("ServerError")]
        public ActionResult GetServerErrorResponse() 
        {
            var product = _dbContext.Products.Find(100);
            var productToReturn = product.ToString();
            return Ok(productToReturn);
        }
                                // EndPoint => Bad Request
        //baseUrl/api/Buggy/BadRequest
        [HttpGet("BadRequest")]
        public ActionResult GetBadRequesrtError() => BadRequest();

                                // EndPoint => Validation Error(send string instead of an integer)
        [HttpGet("BadRequest/{id}")]
        public ActionResult GetBadRequest(int id) => Ok();
    }
}
