# gRPC

This repository demonstrates synchronous gRPC communication between two C# .NET Worker projects.

## Overview

The solution contains two projects that communicate using gRPC:

1. **GrpcServer**: A .NET Web application that hosts a gRPC service
2. **GrpcClient**: A .NET Worker service that calls the gRPC server

## Architecture

```
GrpcClient (Worker Service) --> gRPC Call --> GrpcServer (Web Service)
```

### GrpcServer
- Hosts a gRPC service on `http://localhost:5000`
- Implements the `Greeter` service defined in `greeter.proto`
- Receives `SayHello` requests and returns greeting messages with timestamps
- Uses ASP.NET Core with Kestrel configured for HTTP/2 (required for gRPC)

### GrpcClient
- Background worker service that periodically calls the gRPC server
- Makes a `SayHello` request every 5 seconds
- Logs the response from the server
- Demonstrates error handling for gRPC communication

## Prerequisites

- .NET 8.0 SDK or later
- Terminal or command prompt

## Getting Started

### Build the Solution

```bash
dotnet restore
dotnet build
```

### Running the Applications

You need to run both applications simultaneously. Open two terminal windows:

**Terminal 1 - Start the gRPC Server:**
```bash
cd src/GrpcServer
dotnet run
```

You should see output indicating the server is listening:
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
```

**Terminal 2 - Start the gRPC Client:**
```bash
cd src/GrpcClient
dotnet run
```

You should see the client making requests and logging responses:
```
info: GrpcClient.Worker[0]
      Greeting response: Hello gRPC Client Worker! Current time is ...
```

### Expected Behavior

- **Server**: Logs each incoming gRPC request with the client's name
- **Client**: Makes requests every 5 seconds and logs the greeting response from the server
- The communication uses HTTP/2 protocol, which is required for gRPC

## Project Structure

```
gRPC/
├── GrpcWorkers.sln                  # Solution file
├── src/
│   ├── GrpcServer/
│   │   ├── Protos/
│   │   │   └── greeter.proto        # gRPC service definition
│   │   ├── GreeterService.cs        # gRPC service implementation
│   │   ├── Program.cs               # Server startup and configuration
│   │   ├── appsettings.json         # Server configuration
│   │   └── GrpcServer.csproj        # Server project file
│   └── GrpcClient/
│       ├── Protos/
│       │   └── greeter.proto        # gRPC service definition (client copy)
│       ├── Worker.cs                # Background worker with gRPC client
│       ├── Program.cs               # Client startup and configuration
│       ├── appsettings.json         # Client configuration
│       └── GrpcClient.csproj        # Client project file
└── README.md
```

## Key Technologies

- **gRPC**: High-performance RPC framework
- **Protocol Buffers**: Language-neutral data serialization
- **ASP.NET Core**: Web framework for hosting the gRPC server
- **Worker Services**: Background service framework for the client
- **HTTP/2**: Required protocol for gRPC communication

## Configuration

### Server Configuration (appsettings.json)
- Kestrel is configured to use HTTP/2 protocol
- Server listens on `http://localhost:5000`

### Client Configuration (appsettings.json)
- Server address is configurable via `GrpcServer:Address`
- Default: `http://localhost:5000`

## Learn More

- [gRPC on .NET](https://docs.microsoft.com/aspnet/core/grpc/)
- [Protocol Buffers](https://developers.google.com/protocol-buffers)
- [Worker Services in .NET](https://docs.microsoft.com/dotnet/core/extensions/workers)
