using System.Collections.Generic;
using UnityEngine;

public class CyclicAlgorithm {
    private int n;
    private float min;
    private float max;
    private List<float> a = new List<float>();
    private List<float> b = new List<float>();

    public static bool CheckVars(int n, float min, float max) {
        return n >= 1 && !(min > max);
    }

    public CyclicAlgorithm(int n, float min, float max) {
        this.n = n;
        this.min = min;
        this.max = max;
        for (var i = 0; i <= n; i++) {
            a.Add(Random.Range(min, max));
            b.Add(Random.Range(min, max));
        }
    }

    public float FindResult() {
        var p = 1f;
        var s = 0f;
        for (var i = 0; i < n; i++) {
            p *= a[i] + b[i + 1];
            s += a[i + 1] * b[i];
        }

        return p + s;
    }
}