﻿using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI
{
    [ApiController]
    [Produces("application/json")]
    [Route("categories")]   

    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult> GetCategoriesAsync()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            return Ok(categories);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult> GetCategoryByIdAsync([Required] Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            return Ok(category);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateCategoryAsync([FromForm][Required] CreateCategoryDTO request)
        {
            var category = await _categoryService.CreateCategoryAsync(request);
            return Ok(category);
        }

        [Authorize]
        [HttpPut("{id:Guid}")]
        public async Task<ActionResult> UpdateCategoryAsync(Guid id, [FromForm][Required] UpdateCategoryDTO request)
        {
            var category = await _categoryService.UpdateCategoryAsync(id, request);
            return Ok(category);
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        public async Task<ActionResult> DeleteCategoryAsync([Required] Guid id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
