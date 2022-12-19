using System;
using System.Collections.Generic;
using HideAndSeek.Roles;
using OWML.Common;
using QSB.Player;
using UnityEngine;

namespace HideAndSeek{
    public static class PlayerManager{

        public static HashSet<PlayerInfo> hiders = new();
        public static HashSet<PlayerInfo> seekers = new();

        public static Dictionary<PlayerInfo, RoleHandler> playerInfo = new();

        public static PlayerState LocalPlayerState;

        public static event Action<PlayerState> OnLocalPlayerStateChange;

        public static void RemovePlayer(PlayerInfo playerInfo){
            PlayerManager.playerInfo.Remove(playerInfo);
            PlayerManager.hiders.Remove(playerInfo);
            PlayerManager.seekers.Remove(playerInfo);
        }

        public static void SetPlayerState(PlayerInfo playerInfo, PlayerState state){
            //if (!PlayerManager.playerInfo.ContainsKey(playerInfo)){
            //    PlayerManager.playerInfo.Add(playerInfo, new HideAndSeekInfo());
            //}            
            
            switch (state){
                case PlayerState.Hiding:
                    hiders.Add(playerInfo);
                    seekers.Remove(playerInfo);
                    PlayerManager.playerInfo[playerInfo].ChangeToHider();
                    break;
                case PlayerState.Seeking:
                    hiders.Remove(playerInfo);
                    seekers.Add(playerInfo);
                    PlayerManager.playerInfo[playerInfo].ChangeToSeeker();
                    break;
                case  PlayerState.Spectating:
                    hiders.Remove(playerInfo);
                    seekers.Remove(playerInfo);
                    PlayerManager.playerInfo[playerInfo].ChangeToSpectator();
                    break;
                case PlayerState.None:
                    Utils.WriteLine("Player Set to None State", MessageType.Error);
                    break;
            }

            if (playerInfo.IsLocalPlayer){
                LocalPlayerState = state;
                OnLocalPlayerStateChange?.Invoke(state);
            }
        }

        //This should run once every loop to initialize everything needed for Hide and Seek
        public static void SetupPlayer(PlayerInfo playerInfo){
            HideAndSeek.instance.ModHelper.Events.Unity.RunWhen(() => playerInfo.Body != null, () => {
                if (playerInfo.Body.GetComponentInChildren<RoleHandler>() == null)
                {
                    Utils.WriteLine("Setting up " + playerInfo.Name + ": ", MessageType.Debug);

                    GameObject hideAndSeekRoleHandler = new("HideAndSeekRoleHandler");
                    hideAndSeekRoleHandler.transform.parent = playerInfo.Body.transform;
                    hideAndSeekRoleHandler.transform.localPosition = Vector3.zero;
                    hideAndSeekRoleHandler.transform.localRotation = Quaternion.identity;
                    RoleHandler roleHandler = hideAndSeekRoleHandler.AddComponent<RoleHandler>();

                    roleHandler.Init(playerInfo);
                }
            });
        }

        
        public static void SetPlayerSignalSize(PlayerInfo info){
            //PlayerTransformSync.LocalInstance?.ReferenceSector?.AttachedObject.GetRootSector();
            //TODO :: WHEN ADDED TO QSB
        }
    }

    public enum PlayerState{
        Hiding,
        Seeking,
        Spectating,
        None
    }
}