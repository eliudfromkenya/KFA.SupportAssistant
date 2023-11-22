//using Pilgrims.Projects.Assistant.Contracts.DataLayer;

//namespace Pilgrims.Projects.Assistant;

//  public static class DataLayerFactory
//  {
//      static readonly Dictionary<Type, IIdGenerator> idGenerators = new();
//      static IIdGenerator idGenerator;
//      public static T CreateDataModel<T>(bool initializeId = false) where T : IBaseModel
//      {
//          var obj = Declarations.UnityContainer.Resolve<T>();
//          if (initializeId)
//          {
//              if (idGenerator == null)
//                  idGenerator = CreateIdGenerator();
//              obj.Id = idGenerator.GetNextId<T>();
//          }
//          return obj;
//      }

//      public static IEncryptor GetEncryptor()
//      {
//          return Declarations.UnityContainer.Resolve<IEncryptor>();
//      }

//      public static IDataRepository<T> CreateDataRepository<T>() where T : IBaseModel
//      {
//          return Declarations.UnityContainer.Resolve<IDataRepository<T>>();
//      }

//      public static IIdGenerator CreateIdGenerator()
//      {
//          return Declarations.UnityContainer.Resolve<IIdGenerator>();
//      }

//      public static IControlMakerService CreateControlMakerService(IProjectSolutionModel model)
//      {
//          var obj = Declarations.UnityContainer.Resolve<IControlMakerService>();
//          obj.SetMainSolution(model);
//          return obj;
//      }
//  }
