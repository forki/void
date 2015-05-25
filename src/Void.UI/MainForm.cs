﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Void.Core;
using Void.Util;
using Void.ViewModel;
using Microsoft.FSharp.Core;
using Message = Void.Core.Message;

namespace Void.UI
{
    public partial class MainForm : Form, MainView, InputModeChanger
    {
        private Font _font = new Font(FontFamily.GenericMonospace, 9);
        private InputMode<Unit> _inputHandler;
        private CellMetrics _cellMetrics;
        private IEnumerable<DrawingObject> _drawings;

        public MainForm()
        {
            InitializeComponent();
            SubscribeToPaint();
            WireUpInputEvents();
            var messagingSystem = Init.buildVoid(this, this);
            messagingSystem.EventChannel.addHandler(FSharpFuncUtil.Create<Event, Message>(HandleEvent));
            messagingSystem.VMEventChannel.addHandler(FSharpFuncUtil.Create<VMEvent, Message>(HandleViewModelEvent));
            Init.launchVoid(messagingSystem);
        }

        private Message HandleEvent(Event eventMsg)
        {
            if (eventMsg.IsLastWindowClosed)
            {
                Close();
            }
            return null;
        }

        private Message HandleViewModelEvent(VMEvent eventMsg)
        {
            if (eventMsg.IsViewPortionRendered)
            {
                _drawings = ((VMEvent.ViewPortionRendered)eventMsg).Item2;
                TriggerDraw(((VMEvent.ViewPortionRendered)eventMsg).Item1);
            }
            return null;
        }

        private void WireUpInputEvents()
        {
            KeyUp += (sender, eventArgs) =>
            {
                if (_inputHandler.IsKeyPresses)
                {
                    var keyPress = eventArgs.AsVoidKeyPress();
                    if (keyPress == null)
                    {
                        Console.WriteLine("Warning: failed to translate key stroke");
                    }
                    else
                    {
                        _inputHandler.AsKeyPressesHandler()(keyPress);
                    }
                }
                else
                {
                    // TODO!!! This is a provisional hack; see GitHub issue #2
                    // Should probably be attaching this to a TextInput event or
                    // something instead, but those sort of events don't seem to just
                    // be wired on on the Form level, only in actual text boxes...
                    var textOrHotKey = eventArgs.AsVoidTextOrHotKeyProvisionalHack();
                    if (textOrHotKey == null)
                    {
                        Console.WriteLine("Warning: failed to translate key stroke");
                    }
                    else
                    {
                        _inputHandler.AsTextAndHotKeysHandler()(textOrHotKey);
                    }
                }
            };
        }

        public void SubscribeToPaint()
        {
            Paint += (sender, eventArgs) =>
            {
                var artist = new WinFormsArtist(eventArgs.Graphics, _font, _cellMetrics);
                foreach (var drawing in _drawings)
                {
                    artist.Draw(drawing);
                }
                _drawings = Enumerable.Empty<DrawingObject>();
            };
        }

        public void SetInputHandler(InputMode<Unit> handler)
        {
            _inputHandler = handler;
        }

        private int MeasureFontWidth()
        {
            // TODO this isn't working 100% well
            // Ironically, it seems to work better on Mono than .NET
            return Convert.ToInt32(Math.Ceiling(CreateGraphics().MeasureString(new string('X', 80), _font).Width / 80));
        }

        public void SetBackgroundColor(RGBColor color)
        {
            BackColor = color.AsWinFormsColor();
        }

        public void SetFontBySize(byte size)
        {
            _font = new Font(FontFamily.GenericMonospace, 9);
            _cellMetrics = new CellMetrics(_font.Height, MeasureFontWidth());
        }

        public void SetViewSize(PointGrid.Dimensions size)
        {
            ClientSize = size.AsWinFormsSize(_cellMetrics);
        }

        public void SetViewTitle(string title)
        {
            Text = title;
        }

        public void TriggerDraw(PointGrid.Block block)
        {
            Invalidate(block.AsWinFormsRectangle(_cellMetrics));
        }
    }
}
