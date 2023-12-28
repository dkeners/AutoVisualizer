using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoVisualizer.Utils
{
    internal class ButtonRunCheck
    {
        private ButtonState _buttonState;

        public ButtonRunCheck()
        {
            _buttonState = ButtonState.Unclicked;
        }

        public ButtonRunCheck(ButtonState buttonState)
        {
            _buttonState = buttonState;
        }

        public void SetButtonState(ButtonState state)
        {
            _buttonState = state;
        }

        public bool IsClicked()
        {
            return _buttonState == ButtonState.Clicked;
        }

        public void Reset()
        {
            _buttonState = ButtonState.Unclicked;
        }
    }
}
