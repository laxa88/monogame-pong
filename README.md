# Monogame Pong example in VS Code

This is a Monogame "Hello World" project, structured VS code environment.

## To-dos

- Background (stripes)
  - [x] draw stripes
  - [x] set boundaries when window resizes

- Ball
  - [x] ball constantly moves
  - [x] ball bounces off all sides of screen
  - [x] ball bounces off paddle

- Paddle
  - [x] paddle only moves up and down
  - [x] paddle must not move out of screen
  - [x] paddle can be controlled by 2 players

- Round Start
  - [ ] (get ready) pause game at round start
  - [ ] (round start) unpause game after 2 seconds

- Round End
  - [ ] round ends when ball touches left or right side
  - [ ] calculate score
  - [ ] play end-round sound
  - [ ] reset to Round Start

- Sound
  - [ ] Play sound when ball hits wall
  - [ ] Play sound when ball hits paddle
  - [ ] Play jingle when round ends

## Compilation

Refer to `.vscode` folder

## VS code plugins

- C# `v1.25.0`
- CSharpier `v1.2.4`
- Monogame Content Builder `v0.0.4`

Additionally, if OmniSharp doesn't work, make sure the `dotnet` SDK is up-to-date. As of writing, it was `.NET 6.0 SDK (v6.0.302)`

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