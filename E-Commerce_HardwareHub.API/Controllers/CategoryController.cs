using AutoMapper;
using HardwareHub.Data.Services.UOW;
using HardwareHub.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Net;

namespace E_Commerce_HardwareHub.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet("Categories")]
        public async Task<ActionResult<APIResponse>> GetAllCategories() 
        {
            try 
            {
                List<Category> categories = await _unitOfWork.categoryRepository.GetAll(tracked: false);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = categories;
                return Ok(_apiResponse);
            }
            catch(Exception ex) 
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = new List<string> {ex.ToString()};
            }
            return BadRequest(_apiResponse);
        }
    }
}
