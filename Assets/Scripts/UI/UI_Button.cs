using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button : MonoBehaviour
{
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    [SerializeField]
    Text _text;

    enum Buttons
    {
        PointButton,
    }

    enum Texts
    {
        PointText,
        ScroeText,
    }

    private void Start()
    {
        Bind<Button>(typeof(Buttons)) ;
        Bind<Text>(typeof(Texts));
    }

    void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] names = Enum.GetNames(type);

        UnityEngine.Object[] objects = new UnityEngine.Object[name.Length];
        _objects.Add(typeof(T), objects);

        for(int i = 0; i < name.Length; i++)
        {
            objects[i] = Util.FindChild<T>(gameObject, names[i], true);
        }
    }

    int _score = 0;

    public void OnButtonClicked()
    {
        _score++;
        _text.text = $"점수: {_score}";
    }
}
