using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    public static CanvasController instance { get; private set; }
    private const float DEFAULT_ACCURACY = 0.001f;
    private const float ERROR_MESSAGE_TIME = 2f;
    private const string INPUT_ERROR_MESSAGE = "Wrong input";

    public GameObject info;
    public GameObject jacobiMethodGO;

    public GameObject error;
    public Image errorImage;
    public Text errorText;
    private Coroutine errorCoroutine;

    public InputField[] inputMatrix;
    public InputField[] inputAdditional;
    public Text[] textResults;

    private double[,] matrix = new double[3, 3];
    private double[] additional = new double[3];
    private int numOfDimension;


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
        numOfDimension = (int) Mathf.Sqrt(inputMatrix.Length);
        var test1 = new double[,] {{1, 1, 1}, {1, -1, 1}, {1, -1, -1}};
        var test2 = new double[] {3, 1, -1};
        JacobiMethod.instance.SetData(test1, test2, DEFAULT_ACCURACY);
        JacobiMethod.instance.CalculateMatrix();
    }

    public void SetDefault() {
        info.SetActive(true);
        jacobiMethodGO.SetActive(false);
    }

    private void OnClick(GameObject other) {
        info.SetActive(false);
        other.SetActive(true);
    }

    public void OnJacobiMethod() {
        OnClick(jacobiMethodGO);
    }

    private bool TryReadInput() {
        for (var i = 0; i < inputMatrix.Length; i++) {
            var _i = i / numOfDimension;
            var _j = i % numOfDimension;
            if (!double.TryParse(inputMatrix[i].text, NumberStyles.Float, CultureInfo.InvariantCulture,
                out matrix[_i, _j])) {
                return false;
            }
        }
        for (var i = 0; i < inputAdditional.Length; i++) {
            if (!double.TryParse(inputAdditional[i].text, NumberStyles.Float, CultureInfo.InvariantCulture,
                out additional[i])) {
                return false;
            }
        }

        return true;
    }


    public void OnFindResults() {
        if (!TryReadInput()) {
            RaiseAndShowError(INPUT_ERROR_MESSAGE);
            return;
        }

        PrintResults(FindResults());
    }

    private double[] FindResults() {
        JacobiMethod.instance.SetData(matrix, additional, DEFAULT_ACCURACY);
        return JacobiMethod.instance.CalculateMatrix();
    }

    private void PrintResults(double[] results) {
        for (var i = 0; i < textResults.Length; i++) {
            textResults[i].text = $"x{i+1} = {results[i]:f4}";
        }
    }
}