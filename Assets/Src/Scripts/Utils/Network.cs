
namespace YsoPlayable {
    public static class Network {
        public static void GameReady() {
            //Luna.Unity.LifeCycle.GameLoaded();
            UnityEngine.Debug.Log("YsoPlayable:Network:GameReady"); 
        } // CTA/Tuto phase
        public static void GameEnd() {
            //Luna.Unity.LifeCycle.GameEnded();
            UnityEngine.Debug.Log("YsoPlayable:Network:GameEnd"); 
        } // EndCard
        public static void Install() {
            //Luna.Unity.Playable.InstallFullGame();
            UnityEngine.Debug.Log("YsoPlayable:Network:Install"); 
        } // Redirection
        public static void SendMsg(string msg) { 
            UnityEngine.Debug.Log($"YsoPlayable:Network:Send:{msg}"); 
        }
    }
}
