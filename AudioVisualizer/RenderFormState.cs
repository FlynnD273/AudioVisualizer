using System;
using System.Collections.Generic;
using System.Text;

namespace AudioVisualizer
{
    class RenderFormState : NotifyPropertyChangedBase
    {
        private bool _inPanel;
        private bool _inButton;
        private bool _buttonIsVisible;

        public bool InPanel
        {
            get => _inPanel;
            set => UpdateField(ref _inPanel, value, _OnInPanelChanged);
        }

        public bool InButton
        {
            get => _inButton;
            set => UpdateField(ref _inButton, value, _OnInButtonChanged);
        }

        public bool ButtonIsVisible
        {
            get => _buttonIsVisible;
            private set => UpdateField(ref _buttonIsVisible, value);
        }

        private void _OnInPanelChanged(bool obj)
        {
            ButtonIsVisible = InPanel || InButton;
        }

        private void _OnInButtonChanged(bool obj)
        {
            ButtonIsVisible = InPanel || InButton;
        }
    }
}
