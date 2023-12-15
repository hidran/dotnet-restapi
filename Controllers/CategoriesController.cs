using AutoMapper;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using PmsApi.DataContexts;
using PmsApi.DTO;
using PmsApi.Models;

namespace Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    private readonly PmsContext _context;
    private readonly IMapper _mapper;
    public CategoriesController(PmsContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {

        var categories = await _context.Categories.ToListAsync();
        var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
        return Ok(categoriesDto);
    }



    [HttpGet("{categoryId}")]
    public async Task<ActionResult<CategoryDto>> GetCategory(int categoryId)
    {

        ProjectCategory? category = await _context.Categories.FirstOrDefaultAsync(p => p.CategoryId == categoryId);
        if (category is null)
        {
            return NotFound();
        }
        var categoryDto = _mapper.Map<CategoryDto>(category);
        return Ok(categoryDto);
    }
    [HttpPost]
    public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryDto categoryDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var category = _mapper.Map<ProjectCategory>(categoryDto);

        _context.Categories.Add(category);
        try
        {
            await _context.SaveChangesAsync();
            var newCategoryDto = _mapper.Map<CategoryDto>(category);

            return CreatedAtAction(nameof(GetCategory),
            new { categoryId = category.CategoryId }, newCategoryDto);
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("category name already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }
    [HttpPut("{categoryId:int}")]
    public async Task<ActionResult> UpdateCategory(
        [FromRoute] int categoryId, [FromBody] CreateCategoryDto categoryDto
        )
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        ProjectCategory? category = await _context.Categories.FindAsync(categoryId);

        if (category is null)
        {
            return NotFound($"Category with ID {categoryId} not found.");
        }

        _mapper.Map(categoryDto, category);
        try
        {
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException e)
        when (e.InnerException is MySqlException
         mySqlException && mySqlException.Number == 1062)
        {

            return BadRequest("Category name already taken");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }
    }


    [HttpDelete("{categoryId:int}")]
    public async Task<ActionResult> DeleteCategory(int categoryId)
    {
        ProjectCategory? category = await _context.Categories.FindAsync(categoryId);

        if (category is null)
        {
            return NotFound($"No category found with ID {categoryId}");
        }
        try
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateException e)
      when (e.InnerException is MySqlException)
        {

            return BadRequest("Category has other records, please delete assigned tasks");
        }
        catch (Exception)
        {
            return StatusCode(500, "An error has occurred");
        }

    }
}
