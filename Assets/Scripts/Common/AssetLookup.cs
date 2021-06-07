using UnityEngine;

namespace CV
{
    public class AssetLookup : MonoBehaviour
    {
        public static AssetLookup _a;

        public static AssetLookup a
        {
            get
            {
                //if (_a == null) _a = Instantiate(Resources.Load<AssetLookup>("AssetLookup"));
                if (_a == null) _a = (Instantiate(Resources.Load("AssetLookup")) as GameObject).GetComponent<AssetLookup>();
                return _a;
            }
        }

        public Transform damagePopup;
    }

    
}