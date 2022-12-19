using OWML.Common;
using QSB.Player;
using UnityEngine;

namespace HideAndSeek.Roles{
    public class RoleHandler : MonoBehaviour{
        private bool IsInit = false;

        protected PlayerInfo playerInfo;
        protected AudioSignal signal;
        protected PlayerState state = PlayerState.None;

        HideAndSeekRole currentRole;

        HiderRole hiderRole;
        SeekerRole seekerRole;

        public void Init(PlayerInfo playerInfo){
            this.playerInfo = playerInfo;
            SetUpPlayer();
            IsInit = true;
        }

        public void Start(){
            if (!IsInit){
                Init(playerInfo);
            }
        }

        public void SetUpPlayer(){
            PlayerManager.playerInfo[this.playerInfo] = this;

            hiderRole = gameObject.AddComponent<HiderRole>();
            hiderRole.playerInfo = this.playerInfo;
            hiderRole.signal = this.signal;

            seekerRole = gameObject.AddComponent<SeekerRole>();
            seekerRole.playerInfo = this.playerInfo;

            if (playerInfo.IsLocalPlayer){ //Technically Double check lol
                Utils.WriteLine("Local Player! Skipping Adding Audio Signal", MessageType.Info);
                return;
            }

            //Everyone gets an audio signal
            Utils.WriteLine("Adding Audio Signal", MessageType.Success);
            AudioSignal signal = playerInfo.Body.AddComponent<AudioSignal>();

            Utils.WriteLine("Add the known signal for the local player", MessageType.Success);
            signal._name = SignalName.RadioTower;
            signal._frequency = SignalFrequency.HideAndSeek;
        }

        public void ChangeToSeeker(){
            if (currentRole != seekerRole){
                if (currentRole != null)
                    currentRole.DisableRole();

                seekerRole.EnableRole();
                currentRole = seekerRole;
            }
        }

        public void ChangeToHider()
        {
            if (currentRole != hiderRole)
            {
                if (currentRole != null)
                    currentRole.DisableRole();

                hiderRole.EnableRole();
                currentRole = hiderRole;
            }
        }

        public void ChangeToSpectator(){
            if (currentRole != null)
            {
                currentRole.DisableRole();
                currentRole = null;
            }

            //Does Nothing Rn
            //Heard QSB Is gonna add Spectating
        }

        public void OnDestroy(){
        
        }
    }
}
