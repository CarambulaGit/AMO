using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {
    public static CanvasController instance;
    private const string inputErrorMessage = "Wrong input";
    private const string resultErrorMessage = "Result is not a number (NaN)";
    public GameObject info;
    public GameObject linear;
    public GameObject ramified;
    public GameObject cyclic;
    public GameObject error;
    public InputField a;
    public InputField b;
    public InputField c;
    public InputField k;
    public InputField d;
    public InputField n;
    public InputField min;
    public InputField max;
    public Text resultLinear;
    public Text resultRamified;
    public Text resultCyclic;
    private Text errorText;
    private Coroutine errorCoroutine;
    

    public void ShowMessage(string message) {
        if (errorCoroutine != null) {
            StopCoroutine(errorCoroutine);
        }

        errorCoroutine = StartCoroutine(_ShowMessage(message));
    }

    private IEnumerator _ShowMessage(string message) {
        error.SetActive(true);
        errorText.text = message;
        yield return new WaitForSeconds(2);
        error.SetActive(false);
        errorCoroutine = null;
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        SetDefault();
        errorText = error.GetComponent<Text>();
    }

    private void Update() { }

    public void SetDefault() {
        info.SetActive(true);
        linear.SetActive(false);
        ramified.SetActive(false);
        cyclic.SetActive(false);
    }

    private void OnClick(GameObject other) {
        info.SetActive(false);
        other.SetActive(true);
    }

    public void OnLinear() {
        OnClick(linear);
    }

    public void OnRamified() {
        OnClick(ramified);
    }

    public void OnCyclic() {
        OnClick(cyclic);
    }

    public void OnCalculateLinear() {
        if (!float.TryParse(a.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var _a) ||
            !float.TryParse(b.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var _b) ||
            !float.TryParse(c.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var _c)) {
            ShowMessage(inputErrorMessage);
            resultLinear.text = "=";
            return;
        }

        var result = new LinearAlgorithm(_a, _b, _c).FindResult();
        if (float.IsNaN(result)) {
            ShowMessage(resultErrorMessage);
            resultLinear.text = "=";
            return;
        }

        resultLinear.text =
            $"= {result.ToString("G3", CultureInfo.InvariantCulture)}";
    }

    public void OnCalculateRamified() {
        if (!float.TryParse(k.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var _k) ||
            !float.TryParse(d.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var _d)) {
            ShowMessage(inputErrorMessage);
            resultLinear.text = "y =";
            return;
        }

        var result = new RamifiedAlgorithm(_k, _d).FindResult();
        if (float.IsNaN(result)) {
            ShowMessage(resultErrorMessage);
            resultLinear.text = "y =";
            return;
        }

        resultRamified.text =
            $"y = {result.ToString("G3", CultureInfo.InvariantCulture)}";
    }

    public void OnCalculateCyclic() {
        if (!int.TryParse(n.text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var _n) ||
            !float.TryParse(min.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var _min) ||
            !float.TryParse(max.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var _max) ||
            !CyclicAlgorithm.CheckVars(_n, _min, _max)) {
            ShowMessage(inputErrorMessage);
            resultLinear.text = "f =";
            return;
        }

        var result = new CyclicAlgorithm(_n, _min, _max).FindResult();
        if (float.IsNaN(result)) {
            ShowMessage(resultErrorMessage);
            resultLinear.text = "f =";
            return;
        }

        resultCyclic.text =
            $"f = {result.ToString("G3", CultureInfo.InvariantCulture)}";
    }
}