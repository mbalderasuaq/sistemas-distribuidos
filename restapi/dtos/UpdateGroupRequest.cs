namespace RestApi.Dtos;

public class UpdateGroupRequest
{
    public string Name {get; set;} = null!;
    public Guid[] Users { get; set; } = null!;
}