namespace KFA.SupportAssistant.Globals.DataLayer;
public interface IEndPointManager
{
  string[] GetDefaultAccessRights(string rightId);
  string[] GetDefaultAccessRights(string name, string type);
}
