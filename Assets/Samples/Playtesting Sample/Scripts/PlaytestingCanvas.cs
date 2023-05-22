#pragma warning disable

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using GamePush;
using Random = UnityEngine.Random;

namespace Agava.GamePush.Samples
{
    public class PlaytestingCanvas : MonoBehaviour
    {
        private const string TestSaveKey = "TestSaveKey";
        
        [SerializeField] private Text _authorizationStatusText;
        [SerializeField] private Text _personalProfileDataPermissionStatusText;
        [SerializeField] private InputField _cloudSaveDataInputField;

        private bool _playerReady;

        private void Awake()
        {
            GP_Player.OnReady += OnPlayerReady;
        }

        private void OnPlayerReady()
        {
            GP_Player.OnReady -= OnPlayerReady;
            _playerReady = true;
        }

        private IEnumerator Start()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            yield break;
#endif

            yield return new WaitUntil(() => _playerReady);

            while (true)
            {
                _authorizationStatusText.color = GP_Player.IsLoggedIn() ? Color.green : Color.red;

                if (GP_Player.IsLoggedIn())
                    _personalProfileDataPermissionStatusText.color = string.IsNullOrEmpty(GP_Player.GetName()) == false 
                        ? Color.green : Color.red;
                else
                    _personalProfileDataPermissionStatusText.color = Color.red;

                yield return new WaitForSecondsRealtime(0.25f);
            }
        }

        public void OnShowInterstitialButtonClick()
        {
            GP_Ads.ShowFullscreen();
        }

        public void OnShowVideoButtonClick()
        {
            GP_Ads.ShowRewarded();
        }

        public void OnShowStickyAdButtonClick()
        {
            GP_Ads.ShowSticky();
        }

        public void OnHideStickyAdButtonClick()
        {
            GP_Ads.CloseSticky();
        }

        public void OnAuthorizeButtonClick()
        {
            if (GP_Player.IsLoggedIn())
                return;
            
            GP_Player.Login();
            GP_Player.OnLoginComplete += OnLoginComplete;
            
            void OnLoginComplete()
            {
                GP_Player.OnLoginComplete -= OnLoginComplete;
            }
        }

        public void OnRequestPersonalProfileDataPermissionButtonClick()
        {
            // GamePush can't
        }

        public void OnGetProfileDataButtonClick()
        {
            string name = GP_Player.GetName();
                
            if (string.IsNullOrEmpty(name))
                name = "Anonymous";
                
            Debug.Log($"My id = {GP_Player.GetID()}, name = {name}");
        }

        public void OnSetLeaderboardScoreButtonClick()
        {
            GP_Player.SetScore(Random.Range(1, 100));
            GP_Player.Sync();
        }

        public void OnGetLeaderboardEntriesButtonClick()
        {
            GP_Leaderboard.OnFetchPlayerRatingSuccess += OnFetchPlayerRatingSuccess;
            GP_Leaderboard.OnFetchSuccess += OnFetchSuccess;

            GP_Leaderboard.FetchPlayerRating();
            GP_Leaderboard.Fetch();

            void OnFetchPlayerRatingSuccess(string tag, int rank)
            {
                GP_Leaderboard.OnFetchPlayerRatingSuccess -= OnFetchPlayerRatingSuccess;
                Debug.Log($"My rank = {rank}");
            }

            void OnFetchSuccess(string tag, GP_Data data)
            {
                GP_Leaderboard.OnFetchSuccess -= OnFetchSuccess;
                
                var players = data.GetList<LeaderboardFetchData>();
                
                foreach (var entry in players)
                {
                    string name = entry.name;
                    
                    if (string.IsNullOrEmpty(name))
                        name = "Anonymous";
                    
                    Debug.Log(name + " " + entry.score);
                }
            }
        }

        public void OnGetLeaderboardPlayerEntryButtonClick()
        {
            GP_Leaderboard.OnFetchPlayerRatingSuccess += OnFetchPlayerRatingSuccess;
            GP_Leaderboard.OnFetchPlayerRatingError += OnFetchPlayerRatingError;

            GP_Leaderboard.FetchPlayerRating();
            
            void OnFetchPlayerRatingSuccess(string tag, int rank)
            {
                GP_Leaderboard.OnFetchPlayerRatingSuccess -= OnFetchPlayerRatingSuccess;
                Debug.Log($"My rank = {rank}, score = {GP_Player.GetScore()}");
            }
            
            void OnFetchPlayerRatingError()
            {
                GP_Leaderboard.OnFetchPlayerRatingError -= OnFetchPlayerRatingError;
                Debug.Log("Player is not present in the leaderboard.");
            }
        }

        public void OnSetCloudSaveDataButtonClick()
        {
            GP_Player.Set(TestSaveKey, _cloudSaveDataInputField.text);
            GP_Player.Sync();
        }

        public void OnGetCloudSaveDataButtonClick()
        {
            _cloudSaveDataInputField.text = GP_Player.Has(TestSaveKey) ? GP_Player.GetString(TestSaveKey) : "";
        }

        public void OnGetEnvironmentButtonClick()
        {
            Debug.Log($"Current language: {GP_Language.Current()}");
        }
    }
}
