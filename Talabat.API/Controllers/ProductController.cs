using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.Core.Interfaces;
using Talabat.Core.Interfaces.InterfaceUnitOfWork;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet("GetProducts")]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetAllProducts([FromQuery] SpecificationParams Params)
        {
            var spec = new ProductWithBrandAndTypeSpecification(Params);
            var Products = await _unitOfWork.Repository<Product>().GetAllAsync(spec);
            if (Products is null) return NotFound(new ApiResponse(404));
            var mappedProduct = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(Products);
            var CountSpec = new ProductWithFilterationForCountAsync(Params);
            var count = await _unitOfWork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDTO>(Params.PageIndex, Params.PageSize, mappedProduct, count));
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            if (product is null) return NotFound(new ApiResponse(404));
            var mappedProduct = _mapper.Map<Product, ProductToReturnDTO>(product);
            return Ok(mappedProduct);
        }
        // Get All Types
        [HttpGet("Types")]
        [ProducesResponseType(typeof(ProductType), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
        {
            var Types = await _unitOfWork.Repository<ProductType>().GetAllAsync();
            if (Types is null) return NotFound(new ApiResponse(404, "There's No Types Found"));
            return Ok(Types);
        }
        // Get All Brands
        [HttpGet("Brands")]
        [ProducesResponseType(typeof(ProductBrand), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var Brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            if (Brands is null) return NotFound(new ApiResponse(404, "There's No Types Found"));
            return Ok(Brands);
        }
    }
}
