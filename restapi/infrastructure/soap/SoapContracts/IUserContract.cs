using System.ServiceModel;

namespace RestApi.Infrastructure.Soap.SoapContracts;

[ServiceContract]
public interface IUserContract
{
    [OperationContract]
    public Task<UserResponseDto> GetUserById(Guid id, CancellationToken cancellationToken);

    [OperationContract]
    public Task<IList<UserResponseDto>> GetAll(CancellationToken cancellationToken);

    [OperationContract]
    public Task<IList<UserResponseDto>> GetAllByEmail(string email, CancellationToken cancellationToken);
    
    [OperationContract]
    public Task<bool> DeleteUserById(Guid Id, CancellationToken cancellationToken);
    
}