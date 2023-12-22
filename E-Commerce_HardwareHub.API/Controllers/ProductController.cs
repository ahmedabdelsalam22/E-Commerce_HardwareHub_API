using AutoMapper;
using Azure;
using HardwareHub.Data.Services.UOW;
using HardwareHub.Models.Dtos;
using HardwareHub.Models.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = new List<string> { ex.ToString() };
            }
            return _apiResponse;
        }

        [Authorize]
        [HttpGet("Product/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetProductById(int? id)
        {
            try
            {
                if (id == null || id == 0)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Message = new List<string> { "invalid id!" };
                    _apiResponse.IsSuccess = true;
                }
                Product product = await _unitOfWork.productRepository.Get(tracked: false, filter: x => x.ProductId == id, includeProperties: "Category");

                if (product == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Message = new List<string> { "No product found with this id!" };
                    _apiResponse.IsSuccess = true;
                }

                var productDto = _mapper.Map<Product>(product);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = productDto;
            }
            catch (Exception ex)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = new List<string> { ex.ToString() };
            }
            return _apiResponse;
        }

        [Authorize]
        [HttpPost("product/create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateProduct([FromBody] ProductDto productDto) 
        {
            try 
            {
                if (productDto == null)
                {
                    return BadRequest(ModelState);
                }
                // related entities 
                Category category = await _unitOfWork.categoryRepository.Get(tracked: false , filter:x=>x.Name.ToLower() == productDto.CategoryDto.Name.ToLower());

                if (category == null)
                {
                    return BadRequest("category is't exists");
                }
                var categoryToCreate = _mapper.Map<CategoryCreateDto>(category);
                productDto.CategoryDto = categoryToCreate;


                var productToCreate = _mapper.Map<Product>(productDto);

                await _unitOfWork.productRepository.Create(productToCreate);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = productToCreate;
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = new List<string> { ex.ToString() };
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
            }
            return _apiResponse;
        }

        [Authorize]
        [HttpPut("product/update/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateProduct(int productId, [FromBody] ProductUpdateDto productToUpdate)
        {
            try
            {
                if (productToUpdate == null)
                {
                    _apiResponse.IsSuccess = false;
                    _apiResponse.Message = new List<string> { "fill fields" };
                }
                if (productId != productToUpdate!.ProductId)
                {
                    return BadRequest(ModelState);
                }
                Product product = await _unitOfWork.productRepository.Get(filter: x => x.ProductId == productId, tracked: false);
                if (product == null)
                {
                    return NotFound("No product exists with this id");
                }
                Category category = await _unitOfWork.categoryRepository.Get(filter: x => x.CategoryId == productToUpdate.CategoryDto.CategoryId);
                if (category == null)
                {
                    return NotFound("No category exists with this id");
                }

                var categoryDto = _mapper.Map<CategoryUpdateDto>(category);

                productToUpdate.CategoryDto = categoryDto;

                var productToUpd = _mapper.Map<Product>(productToUpdate);

                await _unitOfWork.productRepository.Update(productToUpd);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = productToUpd;
            }
            catch (Exception ex)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = new List<string> { ex.ToString() };
            }
            return _apiResponse;
        }

        [Authorize]
        [HttpDelete("product/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteProduct(int productId)
        {
            try
            {
                Product product = await _unitOfWork.productRepository.Get(filter: x => x.ProductId == productId, tracked: false);

                if (product == null)
                {
                    return NotFound("product does't exists");
                }
                await _unitOfWork.productRepository.Delete(product);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _apiResponse.StatusCode = HttpStatusCode.BadRequest;
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = new List<string> { ex.ToString() };
            }
            return _apiResponse;
        }
    }
}
