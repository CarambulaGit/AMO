using System;

public class NewtonMethod {
    public static NewtonMethod instance { get; } = new NewtonMethod();

    private const string NO_ROOT = "There is no root on this interval";
    private const string WRONG_BORDERS = "Left border can't be bigger than right border";

    private float leftBorder;
    private float rightBorder;
    private float epsilon;

    public delegate float Function(float x);

    public delegate float FirstDerivative(float x);

    public delegate float SecondDerivative(float x);

    private Function fx;
    private FirstDerivative dfx;
    private SecondDerivative d2fx;

    public void SetData(Function fx, FirstDerivative dfx, SecondDerivative d2fx, float leftBorder, float rightBorder,
        float epsilon) {
        if (leftBorder > rightBorder) {
            throw new Exception(WRONG_BORDERS);
        }

        this.fx = fx;
        this.dfx = dfx;
        this.d2fx = d2fx;
        this.leftBorder = leftBorder;
        this.rightBorder = rightBorder;
        this.epsilon = epsilon;
    }

    public float FindRoot() {
        return FindRoot(leftBorder, rightBorder);
    }

    private float FindRoot(float a, float b) {
        if (fx(a) * fx(b) > 0) {
            throw new Exception(NO_ROOT);
        }

        if (b - a < epsilon) {
            return (b + a) / 2f;
        }

        if (fx(b) * d2fx(b) < 0) {
            // (a, b) = Utils.Swap(a, b);
            b = a;
        }

        var root = b;
        do {
            b = root;  
            root = b - fx(b) / dfx(b);
        } while (Math.Abs(root - b) > epsilon);
        
        return root;
    }
}

public static class Utils {
    public static (T b, T a) Swap<T>(T a, T b) {
        return (b, a);
    }
}