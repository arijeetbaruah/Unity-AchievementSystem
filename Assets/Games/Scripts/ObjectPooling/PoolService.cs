using UnityEngine;

namespace Game.Service
{
    public class PoolService : IService
    {
        public SpellBtnObjectPool spellBtnPool;

        public PoolService()
        {
            spellBtnPool = Resources.Load<SpellBtnObjectPool>("SpellBtnObjectPool");
            MonoBehaviour.DontDestroyOnLoad(spellBtnPool.gameObject);
            spellBtnPool.gameObject.SetActive(true);
        }
    }
}
