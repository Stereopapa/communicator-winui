# Real-Time Asynchronous Communicator

A robust, full-stack messaging system utilizing a decoupled architecture. The solution comprises a high-concurrency Python backend and a modern WinUI 3 desktop client built on .NET 8.

---

## System Architecture
The project is engineered with a strict separation of concerns, facilitating scalability and maintainability.

### 1. Backend: Python Async Server
* **Concurrency Model:** Powered by `asyncio` and `websockets` for non-blocking I/O operations.
* **Resilience:** Implements a `@safe_dispatch` decorator to handle connection lifecycles, ensuring graceful cleanup of disconnected clients from the internal registry.
* **Broadcasting:** Utilizes `asyncio.gather` for efficient, parallel message distribution to all active sessions.
* **Data Persistence:** Lightweight state management via `logs.json`, featuring automated file initialization and synchronization.

### 2. Frontend: WinUI 3 Client
The client is architected into two distinct projects:

#### **Client.Core (Business Logic Layer)**
* **WebSocket Engine:** Built on `ClientWebSocket` with full support for asynchronous streams and cancellation tokens.
* **Modern MVVM:** Leverages `CommunityToolkit.Mvvm` with Source Generators. Use of `[ObservableProperty]` and `[RelayCommand]` significantly reduces boilerplate code.
* **Performance:** Uses **Source-Generated JSON Serialization** (`JsonSerializerContext`) to eliminate reflection overhead, ensuring a low memory footprint and AOT (Ahead-of-Time) compatibility.
* **Dependency Injection:** Services like `IServerConnectionService` and `IUserService` are registered within an `IServiceProvider` for decoupled dependency management.

#### **Client.WinUi (Presentation Layer)**
* **Reactive UI:** Bindings use `x:Bind` for compile-time validation and performance.
* **View Navigation:** Implements `NavigationView` with `Frame` content switching between `ConnectPage` and `MainPage`.
* **UI Dispatching:** Implements `IMainThreadDispatcher` to bridge background network events with the UI thread safely.
* **Advanced XAML Styling:** Custom `DataTemplates` and `Converters` handle dynamic message alignment (Left/Right) based on message ownership (`IsMine` logic).

---

## Technical Stack
* **Backend:** Python 3.10+, `websockets`, `asyncio`.
* **Frontend:** C# 13, .NET 8, WinUI 3 (Windows App SDK).
* **Patterns:** MVVM, Dependency Injection, Singleton, Observer, Proxy (Dispatcher).

---

## Execution & Build Instructions

### Server Deployment
#### 1. Server Deployment
1. **Location:** Navigate to the `/Server` directory.
2. **Entry Point:** Execute `main.py`.
* **Command:**
  ```bash
   python main.py
**Network Config:** The server defaults to ws://127.0.0.1:8765
#### 2. Client Deployment
1. **Location:** Open Solution: Load Communicator.sln.
2. **Entry Point:** Configuration: Set Build Configuration to Release and Platform to x64.
3. **Build:** Run Build Solution (Ctrl+Shift+B).
4. **Binary Output:**
  The executable and its dependencies are generated in:
bin\x64\Release\net8.0-windows10.0.19041.0\win-x64\

**Pro Tip** For testing real-time synchronization, launch multiple instances of the generated .exe directly from the binary path to observe multi-user interaction and message broadcasting.

**Note** The application is designed for Windows environments and should be built using Visual Studio 2022:
