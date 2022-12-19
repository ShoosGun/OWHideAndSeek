using QSB.Player;
using UnityEngine;

namespace HideAndSeek.Roles
{
    public class HideAndSeekRole : MonoBehaviour{

        public PlayerInfo playerInfo;

        public virtual void Start(){
            if (playerInfo.IsLocalPlayer)
                return;

            PlayerManager.OnLocalPlayerStateChange += OnLocalPlayerRoleChange;
        }

        public virtual void EnableRole(){
        }

        public virtual void DisableRole(){
        }

        public virtual void OnLocalPlayerRoleChange(PlayerState newLocalPlayerRole){
        }

        public virtual void OnDestroy(){
            if (playerInfo.IsLocalPlayer)
                return;

            PlayerManager.OnLocalPlayerStateChange -= OnLocalPlayerRoleChange;
        }
    }
}
