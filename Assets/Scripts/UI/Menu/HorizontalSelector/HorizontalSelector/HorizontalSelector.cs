using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;

namespace UnityEngine.UI
{
    public class HorizontalSelector : MonoBehaviour
    {
        public int Value { get { return m_Value; } set { SetValue(value); } }
        [SerializeField] private int m_Value;
        [SerializeField] List<string> m_Options = new List<string> { "Option 1", "Option 2", "Option 3" };

        [Space]
        [SerializeField] private UnityEvent<int> m_OnValueChanged;

        [Header("References")]
        [SerializeField] private TextMeshProUGUI m_Label;
        [SerializeField] private Button m_ButtonLeft;
        [SerializeField] private Button m_ButtonRight;

        private void OnEnable()
        {
            m_ButtonLeft.onClick.AddListener(OnLeftClick);
            m_ButtonRight.onClick.AddListener(OnRightClick);
        }

        private void OnDisable()
        {
            m_ButtonLeft.onClick.RemoveListener(OnLeftClick);
            m_ButtonRight.onClick.RemoveListener(OnRightClick);
        }

        private void Start()
        {
            RefreshShownValue();
        }

        public void OnLeftClick()
        {
            if (m_Value == 0)
                m_Value = m_Options.Count - 1;
            else
                m_Value--;

            RefreshShownValue();
            Notify(m_Value);
        }

        public void OnRightClick()
        {
            if ((m_Value + 1) >= m_Options.Count)
                m_Value = 0;
            else
                m_Value++;

            RefreshShownValue();
            Notify(m_Value);
        }

        private void Notify(int value)
        {
            if (m_OnValueChanged != null)
                m_OnValueChanged?.Invoke(value);
        }

        private void SetValue(int value)
        {
            m_Value = value;
            Notify(value);
        }

        public void SetValueWithoutNotify(int value)
        {
            m_Value = value;
        }

        public void AddOptions(List<string> options)
        {
            for (int i = 0; i < options.Count; i++)
            {
                string option = options[i];
                m_Options.Add(option);
            }
        }
   
        public void RefreshShownValue()
        {
            if (m_Options != null)
                m_Label.text = m_Options[m_Value];
        }

        public void ClearOptions()
        {
            if (m_Options != null)
                m_Options.Clear();
        }
    }
}