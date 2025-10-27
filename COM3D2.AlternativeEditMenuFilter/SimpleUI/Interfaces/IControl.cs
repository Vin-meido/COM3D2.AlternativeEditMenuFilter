using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace COM3D2.SimpleUI
{
    public interface IControl
    {
        Vector2 size { get; set; }

        Vector2 position { get; set; }

        bool Visible { get; set; }

        string Name { get; set; }

        void Remove();
    }
}
