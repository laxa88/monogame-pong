# Monogame Pong example in VS Code

This is a Monogame "Hello World" project, structured VS code environment.

## Compilation

Refer to `.vscode` folder

## VS code plugins

- C# `v1.25.0`
- CSharpier `v1.2.4`
- Monogame Content Builder `v0.0.4`

Additionally, if OmniSharp doesn't make work, make sure the `dotnet` SDK is up-to-date. As of writing, it was `.NET 6.0 SDK (v6.0.302)`

## Settings

Additional `settings.json` config:

```json
{
  "editor.formatOnSave": true,
  "omnisharp.enableImportCompletion": true,
  "csharpier.enableDebugLogs": true,
  "[csharp]": {
    "editor.defaultFormatter": "csharpier.csharpier-vscode",
  }
}
```