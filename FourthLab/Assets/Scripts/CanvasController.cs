using System;
using System.Collections;
using System.Globalization;
using AwesomeCharts;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    public static CanvasController instance { get; private set; }
    private const float ERROR_MESSAGE_TIME = 2f;
    private const string INPUT_ERROR_MESSAGE = "Wrong input";

    public GameObject info;
    public GameObject newtonMethodGO;

    public GameObject error;
    public Image errorImage;
    public Text errorText;
    private Coroutine errorCoroutine;

    public LineChart plot;

    public InputField inputLeftBorder;
    public InputField inputRightBorder;
    public InputField inputEpsilon;

    private float MyFunction(float x) => x * x * x - x - 3;
    private float MyFirstDerivativeFunction(float x) => 3 * x * x - 1;
    private float MySecondDerivativeFunction(float x) => 6 * x;


    private void ShowErrorMessage(string message) {
        if (errorCoroutine != null) {
            StopCoroutine(errorCoroutine);
        }

        errorCoroutine = StartCoroutine(_ShowErrorMessage(message));
    }

    private IEnumerator _ShowErrorMessage(string message) {
        error.SetActive(true);
        errorText.text = message;
        var time = 0f;
        while (time < ERROR_MESSAGE_TIME) {
            errorImage.color = Color.Lerp(Color.red, Color.clear, time / ERROR_MESSAGE_TIME);
            time += Time.deltaTime;
            yield return null;
        }

        error.SetActive(false);
        errorCoroutine = null;
    }

    public void RaiseAndShowError(string message) {
        ShowErrorMessage(message);
    }

    private void Awake() {
        instance = this;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
    }

    private void Start() {
        SetDefault();
    }

    public void SetDefault() {
        info.SetActive(true);
        newtonMethodGO.SetActive(false);
    }

    private void OnClick(GameObject other) {
        info.SetActive(false);
        other.SetActive(true);
    }

    public void OnNewtonMethod() {
        OnClick(newtonMethodGO);
    }

    private bool CheckInputs(out float leftBorder, out float rightBorder, out float epsilon) {
        if (!float.TryParse(inputLeftBorder.text, NumberStyles.Float, CultureInfo.InvariantCulture, out leftBorder) ||
            !float.TryParse(inputRightBorder.text, NumberStyles.Float, CultureInfo.InvariantCulture, out rightBorder) ||
            !float.TryParse(inputEpsilon.text, NumberStyles.Float, CultureInfo.InvariantCulture, out epsilon)) {
            (leftBorder, rightBorder, epsilon) = (0, 0, 0);
            leftBorder = rightBorder = epsilon = 0;
            return false;
        }

        return true;
    }


    public void OnFindRoot() {
        ClearPlotChart(plot);
        if (!CheckInputs(out var leftBorder, out var rightBorder, out var epsilon)) {
            RaiseAndShowError(INPUT_ERROR_MESSAGE);
            return;
        }

        DrawFunction(leftBorder, rightBorder);
        float root;
        try {
            root = FindRoot(leftBorder, rightBorder, epsilon);
        }
        catch (Exception e) {
            RaiseAndShowError(e.Message);
            return;
        }

        AddEntryToPlot(1, root - root / 1000, MyFunction(root + root / 1000), plot);
        AddEntryToPlot(1, root + root / 1000, MyFunction(root - root / 1000), plot);
        RefreshPlotChart();
    }

    private float FindRoot(float leftBorder, float rightBorder, float epsilon) {
        NewtonMethod.instance.SetData(MyFunction, MyFirstDerivativeFunction, MySecondDerivativeFunction, leftBorder,
            rightBorder, epsilon);
        return NewtonMethod.instance.FindRoot();
    }

    private void DrawFunction(float leftBorder, float rightBorder, int numOfPoints = 250) {
        ConfigurePlotChartAxisBorders(plot.XAxis, new Vector2(leftBorder, rightBorder));
        for (var x = leftBorder; x < rightBorder; x += (rightBorder - leftBorder) / numOfPoints) {
            AddEntryToPlot(0, x, MyFunction(x), plot);
        }

        RefreshPlotChart();
    }

    private void AddEntryToPlot(int dataSet, float x, float y, LineChart plot) {
        plot.GetChartData().DataSets[dataSet].AddEntry(new LineEntry(x, y));
    }

    private void AddEntryToPlot(int dataSet, Vector2 entry, LineChart plot) {
        AddEntryToPlot(dataSet, entry.x, entry.y, plot);
    }


    private void ConfigurePlotChartAxisBorders(AxisBase axisObj, Vector2 axisLimits) {
        axisObj.MinAxisValue = axisLimits.x;
        axisObj.MaxAxisValue = axisLimits.y;
    }

    private void ClearPlotChart(LineChart lineChart) {
        foreach (var dataSet in lineChart.GetChartData().DataSets) {
            dataSet.Clear();
        }
    }

    private void RefreshPlotChart() {
        plot.SetDirty();
    }
}