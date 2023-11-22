using LiteDB;

namespace KFA.SupportAssistant.Globals.Classes;
public static class LocalCache
{
  private record Poco<T>
  {
    public T? Value { get; set; }
    public string? Id { get; set; }

    public Poco(T? value)
    {
      Value = value;
      Id = ObjectId.NewObjectId()?.ToString();
    }

    [BsonCtor]
    public Poco(string id, T? value)
    {
      Value = value;
      Id = id;
    }
  }

  static readonly string conString = @"";
  public static List<T?> Get<T>(int limit = 0)
  {
    using var db = new LiteDatabase(conString);
    // Get a collection (or create, if doesn't exist)
    var col = db.GetCollection<Poco<T>>(GetCollectionName<T>());

    var query = col.Query()
       .OrderBy(x => x.Id)
       .Select(x => x.Value);

    if(limit > 0)
      query= query.Skip(limit);

    return query.ToList();
  }

  private static string GetCollectionName<T>()
  {
    return typeof(T).ToString().MakeName();
  }

  public static List<T?>? Get<T>(Func<T?, bool> condition, int limit = 0)
  {
    using var db = new LiteDatabase(conString);
    // Get a collection (or create, if doesn't exist)
    var col = db.GetCollection<Poco<T>>(GetCollectionName<T>());

    var query = col.Query()
       .Where(v => condition(v.Value))
       .OrderBy(x => x.Id)
       .Select(x => x.Value);

    if (limit > 0)
      query = query.Skip(limit);

    return query.ToList();
  }

  public static T? Get<T>(string id)
  {
    using var db = new LiteDatabase(conString);
    // Get a collection (or create, if doesn't exist)
    var col = db.GetCollection<Poco<T>>(GetCollectionName<T>());

   return col.Query()
       .Where(x => x.Id == id)
       .OrderBy(x => x.Id)
       .Limit(1)
       .FirstOrDefault().Value;
  }

  public static void Add<T>(string id, T? obj)
  {
    using var db = new LiteDatabase(conString);
    // Get a collection (or create, if doesn't exist)
    var col = db.GetCollection<Poco<T>>(GetCollectionName<T>());

    // Insert new customer document (Id will be auto-incremented)
    col.Insert(new Poco<T>(id,obj));
  }

  public static void Update<T>(string id, T? obj)
  {
    using var db = new LiteDatabase(conString);
    // Get a collection (or create, if doesn't exist)
    var col = db.GetCollection<Poco<T>>(GetCollectionName<T>());

    var oldObj = col.Query()
       .Where(v => v.Id == id)
       .FirstOrDefault() ?? throw new Exception("Object was not found in local cache");

    oldObj.Value = obj;
    col.Update(oldObj);
  }


  public static void Upsert<T>(string id, T? obj)
  {
    using var db = new LiteDatabase(conString);
    // Get a collection (or create, if doesn't exist)
    var col = db.GetCollection<Poco<T>>(GetCollectionName<T>());

    var oldObj = col.Query()
       .Where(v => v.Id == id)
       .FirstOrDefault();

    if(obj == null)
    {
      // Insert new customer document (Id will be auto-incremented)
      col.Insert(new Poco<T>(id, obj));
    }
    else
    {
      oldObj.Value = obj;
      col.Update(oldObj);
    }    
  }
}
