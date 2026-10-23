using System;
using System.Collections;
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;
using UnityEngine.InputSystem;

namespace ChristinaCreatesGames.Typography.Typewriter
{
    [RequireComponent(typeof(TMP_Text))]
    public class TypewriterEffect : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textBox;

        // Basic Typewriter Functionality
        [SerializeField] private int _currentVisibleCharacterIndex;
        [SerializeField] private Coroutine _typewriterCoroutine;
        [SerializeField] private bool _readyForNewText = true;
        public PlayerInput playerInput;

        [SerializeField] private WaitForSeconds _simpleDelay;
        [SerializeField] private WaitForSeconds _interpunctuationDelay;

        [Header("Typewriter Settings")] 
        [SerializeField] private float charactersPerSecond = 20;
        [SerializeField] private float interpunctuationDelay = 0.5f;


        // Skipping Functionality
        public bool CurrentlySkipping { get; private set; }
        [SerializeField] private WaitForSeconds _skipDelay;

        [Header("Skip options")] 
        [SerializeField] private bool quickSkip;
        [SerializeField] [Min(1)] private int skipSpeedup = 5;


        // Event Functionality
        [SerializeField] private WaitForSeconds _textboxFullEventDelay;
        [SerializeField] [Range(0.1f, 0.5f)] private float sendDoneDelay = 0.25f; // In testing, I found 0.25 to be a good value

        public static event Action CompleteTextRevealed;
        public static event Action<char> CharacterRevealed;

        public AudioSource audioSource;
        public DialogueManager dm;
        public int currentSpeaker;
        public AudioClip[] speakerBlips;
        public GameObject eTipImage;


        private void Awake()
        {
            _textBox = GetComponent<TMP_Text>();

            _simpleDelay = new WaitForSeconds(1 / charactersPerSecond);
            _interpunctuationDelay = new WaitForSeconds(interpunctuationDelay);
            
            _skipDelay = new WaitForSeconds(1 / (charactersPerSecond * skipSpeedup));
            _textboxFullEventDelay = new WaitForSeconds(sendDoneDelay);
        }
        
         public void OnSpace(InputAction.CallbackContext context)
         {
            if (context.performed)
            {
                if (_textBox.maxVisibleCharacters != _textBox.textInfo.characterCount - 1)
                    Skip();
            }
         }

        public void SetText(string text)
        {
            CurrentlySkipping = false;
            _readyForNewText = false;
            _currentVisibleCharacterIndex = 0;
            _textBox.maxVisibleCharacters = 0;

            _textBox.text = text;
            _textBox.ForceMeshUpdate();

            _typewriterCoroutine = StartCoroutine(Typewriter());
        }

        private IEnumerator Typewriter()
        {
            TMP_TextInfo textInfo = _textBox.textInfo;

            while (_currentVisibleCharacterIndex < textInfo.characterCount + 1)
            {
                var lastCharacterIndex = textInfo.characterCount - 1;

                if (_currentVisibleCharacterIndex >= lastCharacterIndex)
                {
                    _textBox.maxVisibleCharacters++;
                    yield return _textboxFullEventDelay;
                    CompleteTextRevealed?.Invoke();
                    _readyForNewText = true;
                    yield break;
                }

                char character = textInfo.characterInfo[_currentVisibleCharacterIndex].character;

                _textBox.maxVisibleCharacters++;
                
                if (!CurrentlySkipping &&
                    (character == '?' || character == '.' || character == ',' || character == ':' ||
                     character == ';' || character == '!' || character == '-'))
                {
                    yield return _interpunctuationDelay;
                }
                else
                {
                    yield return CurrentlySkipping ? _skipDelay : _simpleDelay;
                }

                CharacterRevealed?.Invoke(character);
                _currentVisibleCharacterIndex++;
            }
        }

        private void Skip(bool quickSkipNeeded = false)
        {
            if (CurrentlySkipping)
                return;
            
            CurrentlySkipping = true;

            if (!quickSkip || !quickSkipNeeded)
            {
                StartCoroutine(SkipSpeedupReset());
                return;
            }

            StopCoroutine(_typewriterCoroutine);
            _textBox.maxVisibleCharacters = _textBox.textInfo.characterCount;
            _readyForNewText = true;
            CompleteTextRevealed?.Invoke();
        }

        private IEnumerator SkipSpeedupReset()
        {
            yield return new WaitUntil(() => _textBox.maxVisibleCharacters == _textBox.textInfo.characterCount - 1);
            CurrentlySkipping = false;
        }

        private void OnEnable()
        {
            CharacterRevealed += OnCharacterRevealed;
            CompleteTextRevealed += OnTextCompleted;
        }

        private void OnDisable()
        {
            CharacterRevealed -= OnCharacterRevealed;
            CompleteTextRevealed -= OnTextCompleted;
        }

        public void Update()
        {
            switch (dm.actorName.text)
            {
                case "Aldred":
                    currentSpeaker = 0;
                    break;
                case "Edric":
                    currentSpeaker = 1;
                    break;
                case "Godfrey":
                    currentSpeaker = 2;
                    break;
                case "William":
                    currentSpeaker = 3;
                    break;
                case "The Guard":
                    currentSpeaker = 4;
                    break;
            }
        }
        private void OnCharacterRevealed(char revealedCharacter)
        {

            char lowerChar = char.ToLower(revealedCharacter);
            if (lowerChar >= 'a' && lowerChar <= 'z')
            {
                if (!audioSource.isPlaying)
                audioSource.clip = speakerBlips[currentSpeaker];
                audioSource.Play();
                eTipImage.SetActive(false);
            }

        }

        private void OnTextCompleted()
        {
            eTipImage.SetActive(true);
        }
    }
}