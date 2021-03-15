using UnityEngine;

public class Interpolation {
    private CanvasController ui;

    public Interpolation(CanvasController ui) {
        this.ui = ui;
    }

    public void SinFunctionInterpolation(int degree, float leftBorder, float rightBorder) {
        float[] xArray = ValuesInInterval(leftBorder, rightBorder, degree);
        float[] yArray = new float[xArray.Length];
        for (int i = 0; i < xArray.Length; i++) {
            yArray[i] = Mathf.Sin(xArray[i]);
        }

        float xValue = leftBorder;
        float step = 0.01f;
        while (xValue < rightBorder) {
            float interpolatedValue = Interpolate(xArray, yArray, xValue);
            float analyticValue = Mathf.Sin(xValue);
            ui.AddEntryToMainPlot(0, new Vector2(xValue, interpolatedValue), CanvasController.instance.sinPlot);
            ui.AddEntryToMainPlot(1, new Vector2(xValue, analyticValue), CanvasController.instance.sinPlot);
            xValue += step;
        }
    }

    public void MyFunctionInterpolation(int degree, float leftBorder, float rightBorder) {
        float[] xArray = ValuesInInterval(leftBorder, rightBorder, degree);
        float[] yArray = new float[xArray.Length];
        for (int i = 0; i < xArray.Length; i++) {
            yArray[i] = 3 * Mathf.Pow(Mathf.Cos(xArray[i]), 2) - Mathf.Sqrt(xArray[i]);
        }

        float xValue = leftBorder;
        float step = 0.01f;
        while (xValue < rightBorder) {
            float interpolatedValue = Interpolate(xArray, yArray, xValue);
            float analyticValue = 3 * Mathf.Pow(Mathf.Cos(xValue), 2) - Mathf.Sqrt(xValue);
            ui.AddEntryToMainPlot(0, new Vector2(xValue, interpolatedValue), CanvasController.instance.myFuncPlot);
            ui.AddEntryToMainPlot(1, new Vector2(xValue, analyticValue), CanvasController.instance.myFuncPlot);
            xValue += step;
        }
    }

    public void SinFunctionError(int maxDegree, float leftBorder, float rightBorder) {
        float[][] xArrays = new float[maxDegree + 1][];
        float[][] yArrays = new float[maxDegree + 1][];

        for (int i = 0; i < maxDegree + 1; i++) {
            xArrays[i] = ValuesInInterval(leftBorder, rightBorder, i + 2);
            yArrays[i] = new float[xArrays[i].Length];
            for (int j = 0; j < xArrays[i].Length; j++) {
                yArrays[i][j] = Mathf.Sin(xArrays[i][j]);
            }
        }

        for (int i = 0; i < maxDegree - 1; i++) {
            float xValue = leftBorder;
            float step = 0.01f;
            while (xValue < rightBorder) {
                float yValue = Mathf.Abs(Interpolate(xArrays[i], yArrays[i], xValue) -
                                         Interpolate(xArrays[i + 1], yArrays[i + 1], xValue));
                ui.AddEntryToErrorPlot(i, new Vector2(xValue, yValue), CanvasController.instance.sinErrorPlot);
                xValue += step;
            }
        }
    }

    public void MyFunctionError(int maxDegree, float leftBorder, float rightBorder) {
        float[][] xArrays = new float[maxDegree + 1][];
        float[][] yArrays = new float[maxDegree + 1][];

        for (int i = 0; i < maxDegree + 1; i++) {
            xArrays[i] = ValuesInInterval(leftBorder, rightBorder, i + 2);
            yArrays[i] = new float[xArrays[i].Length];
            for (int j = 0; j < xArrays[i].Length; j++) {
                yArrays[i][j] = Mathf.Exp(-(xArrays[i][j] + Mathf.Sin(xArrays[i][j])));
            }
        }

        for (int i = 0; i < maxDegree - 1; i++) {
            float xValue = leftBorder;
            float step = 0.01f;
            while (xValue < rightBorder) {
                float yValue = Mathf.Abs(Interpolate(xArrays[i], yArrays[i], xValue) -
                                         Interpolate(xArrays[i + 1], yArrays[i + 1], xValue));
                ui.AddEntryToErrorPlot(i, new Vector2(xValue, yValue), CanvasController.instance.myFuncErrorPlot);
                xValue += step;
            }
        }
    }

    public float Interpolate(float[] xArray, float[] yArray, float xValue) {
        float functionValue = 0;

        for (int i = 0; i < xArray.Length; i++) {
            functionValue += DifferenceOfX(i, xValue, xArray) * DividedDifference(0, i, xArray, yArray);
        }

        return functionValue;
    }

    private float DividedDifference(int leftBorderIndex, int rightBorderIndex, float[] xArray, float[] yArray) {
        float dividedDifference = 0;

        for (int j = leftBorderIndex; j <= rightBorderIndex; j++) {
            float denominator = 1;
            for (int i = leftBorderIndex; i <= rightBorderIndex; i++) {
                if (i == j) {
                    continue;
                }

                denominator *= (xArray[j] - xArray[i]);
            }

            dividedDifference += yArray[j] / denominator;
        }

        return dividedDifference;
    }

    private float DifferenceOfX(int amountOfX, float xValue, float[] xArray) {
        float differenceOfX = 1;

        for (int i = 0; i < amountOfX; i++) {
            differenceOfX *= xValue - xArray[i];
        }

        return differenceOfX;
    }

    private float[] ValuesInInterval(float leftBorder, float rightBorder, int amountOfValues) {
        float step = Mathf.Abs(rightBorder - leftBorder) / (amountOfValues - 1);
        float[] values = new float[amountOfValues];

        for (int i = 0; i < amountOfValues; i++) {
            values[i] = leftBorder + step * i;
        }

        return values;
    }
}