using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core {
	public abstract class IBuff {

		protected int numberOfLifeTick = 1;

		protected IBuff(int lifeTick) {
			numberOfLifeTick = lifeTick;
		}
		protected IBuff(IBuff buff) {
			numberOfLifeTick = buff.numberOfLifeTick;
		}

		public abstract IBuff Copy();

		public virtual void PreLogicTick(IUnit unit) { }
		public virtual void PostLogicTick(IUnit unit) { }

		public void PreTick(IUnit unit) {
			PreLogicTick(unit);
		}

		public void PostTick(IUnit unit) {
			PostLogicTick(unit);

			numberOfLifeTick--;

			if (numberOfLifeTick <= 0) {
				unit.RemoveBuff(this);
				return;
			}
		}

		public virtual void StartBuff(IUnit unit) { }
		public virtual void EndBuff(IUnit unit) { }

		public virtual int OnDamage(int damage) {
			return damage;
		}
		public virtual int OnBeforeManaChange(int delta) {
			return delta;
		}
		public virtual void OnDie() {
		}
	}
}
