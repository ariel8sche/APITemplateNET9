# Configuracion del Agente

## Reglas de codificación
- Usa C# 13
- Usa .NET 9
- Usa Entity Framework Core
- Usa PascalCase para nombrar clases, métodos y propiedades
- Utiliza LINQ con métodos encadenados

## Rendimiento y Optimización
- Aplica async/await correctamente en operaciones de E/S

## General
- Cada que me respondas algo en el chat dime "Si mi lider"

## Overview

Este documento describe la configuración, responsabilidades y buenas prácticas para las capas del proyecto: APITemplate.Contracts, APITemplate.Data, APITemplate.Services y APITemplate.Web.

## Descripción de capas

- APITemplate.Contracts
  - Contiene DTOs, contratos de transferencia y las interfaces públicas (por ejemplo interfaces de servicios) que definen los contratos entre capas.
  - Debe ser agnóstico a implementaciones y no depender de Entity Framework ni de infraestructura.

- APITemplate.Data
  - Contiene entidades de dominio, DbContext de Entity Framework Core, migraciones y repositorios de acceso a datos.
  - Responsable de la persistencia, configuraciones de modelos y mappings.
  - Recomendación: exponer solo interfaces o métodos asíncronos y mantener la lógica de negocio fuera de esta capa.

- APITemplate.Services
  - Implementa la lógica de negocio y orquesta llamadas entre repositorios y otras dependencias.
  - Implementa las interfaces definidas en Contracts.
  - Aquí se aplican validaciones, reglas de negocio y transformaciones entre entidades y DTOs.

- APITemplate.Web
  - Proyecto ASP.NET Core que expone controladores API, configuración de DI (Program.cs), middlewares y documentación (Swagger).
  - Consume los servicios definidos en Services y devuelve/recibe DTOs definidos en Contracts.

## Configuración y dependencias clave

- Uso de EF Core: la configuración del DbContext y la cadena de conexión se encuentra en APITemplate.Data y se registra desde APITemplate.Web en Program.cs.
- Migraciones: mantenerlas en el proyecto APITemplate.Data (o en un proyecto dedicado) y ejecutarlas desde la línea de comandos o CI/CD.
- Inyección de dependencias: registrar implementaciones de Services y DbContext en Program.cs del proyecto Web.

## Recomendaciones de uso

- Mantener Contracts libres de referencias a EF Core o infraestructuras.
- Hacer métodos asíncronos (async/await) en servicios y repositorios.
- Usar patrones de nombres PascalCase para clases, métodos y propiedades.
- Preferir consultas LINQ con métodos encadenados para consistencia.

## Ejemplo de responsabilidades (resumen)

- Flujo típico: Controller (Web) -> Service (Services) -> Repository/DbContext (Data) -> Database
- Mapping: usar mapeo manual ligero o una librería de mapeo (AutoMapper) dentro de Services para convertir entre entidades y DTOs.

## Notas

- Este archivo sirve como guía para el agente y para nuevos contribuidores. Mantenerlo actualizado cuando cambien responsabilidades o estructura del proyecto.
- Para fijar explícitamente la versión de C# usar la propiedad <LangVersion> en los csproj si se requiere forzar una versión del compilador.
