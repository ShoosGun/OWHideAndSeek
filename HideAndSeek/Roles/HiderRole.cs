using OWML.Common;

namespace HideAndSeek.Roles
{
    public class HiderRole : HideAndSeekRole{
        public AudioSignal signal;

        public override void EnableRole(){
            if (playerInfo.IsLocalPlayer)
                return;
            Utils.WriteLine("Removing the HUD Marker", MessageType.Success);
            this.signal._sourceRadius = 500f;
            this.playerInfo.HudMarker.enabled = false;
            this.playerInfo.MapMarker.enabled = false;
        }

        public override void DisableRole(){
            if (playerInfo.IsLocalPlayer)
                return;
            this.signal._sourceRadius = 2f;
        }
    }
}
