﻿using AutoMapper;
using HardwareHub.Data.Services.UOW;
using HardwareHub.Models.Dtos;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllCategories() 
        {
            try 
            {
                List<Category> categories = await _unitOfWork.categoryRepository.GetAll(tracked: false);

                if (categories == null) 
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Message = new List<string> {"No data found!"};
                    _apiResponse.IsSuccess = true;
                }

                var categoriesDto = _mapper.Map<List<CategoryDto>>(categories);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = categoriesDto;
            }
            catch(Exception ex) 
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = new List<string> {ex.ToString()};
            }
            return _apiResponse;
        }

        [HttpGet("Category/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCategoryById(int? id)
        {
            try
            {
                if (id == null || id == 0) 
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Message = new List<string> { "invalid id!"};
                    _apiResponse.IsSuccess = true;
                }
                Category category = await _unitOfWork.categoryRepository.Get(tracked: false , filter:x=>x.CategoryId == id);

                if (category == null)
                {
                    _apiResponse.StatusCode = HttpStatusCode.NotFound;
                    _apiResponse.Message = new List<string> { "No category found with this id!" };
                    _apiResponse.IsSuccess = true;
                }

                var categoryDto = _mapper.Map<CategoryDto>(category);

                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.IsSuccess = true;
                _apiResponse.Result = categoryDto;
            }
            catch (Exception ex)
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.Message = new List<string> { ex.ToString() };
            }
            return _apiResponse;
        }
    }
}
