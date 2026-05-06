using System;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
// clase de extension para registrar los servicios de la capa de aplicacion en el contenedor de inyeccion de dependencias
{
    public static IServiceCollection AddAplicationServices(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ApplicationAssemblyReference).Assembly;
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(applicationAssembly));
        services.AddValidatorsFromAssembly(applicationAssembly);
        
        return services;
    }
}
// var applicationAssembly = typeof(ApplicationAssemblyReference).Assembly; = busca todas las clases dentro del proyecto para no tener que registrar una por una
//         services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
// registra todos los commands, handlers, etc de MediatR que se encuentran en el proyecto
// services.AddValidatorsFromAssembly(applicationAssembly); = registra todas las clases de validacion de FluentValidation que se encuentran en el proyecto