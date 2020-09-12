using System.Collections;
using System.Collections.Generic;
using Core;

namespace Logic {
	public class StunningDeBuff : IBuff {
		private int healthUnit;

		public StunningDeBuff(int lifeTick) : base(lifeTick) {
			numberOfLifeTick = lifeTick;
		}

		private StunningDeBuff(StunningDeBuff buff, IUnit unit, ICore core) : base(buff, unit, core) {
			numberOfLifeTick = buff.numberOfLifeTick;
		}

		public override void StartBuff() {
			Unit.LockAction = true;
			healthUnit = Unit.Health;
		}
		public override void EndBuff() {
			Unit.LockAction = false;
		}

		public override void PostLogicTick() {
			if (Unit.Health < healthUnit - 100) {
				Unit.RemoveBuff(this);
				return;
			}
			healthUnit = Unit.Health;
		}

		public override IBuff Copy(IUnit unit, ICore core) {
			return new StunningDeBuff(this, unit, core);
		}
	}
}
