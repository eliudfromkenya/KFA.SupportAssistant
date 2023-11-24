﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using Autofac;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.Infrastructure.Data.Queries;
using KFA.SupportAssistant.UseCases.Contributors.List;
using MediatR;
using KFA.SupportAssistant.UseCases.Models.Create;
using KFA.SupportAssistant.UseCases.DTOs;
using KFA.SupportAssistant.UseCases.Models.Delete;
using KFA.SupportAssistant.UseCases.Models.Update;
using KFA.SupportAssistant.UseCases.Models.Get;
using KFA.SupportAssistant.UseCases.Xs.Get;
using KFA.SupportAssistant.UseCases.Models.List;
using KFA.SupportAssistant.Globals.DataLayer;
using KFA.SupportAssistant.UseCases.Contributors.Create;
using KFA.SupportAssistant.Globals.Models;
using KFA.SupportAssistant.Infrastructure.Models;
using KFA.SupportAssistant.Infrastructure.Data;

namespace KFA.SupportAssistant.Infrastructure;
internal static class RegisterEntities
{
  public static void RegisterQueries(ContainerBuilder builder )
  {
    //builder.RegisterType<ListContributorsQueryService>()
    //  .As<IListContributorsQueryService>()
    //  .InstancePerLifetimeScope();

    var classes = new[]
    {
       System.Reflection.Assembly.GetAssembly(typeof(BaseModel)),
       System.Reflection.Assembly.GetAssembly(typeof(CostCentre))
         } /*AppDomain.CurrentDomain.GetAssemblies()*/
        .SelectMany(s => s?.GetTypes() ?? [])
    .Where(typeof(BaseModel).IsAssignableFrom)
        .Where(c => c != typeof(BaseModel)).ToList();

    RegisterCreateModels(builder, classes);
    RegisterDeleteModels(builder, classes);
    RegisterUpdateModels(builder, classes);
    RegisterByIdModels(builder, classes);
    RegisterByIdsModels(builder, classes);
    RegisterListsModels(builder, classes);
  }

  private static void RegisterDataServices(ContainerBuilder builder)
  {
    builder.RegisterType<UserLoginHandler>()
           .As<IRequestHandler<UserLoginCommand, Result<LoginResult>>>()
           .InstancePerLifetimeScope();
    builder.RegisterType<IdGenerator>()
           .As<IIdGenerator>()
           .InstancePerLifetimeScope();
  }

  private static void RegisterListsModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(ListModelsQuery<,>);
        var listType = typeof(IList<>).MakeGenericType(dtoType);
        var resultType = typeof(Result<>);// .MakeGenericType(listType);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(listType);
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(ListModelsHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([dtoType, type]);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }

  private static void RegisterByIdModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(GetModelQuery<,>);
        var resultType = typeof(Result<>);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(dtoType);
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(GetModelHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([dtoType, type]);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }
  private static void RegisterByIdsModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(GetModelsByIdsQuery<,>);
        var resultType = typeof(Result<>);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(dtoType.MakeArrayType());
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(GetModelsByIdsHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([dtoType, type]);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch(Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }

  private static void RegisterUpdateModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(UpdateModelCommand<,>);
        var resultType = typeof(Result<>);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(dtoType);
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(UpdateModelHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([dtoType, type]);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }

  private static void RegisterDeleteModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(DeleteModelCommand<>);
        var resultType = typeof(Result);
        var genericCommandType = modelCommandType.MakeGenericType(type);
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, resultType);

        Type genericHandlerType = typeof(DeleteModelHandler<>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([type]);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }

  private static void RegisterCreateModels(ContainerBuilder builder, List<Type> allDTOTypes)
  {
    allDTOTypes.ForEach(type =>
    {
      try
      {
        var dtoAssemblyName = typeof(CostCentreDTO).Assembly.GetName()?.Name;
        var dtoType = Type.GetType($"{dtoAssemblyName}.DTOs.{type.Name}DTO, {dtoAssemblyName}")!;

        var requestHandlerType = typeof(IRequestHandler<,>);
        var modelCommandType = typeof(CreateModelCommand<,>);
        var resultType = typeof(Result<>);
        var genericCommandType = modelCommandType.MakeGenericType(dtoType, type);
        var genericResultType = resultType.MakeGenericType(dtoType.MakeArrayType());
        var constructedRequestHandlerType = requestHandlerType.MakeGenericType(genericCommandType, genericResultType);

        Type genericHandlerType = typeof(CreateModelHandler<,>);
        Type constructedHandlerType = genericHandlerType.MakeGenericType([dtoType, type]);

        builder.RegisterType(constructedHandlerType)
          .As(constructedRequestHandlerType)
          .InstancePerLifetimeScope();
      }
      catch (Exception ex)
      {
        var dd = ex.ToString();
      }
    });
  }
}