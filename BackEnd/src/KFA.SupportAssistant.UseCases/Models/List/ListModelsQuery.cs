﻿using Ardalis.Result;
using Ardalis.SharedKernel;
using KFA.SupportAssistant.Globals;
using KFA.SupportAssistant.UseCases.ModelCommandsAndQueries;

namespace KFA.SupportAssistant.UseCases.Models.List;

public record ListModelsQuery<T, X>(ListParam param) : IQuery<Result<List<T>>> where T : BaseDTO<X>, new() where X : BaseModel, new();
