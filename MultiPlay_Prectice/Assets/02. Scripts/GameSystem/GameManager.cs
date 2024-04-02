using MP.Authentication;
using MP.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MP.GameSystem
{
    public enum GameState
    {
        None,
        WaitUntilUserDataVerified,
        InLobby,
        InGamePlay,
    }
    public class GameManager : SingletonMonoBase<GameManager>
    {
        [field: SerializeField]public GameState state {  get; private set; }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            Workflow();            
        }

        private void Workflow()
        {
            switch (state)
            {
                case GameState.None:
                    break;
                case GameState.WaitUntilUserDataVerified:
                    {
                        if (string.IsNullOrEmpty(Login_Information.nickname))
                            return;

                        SceneManager.LoadScene("Lobby");
                        state = GameState.InLobby;
                    }
                    break;
                case GameState.InLobby:
                    break;
                case GameState.InGamePlay:
                    break;
            }
        }
    }
}

