using UnityEngine;

public class LinearAlgorithm {
    private float a;
    private float b;
    private float c;

    public LinearAlgorithm(float a, float b, float c) {
        this.a = a;
        this.b = b;
        this.c = c;
    }

    public float FindResult() {
        return Mathf.Pow(5 + c * Mathf.Sqrt(b + 5 * Mathf.Sqrt(a)), 1 / 3f);
    }
}