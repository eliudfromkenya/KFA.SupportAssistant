using Autofac;
using KFA.SupportAssistant.Core.Interfaces;
using KFA.SupportAssistant.Core.Models;
using KFA.SupportAssistant.Core.Services;
using KFA.SupportAssistant.Globals;

namespace KFA.SupportAssistant.Core;

/// <summary>
/// An Autofac module that is responsible for wiring up services defined in the Core project.
/// </summary>
public class DefaultCoreModule : Module
{
  protected override void Load(ContainerBuilder builder)
  {
    builder.RegisterType<DeleteContributorService>()
        .As<IDeleteContributorService>().InstancePerLifetimeScope();

    //builder.RegisterType(typeof(DeleteModelService<>))
    //   .As(typeof(IDeleteModelService<>)).InstancePerLifetimeScope();

    //builder.RegisterType(typeof(InsertModelService<>))
    //   .As(typeof(IInsertModelService<>)).InstancePerLifetimeScope();

    //builder.RegisterType(typeof(UpdateModelService<>))
    //   .As(typeof(IUpdateModelService<>)).InstancePerLifetimeScope();

    RegisterDeleteService(builder);
  }

  private void RegisterDeleteService(ContainerBuilder builder)
  {
    var classes = new[]
    {
       System.Reflection.Assembly.GetAssembly(typeof(BaseModel)),
       System.Reflection.Assembly.GetAssembly(typeof(CostCentre))
         } /*AppDomain.CurrentDomain.GetAssemblies()*/
        .SelectMany(s => s?.GetTypes() ?? Array.Empty<Type>())
    .Where(typeof(BaseModel).IsAssignableFrom)
        .Where(c => c != typeof(BaseModel)).ToList();

    var typeIA = typeof(IDeleteModelService<>);
    var typeIB = typeof(IInsertModelService<>);
    var typeIC = typeof(IUpdateModelService<>);
    var typeA = typeof(DeleteModelService<>);
    var typeB = typeof(InsertModelService<>);
    var typeC = typeof(UpdateModelService<>);

    classes.ToList()
   .ForEach(type =>
   {
     if (type != null)
     {
       try
       {
         var genericTypeA = typeA.MakeGenericType(type);
         var genericTypeIA = typeIA.MakeGenericType(type);
         var genericTypeB = typeB.MakeGenericType(type);
         var genericTypeIB = typeIB.MakeGenericType(type);
         var genericTypeC = typeC.MakeGenericType(type);
         var genericTypeIC = typeIC.MakeGenericType(type);

         builder.RegisterType(genericTypeA)
            .As(genericTypeIA)
            .InstancePerLifetimeScope();
         builder.RegisterType(genericTypeB)
            .As(genericTypeIB)
            .InstancePerLifetimeScope();
         builder.RegisterType(genericTypeC)
            .As(genericTypeIC)
            .InstancePerLifetimeScope();
       }
       catch { }
     }
   });
  }
}
