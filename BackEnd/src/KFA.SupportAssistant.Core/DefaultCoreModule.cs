﻿using Autofac;
using KFA.SupportAssistant.Core.Interfaces;
using KFA.SupportAssistant.Core.Services;

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
  }
}
