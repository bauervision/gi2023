using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
	public static CameraShake instance;

public Vector3 _originalPos;
private float _timeAtCurrentFrame;
private float _timeAtLastFrame;
private float _fakeDelta;

void Awake()
{
    instance = this;
	_originalPos = transform.position;
}

void Update() {
    // Calculate a fake delta time, so we can Shake while game is paused.
    _timeAtCurrentFrame = Time.realtimeSinceStartup;
    _fakeDelta = _timeAtCurrentFrame - _timeAtLastFrame;
    _timeAtLastFrame = _timeAtCurrentFrame; 
}

public static void Shake (float duration, float amount) {
 
    instance.StopAllCoroutines();
    instance.StartCoroutine(instance.cShake(duration, amount));
}

public IEnumerator cShake (float duration, float amount) {
    float endTime = Time.time + duration;
    _originalPos = transform.localPosition;
    while (duration > 0) {
        transform.localPosition = _originalPos + Random.insideUnitSphere * amount;

        duration -= _fakeDelta;

        yield return null;
    }

    transform.localPosition = _originalPos;
}
}