using System.Collections;
using AwesomeCharts;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    public static CanvasController instance;
    public static Interpolation interpolation;
    private const float ERROR_MESSAGE_TIME = 2f;
    private const string INPUT_ERROR_MESSAGE = "Wrong input";
    public GameObject info;
    public GameObject myFunc;
    public GameObject sin;
    public GameObject error;
    public Image errorImage;
    public Text errorText;
    private Coroutine errorCoroutine;

    public LineChart myFuncPlot;
    public LineChart myFuncErrorPlot;
    public LineChart sinPlot;
    public LineChart sinErrorPlot;
    public float leftBorder;
    public float rightBorder;


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
        interpolation = new Interpolation(this);
    }

    private void Start() {
        SetDefault();
        ConfigurePlotChartAxisBorders(myFuncPlot.XAxis, new Vector2(leftBorder, rightBorder));
        ConfigurePlotChartAxisBorders(myFuncErrorPlot.XAxis, new Vector2(leftBorder, rightBorder)); 
        ConfigurePlotChartAxisBorders(sinPlot.XAxis, new Vector2(leftBorder, rightBorder));
        ConfigurePlotChartAxisBorders(sinErrorPlot.XAxis, new Vector2(leftBorder, rightBorder)); 
    }

    public void SetDefault() {
        info.SetActive(true);
        myFunc.SetActive(false);
        sin.SetActive(false);
    }

    private void OnClick(GameObject other) {
        info.SetActive(false);
        other.SetActive(true);
    }

    public void OnMyFunc() {
        OnClick(myFunc);
        MyFunction();
    }

    public void OnSin() {
        OnClick(sin);
        SinFunction();
    }

    private void SinFunction() {
        ClearPlotChart(sinPlot);
        ClearPlotChart(sinErrorPlot);
        
        interpolation.SinFunctionInterpolation(11, leftBorder, rightBorder);
        interpolation.SinFunctionError(11, leftBorder, rightBorder);

        RefreshAllPlotCharts();
    }

    private void MyFunction() {
        ClearPlotChart(myFuncPlot);
        ClearPlotChart(myFuncErrorPlot);
        
        interpolation.MyFunctionInterpolation(11, leftBorder, rightBorder);
        interpolation.MyFunctionError(11, leftBorder, rightBorder);

        RefreshAllPlotCharts();
    }

    public void AddEntryToMainPlot(int dataSet, Vector2 entry, LineChart plot) {
        plot.GetChartData().DataSets[dataSet].AddEntry(new LineEntry(entry.x, entry.y)); 
    }

    public void AddEntryToErrorPlot(int dataSet, Vector2 entry, LineChart plot) {
        plot.GetChartData().DataSets[dataSet].AddEntry(new LineEntry(entry.x, entry.y));
    }

    private void ConfigurePlotChartAxisBorders(AxisBase axisObj, Vector2 axis) {
        axisObj.MinAxisValue = axis.x;
        axisObj.MaxAxisValue = axis.y;
    }

    private void ClearPlotChart(LineChart lineChart) {
        foreach (var dataSet in lineChart.GetChartData().DataSets) {
            dataSet.Clear();
        }
    }

    private void RefreshAllPlotCharts() {
        myFuncPlot.SetDirty();
        myFuncErrorPlot.SetDirty();
        sinPlot.SetDirty();
        sinErrorPlot.SetDirty();
    }
}