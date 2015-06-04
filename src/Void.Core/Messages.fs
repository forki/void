﻿namespace Void.Core

type Message = interface end

[<AutoOpen>]
module ``This module is auto-opened to provide a null message`` =
    type private NullMessage =
        | NoMsg
        interface Message
    let noMessage = NoMsg :> Message

[<RequireQualifiedAccess>]
type Event =
    | BufferLoadedIntoWindow of BufferType
    | CommandEntryCancelled
    | CommandMode_CharacterBackspaced
    | CommandMode_TextAppended of string
    | EditorInitialized of EditorState
    | ErrorOccurred of Error
    | LastWindowClosed
    | LineCommandCompleted
    | NotificationAdded of UserNotification
    | ModeSet of Mode
    | ModeChanged of ModeChange
    interface Message

type Displayable =
    | Notifications of UserNotification list

[<RequireQualifiedAccess>]
type Command =
    | ChangeToMode of Mode
    | Display of Displayable
    | Echo of string
    | Edit of FileIdentifier
    | FormatCurrentLine
    | Help
    | InitializeVoid
    | Put
    | Quit
    | QuitAll
    | QuitAllWithoutSaving
    | QuitWithoutSaving
    | Redraw
    | ShowNotificationHistory
    | View of FileIdentifier
    | ViewTestBuffer // TODO for Debug/Test only
    | Yank
    interface Message

[<AutoOpen>]
module ``This module is auto-opened to provide message aliases`` =
    let notImplemented =
        Event.ErrorOccurred Error.NotImplemented :> Message