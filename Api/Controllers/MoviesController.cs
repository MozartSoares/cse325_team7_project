using cse325_team7_project.Api.DTOs;
using cse325_team7_project.Api.Mappings;
using cse325_team7_project.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController(IMovieService service) : ControllerBase
{
    private readonly IMovieService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovieResponseDto>>> GetAll()
        => Ok((await _service.List()).Select(m => m.ToDto()));

    [HttpGet("{id}")]
    public async Task<ActionResult<MovieResponseDto>> GetById(ObjectId id)
        => Ok((await _service.Get(id)).ToDto());

    [HttpPost]
    public async Task<ActionResult<MovieResponseDto>> Create([FromBody] MovieCreateDto dto)
    {
        var created = await _service.Create(dto.ToModel());
        return CreatedAtAction(nameof(GetById), new { id = created.Id.ToString() }, created.ToDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MovieResponseDto>> Update(ObjectId id, [FromBody] MovieUpdateDto dto)
    {
        var current = await _service.Get(id);
        current.Apply(dto);
        var updated = await _service.Update(id, current);
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(ObjectId id)
    {
        var ok = await _service.Delete(id);
        return ok ? NoContent() : NotFound();
    }
}
