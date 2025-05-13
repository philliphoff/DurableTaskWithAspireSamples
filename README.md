# Durable Task with .NET Aspire Samples

A collection of samples that demonstrate use of the Durable Task Scheduler (emulator) with .NET Aspire-based Durable Task applications.

## Durable Task Framework (DTFx) SDK

The Durable Task Framework (DTFx) SDK was the original Durable Task SDK and used, for example, with in-process Azure Durable Functions applications but can also be used independently (as shown in this sample).

### Starting the Application

```bash
cd DTFx\DTFx.AppHost
dotnet run
```

## Durable Task (Portable) Framework SDK

The Durable Task (Portable) Framework SDK is a newer, more modern Durable Task SDK and used, for example, with isolated Azure Durable Functions applications but can also be used independently (as shown in this sample).

### Starting the Application

```bash
cd Portable\Portable.AppHost
dotnet run
```

## Azure Durable Functions

Durable Task applications can also be used with the latest versions of .NET Aspire (9.1 and newer).

### Starting the Application

```bash
cd DurableFunctions\DurableFunctions.AppHost
dotnet run
```
