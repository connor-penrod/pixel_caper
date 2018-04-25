using UnityEngine;
using MoreMountains.Tools;
using System.Collections;
using UnityEngine.UI;

namespace MoreMountains.CorgiEngine
{	
	[RequireComponent(typeof(Health))]
	/// <summary>
	/// Add this component to an object and it will show a healthbar above it
	/// You can either use a prefab for it, or have the component draw one at the start
	/// </summary>
	public class HealthBar : MonoBehaviour 
	{
		/// the possible health bar types
		public enum HealthBarTypes { Prefab, Drawn }
		[Information("Add this component to an object and it'll add a healthbar next to it to reflect its health level in real time. You can decide here whether the health bar should be drawn automatically or use a prefab.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// whether the healthbar uses a prefab or is drawn automatically
		public HealthBarTypes HealthBarType = HealthBarTypes.Drawn;

		[Header("Select a Prefab")]
		[Information("Select a prefab with a progress bar script on it. There is one example of such a prefab in Common/Resources/GUI.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// the prefab to use as the health bar
		public ProgressBar HealthBarPrefab;

		[Header("Healthbar Properties ")]
		[Information("Set the size (in world units), padding, back and front colors of the healthbar.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		/// if the healthbar is drawn, its size in world units
		public Vector2 Size = new Vector2(1f,0.2f);
		/// if the healthbar is drawn, the padding to apply to the foreground, in world units
		public Vector2 BackgroundPadding = new Vector2(0.01f,0.01f);
		/// if the healthbar is drawn, the color of its foreground
		public Color ForegroundColor = Color.red;
		/// if the healthbar is drawn, the color of its background
		public Color BackgroundColor = Color.white;

		[Header("Offset")]
		[Information("Set the offset (in world units), relative to the object's center, to which the health bar will be displayed.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		public Vector3 HealthBarOffset;

		[Header("Display")]
		[Information("Here you can define whether or not the healthbar should always be visible. If not, you can set here how long after a hit it'll remain visible.",MoreMountains.Tools.InformationAttribute.InformationType.Info,false)]
		public bool AlwaysVisible = true;
		public float DisplayDurationOnHit = 1f;

		protected Health _health;
		protected ProgressBar _progressBar;

		/// <summary>
		/// On Start, creates the health bar
		/// </summary>
		protected virtual void Start()
		{
			if (HealthBarType == HealthBarTypes.Prefab)
			{
				if (HealthBarPrefab == null)
				{
					Debug.LogWarning(this.name+" : the HealthBar has no prefab associated to it, nothing will be displayed.");
					return;
				}
				_progressBar = Instantiate(HealthBarPrefab,transform.position + HealthBarOffset,transform.rotation) as ProgressBar;
				_progressBar.transform.SetParent(this.transform);
				_progressBar.gameObject.name = "HealthBar";
			}

			if (HealthBarType == HealthBarTypes.Drawn)
			{
				GameObject newGameObject = new GameObject();
				newGameObject.transform.SetParent(this.transform);
				newGameObject.name = "HealthBar";

				_progressBar = newGameObject.AddComponent<ProgressBar>();

				Canvas newCanvas = newGameObject.AddComponent<Canvas>();
				newCanvas.renderMode = RenderMode.WorldSpace;
				newCanvas.transform.localScale = Vector3.one;
				newCanvas.GetComponent<RectTransform>().sizeDelta = Size;
				newCanvas.transform.position = this.transform.position + HealthBarOffset;

				GameObject bgImageGameObject = new GameObject();
				bgImageGameObject.transform.SetParent(newGameObject.transform);
				bgImageGameObject.name = "HealthBar Background";
				Image backgroundImage = bgImageGameObject.AddComponent<Image>();
				backgroundImage.color = BackgroundColor;
				backgroundImage.transform.localScale = Vector3.one;
				backgroundImage.GetComponent<RectTransform>().sizeDelta = Size;
				backgroundImage.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;

				GameObject frontImageGameObject = new GameObject();
				frontImageGameObject.transform.SetParent(newGameObject.transform);
				frontImageGameObject.name = "HealthBar Foreground";
				Image foregroundImage = frontImageGameObject.AddComponent<Image>();
				foregroundImage.color = ForegroundColor;
				foregroundImage.transform.localScale = Vector3.one;
				RectTransform foregroundImageRectTransform = foregroundImage.GetComponent<RectTransform>();
				foregroundImageRectTransform.sizeDelta = Size - BackgroundPadding*2;
				foregroundImageRectTransform.anchoredPosition = -foregroundImageRectTransform.sizeDelta/2;

				foregroundImage.GetComponent<RectTransform>().pivot = Vector2.zero;

				_progressBar.ForegroundBar = foregroundImage.transform;

			}

			if (!AlwaysVisible)
			{
				_progressBar.gameObject.SetActive(false) ;
			}
		}

		/// <summary>
		/// Updates the bar
		/// </summary>
		/// <param name="currentHealth">Current health.</param>
		/// <param name="minHealth">Minimum health.</param>
		/// <param name="maxHealth">Max health.</param>
		public virtual void UpdateBar(float currentHealth, float minHealth, float maxHealth)
		{
			if (_progressBar != null)
			{
				_progressBar.UpdateBar(currentHealth, minHealth, maxHealth)	;
			}

			// if the healthbar isn't supposed to be always displayed, we turn it on for the specified duration
			if (!AlwaysVisible)
			{
				StartCoroutine(ShowProgressBar(DisplayDurationOnHit));
			}
		}

		/// <summary>
		/// Shows the progress bar.
		/// </summary>
		/// <returns>The progress bar.</returns>
		/// <param name="duration">Duration.</param>
		protected virtual IEnumerator ShowProgressBar(float duration)
		{
			if (_progressBar == null) { yield break; }

			_progressBar.gameObject.SetActive(true);
			yield return new WaitForSeconds(duration);
			_progressBar.gameObject.SetActive(false);
		}
	}
}
