﻿using backend.BLL.Common.VMs.Email;
using backend.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace backend.API.Services;

public class RazorRenderService : IRazorRenderService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ITempDataProvider _tempDataProvider;
    private readonly IRazorViewEngine _viewEngine;

    public RazorRenderService(IRazorViewEngine viewEngine,
        ITempDataProvider tempDataProvider,
        IServiceProvider serviceProvider)
    {
        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
        _serviceProvider = serviceProvider;
    }

    public async Task<string> RenderEmailConfirmationAsync(ConfirmCodeVM model)
    {
        var path = Path.Combine("RazorTemplates", "EmailConfirmationCodeTemplate.cshtml");

        return await GenerateTemplateAsync(path, model);
    }

    private async Task<string> GenerateTemplateAsync<T>(string templateName, T model)
    {
        var actionContext = GetActionContext();
        var view = FindView(actionContext, templateName);

        using (var output = new StringWriter())
        {
            var viewContext = new ViewContext(
                actionContext,
                view,
                new ViewDataDictionary<T>(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                output,
                new HtmlHelperOptions()
            );

            await view.RenderAsync(viewContext);
            return output.ToString();
        }
    }

    private IView FindView(ActionContext actionContext, string viewName)
    {
        var getViewResult = _viewEngine.GetView(null, viewName, true);
        if (getViewResult.Success) return getViewResult.View;

        var findViewResult = _viewEngine.FindView(actionContext, viewName, true);
        if (findViewResult.Success) return findViewResult.View;

        var searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
        var errorMessage = string.Join(
            Environment.NewLine,
            new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(
                searchedLocations));
        ;

        throw new InvalidOperationException(errorMessage);
    }

    private ActionContext GetActionContext()
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = _serviceProvider
        };

        return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
    }
}