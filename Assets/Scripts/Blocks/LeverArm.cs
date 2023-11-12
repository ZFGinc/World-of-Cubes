using System.Collections;
using UnityEngine;

[RequireComponent(typeof(TriggerBlock))]
public class LeverArm : MonoBehaviour
{
    private TriggerBlock _trigger;

    [SerializeField] private Vector2 _angles;
    [SerializeField] private Transform _lever;

    void Start()
    {
        _trigger = GetComponent<TriggerBlock>();
        _trigger.OnTurn += Turn;
    }

    public void Turn(bool state)
    {
        if(state)
        {
            StartCoroutine(SlowTurn(_angles.y, _angles.x));
        }
        else
        {
            StartCoroutine(SlowTurn(_angles.x, _angles.y));
        }
    }

    private IEnumerator SlowTurn(float start, float end)
    {
        int steps = 30;
        float delta = end - start;
        float step = delta / steps;

        while(steps > 0)
        {
            start += step;
            _lever.localRotation = Quaternion.Euler(start, 0, 0);
            yield return new WaitForSeconds(0.01f);
            steps--;
        }
    }
}
