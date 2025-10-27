using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.Events;

using COM3D2.SimpleUI.Events;

namespace COM3D2.SimpleUI.Implementation
{
    public class SimpleTextField : SimpleControl, ITextField
    {
        UISprite uiSprite;
        UISprite bgSprite;
        UIInput uiInput;
        UILabel uiLabel;

        readonly TextChangeEvent onSubmit = new TextChangeEvent();
        readonly TextChangeEvent onChange = new TextChangeEvent();

        public string Value {
            get => this.uiInput.value;
            set => this.uiInput.value = value;
        }

        public override void InitControl()
        {
            var atlas = UIUtils.GetAtlas("AtlasCommon");
            this.bgSprite = NGUITools.AddSprite(this.gameObject, atlas, "cm3d2_common_plate_white");
            this.bgSprite.color = new Color(.2f, .2f, .2f);

            this.uiSprite = NGUITools.AddSprite(this.gameObject, atlas, "cm3d2_common_lineframe_white");
            this.uiSprite.color = Color.gray;
            NGUITools.AddWidgetCollider(this.uiSprite.gameObject);

            this.uiLabel = NGUITools.AddWidget<UILabel>(uiSprite.gameObject);
            uiLabel.trueTypeFont = UIUtils.GetFont("NotoSansCJKjp-DemiLight");
            uiLabel.rawPivot = UIWidget.Pivot.Left;

            this.uiInput = uiSprite.gameObject.AddComponent<UIInput>();
            this.uiInput.label = this.uiLabel;
            this.uiInput.value = "";
            this.uiInput.activeTextColor = Color.white;
            this.uiInput.caretColor = Color.gray;
            this.uiInput.onReturnKey = UIInput.OnReturnKey.Submit;

            EventDelegate.Add(uiInput.onChange, new EventDelegate.Callback(this.ChangeEvent));
            EventDelegate.Add(uiInput.onSubmit, new EventDelegate.Callback(this.SubmitEvent));
        }

        void ChangeEvent()
        {
            onChange.Invoke(this.Value);
        }

        void SubmitEvent()
        {
            onSubmit.Invoke(this.Value);
        }

        public override void UpdateUI()
        {
            var width = Mathf.FloorToInt(size.x + 0.5f);
            var height = Mathf.FloorToInt(size.y + 0.5f);

            bgSprite.SetDimensions(width, height);
            uiSprite.SetDimensions(width, height);
            uiLabel.SetDimensions(width - 20, height - 20);
            uiLabel.gameObject.transform.localPosition = new Vector3(-size.x / 2f + 10, 0);

            uiSprite.ResizeCollider();
        }

        public void AddChangeCallback(UnityAction<string> callback)
        {
            onChange.AddListener(callback);
        }

        public void RemoveChangeCallback(UnityAction<string> callback)
        {
            onChange.RemoveListener(callback);
        }

        public void AddSubmitCallback(UnityAction<string> callback)
        {
            onSubmit.AddListener(callback);
        }

        public void RemoveSubmitCallback(UnityAction<string> callback)
        {
            onSubmit.RemoveListener(callback);
        }

    }
}
