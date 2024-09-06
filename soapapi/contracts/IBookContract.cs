using System.ServiceModel;
using SoapApi.Dtos;

namespace SoapApi.Contracts;

[ServiceContract]
public interface IBookContract
{
    [OperationContract]
    public Task<IList<BookResponseDto>> GetBooksByName(string name, CancellationToken cancellationToken);
}