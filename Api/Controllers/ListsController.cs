using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using cse325_team7_project.Api.Common;
using cse325_team7_project.Api.DTOs;
using cse325_team7_project.Api.Mappings;
using cse325_team7_project.Api.Services.Interfaces;
using cse325_team7_project.Domain.Enums;
using cse325_team7_project.Api.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace cse325_team7_project.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListsController(IMoviesListService service, IUserService users) : ControllerBase
{
    private readonly IMoviesListService _service = service;
    private readonly IUserService _users = users;

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<MoviesListResponseDto>>> GetAll()
        => Ok((await _service.List()).Select(l => l.ToDto()));

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<MoviesListResponseDto>> GetById(ObjectId id)
        => Ok((await _service.Get(id)).ToDto());

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<MoviesListResponseDto>> Create([FromBody] MoviesListCreateDto dto)
    {
        var userId = User.GetUserIdOrThrow();
        var created = await _service.Create(dto.ToModel(), userId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id.ToString() }, created.ToDto());
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<MoviesListResponseDto>> Update(ObjectId id, [FromBody] MoviesListUpdateDto dto)
    {
        if (!await User.OwnsListOrAdmin(id, _users)) return Forbid();

        var current = await _service.Get(id);
        current.Apply(dto);
        var updated = await _service.Update(id, current);
        return Ok(updated.ToDto());
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(ObjectId id)
    {
        if (!await User.OwnsListOrAdmin(id, _users)) return Forbid();

        var ok = await _service.Delete(id);
        return ok ? NoContent() : NotFound();
    }
}
