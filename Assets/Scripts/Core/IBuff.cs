using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Core {
	public abstract class IBuff {

		protected int NumberOfLifeTick = 1;
		protected IUnit Unit { get; }
		protected ICore Core { get; }

		protected IBuff(int lifeTick) {
			NumberOfLifeTick = lifeTick;
		}
		protected IBuff(IBuff buff, IUnit unit, ICore core) {
			NumberOfLifeTick = buff.NumberOfLifeTick;
			Unit = unit;
			Core = core;
		}

		public abstract IBuff Copy(IUnit unit, ICore core);

		public virtual void PreLogicTick() { }
		public virtual void PostLogicTick() { }

		public void PreTick() {
			PreLogicTick();
		}

		public void PostTick() {
			PostLogicTick();

			NumberOfLifeTick--;

			if (NumberOfLifeTick <= 0) {
				Unit.RemoveBuff(this);
				return;
			}
		}

		public virtual void StartBuff() { }
		public virtual void EndBuff() { }

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
