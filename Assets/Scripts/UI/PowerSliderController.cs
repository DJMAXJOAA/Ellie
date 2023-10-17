using Assets.Scripts.Data.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class PowerSliderController : MonoBehaviour
    {
        [SerializeField] private SliderData sliderData;

        [SerializeField] private GameObject fillArea;

        [Header("Set Slider Value")]
        [SerializeField] private Color startColor = Color.white;
        [SerializeField] private Color endColor = Color.red;

        private Slider slider;
        private Image fillAreaImage;

        private readonly float maxChargingTime = 1.5f;
        private float chargingTime = 0.0f;
        private bool onCharge = false;

        private void Awake()
        {
            slider = gameObject.GetComponent<Slider>();
            fillAreaImage = fillArea.GetComponentInChildren<Image>();
        }

        private void Start()
        {
            sliderData.SliderValue.OnChange -= OnChangeSliderValue;
            sliderData.SliderValue.OnChange += OnChangeSliderValue;
        }

        private void Update()
        {
            Charge();
        }

        private void Charge()
        {
            if (Input.GetMouseButtonDown(0) && !onCharge)
            {
                onCharge = true;
                chargingTime += Time.deltaTime;
                sliderData.SliderValue.Value = chargingTime / maxChargingTime;
            }
            else if (Input.GetMouseButton(0) && onCharge)
            {
                chargingTime = Mathf.Clamp(chargingTime + Time.deltaTime, 0.0f, maxChargingTime);
                sliderData.SliderValue.Value = chargingTime / maxChargingTime;
            }
            else if (Input.GetMouseButtonUp(0) && onCharge)
            {
                onCharge = false;
                sliderData.SliderValue.Value = chargingTime / maxChargingTime;

                chargingTime = 0.0f;
                slider.value = 0.0f;
                fillAreaImage.color = startColor;
            }
        }

        private void OnChangeSliderValue(float value)
        {
            slider.value = value;

            float r = startColor.r * (1.0f - value) + endColor.r * value;
            float g = startColor.g * (1.0f - value) + endColor.g * value;
            float b = startColor.b * (1.0f - value) + endColor.b * value;

            fillAreaImage.color = new Color(r, g, b);
        }
    }
}