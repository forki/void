﻿namespace Void.Core

[<RequireQualifiedAccess>]
type Event =
    | BufferLoadedIntoWindow of BufferType
    | CommandEntryCancelled
    | EditorInitialized of EditorState
    | ErrorOccurred of Error
    | LastWindowClosed
    | LineCommandCompleted
    | MessageAdded of Message
    | ModeSet of Mode
    | ModeChanged of ModeChange
    | ViewInitialized // Vim equivalent: GUIEnter