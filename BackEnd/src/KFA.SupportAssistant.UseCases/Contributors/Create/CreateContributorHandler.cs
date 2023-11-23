using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Core.ContributorAggregate;

namespace KFA.SupportAssistant.UseCases.Contributors.Create;

public class CreateContributorHandler(IRepository<Contributor> _repository)
  : ICommandHandler<CreateContributorCommand, Result<string>>
{
  public async Task<Result<string>> Handle(CreateContributorCommand request,
    CancellationToken cancellationToken)
  {
    var newContributor = new Contributor(request.Name);
    var createdItem = await _repository.AddAsync(newContributor, cancellationToken);

    return createdItem.Id ?? string.Empty;
  }
}
