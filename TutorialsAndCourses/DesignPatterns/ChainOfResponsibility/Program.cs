﻿using System.ComponentModel.DataAnnotations;
using ChainOfResponsibility;

var validDocument = new Document("How to Avoid Java Development", DateTimeOffset.UtcNow, true, true);
var invalidDocument = new Document("Head-first JavaScript", DateTimeOffset.UtcNow, true, false);

var documentHandlerChain = new DocumentTitleHandler();
documentHandlerChain
    .SetSuccessor(new DocumentLastModifiedHandler())
    .SetSuccessor(new DocumentApprovedByLitigationHandler())
    .SetSuccessor(new DocumentApprovedByManagementHandler());

try
{
    documentHandlerChain.Handle(validDocument);
    Console.WriteLine("Valid document is valid.");
    documentHandlerChain.Handle(invalidDocument);
    Console.WriteLine("Invalid document is valid.");
}
catch (ValidationException validationException)
{
    Console.WriteLine(validationException.Message);
}

Console.ReadKey();