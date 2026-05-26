# 📨 EdiSharp

**EdiSharp** is a small, modular .NET library and desktop UI for tokenizing and processing EDI messages (EDIFACT, X12). The solution is split into `EdiSharp.Domain`, `EdiSharp.Core` and `EdiSharp.UI` (Avalonia) so parsing logic can be reused in services, tools, or a UI viewer.

## Features

### 🔧 Core (Library)
- Tokenization primitives for EDI formats (EDIFACT implemented; X12 scaffolded)
- `IEdiTokenizer` abstraction and tokenizer factory for easy extension
- `EdiProcessingService` to coordinate tokenization and higher-level processing
- Low-level delimiter detection and segment parsing helpers

### 🖥️ Desktop UI
- Avalonia-based viewer built in `EdiSharp.UI`
- MVVM structure using `CommunityToolkit.Mvvm`
- Simple file open / view workflow for EDI documents
- Lightweight demo / inspector for tokens and segments

### ⚙️ Integration
- Designed to be used as a library from other apps or services
- DI-friendly: `Microsoft.Extensions.DependencyInjection` is referenced in projects

## 🧰 Technologies
- .NET 10 / C#
- Avalonia (desktop UI)
- CommunityToolkit.Mvvm
- Microsoft.Extensions.DependencyInjection

## 🏗️ Project Structure
- `src\EdiSharp.Domain` — domain models and core abstractions
- `src\EdiSharp.Core` — tokenizers, processing services, parsing logic
- `src\EdiSharp.UI` — Avalonia desktop application and ViewModels

This separation keeps parsing logic independent of UI concerns and makes it easier to write tests or reuse the core library.

## 🚀 Quick Start

1. Clone the repository
   - `git clone https://github.com/TheCrudClapper/EdiSharp.git`
   - `cd EdiSharp`

2. Prerequisites
   - .NET 10 SDK installed
   - (Optional) Visual Studio 2022/2026 or Rider for opening the solution

3. Build and run
   - Open `EdiSharp.slnx` in your IDE
   - Set `EdiSharp.UI` as the startup project
   - Build and run the solution

4. Run the UI
   - Use the file menu in the desktop app to open EDI files and inspect tokenization

## 🗺️ Roadmap / Improvements
1. Complete `X12Tokenizer` implementation and add comprehensive unit tests
2. Add Benchmark.NET benchmarks for `Tokenize` to measure allocations and throughput
3. Improve span-based parsing to reduce allocations for large EDI payloads
4. Add sample EDI files and a demo "inspect & replay" feature in the UI
5. Package `EdiSharp.Core` as a reusable NuGet package

## 🧪 Testing & Benchmarking
- No benchmarks included yet — planned to add Benchmark.NET tests to measure `Tokenize` performance.
- Add unit tests around `IEdiTokenizer` implementations to validate delimiter detection and segment parsing.

## 🔗 Useful Links
- [.NET](https://learn.microsoft.com/dotnet/)
- [Avalonia UI](https://avaloniaui.net)
- [CommunityToolkit.MVVM](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm)
- [Microsoft.Extensions.DependencyInjection](https://learn.microsoft.com/dotnet/core/extensions/dependency-injection)
