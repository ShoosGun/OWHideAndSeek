using OWML.Common;
using UnityEngine;

namespace HideAndSeek.Roles
{
    public class SeekerRole : HideAndSeekRole{

        GameObject seekerVolume;

        public override void Start(){

            if (playerInfo.IsLocalPlayer)
                return;

            seekerVolume = new("seeker_volume");
            seekerVolume.transform.parent = gameObject.transform;
            seekerVolume.transform.localPosition = Vector3.zero;
            seekerVolume.transform.localRotation = Quaternion.identity;
            seekerVolume.AddComponent<SeekerTrigger>();


            seekerVolume.SetActive(false);

            PlayerManager.OnLocalPlayerStateChange += OnLocalPlayerRoleChange;
        }

        public override void DisableRole(){
            if (!playerInfo.IsLocalPlayer)
                seekerVolume.SetActive(false);
        }

        public override void OnLocalPlayerRoleChange(PlayerState newLocalPlayerRole){
            if(newLocalPlayerRole == PlayerState.Hiding){
                seekerVolume.SetActive(true);
                this.playerInfo.HudMarker.enabled = false;
                this.playerInfo.MapMarker.enabled = false;
            }
            else{
                seekerVolume.SetActive(false);
                Utils.WriteLine("Adding the HUD Marker", MessageType.Success);
                this.playerInfo.HudMarker.enabled = true;
                this.playerInfo.MapMarker.enabled = true;
            }
        }
    }
}
