using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Services;
using RestApi.Mappers;

namespace RestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupsController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    // GET /groups/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<GroupResponse>> GetGroupById(string id, CancellationToken cancellationToken)
    {
        var group = await _groupService.GetGroupByIdAsync(id, cancellationToken);

        if (group == null)
        {
            return NotFound();
        }

        return Ok(group.ToDto());
    }
    
    // GET /groups?name={name}
    [HttpGet]
    public async Task<ActionResult<IList<GroupResponse>>> GetAllByName([FromQuery] string name, CancellationToken cancellationToken)
    {
        var groups = await _groupService.GetAllByNameAsync(name, cancellationToken);

        return Ok(groups.Select(group => group.ToDto()).ToList());
    }
}