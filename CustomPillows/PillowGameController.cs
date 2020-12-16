using UnityEngine;
using Zenject;

namespace CustomPillows
{
    class PillowGameController : MonoBehaviour
    {
        private PillowSpawner _pillowSpawner;

        [Inject]
        private void Construct(PillowSpawner pillowSpawner)
        {
            _pillowSpawner = pillowSpawner;
        }

        void Start()
        {
            _pillowSpawner.SetActive(false);
        }

        void OnDestroy()
        {
            _pillowSpawner.SetActive(true);
        }
    }
}
