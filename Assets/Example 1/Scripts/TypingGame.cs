using System;
using System.Globalization;
using R3;
using TMPro;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class TypingGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _paragraphTMP;
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private TextMeshProUGUI _timerText;
    [SerializeField] private TextMeshProUGUI _progressText;
    
    private string[] _words;
    private int _wordIndex = -1;
    private int _lastCharacterIndex;
    private int _correctWordCount;

    private IDisposable _timerDisposable;
    private float _remainingTime;
    private static readonly char[] _separator = {';', ' ', '.', ',', '!', '?', '\n', '\r', '\t'};

    private void Start()
    {
        SplitParagraph();
        MoveToNextWord();
        _inputField.Select();
        _inputField.onValueChanged.AsObservable().Subscribe(OnTextInput).AddTo(gameObject);
    }

    private void SplitParagraph()
    {
        _words = _paragraphTMP.text.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
    }

    private void MoveToNextWord()
    {
        _timerDisposable?.Dispose();

        if (_wordIndex == _words.Length - 1)
        {
            _timerText.gameObject.SetActive(false);
            _inputField.gameObject.SetActive(false);
            return;
        }

        _wordIndex++;
        _remainingTime = 5;
        _timerText.text = string.Empty;
        _inputField.text = string.Empty;
        _timerDisposable = CreateTimer();
    }

    private IDisposable CreateTimer()
    {
        return Observable.Timer(TimeSpan.FromSeconds(0.1))
            .SelectMany(_ => Observable.EveryUpdate().TakeUntil(Observable.Timer(TimeSpan.FromSeconds(5))))
            .Subscribe(UpdateTimerText, onCompleted: OnTimeOut);
    }

    private void UpdateTimerText(Unit _)
    {
        _timerText.text = (_remainingTime -= Time.deltaTime).ToString("0.00", CultureInfo.InvariantCulture) + " sec";
    }

    private void OnTimeOut(Result _)
    {
        HighlightCurrentAndMoveToNext(false);
    }

    private void OnTextInput(string input)
    {
        if (AreStringsEqual(_inputField.text, _words[_wordIndex]))
            HighlightCurrentAndMoveToNext(true);
    }

    private void HighlightCurrentAndMoveToNext(bool isCorrect)
    {
        _progressText.text = $"{(isCorrect ? ++_correctWordCount : _correctWordCount)}/{_wordIndex + 1}";
        HighlightWordWithColor(_words[_wordIndex], isCorrect ? Color.green : Color.red);
        MoveToNextWord();
    }

    private void HighlightWordWithColor(string word, Color color)
    {
        var colorCode = ColorUtility.ToHtmlStringRGB(color);
        var colorPrefix = $"<color=#{colorCode}>";

        var prefixIndex = _paragraphTMP.text.IndexOf(word, _lastCharacterIndex);
        var suffixIndex = prefixIndex + colorPrefix.Length + word.Length;

        _lastCharacterIndex = suffixIndex;
        _paragraphTMP.text = _paragraphTMP.text.Insert(prefixIndex, colorPrefix).Insert(suffixIndex, "</color>");
    }

    private bool AreStringsEqual(string strA, string strB)
    {
        return string.Equals(strA, strB, StringComparison.InvariantCultureIgnoreCase);
    }
}