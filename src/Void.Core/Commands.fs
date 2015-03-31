﻿namespace Void.Core

type Displayable =
    | Messages of Message list

[<RequireQualifiedAccess>]
type Command =
    | Noop
    | PublishEvent of Event
    //
    | ChangeToMode of Mode
    | Display of Displayable
    | Edit of FileIdentifier
    | FormatCurrentLine
    | InitializeVoid
    | InterpretCommandModeCommand of string
    | Put
    | Quit
    | Redraw
    | ShowMessages
    | ViewTestBuffer // TODO for Debug/Test only
    | Yank

[<AutoOpen>]
module Util =
    let notImplemented =
        Event.ErrorOccurred Error.NotImplemented
        |> Command.PublishEvent
