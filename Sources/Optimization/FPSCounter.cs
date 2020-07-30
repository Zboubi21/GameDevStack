using UnityEngine;
using GameDevStack.CommonEnums;
using TMPro;

namespace GameDevStack.Optimization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private string m_FpsTxt = "FPS ";
        [SerializeField] private ColorType m_FpsColor = ColorType.Yellow;
        [SerializeField] private float m_UpdateInterval = 0.5f;

        private float m_FramesAccumulated = 0f;
        private float m_FramesDrawnInTheInterval = 0f;
        private float m_TimeLeft ;
        private TextMeshProUGUI m_Text;
        private int m_CurrentFPS;

        private static string[] m_StringsFrom00To300 = {
            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
            "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
            "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
            "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
            "90", "91", "92", "93", "94", "95", "96", "97", "98", "99",
            "100", "101", "102", "103", "104", "105", "106", "107", "108", "109",
            "110", "111", "112", "113", "114", "115", "116", "117", "118", "119",
            "120", "121", "122", "123", "124", "125", "126", "127", "128", "129",
            "130", "131", "132", "133", "134", "135", "136", "137", "138", "139",
            "140", "141", "142", "143", "144", "145", "146", "147", "148", "149",
            "150", "151", "152", "153", "154", "155", "156", "157", "158", "159",
            "160", "161", "162", "163", "164", "165", "166", "167", "168", "169",
            "170", "171", "172", "173", "174", "175", "176", "177", "178", "179",
            "180", "181", "182", "183", "184", "185", "186", "187", "188", "189",
            "190", "191", "192", "193", "194", "195", "196", "197", "198", "199",
            "200", "201", "202", "203", "204", "205", "206", "207", "208", "209",
            "210", "211", "212", "213", "214", "215", "216", "217", "218", "219",
            "220", "221", "222", "223", "224", "225", "226", "227", "228", "229",
            "230", "231", "232", "233", "234", "235", "236", "237", "238", "239",
            "240", "241", "242", "243", "244", "245", "246", "247", "248", "249",
            "250", "251", "252", "253", "254", "255", "256", "257", "258", "259",
            "260", "261", "262", "263", "264", "265", "266", "267", "268", "269",
            "270", "271", "272", "273", "274", "275", "276", "277", "278", "279",
            "280", "281", "282", "283", "284", "285", "286", "287", "288", "289",
            "290", "291", "292", "293", "294", "295", "296", "297", "298", "299",
            "300"
        };

        /// <summary>
        /// On Start(), we get the Text component and initialize our counter
        /// </summary>
        private void Start()
        {
            if(GetComponent<TextMeshProUGUI>()==null)
            {
                Debug.LogWarning ("FPSCounter requires a Text component.");
                return;
            }
            m_Text = GetComponent<TextMeshProUGUI>();
            m_TimeLeft = m_UpdateInterval;
        }

        /// <summary>
        /// On Update, we increment our various counters, and if we've reached our UpdateInterval, we update our FPS counter
        /// with the number of frames displayed since the last counter update
        /// </summary>
        private void Update()
        {
            m_FramesDrawnInTheInterval++;
            m_FramesAccumulated = m_FramesAccumulated + Time.timeScale/Time.deltaTime;
            m_TimeLeft = m_TimeLeft - Time.deltaTime;

            if( m_TimeLeft <= 0.0 )
            {
                m_CurrentFPS = (int)Mathf.Clamp (m_FramesAccumulated / m_FramesDrawnInTheInterval, 0, 300);
                if (m_CurrentFPS >=0 && m_CurrentFPS <= 300)
                {
                    string fpsText = string.Concat(m_FpsTxt, "<color=", ConvertColorTypeToString(m_FpsColor), ">", m_StringsFrom00To300 [m_CurrentFPS], "</color>");
                    m_Text.text = fpsText;	
                }
                m_FramesDrawnInTheInterval = 0;
                m_FramesAccumulated = 0f;
                m_TimeLeft = m_UpdateInterval;
            }
        }

        private string ConvertColorTypeToString(ColorType colorType)
        {
            string colorName;
            switch (colorType)
            {
                case ColorType.White:
                    colorName = "white";
                break;
                case ColorType.Black:
                    colorName = "black";
                break;
                case ColorType.Red:
                    colorName = "red";
                break;
                case ColorType.Green:
                    colorName = "green";
                break;
                case ColorType.Blue:
                    colorName = "blue";
                break;
                case ColorType.Yellow:
                    colorName = "yellow";
                break;
                case ColorType.Orange:
                    colorName = "orange";
                break;
                case ColorType.Purple:
                    colorName = "purple";
                break;
                default:
                    colorName = "white";
                break;
            }
            return colorName;
        }
    }
}