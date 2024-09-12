using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
namespace UI
{
    public class CounterTextUI : MonoBehaviour
    {
        [SerializeField] RessourceSystem.Type _type;
        private const float TIME_TO_START = 1f;
        private static readonly int IsGain = Animator.StringToHash("isGain");

        [SerializeField] private TextMeshProUGUI _textScore;
        [SerializeField] private int _speed = 4;

        private Animator _animator;
        private int _score = 0;
        private int _current = 0;
        private float _counterTime = 0;
        private RessourceSystem _ressource;

        private void Awake()
        {
            _textScore.text = "0";
            _animator = _textScore.GetComponent<Animator>();


            _ressource = FindAnyObjectByType<RessourceSystem>();
            if (_ressource != null)
            {
                if (_ressource.GetRessource(_type) != 0)
                {
                    _score = _ressource.GetRessource(_type);
                }
            }

            Cursor.lockState = CursorLockMode.None;
        }



        private void Update()
        {
            if (_ressource != null)
            {
                if (_ressource.GetRessource(_type) != _score)
                {
                    _score = _ressource.GetRessource(_type);
                }
            }
        }
        private void FixedUpdate()
        {
            _counterTime += Time.deltaTime;
            if (!(_counterTime > TIME_TO_START)) return;


            if (_current >= _score)
            {
                _animator.SetBool(IsGain, false);
                return;
            }

            _animator.SetBool(IsGain, true);
            _current += _speed;
            if (_current > _score)
            {
                _current = _score;
            }

           _textScore.text = _current.ToString();
        }
    };
}