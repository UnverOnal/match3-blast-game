using System;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

public class Test : MonoBehaviour
{
    public GameObject cube;
    public Ease ease;
    public float duration;
    
    private void Start()
    {
        // var targetPosition = cube.transform.position;
        // targetPosition.y -= 5f;
        // cube.transform.DOMove(targetPosition, duration).SetEase(ease).OnComplete(Bounce);
    }
    
    void Bounce()
    {
        // Use DOTween to make the object bounce
        cube.transform.DOJump(cube.transform.position, 0.2f, 1, 0.15f);
    }
}