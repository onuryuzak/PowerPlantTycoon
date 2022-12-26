using System;
using UnityEngine;
using Obi;
using Unity.VisualScripting;


[RequireComponent(typeof(ObiRope))]
public class HosePump : MonoBehaviour {

    [Header("Bulge controls")]
    public float pumpSpeed = 5;
    public float bulgeFrequency = 3;
    public float baseThickness = 0.04f;
    public float bulgeThickness = 0.06f;
    public Color bulgeColor = Color.cyan;

    public ObiPathSmoother smoother;
    private ObiRope rope;
    private float time = 0;

    void OnEnable()
    {
        rope = GetComponent<ObiRope>();
        smoother = GetComponent<ObiPathSmoother>();
        rope.OnBeginStep += Rope_OnBeginStep;
    }
    void OnDisable()
    {
        rope.OnBeginStep -= Rope_OnBeginStep;
    }

    private void Rope_OnBeginStep(ObiActor actor, float stepTime)
    {
        time += stepTime * pumpSpeed;

        float distance = 0;
        float sine = 0;
        
        for (int i = 0; i < rope.solverIndices.Length; ++i)
        {
            int solverIndex = rope.solverIndices[i];

            if (i > 0)
            {
                int previousIndex = rope.solverIndices[i - 1];
                distance += Vector3.Distance(rope.solver.positions[solverIndex],rope.solver.positions[previousIndex]);
            }

            sine = Mathf.Max(0, Mathf.Sin(distance * bulgeFrequency - time));

            rope.solver.principalRadii[solverIndex] = Vector3.one * Mathf.Lerp(baseThickness,bulgeThickness, sine);
            rope.solver.colors[solverIndex] = Color.Lerp(Color.white, bulgeColor, sine);
        }
    }
}   

