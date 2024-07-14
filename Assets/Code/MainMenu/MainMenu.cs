using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private AudioClip menuMusic;
	[SerializeField] private List<AudioClip> clickClips;
	[SerializeField] private Transform gameLogo;

	public static AudioSource mainMenuMusic;

	void Start()
	{
		// gameLogo.transform.DOJump(gameLogo.transform.position, 0.4f, 1, 4).SetLoops(-1).SetEase(Ease.Linear);
		gameLogo
      .DOLocalMoveY(gameLogo.localPosition.y - 35f, 2.5f)
      .SetLoops(-1, LoopType.Yoyo)
      .SetEase(Ease.Linear)
      .SetLink(gameLogo.gameObject, LinkBehaviour.KillOnDestroy);

		if(mainMenuMusic != null) {
			// Destroy(mainMenuMusic);
      return;
		}
		mainMenuMusic = Sound.PlaySoundAtPos(transform.position, menuMusic, Sound.MixerTypes.BGMMain, 1, sound2D: true, destroyAfter: false, initialFadeDur: 1f);
		mainMenuMusic.loop = true;
		DontDestroyOnLoad(mainMenuMusic.gameObject);
		mainMenuMusic.gameObject.AddComponent<DestroyWhenSceneChangeTo>().SetSceneIndex(2);
	}

	void Update() {
	}

	public void DisplayPanel(CanvasGroup menuPanel) {
		menuPanel.transform.DOMove(Vector3.zero, 1f);
		menuPanel.interactable = true;
		menuPanel.blocksRaycasts = true;
		menuPanel.DOFade(1, 1f);
	}

	public void HidePanel(CanvasGroup menuPanel) {
		menuPanel.transform.DOMoveY(10, 1f);
		menuPanel.interactable = false;
		menuPanel.blocksRaycasts = false;
		menuPanel.DOFade(0, 1f);
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void PlayRandomClickClip()
	{
		if (clickClips.Count > 0)
		{
			AudioClip audioClip = clickClips[Random.Range(0, clickClips.Count)];
			Sound.PlaySoundAtPos(Vector3.zero, audioClip, Sound.MixerTypes.SFX, 1f, true, false, true);
		}
	}
}
