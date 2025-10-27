using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using COM3D2.SimpleUI.Events;

namespace COM3D2.SimpleUI
{
    public interface ISlider: IControl, IFloatControlValue
    {
        void SetValues(float current, float minimum, float maximum);
    }
}
