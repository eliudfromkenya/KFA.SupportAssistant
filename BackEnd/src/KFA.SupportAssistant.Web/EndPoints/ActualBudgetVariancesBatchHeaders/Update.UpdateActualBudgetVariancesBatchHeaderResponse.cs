namespace KFA.SupportAssistant.Web.EndPoints.ActualBudgetVariancesBatchHeaders;

public class UpdateActualBudgetVariancesBatchHeaderResponse
{
  public UpdateActualBudgetVariancesBatchHeaderResponse(ActualBudgetVariancesBatchHeaderRecord actualBudgetVariancesBatchHeader)
  {
    ActualBudgetVariancesBatchHeader = actualBudgetVariancesBatchHeader;
  }

  public ActualBudgetVariancesBatchHeaderRecord ActualBudgetVariancesBatchHeader { get; set; }
}
