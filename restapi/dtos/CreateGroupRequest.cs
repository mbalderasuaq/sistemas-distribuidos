namespace RestApi.Dtos;

public class CreateGroupRequest
{
    public string Name {get; set;} = null!;
    public Guid[] Users {get; set;} = null!;
}