using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExcelManager : MonoBehaviour
{
    [SerializeField] Scenario m_scenario;
    /// <summary>今の背景</summary>
    [SerializeField] GameObject m_nowBackground;
    /// <summary>新しい背景</summary>
    [SerializeField] GameObject m_newBackground;
    /// <summary>背景配列</summary>
    [SerializeField] Sprite[] m_backgrounds;
    /// <summary>登場人物の配列</summary>
    [SerializeField] GameObject[] m_people;
    /// <summary>フェード</summary>
    [SerializeField] Image m_fade;
    /// <summary>ネームボックス</summary>
    [SerializeField] Text m_nameBox;
    /// <summary>テキストボックス</summary>
    [SerializeField] Text m_textBox;
    /// <summary>セリフの速さ</summary>
    [SerializeField] int m_textSpeed = 40;
    /// <summary>コルーチン</summary>
    Coroutine m_coroutine;
    /// <summary>進行度</summary>
    int m_degreeOfProgress = 0;
    /// <summary>喋ってる人</summary>
    int m_talk = 0;
    void Start()
    {
        Setting();
    }
    void Setting()
    {
        if (m_scenario.Sheet1[m_degreeOfProgress].fade != "")
        {
            m_nameBox.text = "";
            m_textBox.text = "";
            string[] f = m_scenario.Sheet1[m_degreeOfProgress].fade.Split(char.Parse(","));
            if (f[0] == "c")
            {
                m_coroutine = StartCoroutine(BackgroundChange(m_backgrounds[int.Parse(f[1])]));
            }
            else
            {
                bool[] p = new bool[f.Length - 1];
                for (int i = 1; i < p.Length + 1; i++)
                {
                    p[i - 1] = f[i] == "0" ? false : true;
                }
                m_coroutine = StartCoroutine(Fade(m_fade, p[0], p[1]));
            }
        }
        else if (m_scenario.Sheet1[m_degreeOfProgress].dialogue == "e")
        {
            Debug.Log("END");
        }
        else
        {
            m_nameBox.text = m_scenario.Sheet1[m_scenario.Sheet1[m_degreeOfProgress].human].name;
            m_coroutine = StartCoroutine(Text(m_scenario.Sheet1[m_degreeOfProgress].dialogue));
            if (m_talk != m_scenario.Sheet1[m_degreeOfProgress].human)
            {
                if (m_scenario.Sheet1[m_degreeOfProgress].human == 2)
                {
                    return;
                }
                if (m_people[m_talk].GetComponent<Image>().color.r >= 0.5)
                {
                    StartCoroutine(CharacterGray(m_people[m_talk]));
                }
                if (m_people[m_scenario.Sheet1[m_degreeOfProgress].human].GetComponent<Image>().color.r <= 0.5)
                {
                    StartCoroutine(CharacterClear(m_people[m_scenario.Sheet1[m_degreeOfProgress].human]));
                }
                m_talk = m_scenario.Sheet1[m_degreeOfProgress].human;
            }
        }
    }
    void Update()
    {

    }

    IEnumerator Text(string b)
    {
        string a = "";
        int intr = 0;
        int i = 0;
        yield return null;
        while (i < b.Length)
        {
            if (intr == m_textSpeed)
            {
                a += b[i];
                m_textBox.text = a;
                i++;
                intr = 0;
            }
            intr++;
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
        m_textBox.text = m_scenario.Sheet1[m_degreeOfProgress].dialogue;
        m_degreeOfProgress++;
        yield return null;
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Setting();
                break;
            }
            yield return null;
        }
    }
    /// <summary>
    /// 背景を徐々に変える
    /// </summary>
    /// <param name="newBackground"></param>
    /// <returns></returns>
    IEnumerator BackgroundChange(Sprite newBackground)
    {
        foreach (var item in m_people)
        {
            StartCoroutine(CharacterGray(item));
        }
        m_newBackground.GetComponent<Image>().sprite = newBackground;
        Image nowImage = m_nowBackground.GetComponent<Image>();
        int intr = 0;
        int i = 0;
        yield return null;
        while (i < 10)
        {
            if (intr >= 20)
            {
                nowImage.color = new Color(1, 1, 1, 1 - i * 0.1f);
                i++;
                intr = 0;
            }
            intr++;
            if (Input.GetMouseButtonDown(0))
            {
                break;
            }
            yield return null;
        }
        nowImage.sprite = newBackground;
        nowImage.color = new Color(1, 1, 1, 1);
        yield return null;
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_degreeOfProgress++;
                Setting();
                break;
            }
            yield return null;
        }
    }

    /// <summary>
    /// フェードイン、フェードアウトをする
    /// </summary>
    /// <param name="inOut"></param>
    /// <returns></returns>
    IEnumerator Fade(Image target, bool inOut, bool pattern)
    {
        Debug.Log(inOut + " " + pattern);
        float after;
        float before;
        float p = pattern ? 0 : 1;
        int i = 0;
        Image targetColor = target;
        yield return null;
        if (inOut)
        {
            after = 1;
            before = 0;
            targetColor.color = new Color(p, p, p, before);
            while (after > before)
            {
                if (i == 20)
                {
                    targetColor.color = new Color(p, p, p, before);
                    before += 0.05f;
                    i = 0;
                }
                i++;
                if (Input.GetMouseButtonDown(0))
                {
                    break;
                }
                yield return null;
            }
        }
        else
        {
            after = 0;
            before = 1;
            targetColor.color = new Color(p, p, p, before);
            while (after < before)
            {
                if (i == 20)
                {
                    targetColor.color = new Color(p, p, p, before);
                    before -= 0.05f;
                    i = 0;
                }
                i++;
                if (Input.GetMouseButtonDown(0))
                {
                    break;
                }
                yield return null;
            }
        }
        m_fade.color = new Color(p, p, p, after);
        yield return null;
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                m_degreeOfProgress++;
                Setting();
                break;
            }
            yield return null;
        }
    }
    /// <summary>
    /// キャラクターを灰色にする
    /// </summary>
    /// <param name="chara"></param>
    /// <returns></returns>
    IEnumerator CharacterGray(GameObject chara)
    {
        Image c = chara.GetComponent<Image>();
        float a = 0.3f;
        float n = 1;
        int intr = 0;
        while (n > a)
        {
            if (intr >= 10)
            {
                n -= 0.1f;
                c.color = new Color(n, n, n);
                if (Input.GetMouseButtonDown(0))
                {
                    n = a;
                    c.color = new Color(n, n, n);
                    break;
                }
                intr = 0;
            }
            intr++;
            yield return null;
        }
    }
    /// <summary>
    /// キャラクターを元の色に戻す
    /// </summary>
    /// <param name="chara"></param>
    /// <returns></returns>
    IEnumerator CharacterClear(GameObject chara)
    {
        Image c = chara.GetComponent<Image>();
        float a = 1;
        float n = 0.3f;
        int intr = 0;
        while (n < a)
        {
            if (intr >= 10)
            {
                n += 0.1f;
                c.color = new Color(n, n, n);
                if (Input.GetMouseButtonDown(0))
                {
                    n = a;
                    c.color = new Color(n, n, n);
                    break;
                }
                intr = 0;
            }
            intr++;
            yield return null;
        }
    }
}
