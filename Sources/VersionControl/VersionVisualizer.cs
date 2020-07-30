using UnityEngine;
using TMPro;

namespace GameDevStack.VersionControl
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class VersionVisualizer : MonoBehaviour
    {
        private TextMeshProUGUI m_Text = null;

        private void Awake()
        {
            m_Text = GetComponent<TextMeshProUGUI>();
            m_Text.text = string.Concat(Application.version);
        }
    }
}