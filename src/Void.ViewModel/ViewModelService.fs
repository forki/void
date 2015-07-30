﻿namespace Void.ViewModel

open Void.Core

type ViewModelService() =
    let mutable _viewModel = ViewModel.defaultViewModel

    member x.handleCommand =
        function
        | CoreCommand.InitializeVoid ->
            VMEvent.ViewModelInitialized _viewModel :> Message
        | CoreCommand.Display _ ->
            notImplemented
        | CoreCommand.Redraw ->
            (GridConvert.boxAround (ViewModel.wholeArea _viewModel), Render.viewModelAsDrawingObjects _viewModel)
            |> VMEvent.ViewPortionRendered :> Message
        | _ ->
            noMessage

    member x.handleVMCommand =
        function
        | VMCommand.Scroll movement ->
            match movement with
            | Move.Backward xLines ->
                noMessage
            | Move.Forward xLines ->
                noMessage
        | _ ->
            noMessage

    member x.handleCommandBarEvent event =
        let commandBarOrigin = ViewModel.upperLeftCellOfCommandBar _viewModel
        let renderCommandBar commandBar =
            RenderCommandBar.asDrawingObjects commandBar commandBarOrigin
            |> VMEvent.ViewPortionRendered :> Message
        match event with
        | CommandBar.Event.CharacterBackspacedFromLine cell ->
            RenderCommandBar.backspacedCharacterAsDrawingObject cell commandBarOrigin
            |> VMEvent.ViewPortionRendered :> Message
        | CommandBar.Event.Displayed commandBar ->
            renderCommandBar commandBar
        | CommandBar.Event.Hidden commandBar ->
            renderCommandBar commandBar
        | CommandBar.Event.TextAppendedToLine textSegment ->
            RenderCommandBar.appendedTextAsDrawingObject textSegment commandBarOrigin
            |> VMEvent.ViewPortionRendered :> Message
        | CommandBar.Event.TextChanged commandBar ->
            renderCommandBar commandBar
        | CommandBar.Event.TextReflowed commandBar ->
            renderCommandBar commandBar

    member x.handleBufferEvent event =
        match event.Message with
        | BufferEvent.Added buffer ->
            _viewModel <- ViewModel.loadBuffer buffer _viewModel
            let drawings = Render.currentBufferAsDrawingObjects _viewModel
            let area = GridConvert.boxAround (ViewModel.wholeArea _viewModel) (* TODO shouldn't redraw the whole UI *)
            VMEvent.ViewPortionRendered(area, drawings) :> Message

    member x.handleEvent =
        function // TODO clearly the code below needs to be refactored
        | CoreEvent.NotificationAdded notification ->
            let area = ViewModel.areaOfCommandBarOrNotifications _viewModel
            let drawing = ViewModel.toScreenNotification notification
                          |> Render.notificationAsDrawingObject area.UpperLeftCell
            let areaInPoints = GridConvert.boxAround area
            VMEvent.ViewPortionRendered(areaInPoints, [drawing]) :> Message
        | _ -> noMessage

    member x.subscribe (bus : Bus) =
        bus.subscribe x.handleEvent
        bus.subscribe x.handleBufferEvent
        bus.subscribe x.handleCommand
        bus.subscribe x.handleVMCommand
        bus.subscribe x.handleCommandBarEvent