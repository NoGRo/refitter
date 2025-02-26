﻿using NSwag;
using NSwag.CodeGeneration.OperationNameGenerators;
using System.Collections.Generic;
using System.Linq;

namespace Refitter.Core;

public class OperationNameGenerator : IOperationNameGenerator
{
    private readonly IOperationNameGenerator defaultGenerator =
        new MultipleClientsFromOperationIdOperationNameGenerator();

    public OperationNameGenerator(OpenApiDocument document)
    {
        if (CheckForDuplicateOperationIds(document))
            defaultGenerator = new MultipleClientsFromFirstTagAndPathSegmentsOperationNameGenerator();
    }

    public bool SupportsMultipleClients => throw new System.NotImplementedException();

    public string GetClientName(OpenApiDocument document, string path, string httpMethod, OpenApiOperation operation)
    {
        return defaultGenerator.GetClientName(document, path, httpMethod, operation);
    }

    public string GetOperationName(
        OpenApiDocument document,
        string path,
        string httpMethod,
        OpenApiOperation operation) =>
        defaultGenerator
            .GetOperationName(document, path, httpMethod, operation)
            .CapitalizeFirstCharacter()
            .ConvertKebabCaseToPascalCase()
            .ConvertRouteToCamelCase();

    public bool CheckForDuplicateOperationIds(
        OpenApiDocument document)
    {
        List<string> operationNames = new();
        foreach (var kv in document.Paths)
        {
            foreach (var operations in kv.Value)
            {
                var operation = operations.Value;
                operationNames.Add(
                    GetOperationName(
                        document,
                        kv.Key,
                        operations.Key,
                        operation));
            }
        }

        return operationNames.Distinct().Count() != operationNames.Count;
    }
}
