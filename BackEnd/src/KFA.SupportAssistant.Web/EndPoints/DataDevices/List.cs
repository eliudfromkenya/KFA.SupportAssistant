using Ardalis.Result;
using KFA.SupportAssistant.Core;
using KFA.SupportAssistant.Core.DTOs;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.Infrastructure.Services;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Web.Services;
using MediatR;
using Newtonsoft.Json;

namespace KFA.SupportAssistant.Web.EndPoints.DataDevices;

/// <summary>
/// List all data devices by specified conditions
/// </summary>
/// <remarks>
/// List all data devices - returns a DataDeviceListResponse containing the data devices.
/// </remarks>
public class List(IMediator mediator, IEndPointManager endPointManager) : Endpoint<ListParam, DataDeviceListResponse>
{
  private const string EndPointId = "ENP-175";
  public const string Route = "/data_devices";

  public override void Configure()
  {
    Get(CoreFunctions.GetURL(Route));
    Permissions([.. endPointManager.GetDefaultAccessRights(EndPointId), UserRoleConstants.ROLE_SUPER_ADMIN, UserRoleConstants.ROLE_ADMIN]);

    Description(x => x.WithName("Get Data Devices List End Point"));

    Summary(s =>
    {
      // XML Docs are used by default but are overridden by these properties:
      s.Summary = $"[End Point - {EndPointId}] Retrieves list of data devices as specified";
      s.Description = "Returns all data devices as specified, i.e filter to specify which records or rows to return, order to specify order criteria";
      s.ResponseExamples[200] = new DataDeviceListResponse { DataDevices = [new DataDeviceRecord("Device Caption", "Device Code", "1000", "Device Name", "Device Number", "Device Right", "Station", "Type Of Device", DateTime.Now, DateTime.Now)] };
      s.ExampleRequest = new ListParam { Param = JsonConvert.SerializeObject(new FilterParam { Predicate = "Id.Trim().StartsWith(@0) and Id >= @1", SelectColumns = "new {Id, Narration}", Parameters = ["S3", "3100"], OrderByConditions = ["Id", "Narration"] }), Skip = 0, Take = 1000 };
    });
  }

  public override async Task HandleAsync(ListParam request,
    CancellationToken cancellationToken)
  {
    var command = new ListModelsQuery<DataDeviceDTO, DataDevice>(CreateEndPointUser.GetEndPointUser(User), request);
    var ans = await mediator.Send(command, cancellationToken);
    var result = Result<List<DataDeviceDTO>>.Success(ans.Select(v => (DataDeviceDTO)v).ToList());

    if (result.Errors.Any())
    {
      result.Errors.ToList().ForEach(n => AddError(n));
      await ErrorsConverter.CheckErrors(HttpContext, result.Status, result.Errors, cancellationToken);
    }

    ThrowIfAnyErrors();

    if (result.IsSuccess)
    {
      Response = new DataDeviceListResponse
      {
        DataDevices = result.Value.Select(obj => new DataDeviceRecord(obj.DeviceCaption, obj.DeviceCode, obj.Id, obj.DeviceName, obj.DeviceNumber, obj.DeviceRight, obj.StationID, obj.TypeOfDevice, obj.DateInserted___, obj.DateUpdated___)).ToList()
      };
    }
  }
}
