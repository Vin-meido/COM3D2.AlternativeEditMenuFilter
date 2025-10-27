using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace COM3D2.SimpleUI.Implementation
{
    public class SimpleGenericDropdown : SimpleDropdown, IGenericDropdown
    {
        readonly ChangeEvent changeEvent = new ChangeEvent();

        public new virtual object Value
        {
            get
            {
                if (uiPopupList.data is DropdownItem data)
                {
                    return data.Value;
                }
                return null;
            }
            set
            {
                var data = uiPopupList.itemData
                    .Select(o => o as DropdownItem)
                    .Where(o => o != null)
                    .Where(o => o.Value.Equals(value))
                    .FirstOrDefault();

                if (data != null)
                {
                    uiPopupList.value = data.ItemName;
                }
            }
        }

        public virtual DropdownItem ValueData
        {
            get => uiPopupList.data as DropdownItem;
        }

        public virtual T GetValue<T>()
        {
            if (this.Value is T data)
            {
                return data;
            }
            return default(T);
        }

        public virtual IGenericDropdown SetValue<T>(T value)
        {
            this.Value = value;
            return this;
        }

        bool updateTextOnValue = false;
        public bool UpdateTextOnValue
        {
            get => updateTextOnValue;
            set
            {
                updateTextOnValue = value;
                SetDirty();
            }
        }

        public override void InitControl()
        {
            base.InitControl();
            EventDelegate.Add(uiPopupList.onChange, new EventDelegate.Callback(this.uiPopupListDataChange));
        }

        protected virtual void uiPopupListDataChange()
        {
            if(this.ready)
            {
                if (this.UpdateTextOnValue)
                {
                    SetDirty();
                }

                changeEvent.Invoke(this.Value);

            }
        }

        public void AddChangeCallback(UnityAction<object> callback)
        {
            changeEvent.AddListener(callback);
        }

        public void RemoveChangeCallback(UnityAction<object> callback)
        {
            changeEvent.RemoveListener(callback);
        }

        public virtual IGenericDropdown ClearChoices()
        {
            this.uiPopupList.Clear();
            return this;
        }

        public IGenericDropdown Choice<T>(T value, string text = "", string selected = "")
        {
            if (string.IsNullOrEmpty(text)) text = value.ToString();
            if (string.IsNullOrEmpty(selected)) selected = text;

            this.uiPopupList.AddItem(text, new DropdownItem()
            {
                Value = value,
                ItemName = text,
                SelectedName = selected,
            });

            return this;
        }

        public IGenericDropdown RemoveChoice<T>(T value)
        {
            throw new NotImplementedException();
            return this;
        }

        public IGenericDropdown SetUpdateTextOnValuechange(bool value)
        {
            this.UpdateTextOnValue = value;
            return this;
        }

        public override void UpdateUI()
        {
            base.UpdateUI();
            var data = this.ValueData;
            if (this.UpdateTextOnValue && data != null)
            {
                this.uiLabel.text = data.SelectedName;
            }
        }

        public class DropdownItem
        {
            public object Value { get; set; }
            public string ItemName { get; set; }
            public string SelectedName { get; set; }

        }

        public class ChangeEvent: UnityEvent<object>
        {

        }
    }
}
