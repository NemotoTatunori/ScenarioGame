using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /// <summary>���̔w�i</summary>
    [SerializeField] GameObject m_nowBackground;
    /// <summary>�V�����w�i</summary>
    [SerializeField] GameObject m_newBackground;
    /// <summary>�w�i�z��</summary>
    [SerializeField] Sprite[] m_backgrounds;
    /// <summary>�t�F�[�h</summary>
    [SerializeField] Image m_fade;
    /// <summary>�l�[���{�b�N�X</summary>
    [SerializeField] Text m_nameBox;
    /// <summary>�e�L�X�g�{�b�N�X</summary>
    [SerializeField] Text m_textBox;
    /// <summary>�Z���t�̑���</summary>
    [SerializeField] float m_textSpeed = 0.2f;
    /// <summary>�e�L�X�g�\���̃R���[�`��</summary>
    Coroutine m_coroutine;
    /// <summary>�V�i���I�S��</summary>
    string textLoad;
    /// <summary>�L�������Ƃ̃Z���t</summary>
    string[] m_conversation;
    /// <summary>�Z���t�̔z��</summary>
    string[] m_text;
    /// <summary>�Z���t�X�L�b�v�̃t���O</summary>
    bool m_skip = false;
    /// <summary>�Z���t</summary>
    int m_textRow = 1;
    /// <summary>��b�̎��</summary>
    int m_conversationNum = 0;
    void Start()
    {
        textLoad = (Resources.Load("Prologue", typeof(TextAsset)) as TextAsset).text;
        m_conversation = textLoad.Split(char.Parse("/"));
        //Cut(m_conversation[m_conversationNum]);
        m_coroutine = StartCoroutine(BackgroundChange(m_backgrounds[1]));
    }
    void Cut(string s)
    {
        m_text = s.Split(char.Parse(" "));
        m_nameBox.text = m_text[0];
        m_coroutine = StartCoroutine(Text(m_text[m_textRow]));
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (m_textRow < m_text.Length)
            {
                if (m_skip)
                {
                    NextText();
                }
                else
                {
                    StopCoroutine(m_coroutine);
                    m_textBox.text = m_text[m_textRow];
                    m_textRow += 1;
                    m_skip = true;
                }
            }
            else
            {
                Next();
            }
        }
    }
    void Next()
    {
        m_skip = false;
        m_conversationNum += 1;
        m_textRow = 1;
        Cut(m_conversation[m_conversationNum]);
    }
    void NextText()
    {
        m_skip = false;
        m_coroutine = StartCoroutine(Text(m_text[m_textRow]));
    }
    IEnumerator Text(string b)
    {
        string a = "";
        for (int i = 0; i < b.Length; i++)
        {
            a += b[i];
            m_textBox.text = a;
            yield return new WaitForSeconds(m_textSpeed);
        }
        m_skip = true;
        m_textRow += 1;
    }
    /// <summary>
    /// �w�i�����X�ɕς���
    /// </summary>
    /// <param name="newBackground"></param>
    /// <returns></returns>
    IEnumerator BackgroundChange(Sprite newBackground)
    {
        m_newBackground.GetComponent<Image>().sprite = newBackground;
        Image nowImage = m_nowBackground.GetComponent<Image>();
        for (int i = 0; i < 10; i++)
        {
            nowImage.color = new Color(1, 1, 1, 1 - i * 0.1f);
            yield return new WaitForSeconds(0.1f);
        }
        nowImage.sprite = newBackground;
        nowImage.color = new Color(1, 1, 1, 1);
    }
    /// <summary>
    /// �t�F�[�h�C���A�t�F�[�h�A�E�g������
    /// </summary>
    /// <param name="inOut"></param>
    /// <returns></returns>
    IEnumerator Fade(GameObject target, bool inOut, bool pattern)
    {
        float a;
        float before;
        float p = pattern ? 0 : 1;
        Image targetColor = target.GetComponent<Image>();
        if (inOut)
        {
            a = 1;
            before = 0;
            targetColor.color = new Color(p, p, p, before);
            while (a > before)
            {
                targetColor.color = new Color(p, p, p, before);
                before += 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else
        {
            a = 0;
            before = 1;
            targetColor.color = new Color(p, p, p, before);
            while (a < before)
            {
                targetColor.color = new Color(p, p, p, before);
                before -= 0.1f;
                yield return new WaitForSeconds(0.1f);
            }
        }
        m_fade.color = new Color(p, p, p, a);
    }
}
