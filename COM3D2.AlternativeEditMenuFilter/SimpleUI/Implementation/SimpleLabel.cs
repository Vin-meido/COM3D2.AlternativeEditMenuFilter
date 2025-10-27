using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

namespace COM3D2.SimpleUI.Implementation
{
    public class SimpleLabel : SimpleControl, ILabel
    {
        UILabel uiLabel;

        public override void InitControl()
        {
            uiLabel = NGUITools.AddChild<UILabel>(this.gameObject);
            uiLabel.trueTypeFont = UIUtils.GetFont("NotoSansCJKjp-DemiLight");
            uiLabel.color = Color.white;
        }

        public override void UpdateUI()
        {
            uiLabel.SetDimensions(
                Mathf.FloorToInt(this.size.x + .5f),
                Mathf.FloorToInt(this.size.y + .5f));
            uiLabel.rawPivot = UIWidget.Pivot.Left;
            uiLabel.gameObject.transform.localPosition = new Vector3(-this.size.x / 2, 0);
            uiLabel.text = this.text;
        }
    }
}
