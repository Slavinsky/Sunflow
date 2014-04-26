// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 4.0.30319.1
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------
using System;
using SunflowSharp.Maths;

namespace SunflowSharp.Core.Primitive
{
	public class Cylinder : PrimitiveList {
		public bool Update(ParameterList pl, SunflowAPI api) {
			return true;
		}
		
		public BoundingBox getWorldBounds(Matrix4 o2w) {
			BoundingBox bounds = new BoundingBox(1);
			if (o2w != null)
				bounds = o2w.transform(bounds);
			return bounds;
		}
		
		public float getPrimitiveBound(int primID, int i) {
			return (i & 1) == 0 ? -1 : 1;
		}
		
		public int getNumPrimitives() {
			return 1;
		}
		
		public void prepareShadingState(ShadingState state) {
			state.init();
			state.getRay().getPoint(state.getPoint());
			Instance parent = state.getInstance();
			Point3 localPoint = state.transformWorldToObject(state.getPoint());
			state.getNormal().set(localPoint.x, localPoint.y, 0);
			state.getNormal().normalize();
			
			float phi = (float) Math.Atan2(state.getNormal().y, state.getNormal().x);
			if (phi < 0)
				phi += (float) (2.0 * Math.PI);
			state.getUV().x = phi / (float) (2 * Math.PI);
			state.getUV().y = (localPoint.z + 1) * 0.5f;
			state.setShader(parent.getShader(0));
			state.setModifier(parent.getModifier(0));
			// into world space
			Vector3 worldNormal = state.transformNormalObjectToWorld(state.getNormal());
			Vector3 v = state.transformVectorObjectToWorld(new Vector3(0, 0, 1));
			state.getNormal().set(worldNormal);
			state.getNormal().normalize();
			state.getGeoNormal().set(state.getNormal());
			// compute basis in world space
			state.setBasis(OrthoNormalBasis.makeFromWV(state.getNormal(), v));
		}
		
		public void intersectPrimitive(Ray r, int primID, IntersectionState state) {
			// intersect in local space
			float qa = r.dx * r.dx + r.dy * r.dy;
			float qb = 2 * ((r.dx * r.ox) + (r.dy * r.oy));
			float qc = ((r.ox * r.ox) + (r.oy * r.oy)) - 1;
			double[] t = Solvers.solveQuadric(qa, qb, qc);
			if (t != null) {
				// early rejection
				if (t[0] >= r.getMax() || t[1] <= r.getMin())
					return;
				if (t[0] > r.getMin()) {
					float z = r.oz + (float) t[0] * r.dz;
					if (z >= -1 && z <= 1) {
						r.setMax((float) t[0]);
						state.setIntersection(0);
						return;
					}
				}
				if (t[1] < r.getMax()) {
					float z = r.oz + (float) t[1] * r.dz;
					if (z >= -1 && z <= 1) {
						r.setMax((float) t[1]);
						state.setIntersection(0);
					}
				}
			}
		}
		
		public PrimitiveList getBakingPrimitives() {
			return null;
		}
	}
}

