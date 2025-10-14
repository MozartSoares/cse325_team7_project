using cse325_team7_project.Api.DTOs;
using cse325_team7_project.Api.Mappings;
using cse325_team7_project.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService service, IAuthService auth) : ControllerBase
{
    private readonly IUserService _service = service;
    private readonly IAuthService _auth = auth;


    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetAll()
        => Ok((await _service.List()).Select(u => u.ToDto()));

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponseDto>> GetById(ObjectId id)
        => Ok((await _service.Get(id)).ToDto());

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<ActionResult<UserResponseDto>> Create([FromBody] UserCreateAdminDto dto)
    {
        var result = await _auth.Register(dto.Username, dto.Name, dto.Email, dto.Password, dto.Role);
        var created = result.User;
        return CreatedAtAction(nameof(GetById), new { id = created.Id.ToString() }, created.ToDto());
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "SelfOrAdmin")]
    public async Task<ActionResult<UserResponseDto>> Update(ObjectId id, [FromBody] UserUpdateDto dto)
    {
        var current = await _service.Get(id);
        current.Apply(dto);
        var updated = await _service.Update(id, current);
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "SelfOrAdmin")]
    public async Task<IActionResult> Delete(ObjectId id)
    {
        var ok = await _service.Delete(id);
        return ok ? NoContent() : NotFound();
    }
}
