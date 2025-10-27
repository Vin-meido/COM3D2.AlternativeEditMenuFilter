using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

using COM3D2.SimpleUI.Events;

namespace COM3D2.SimpleUI
{
    public interface ITextField: IContentControl, IStringControlValue
    {
        void AddSubmitCallback(UnityAction<string> callback);
        void RemoveSubmitCallback(UnityAction<string> callback);
    }
}
