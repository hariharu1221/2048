using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodePiece : MonoBehaviour
{
    public GameObject ValueText;
    RectTransform rect;
    Image img;

    Vector2 pos;

    public Point index;
    public int value;
    public int state;

    bool updating;

    public void Init(Point id, int v, int s)
    {
        rect = GetComponent<RectTransform>();
        img = GetComponent<Image>();
        ValueText.GetComponent<TextMeshProUGUI>().color = new Color(82 / 255f, 82 / 255f, 82 / 255f);

        index = id;

        ResetPosition();
        rect.anchoredPosition = pos;

        SetIndex(id, v, s);
    }

    public void SetState(int sta)
    {
        state = sta;
        if (sta == 0) this.gameObject.SetActive(false);
        else this.gameObject.SetActive(true);
    }

    void SetValue(int val)
    {
        value = val;
    }

    public void SetIndex(Point id, int v, int s)
    {
        index = id;
        SetState(s);
        SetValue(v);
        SetName();
    }
    void SetName()
    {
        this.gameObject.name = "Node [ " + index.x + " , " + index.y + " ], value = " + value;
    }
    void MovePositionTo(Vector2 move)
    {
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, move, Time.deltaTime * 24f);
    }

    public void ResetPosition()
    {
        pos = new Vector2(35 + (60 * index.x), -35 - (60 * index.y));
    }

    public bool StartUpdate()
    {
        if (Vector3.Distance(rect.anchoredPosition, pos) > 1)
        {
            MovePositionTo(pos);
            updating = true;
            return true;
        }
        else
        {
            rect.anchoredPosition = pos;
            updating = false;
            return false;
        }
        return true;
    }

    public void EndUpdate()
    {
        ValueText.GetComponent<TextMeshProUGUI>().text = value.ToString();

        if (value == 2) { img.color = new Color(255 / 255f, 225 / 255f, 225 / 255f); ValueText.GetComponent<TextMeshProUGUI>().fontSize = 35; }
        if (value == 4) img.color = new Color(255 / 255f, 185 / 255f, 185 / 255f);
        if (value == 8) img.color = new Color(255 / 255f, 145 / 255f, 145 / 255f);
        if (value == 16) { img.color = new Color(255 / 255f, 105 / 255f, 105 / 255f); ValueText.GetComponent<TextMeshProUGUI>().fontSize = 30; }
        if (value == 32) img.color = new Color(255 / 255f, 65 / 255f, 65 / 255f);
        if (value == 64) img.color = new Color(185 / 255f, 185 / 255f, 255 / 255f);
        if (value == 128) { img.color = new Color(145 / 255f, 145 / 255f, 255 / 255f); ValueText.GetComponent<TextMeshProUGUI>().fontSize = 25; }
        if (value == 256) img.color = new Color(105 / 255f, 105 / 255f, 255 / 255f);
        if (value == 512) img.color = new Color(65 / 255f, 65 / 255f, 255 / 255f);
        if (value == 1024) img.color = new Color(65 / 255f, 65 / 255f, 185 / 255f);
        if (value == 2048) { img.color = new Color(65 / 255f, 65 / 255f, 145 / 255f); ValueText.GetComponent<TextMeshProUGUI>().fontSize = 20; }
        if (value == 4096) img.color = new Color(65 / 255f, 65 / 255f, 105 / 255f);
    }

    public void zero()
    {
        value = 0;
        state = 0;
    }

    public void mult()
    {
        value *= 2;
        state = 2;
    }

    public GameObject th()
    {
        return this.gameObject;
    }
}
