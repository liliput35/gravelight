// Interactor (uses IInteractable ShowPrompt/HidePrompt)
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;

    private readonly Collider[] _colliders = new Collider[3];
    private IInteractable _currentInteractable;

    private void Update()
    {
        int numFound = Physics.OverlapSphereNonAlloc(
            _interactionPoint.position,
            _interactionPointRadius,
            _colliders,
            _interactableMask
        );

        if (numFound > 0)
        {
            var interactable = _colliders[0].GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (_currentInteractable != interactable)
                {
                    HideCurrentPrompt();
                    _currentInteractable = interactable;
                    _currentInteractable.ShowPrompt();
                }

                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    _currentInteractable.Interact(this);
                }

                return;
            }
        }

        HideCurrentPrompt();
    }

    private void HideCurrentPrompt()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.HidePrompt();
            _currentInteractable = null;
        }
    }
}
