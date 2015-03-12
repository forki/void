﻿namespace Void.ViewModel

open Void.Core
open CellGrid
open System

type ViewController
    (
        _view : MainView
    ) =
    let mutable _fontMetrics = _view.GetFontMetrics()
    let mutable _convert = Sizing.Convert _fontMetrics
    let _viewArea = { UpperLeftCell = originCell; Dimensions = Sizing.defaultViewSize }
    let _colorscheme = Colors.defaultColorscheme
    let mutable _bufferedDrawings = Seq.empty

    // Subscribe to some init event on the view instead of exposing this as a
    // public method?
    member private x.initializeView() =
        _view.SetViewTitle ViewModel.defaultTitle
        _view.SetBackgroundColor Colors.defaultColorscheme.Background
        x.setFont()
        _view.SetViewSize <| _convert.cellDimensionsToPixels _viewArea.Dimensions
        Command.PublishEvent Event.ViewInitialized

    member x.paint (draw : Action<DrawingObject>) =
        for drawing in _bufferedDrawings do draw.Invoke drawing

    member private x.setFont() =
        _view.SetFontBySize ViewModel.defaultFontSize
        _fontMetrics <- _view.GetFontMetrics()
        _convert <- Sizing.Convert _fontMetrics

    member private x.bufferDrawings drawings =
        _bufferedDrawings <- Seq.append _bufferedDrawings drawings

    member private x.bufferDrawing drawing =
        x.bufferDrawings [drawing] // TODO is there a better way to do this?

    member x.handleCommand command =
        match command with
        | Command.Quit -> _view.Close()
        | Command.Redraw -> _view.TriggerDraw()
        | _ -> ()
        Command.Noop

    member x.handleEvent event =
        match event with
        | Event.MessageAdded msg ->
            ViewModel.toScreenMessage msg
            |> Render.outputMessageAsDrawingObject _convert { Row = lastRow _viewArea; Column = 0 }
            |> x.bufferDrawing
            Command.Redraw
        | Event.BufferLoadedIntoWindow buffer ->
            ViewModel.toScreenBuffer _viewArea.Dimensions buffer
            |> Render.bufferAsDrawingObjects _convert _viewArea
            |> x.bufferDrawings
            Command.Redraw
        | Event.CoreInitialized -> x.initializeView()
        | _ -> Command.Noop

type MainController
    (
        _view : MainView
    ) =
    let _normalCtrl = NormalModeController()
    let _coreCtrl = CoreController()
    let _viewCtrl = ViewController(_view)
    let _eventHandlers = [_coreCtrl.handleEvent; _viewCtrl.handleEvent]

    member x.initializeVoid() =
        x.handleCommand <| Command.PublishEvent Event.CoreInitialized
        x.handleCommand Command.ViewTestBuffer // for debugging

    member x.handleViewEvent viewEvent =
        match viewEvent with
        | ViewEvent.PaintInitiated draw ->
            _viewCtrl.paint draw
        | ViewEvent.KeyPressed keyPress ->
            _normalCtrl.handle keyPress |> x.handleCommand
        | ViewEvent.TextEntered text ->
            () // TODO implement input and command modes, etc

    member private x.handleCommand command =
        let notImplemented() =
            Event.ErrorOccurred Error.NotImplemented
            |> Command.PublishEvent
            |> x.handleCommand

        match command with
        | Command.ChangeToMode _
        | Command.Edit
        | Command.Yank
        | Command.Put
        | Command.FormatCurrentLine ->
            notImplemented()
        | Command.PublishEvent event ->
            for handle in _eventHandlers do
                handle event |> x.handleCommand
        | Command.Quit
        | Command.Redraw ->
            _viewCtrl.handleCommand command |> x.handleCommand
        | Command.ViewTestBuffer -> 
            _coreCtrl.handleCommand command |> x.handleCommand
        | Command.Noop -> ()
