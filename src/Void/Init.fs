﻿namespace Void

open Void.Core
open Void.Lang.Interpreter
open Void.Lang.Editor
open Void.ViewModel

type VoidOptions = {
    EnableVerboseMessageLogging : bool
}

module Options =
    let defaults = {
        EnableVerboseMessageLogging = false
    }

    let verboseMessagesFlag = "--verbose-message-logging"

    let parse (args : string[]) =
        let verboseMessages = Array.tryFind (fun x -> x.Equals(verboseMessagesFlag)) args
        {
            EnableVerboseMessageLogging = Option.isSome verboseMessages
        }

type InputModeChanger =
    abstract member SetInputHandler : InputMode<unit> -> unit

module Init =
    let setInputMode (changer : InputModeChanger) (publish : Message -> unit) (inputMode : InputMode<Message>) =
        match inputMode with
        | InputMode.KeyPresses handler ->
            publish << handler
            |> InputMode.KeyPresses
        | InputMode.TextAndHotKeys handler ->
            publish << handler
            |> InputMode.TextAndHotKeys
        |> changer.SetInputHandler

    let buildVoid inputModeChanger (options : VoidOptions) =
        let editorService = EditorService()
        let viewService = ViewModelService()
        let coreCommandChannel =
            Channel [
                viewService.handleCommand
                editorService.handleCommand
            ]
        let coreEventChannel =
            Channel [
                viewService.handleEvent
            ]
        let commandBarEventChannel =
            Channel [
                viewService.handleCommandBarEvent
            ]
        let bus =
            Bus [
                coreCommandChannel
                coreEventChannel
                commandBarEventChannel
            ]
        let interpreter = Interpreter.init <| VoidScriptEditorModule(bus.publish).Commands
        let interpreterWrapperService = InterpreterWrapperService interpreter
        let modeService = ModeService(NormalMode.InputHandler(),
                                      CommandMode.InputHandler(),
                                      VisualModeInputHandler(),
                                      InsertModeInputHandler(),
                                      setInputMode inputModeChanger bus.publish)
        modeService.subscribe bus
        if options.EnableVerboseMessageLogging then MessageLog.Service.subscribe bus
        interpreterWrapperService.subscribe bus
        BufferList.Service.subscribe bus
        DefaultNormalModeBindings.Service.subscribe bus
        CommandHistory.Service.subscribe bus
        CommandLanguage.Service.subscribe bus
        Notifications.Service.subscribe bus
        Filesystem.Service.subscribe bus
        CommandBar.Service.subscribe bus
        WindowBufferMap.Service.subscribe bus
        NotifyUserOfEvent.Service.subscribe bus
        bus

    let launchVoid (bus : Bus) =
        bus.publish CoreCommand.InitializeVoid
