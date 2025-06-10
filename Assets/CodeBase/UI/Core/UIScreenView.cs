using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.CodeBase.UI.Core
{
    public abstract class UIScreenView : MonoBehaviour
    {
        public virtual async UniTask Show()
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            await UniTask.CompletedTask;
        }

        public virtual async UniTask Hide()
        {
            gameObject.SetActive(false);
            await UniTask.CompletedTask;
        }
    }
}
