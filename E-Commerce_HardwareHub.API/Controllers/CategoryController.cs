using AutoMapper;
using HardwareHub.Data.Services.UOW;
using HardwareHub.Models.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace E_Commerce_HardwareHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly APIResponse _apiResponse;

        public CategoryController(IUnitOfWork unitOfWork, IMapper mapper, APIResponse apiResponse)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiResponse = new APIResponse();
        }


    }
}
