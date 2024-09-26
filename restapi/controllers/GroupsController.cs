using Microsoft.AspNetCore.Mvc;
using RestApi.Dtos;
using RestApi.Services;
using RestApi.Mappers;
using RestApi.Exceptions;
using System.Net;

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
    // GET /groups?name={name}?pageNumber={pageNumber}&pageSize={pageSize}
    [HttpGet]
    public async Task<ActionResult<IList<GroupResponse>>> GetAllByName([FromQuery] string name, [FromQuery] int pageNumber, [FromQuery] int pageSize, [FromQuery] string orderBy, CancellationToken cancellationToken)
    {
        var groups = await _groupService.GetAllByNameAsync(name, pageNumber, pageSize, orderBy, cancellationToken);

        return Ok(groups.Select(group => group.ToDto()).ToList());
    }

    // DELETE /groups/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(string id, CancellationToken cancellationToken)
    {
        try
        {
            await _groupService.DeleteGroupByIdAsync(id, cancellationToken);
            return NoContent();
        }
        catch (GroupNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<GroupResponse>> CreateGroup([FromBody] CreateGroupRequest groupRequest, CancellationToken cancellationToken)
    {
        try
        {
            var group = await _groupService.CreateGroupAsync(groupRequest.Name, groupRequest.Users, cancellationToken);
            return CreatedAtAction(nameof(GetGroupById), new { id = group.Id }, group.ToDto());
        }
        catch (InvalidGroupRequestFormatException)
        {
            return BadRequest(NewValidationProblemDetails("One or more validation errors ocurred", HttpStatusCode.BadRequest, new Dictionary<string, string[]>{
                {"Groups", ["Users array is empty"]}
            }));
        }
        catch (GroupAlreadyExistsException)
        {
            return Conflict(NewValidationProblemDetails("One or more validation errors ocurred", HttpStatusCode.Conflict, new Dictionary<string, string[]>{
                {"Groups", ["Group with same name already exists"]}
            }));
        }
    }

    private static ValidationProblemDetails NewValidationProblemDetails(string title, HttpStatusCode statusCode, Dictionary<string, string[]> errors)
    {
        return new ValidationProblemDetails
        {
            Title = title,
            Status = (int) statusCode,
            Errors = errors
        };
    }
}