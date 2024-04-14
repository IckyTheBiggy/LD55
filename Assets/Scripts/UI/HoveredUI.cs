using System.Collections;
using Assets.NnUtils.Scripts;
using Core;
using NnUtils.Scripts;
using NnUtils.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HoveredUI : MonoBehaviour
    {
        private bool _shown;

        [SerializeField] private CanvasGroup _group;
        [SerializeField] private GameObject _panel;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;
        [SerializeField] private NBar _health;
        
        private void Start()
        {
            GameManager.Instance.SelectionManager.OnHoveredObjectChanged += OnHoveredChanged;
        }

        private void OnHoveredChanged()
        {
            var hovered = GameManager.Instance.SelectionManager.HoveredObject;
            if (hovered == null || !hovered.TryGetComponent<IDisplayable>(out var displayable))
            {
                Hide();
                return;
            }
            _image.sprite = displayable.GetSprite();
            _name.text = displayable.GetName();
            _health.Max = displayable.GetMaxHealth();
            _health.Value = displayable.GetHealth();
            Show();
        }

        private void Show()
        {
            _shown = true;
            Misc.RestartCoroutine(this, ref _fadeRoutine, FadeRoutine());
        }
        private void Hide()
        {
            _shown = false;
            Misc.RestartCoroutine(this, ref _fadeRoutine, FadeRoutine());
        }

        private Coroutine _fadeRoutine;
        private IEnumerator FadeRoutine()
        {
            var lerpPos = _group.alpha;
            while (_shown ? lerpPos < 1 : lerpPos > 0)
            {
                _group.alpha = _shown
                    ? Misc.UpdateLerpPos(ref lerpPos, 0.15f, false, Easings.Types.SineInOut)
                    : Misc.ReverseLerpPos(ref lerpPos, 0.15f, false, Easings.Types.SineInOut);
                yield return null;
            }
            _fadeRoutine = null;
        }
    }
}
