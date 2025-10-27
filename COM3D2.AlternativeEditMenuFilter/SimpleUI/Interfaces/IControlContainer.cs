using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM3D2.SimpleUI
{
    public interface IControlContainer
    {
        IEnumerable<T> GetChildren<T>() where T : IControl;
        
        IEnumerable<IControl> GetChildren();

        int width { get; }
        int height { get; }
    }
}
