using UnityEngine;

public class RamifiedAlgorithm {
    private float k;
    private float d;

    public RamifiedAlgorithm(float k, float d) {
        this.k = k;
        this.d = d;
    }

    public float FindResult() {
        return k > 10
            ? Mathf.Sqrt(k * Mathf.Sqrt(Mathf.Pow(d, 2)) + d * Mathf.Sqrt(Mathf.Pow(k, 2)))
            : Mathf.Pow(k + d, 2);
    }
}