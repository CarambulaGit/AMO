using System;
using System.Threading;
using UnityEngine;

class JacobiMethod {
    public static JacobiMethod instance { get; } = new JacobiMethod();
    private double[,] matrix;
    private double[] additional;

    private double _accuracy;

    public double Accuracy {
        get => _accuracy;
        set => _accuracy = value <= 0.0 ? 0.1 : value;
    }

    private JacobiMethod() { }
    

    public void SetData(double[,] Matrix, double[] FreeElements, double Accuracy) {
        matrix = Matrix;
        additional = FreeElements;
        this.Accuracy = Accuracy;
    }

    public double[] CalculateMatrix() {
        var n = additional.Length;
        var x = new double[n];
        for (var i = 0; i < n; i++) {
            x[i] = 0;
        }

        for (var k = 0; k < n - 1; k++) {
            for (var i = k + 1; i < n; i++) {
                for (var j = k + 1; j < n; j++) {
                    matrix[i, j] = matrix[i, j] - matrix[k, j] * (matrix[i, k] / matrix[k, k]);
                }

                additional[i] = additional[i] - additional[k] * matrix[i, k] / matrix[k, k];
            }
        }

        for (var k = n - 1; k >= 0; k--) {
            double s = 0;
            for (var j = k + 1; j < n; j++)
                s += matrix[k, j] * x[j];
            x[k] = (additional[k] - s) / matrix[k, k];
        }

        return x;
    }
}