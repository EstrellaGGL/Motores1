using UnityEngine;

[System.Serializable]
public class PanelInformacion
{
    public GameObject panel;
    [TextArea(3, 6)]
    public string texto;
    public Texture imagen;
}

public class InteraccionObject : MonoBehaviour
{
    public PanelInformacion[] paneles;
}

