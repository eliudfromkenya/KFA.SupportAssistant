using Microsoft.EntityFrameworkCore;
using PPMS.Console.Models;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPMS.Console.Generators
{
    static class MagicOnionClasses
    {
        public static Task Generate()
        {
            return Task.Factory.StartNew(CreateClasses);
        }

        private static void CreateClasses()
        {
            GenerateInterfaces();
            GenerateImplementation();
            GenerateImplementationSeveral();
            GenerateInterfacesSeveral();
            GenerateGeneral();
        }

        private static void GenerateGeneral()
        {
            using var db = new Data.Context();
            var tables = db.Tables.Include(x => x.Columns).ToArray();

            var condtions = tables.Select(table =>
            {
                var name = table.StrimLinedName.MakeName();
                var singular = Functions.Singularize(table.StrimLinedName.MakeName());
                return new
                {
                    Update =
                $@"            if (typeof(T) == typeof({singular}QueryModel))
                return new UpdateDataService<{singular}QueryModel, {singular}>();",
                    Fetch =
                $@"            if (typeof(T) == typeof({singular}QueryModel))
                return new FetchDataService<{singular}QueryModel>();"
                };
            });

            var apiService = $@"using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Data.Services;
using Pilgrims.Projects.Assistant.Contracts.Data.UpdateServices;
using System;

namespace Pilgrims.Projects.Assistant.Contracts.Data
{{
    public static class ApiServiceCreator
    {{
        public static dynamic GetFetcher(Type type)
        {{
{string.Join("\r\n", condtions.Select(x => x.Fetch))}

            throw new NotImplementedException($""Api query for {{ typeof(T).Name}} is not implimented"");
        }}

        public static dynamic GetUpdator<T>(Type type)
        {{
{string.Join("\r\n", condtions.Select(x => x.Update))}

            throw new NotImplementedException($""Api query for {{typeof(T).Name}} is not implimented"");
        }}
    }}
}}
";
            var path = Path.Combine(Defaults.MainPath, "MagicOnionClasses");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, $"ApiServiceCreator.cs"), apiService);
        }

        private static void GenerateImplementationSeveral()
        {

            using var db = new Data.Context();
            var tables = db.Tables.Include(x => x.Columns).ToArray();
            foreach (var table in tables)
            {
                var name = table.StrimLinedName.MakeName();
                var singular = Functions.Singularize(table.StrimLinedName.MakeName());

                var sbUpdate = new StringBuilder($@"using AutoMapper;
using MagicOnion;
using MagicOnion.Server;
using Pilgrims.Projects.Assistant.Contracts.Data.Contracts;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Helpers;
using Pilgrims.Projects.Assistant.Contracts.Models;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
using Pilgrims.Projects.Assistant.DataLayer.Repositories;
using System;
using Pilgrims.Projects.Assistant.Contracts.Data.UpdateServices;
using System.Collections.Generic;
using System.Linq;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.UpdateServices
{{ 
    [MagicOnion.Server.Authentication.Authorize]
    class Update{name}Service : ServiceBase<IUpdate{name}Service>, IUpdate{name}Service
    {{");

                var sbFetch = new StringBuilder($@"using MagicOnion;
using MagicOnion.Server;
using Newtonsoft.Json;
using Pilgrims.Projects.Assistant.Contracts.Data;
using Pilgrims.Projects.Assistant.Contracts.Data.Contracts;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Helpers;
using Pilgrims.Projects.Assistant.DataLayer.Data.Classes;
using System;
using System.Collections.Generic;
using Pilgrims.Projects.Assistant.Contracts.Data.Services;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Pilgrims.Projects.Assistant.DataLayer.Data.Services
{{
    [MagicOnion.Server.Authentication.Authorize]
    class Fetch{name}Service : ServiceBase<IFetch{name}Service>, IFetch{name}Service
    {{");

                var obj = $@"
      
        public async UnaryResult<DeleteResults> DeleteAsync(string[] ids) => await new UpdateDataService<{singular}QueryModel, {singular}>().DeleteAsync(ids, Context);
        public async UnaryResult<DeleteResults> DeleteByParamAsync(PageListParams param) => await new UpdateDataService<{singular}QueryModel, {singular}>().DeleteByParamAsync(param, Context);
        public async UnaryResult<SaveResults> InsertAsync({singular}QueryModel[] objs) => await new UpdateDataService<{singular}QueryModel, {singular}>().InsertAsync(objs, Context);

        public async UnaryResult<SaveResults> UpdateByParamAsync(Dictionary<string, object> objs, PageListParams param) => await new UpdateDataService<{singular}QueryModel, {singular}>().UpdateByParamAsync(objs, param, Context);

        public async UnaryResult<SaveResults> UpdateByIdsAsync(Dictionary<string, {singular}QueryModel> objs)
        => await new UpdateDataService<{singular}QueryModel, {singular}>().UpdateByIdsAsync(objs, Context);

        public async UnaryResult<SaveResults> UpdateAsync(Dictionary<string, Dictionary<string, object>> objs)
         => await new UpdateDataService<{singular}QueryModel, {singular}>().UpdateAsync(objs, Context);

        public async UnaryResult<SaveResults> UpsertAsync({singular}QueryModel[] objs) => await new UpdateDataService<{singular}QueryModel, {singular}>().UpsertAsync(objs, Context);
        ";
                sbUpdate.AppendLine(obj);


                obj = $@"
        #region One {table.OriginalName}
        public async UnaryResult<{singular}QueryModel> GetSingleAsync(PageListParams param)
            => await new FetchDataService<{singular}QueryModel>().GetSingleAsync(param, Context);
        public async UnaryResult<{singular}QueryModel> GetSingleByIdAsync(string id) => await new FetchDataService<{singular}QueryModel>().GetSingleByIdAsync(id, Context);
        #endregion";
                sbFetch.AppendLine(obj);


                obj = $@"
        #region {table.OriginalName}
       public async UnaryResult<List<{singular}QueryModel>> GetAsync() => await new FetchDataService<{singular}QueryModel>().GetAsync(Context);

        public async UnaryResult<List<{singular}QueryModel>> GetByIdsAsync(params string[] ids) => await new FetchDataService<{singular}QueryModel>().GetByIdsAsync(ids, Context);

        public async UnaryResult<List<{singular}QueryModel>> GetByParamAsync(PageListParams param) => await new FetchDataService<{singular}QueryModel>().GetByParamAsync(param, Context);

        public async UnaryResult<object> GetTableAsync(PageListParams param) => await new FetchDataService<{singular}QueryModel>().GetTableAsync(param, Context);
        #endregion";
                sbFetch.AppendLine(obj);
                var path = Path.Combine(Defaults.MainPath, "MagicOnionClasses", "Several", "Updates");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                File.WriteAllText(Path.Combine(path, $"Update{name}Service.cs"), sbUpdate.ToString() + @"    }
}");
                path = Path.Combine(Defaults.MainPath, "MagicOnionClasses", "Several", "Fetches");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                File.WriteAllText(Path.Combine(path, $"Fetch{name}Service.cs"), sbFetch.ToString() + @"    }
}");
            }


        }

        private static void GenerateInterfacesSeveral()
        {
            using var db = new Data.Context();
            var tables = db.Tables.Include(x => x.Columns).ToArray();
            foreach (var table in tables)
            {
                var name = table.StrimLinedName.MakeName();
                var singular = Functions.Singularize(table.StrimLinedName.MakeName());
                var sbUpdate = new StringBuilder($@"using MagicOnion;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Helpers;
using Pilgrims.Projects.Assistant.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilgrims.Projects.Assistant.Contracts.Data.UpdateServices
{{
    public interface IUpdate{name}Service : IService<IUpdate{name}Service>
    {{");

                var sbFetch = new StringBuilder($@"using MagicOnion;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pilgrims.Projects.Assistant.Contracts.Data.Services
{{
    public interface IFetch{name}Service : IService<IFetch{name}Service>
    {{");
                var obj = $@"
        #region {table.OriginalName}
        UnaryResult<DeleteResults> DeleteAsync(string[] ids);
        UnaryResult<DeleteResults> DeleteByParamAsync(PageListParams param);
        UnaryResult<SaveResults> InsertAsync({singular}QueryModel[] objs);
        UnaryResult<SaveResults> UpdateByIdsAsync(Dictionary<string, {singular}QueryModel> objs);
        UnaryResult<SaveResults> UpsertAsync({singular}QueryModel[] objs);
        UnaryResult<SaveResults> UpdateByParamAsync(Dictionary<string, object> objs, PageListParams param);
        UnaryResult<SaveResults> UpdateAsync(Dictionary<string, Dictionary<string, object>> objs);
        #endregion";
                sbUpdate.AppendLine(obj);


                obj = $@"
        #region One {table.OriginalName}
        UnaryResult<{singular}QueryModel> GetSingleByIdAsync(string id);
        UnaryResult<{singular}QueryModel> GetSingleAsync(PageListParams param);
        #endregion";
                sbFetch.AppendLine(obj);


                obj = $@"
        #region {table.OriginalName}
        UnaryResult<List<{singular}QueryModel>> GetAsync();
        UnaryResult<List<{singular}QueryModel>> GetByIdsAsync(params string[] id);
        UnaryResult<List<{singular}QueryModel>> GetByParamAsync(PageListParams param);
        UnaryResult<object> GetTableAsync(PageListParams param);
        #endregion";
                sbFetch.AppendLine(obj);

                var path = Path.Combine(Defaults.MainPath, "MagicOnionClasses", "SeveralServices", "UpdateServices");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                File.WriteAllText(Path.Combine(path, $"IUpdate{name}Service.cs"), sbUpdate.ToString() + @"    }
}");
                path = Path.Combine(Defaults.MainPath, "MagicOnionClasses", "SeveralServices", "Services");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                File.WriteAllText(Path.Combine(path, $"IFetch{name}Service.cs"), sbFetch.ToString() + @"    }
}");
            }

        }
        private static void GenerateImplementation()
        {
            var sbUpdate = new StringBuilder(@"using AutoMapper;
using MagicOnion;
using MagicOnion.Server;
using Pilgrims.Projects.Assistant.Contracts.Data.Contracts;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Helpers;
using Pilgrims.Projects.Assistant.Contracts.Models;
using Pilgrims.Projects.Assistant.DataLayer.Data.Models;
using Pilgrims.Projects.Assistant.DataLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pilgrims.Projects.Assistant.DataLayer.Services
{
    class UpdateDataService : ServiceBase<IUpdateDataService>, IUpdateDataService
    {");

            var sbFetch = new StringBuilder(@"using MagicOnion;
using MagicOnion.Server;
using Newtonsoft.Json;
using Pilgrims.Projects.Assistant.Contracts.Data;
using Pilgrims.Projects.Assistant.Contracts.Data.Contracts;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Helpers;
using Pilgrims.Projects.Assistant.DataLayer.Data.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Pilgrims.Projects.Assistant.DataLayer.Services
{
    class FetchDataService : ServiceBase<IFetchDataService>, IFetchDataService
    {");

            var sbFetchOne = new StringBuilder(@"using MagicOnion;
using MagicOnion.Server;
using Newtonsoft.Json;
using Pilgrims.Projects.Assistant.Contracts.Data;
using Pilgrims.Projects.Assistant.Contracts.Data.Contracts;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Helpers;
using Pilgrims.Projects.Assistant.DataLayer.Data.Classes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Pilgrims.Projects.Assistant.DataLayer.Services
{
    class FetchDataOneService : ServiceBase<IFetchDataOneService>, IFetchDataOneService
    {");
            using var db = new Data.Context();
            var tables = db.Tables.Include(x => x.Columns).ToArray();
            foreach (var table in tables)
            {
                var name = table.StrimLinedName.MakeName();
                var singular = Functions.Singularize(table.StrimLinedName.MakeName());
                var obj = $@"
       #region {table.OriginalName}
       public async UnaryResult<DeleteResults> Delete{name}Async(string[] ids)
        {{
            try
            {{
                return await DataContextRepository<{singular}>.Delete(ids, Context);
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}

        public async UnaryResult<SaveResults> Insert{name}Async({singular}QueryModel[] objs)
        {{
            try
            {{
                var data = new MapperConfiguration(cfg =>
                cfg.CreateMap<{singular}QueryModel, {singular}>())
                .CreateMapper().Map<{singular}QueryModel[], {singular}[]>(objs);
                return await DataContextRepository<{singular}>.Insert(data, Context);
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}

        public async UnaryResult<SaveResults> Update{name}ByParamAsync(Dictionary<string, object> objs, PageListParams param)
        {{
            try
            {{
                return await new DataContextRepository<{singular}>().Update(objs, param, Context);
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}

        public async UnaryResult<SaveResults> Update{name}ByIdsAsync(Dictionary<string, {singular}QueryModel> objs)
        {{
            try
            {{
                var mapper = new MapperConfiguration(cfg =>
                  cfg.CreateMap<{singular}QueryModel, {singular}>())
                  .CreateMapper();

                var data = objs.ToDictionary(x => x.Key,
                    y => mapper.Map<{singular}QueryModel, {singular}>(y.Value));
                return await DataContextRepository<{singular}>.Update(data, Context);
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}

        public async UnaryResult<SaveResults> Update{name}Async(Dictionary<string, Dictionary<string, object>> objs)
        {{
            try
            {{
                return await DataContextRepository<{singular}>.Update(objs, Context);
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}

        public async UnaryResult<SaveResults> Upsert{name}Async({singular}QueryModel[] objs)
        {{
            try
            {{
                var data = new MapperConfiguration(cfg =>
                  cfg.CreateMap<{singular}QueryModel, {singular}>())
                  .CreateMapper().Map<{singular}QueryModel[], {singular}[]>(objs);
                return await DataContextRepository<{singular}>.Upsert(data, Context);
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}
        #endregion";
                sbUpdate.AppendLine(obj);


                obj = $@"
        #region {table.OriginalName}
        public async UnaryResult<{singular}QueryModel> {singular}QueryModelAsync(PageListParams param)
        {{
            try
            {{
                var query = ModelQueries
                    .GetObjects<{singular}QueryModel>(param, Context);
                return await Task.FromResult(query.FirstOrDefault());
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}

        public async UnaryResult<{singular}QueryModel> {singular}QueryModelByIdAsync(string id)
        {{
            try
            {{
                var query = ModelQueries
                    .GetObjects<{singular}QueryModel>(context)
                    .Where(x => id == x.Id);
                return await Task.FromResult(query.FirstOrDefault());
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}
        #endregion";
                sbFetchOne.AppendLine(obj);


                obj = $@"
        #region {table.OriginalName}
       public async UnaryResult<List<{singular}QueryModel>> Get{name}Async()
        {{
            try
            {{
                return await Task.FromResult(ModelQueries.GetObjects<{singular}QueryModel>(Context).ToList());
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}

        public async UnaryResult<List<{singular}QueryModel>> Get{name}ByIdsAsync(params string[] ids)
        {{
            try
            {{
                var query = ModelQueries
                    .GetObjects<{singular}QueryModel>(Context)
                    .Where(x => ids.Contains(x.Id));
                return await Task.FromResult(query.ToList());
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}

        public async UnaryResult<List<{singular}QueryModel>> Get{name}ByParamAsync(PageListParams param)
        {{
            try
            {{
                var query = ModelQueries
                    .GetObjects<{singular}QueryModel>(param, Context);
                return await Task.FromResult(query.ToList());
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}

        public async UnaryResult<object> Get{name}TableAsync(PageListParams param)
        {{
            try
            {{
                using var table = await ModelQueries
                    .GetTable<{singular}QueryModel>(param, Context);
                return JsonConvert.SerializeObject(table);
            }}
            catch (Exception ex)
            {{
                ex = ex.InnerError().GetRpcException();
                throw ex;
            }}
        }}
        #endregion";
                sbFetch.AppendLine(obj);
            }

            var path = Path.Combine(Defaults.MainPath, "MagicOnionClasses");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "UpdateDataService.cs"), sbUpdate.ToString() + @"    }
}");
            File.WriteAllText(Path.Combine(path, "FetchDataService.cs"), sbFetch.ToString() + @"    }
}");
            File.WriteAllText(Path.Combine(path, "FetchDataOneService.cs"), sbFetchOne.ToString() + @"    }
}");
        }

        private static void GenerateInterfaces()
        {
            var sbUpdate = new StringBuilder(@"using MagicOnion;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Helpers;
using Pilgrims.Projects.Assistant.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilgrims.Projects.Assistant.Contracts.Data.Contracts
{
    public interface IUpdateDataService : IService<IUpdateDataService>
    {");

            var sbFetch = new StringBuilder(@"using MagicOnion;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Pilgrims.Projects.Assistant.Contracts.Data.Contracts
{
    public interface IFetchDataService : IService<IFetchDataService>
    {");

            var sbFetchOne = new StringBuilder(@"using MagicOnion;
using Pilgrims.Projects.Assistant.Contracts.Data.DataModels;
using Pilgrims.Projects.Assistant.Contracts.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pilgrims.Projects.Assistant.Contracts.Data.Contracts
{
    public interface IFetchDataOneService : IService<IFetchDataOneService>
    {");
            using var db = new Data.Context();
            var tables = db.Tables.Include(x => x.Columns).ToArray();
            foreach (var table in tables)
            {
                var name = table.StrimLinedName.MakeName();
                var singular = Functions.Singularize(table.StrimLinedName.MakeName());
                var obj = $@"
        #region {table.OriginalName}
        UnaryResult<DeleteResults> Delete{name}Async(string[] ids);
        UnaryResult<SaveResults> Insert{name}Async({singular}QueryModel[] objs);
        UnaryResult<SaveResults> Update{name}ByIdsAsync(Dictionary<string, {singular}QueryModel> objs);
        UnaryResult<SaveResults> Upsert{name}Async({singular}QueryModel[] objs);
        UnaryResult<SaveResults> Update{name}ByParamAsync(Dictionary<string, object> objs, PageListParams param);
        UnaryResult<SaveResults> Update{name}Async(Dictionary<string, Dictionary<string, object>> objs);
        #endregion";
                sbUpdate.AppendLine(obj);


                obj = $@"
        #region {table.OriginalName}
        UnaryResult<{singular}QueryModel> {singular}QueryModelByIdAsync(string id);
        UnaryResult<{singular}QueryModel> {singular}QueryModelAsync(PageListParams param);
        #endregion";
                sbFetchOne.AppendLine(obj);


                obj = $@"
        #region {table.OriginalName}
        UnaryResult<List<{singular}QueryModel>> Get{name}Async();
        UnaryResult<List<{singular}QueryModel>> Get{name}ByIdsAsync(params string[] id);
        UnaryResult<List<{singular}QueryModel>> Get{name}ByParamAsync(PageListParams param);
        UnaryResult<object> Get{name}TableAsync(PageListParams param);
        #endregion";
                sbFetch.AppendLine(obj);
            }

            var path = Path.Combine(Defaults.MainPath, "MagicOnionClasses");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            File.WriteAllText(Path.Combine(path, "IUpdateDataService.cs"), sbUpdate.ToString() + @"    }
}");
            File.WriteAllText(Path.Combine(path, "IFetchDataService.cs"), sbFetch.ToString() + @"    }
}");
            File.WriteAllText(Path.Combine(path, "IFetchDataOneService.cs"), sbFetchOne.ToString() + @"    }
}");

        }
    }
}
