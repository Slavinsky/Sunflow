using System;
using SunflowSharp.Core;
using SunflowSharp.Image;
using SunflowSharp.Maths;

namespace SunflowSharp.Core.Shader
{

    public class IDShader : IShader
    {
        public bool Update(ParameterList pl, SunflowAPI api)
        {
            return true;
        }

        public Color GetRadiance(ShadingState state)
        {
            Vector3 n = state.getNormal();
            float f = n == null ? 1.0f : Math.Abs(state.getRay().dot(n));
            return new Color(state.getInstance().GetHashCode()).mul(f);
        }

        public void ScatterPhoton(ShadingState state, Color power)
        {
        }
    }
}