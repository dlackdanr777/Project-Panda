using System;
using UnityEngine;
using Muks.Tween;
using UnityEngine.UI;

public class Event1_0 : StoryEvent
{
    [SerializeField] private CameraController _camera;
    private Vector3 _tempObjPos;

    private void Start()
    {
        _tempObjPos = gameObject.transform.position;
    }


    public override void EventStart(Action onComplate)
    {
        float cameraTargetPosY = _camera.MapCenter.y - 1;
        Vector3 targetPos = new Vector3(transform.position.x, cameraTargetPosY, Camera.main.transform.position.z);
        Vector3 tempObjPos = gameObject.transform.position;

        Tween.CameraOrthographicSize(Camera.main.gameObject, 15f, 2f, TweenMode.Smoothstep);

        Tween.TransformMove(Camera.main.gameObject, targetPos, 2, TweenMode.Smootherstep, () =>
        {
            Tween.SpriteRendererAlpha(gameObject, 1, 1, TweenMode.Quadratic, () =>
            {
                Tween.SpriteRendererAlpha(gameObject, 1, 1, TweenMode.Quadratic, () =>
                {
                    Tween.TransformMove(gameObject, new Vector3(tempObjPos.x, -13.9f, 0), 0.5f, TweenMode.Smoothstep, () =>
                    {
                        GetComponent<SpriteRenderer>().sortingOrder = 5;
                        Tween.TransformMove(gameObject, new Vector3(tempObjPos.x, tempObjPos.y, 0), 0.5f, TweenMode.Quadratic, () =>
                        {
                            Tween.TransformMove(gameObject, gameObject.transform.position, 0.5f, TweenMode.Constant, onComplate);

                        });
                    });
                });
                
            });
        });

    }

    public override void EventCancel(Action onComplate = null)
    {
        
        Tween.Stop(gameObject);
        Tween.Stop(Camera.main.gameObject);
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        gameObject.transform.position = _tempObjPos;
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        renderer.sortingOrder = 1;
    }

}
