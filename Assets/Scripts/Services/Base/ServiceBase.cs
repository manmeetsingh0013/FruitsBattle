using UnityEngine;

namespace CardMatch.Services.Base
{
    public abstract class ServiceBase : MonoBehaviour
    {
        protected abstract void RegisterService();

        protected virtual void Awake()
        {
            RegisterService();
        }
    }
}