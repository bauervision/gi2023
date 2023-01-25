using UnityEngine;

//[ExecuteInEditMode]
public class AnimateUV : MonoBehaviour
{

    public Vector2 speed;
    private Renderer _renderer;
    public string[] textureParameters = new string[] { "_MainTex" };
    public Vector2 offSet;


    private void Awake()
    {
        _renderer = this.gameObject.GetComponent<MeshRenderer>();

    }

    void Update()
    {
        offSet.x += speed.x * Time.deltaTime;
        offSet.y += speed.y * Time.deltaTime;
        if (_renderer != null)
            for (int i = 0; i < textureParameters.Length; i++)
                _renderer.material.SetTextureOffset(textureParameters[i], offSet);

    }

}