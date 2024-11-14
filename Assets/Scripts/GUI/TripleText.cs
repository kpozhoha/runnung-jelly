using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripleText : MonoBehaviour
{
    [SerializeField]
    [Tooltip("top, middle and bottom texts")]
    private Text[] texts;
    [SerializeField]
    private int fontSize;
    [SerializeField]
    private string text;

    [SerializeField]
    private Color innerColor;
    [SerializeField]
    private Color outerColor;

    public string Text {
        get {
            return text;
        }

        set {
            foreach(Text text in texts) {
                text.text = value;
            }
        }
    }

    public Color InnerColor {
        get {
            return innerColor;
        }
        set {
            texts[1].color = value;
        }
    }

    public Color OuterColor {
        get {
            return outerColor;
        }
        set {
            texts[0].color = value;
            texts[2].color = value;
        }
    }

    public float GeneralOpacity {
        get {
            return texts[0].color.a;
        }

        set {
            Color color;
            foreach(Text text in texts) {
                color = text.color;
                color.a = value;
                text.color = color;
            }
        }
    }

    private void Start() {
        if (texts.Length != 3) {
            throw new System.Exception("tripleText must have exactly 3 text layers");
        }
        for (int i = 0; i < texts.Length; ++i) {
            texts[i].text = text;
            texts[i].fontSize = fontSize;
        }
        texts[0].color = outerColor;
        texts[1].color = innerColor;
        texts[2].color = outerColor;

    }
}
