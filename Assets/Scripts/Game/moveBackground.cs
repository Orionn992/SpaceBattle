using UnityEngine;

public class moveBackground : MonoBehaviour
{
    [SerializeField] private MeshRenderer _bgRenderer;
    [SerializeField] private float _speed = 0.01f;
    private Vector2 _startOffset;
    private int _mainTextured = Shader.PropertyToID("_MainTex");
    private float _tempYOffset;
    void Start()
    {
        _startOffset = _bgRenderer.sharedMaterial.GetTextureOffset(_mainTextured);
    }

    
    void Update()
    {
        _tempYOffset = Mathf.Repeat(_tempYOffset + Time.deltaTime * _speed, 1);
        Vector2 offset = new Vector2(_startOffset.x, _tempYOffset);
        _bgRenderer.sharedMaterial.SetTextureOffset(_mainTextured, offset);
    }
}
