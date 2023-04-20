﻿using FluentValidation;

namespace ToDosMinimalAPI.ToDo
{
    public static class ValidationExtention
    {
        //musi byc statyczna = zawiera metode rozszezajacą

        public static RouteHandlerBuilder WithValidator<T>(this RouteHandlerBuilder buidler)
            where T : class
        { // torche taki middleware dla konkretnego endpoitnu
            buidler.Add(endpointBuilder =>
            {
                var orginalDelegate = endpointBuilder.RequestDelegate;
                endpointBuilder.RequestDelegate = async httpContext =>
                {
                    var validator = httpContext.RequestServices.GetRequiredService<IValidator<T>>();
                    httpContext.Request.EnableBuffering();
                    var body = await httpContext.Request.ReadFromJsonAsync<T>();

                    if (body == null)
                    { 
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await httpContext.Response.WriteAsJsonAsync("Couldynt map body to request model ");
                        return;
                    }  

                    var validationResult = validator.Validate(body);
                    if (!validationResult.IsValid)
                    {
                        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                        await httpContext.Response.WriteAsJsonAsync(validationResult.Errors);
                        return;
                    }
                    httpContext.Request.Body.Position = 0;
                    await orginalDelegate(httpContext);
                };
            });

            return buidler;
        }

    }
}
