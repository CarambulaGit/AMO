using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Classes;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CanvasController : MonoBehaviour {
    public static CanvasController instance;
    private const float ERROR_MESSAGE_TIME = 2f;
    private const string INPUT_ERROR_MESSAGE = "Wrong input";
    private const int NUM_OF_DIGITS = 4;
    private const string FILE_NAME = "Sort analyse.txt";
    public GameObject info;
    public GameObject arrayManipulator;
    public GameObject graphContainer;
    public GameObject error;
    public Image errorImage;
    public Text errorText;
    public InputField inputFieldLength;
    public InputField inputFieldMin;
    public InputField inputFieldMax;
    public InputField inputFieldArray;
    public Text sortedArray;
    public Image graph;
    public Sprite[] graphs;
    private int currentGraphIndex;
    private Coroutine errorCoroutine;


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
            Debug.Log(Mathf.Lerp(5,6,5.5f));
            time += Time.deltaTime;
            yield return null;
        }

        error.SetActive(false);
        errorCoroutine = null;
    }

    public void RaiseAndShowError(string message) {
        sortedArray.text = string.Empty;
        ShowErrorMessage(message);
    }

    private void Awake() {
        instance = this;
        CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
    }

    private void Start() {
        SetDefault();
        // FindInfoForGraph();
    }

    public void SetDefault() {
        info.SetActive(true);
        arrayManipulator.SetActive(false);
        graphContainer.SetActive(false);
    }

    private void OnClick(GameObject other) {
        info.SetActive(false);
        other.SetActive(true);
    }

    public void OnArrayManipulator() {
        OnClick(arrayManipulator);
    }

    public void OnShowGraph() {
        OnClick(graphContainer);
    }


    public void OnNextGraph() {
        if (currentGraphIndex + 1 < graphs.Length) {
            graph.sprite = graphs[++currentGraphIndex];
        }
    }


    public void OnPrevGraph() {
        if (currentGraphIndex - 1 >= 0) {
            graph.sprite = graphs[--currentGraphIndex];
        }
    }

    public void OnRandomize() {
        if (!long.TryParse(inputFieldLength.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var len) ||
            !float.TryParse(inputFieldMin.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var min) ||
            !float.TryParse(inputFieldMax.text, NumberStyles.Float, CultureInfo.InvariantCulture, out var max) ||
            min >= max) {
            RaiseAndShowError(INPUT_ERROR_MESSAGE);
            return;
        }

        var array = RandomizeArray(len, min, max);
        UpdateArrayText(array);
    }


    private IEnumerable<float> RandomizeArray(long len, float min, float max) {
        var arr = new float[len];
        for (var i = 0; i < arr.Length; i++) {
            arr[i] = (float) Math.Round(Random.Range(min, max), NUM_OF_DIGITS);
        }

        sortedArray.text = string.Empty;
        return arr;
    }


    private void UpdateArrayText(IEnumerable<float> array) {
        inputFieldArray.text = GetStringFromArray(array);
    }


    public void OnSortArray() {
        var splited = inputFieldArray.text.Split(',').ToList();
        var array = new float[splited.Count];
        if (splited.Where((t, i) => !float.TryParse(t, NumberStyles.Float, CultureInfo.InvariantCulture, out array[i]))
            .Any()) {
            RaiseAndShowError(INPUT_ERROR_MESSAGE);
            return;
        }

        array.CocktailSort(out var numOfOperations);

        sortedArray.text = GetStringFromArray(array);
    }

    private string GetStringFromArray<T>(IEnumerable<T> array, string sep = ", ") {
        return sortedArray.text = string.Join(sep, array);
    }

    private void FindInfoForGraph() {
        var writePath = Path.Combine(Application.dataPath, FILE_NAME);
        for (var i = 1; i < 1000000; i++) {
            var arr = RandomizeArray(i, 0, 1);
            arr.ToArray().CocktailSort(out var numOfOperations);
            try {
                using (var sw = new StreamWriter(writePath, true, System.Text.Encoding.Default)) {
                    sw.WriteLine($"{i} {numOfOperations}");
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}