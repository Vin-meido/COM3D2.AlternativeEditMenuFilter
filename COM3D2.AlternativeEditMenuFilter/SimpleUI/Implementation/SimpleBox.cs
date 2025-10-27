using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;



namespace COM3D2.SimpleUI.Implementation
{
    public class SimpleBox : SimpleControl, IBox
    {
        UISprite uiSprite;
        UILabel uiLabel;

        public Color textColor
        {
            get => uiLabel.color;
            set => uiLabel.color = value;
        }

        public override void UpdateUI()
        {
            var width = Mathf.FloorToInt(this.size.x + 0.5f);
            var height = Mathf.FloorToInt(this.size.y + 0.5f);
            this.uiSprite.SetDimensions(width, height);
            this.uiLabel.SetDimensions(width, height);
            uiLabel.gameObject.transform.localPosition = new Vector3(0, this.size.y / 2f - 10);

            this.uiLabel.text = this.text;
        }

        public override void InitControl()
        {
            var atlas = UIUtils.GetAtlas("AtlasCommon");
            var spriteName = "cm3d2_common_plate_black";

            this.uiSprite = NGUITools.AddSprite(this.gameObject, atlas, spriteName);
            this.uiLabel = NGUITools.AddWidget<UILabel>(uiSprite.gameObject);

            uiLabel.trueTypeFont = UIUtils.GetFont("NotoSansCJKjp-DemiLight");
            uiLabel.color = Color.white;
            uiLabel.rawPivot = UIWidget.Pivot.Top;
        }

        public void ChangeSprite(UIAtlas atlas, string spriteName)
        {
            this.uiSprite.atlas = atlas;
            this.uiSprite.spriteName = spriteName;
        }
    }
}
