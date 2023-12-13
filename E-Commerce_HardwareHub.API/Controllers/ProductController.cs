using AutoMapper;
using HardwareHub.Data.Services.UOW;
using HardwareHub.Models.Dtos;
using HardwareHub.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_Commerce_HardwareHub.API.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }

        [HttpGet("products")]
        public async Task<ActionResult<APIResponse>> GetAllProducts() 
        {
            try 
            {

                var allProducts = await _unitOfWork.productRepository.GetAll(includeProperties: "Category");

                if (allProducts == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Message = new List<string> { "No data found!" };
                    _apiResponse.IsSuccess = true;
                }

                var productsDtos = _mapper.Map<List<ProductDto>>(allProducts);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = productsDtos;
            }
            catch(Exception ex) 
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = new List<string> { ex.ToString() };
            }
            return _apiResponse;
        }
    }
}
