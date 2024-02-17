using UI;
using Zenject;

namespace DefaultNamespace
{
    public class UIService
    {
        private DiContainer container;

        public UIService(DiContainer container)
        {
            this.container = container;
        }

        public UIMonoBehaviour ShowUI(UIMonoBehaviour ui)
        {
            return container.InstantiatePrefab(ui).GetComponent<UIMonoBehaviour>();
        }

        public void HideUI(UIMonoBehaviour ui)
        {
            UnityEngine.Object.DestroyImmediate(ui.gameObject);
        }
    }
}