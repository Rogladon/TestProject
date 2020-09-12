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

		public abstract void LogicTick(IUnit unit);

		public void Tick(IUnit unit) {
			LogicTick(unit);

			numberOfLifeTick--;

			if (numberOfLifeTick <= 0) {
				unit.RemoveBuff(this);
				return;
			}
		}

		public virtual void StartBuff(IUnit unit) { }
		public virtual void EndBuff(IUnit unit) { }
	}
}
