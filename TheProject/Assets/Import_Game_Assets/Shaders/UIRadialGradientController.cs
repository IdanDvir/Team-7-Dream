using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Shaders {
    [RequireComponent(typeof(Image))]
    
    public class UIRadialGradientController : MonoBehaviour {
        private const string RequiredShaderName = "PixelPie/UIRadialGradient";
        
        private const string OdinIsValidExpression = "@!IsValid()";
        
        private static readonly int PropColor1 = Shader.PropertyToID("_Color1");
        private static readonly int PropColor2 = Shader.PropertyToID("_Color2");
        private static readonly int PropHardness = Shader.PropertyToID("_Hardness");
        private static readonly int PropCenterX = Shader.PropertyToID("_CenterX");
        private static readonly int PropCenterY = Shader.PropertyToID("_CenterY");
        private static readonly int PropScaleX = Shader.PropertyToID("_ScaleX");
        private static readonly int PropScaleY = Shader.PropertyToID("_ScaleY");

        private Image image;
        [InfoBox("Image doesn't have a material with the shader " + RequiredShaderName + " assigned.",
            InfoMessageType.Error, 
            OdinIsValidExpression)]
        [ShowInInspector, DisableIf(OdinIsValidExpression)]
        public Color Color1 {
            get => GetColor(PropColor1);
            set => SetColor(PropColor1, value);
        }

        [ShowInInspector, DisableIf(OdinIsValidExpression)]
        public Color Color2 {
            get => GetColor(PropColor2);
            set => SetColor(PropColor2, value);
        }

        [ShowInInspector, DisableIf(OdinIsValidExpression), PropertyRange(0, 1)]
        public float Hardness {
            get => IsValid() ? image.materialForRendering.GetFloat(PropHardness) : 0;
            set => SetFloat(PropHardness, value);
        }

        [ShowInInspector, DisableIf(OdinIsValidExpression)]
        public Vector2 Center {
            get => IsValid()
                    ? new Vector2(GetFloat(PropCenterX), GetFloat(PropCenterY))
                    : Vector2.zero;
            set {
                if (IsValid()) {
                    SetFloat(PropCenterX, value.x);
                    SetFloat(PropCenterY, value.y);
                }
            }
        }

        [ShowInInspector, DisableIf(OdinIsValidExpression), PropertyRange(0, 3)]
        public float ScaleX {
            get => GetFloat(PropScaleX, 1);
            set => SetFloat(PropScaleX, value);
        }

        [ShowInInspector, DisableIf(OdinIsValidExpression), PropertyRange(0, 3)]
        public float ScaleY {
            get => GetFloat(PropScaleY, 1);
            set => SetFloat(PropScaleY, value);
        }

        private Color GetColor(int propID) {
            return IsValid() ? image.materialForRendering.GetColor(propID) : Color.white;
        }

        private float GetFloat(int propID, float defaultValue = 0) {
            return IsValid() ? image.materialForRendering.GetFloat(propID) : defaultValue;
        }

        private void SetColor(int propID, Color color) {
            if (IsValid()) {
                image.materialForRendering.SetColor(propID, color);
#if UNITY_EDITOR
                image.material.SetColor(propID, color);
#endif
            }
        }

        private void SetFloat(int propID, float value) {
            if (IsValid()) {
                image.materialForRendering.SetFloat(propID, value);
#if UNITY_EDITOR
                image.material.SetFloat(propID, value);
#endif
            }
        }

        private bool IsValid() {
            ValidateImageCache();
            return image != null && image.material != null && image.material.shader.name == RequiredShaderName;
        }

        private void ValidateImageCache() {
            if (image == null) {
                image = GetComponent<Image>();
            }
        }
    }
}