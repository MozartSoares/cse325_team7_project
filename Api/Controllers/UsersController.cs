using cse325_team7_project.Api.DTOs;
using cse325_team7_project.Api.Mappings;
using cse325_team7_project.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService service) : ControllerBase
{
    private readonly IUserService _service = service;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
        => Ok((await _service.List()).Select(u => u.ToDto()));

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponseDto>> GetById(ObjectId id)
        => Ok((await _service.Get(id)).ToDto());

    [HttpPut("{id}")]
    public async Task<ActionResult<UserResponseDto>> Update(ObjectId id, [FromBody] UserUpdateDto dto)
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
